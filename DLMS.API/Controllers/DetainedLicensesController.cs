using DLMS.Core.DTOs.DetainedLicenseDTOs;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetainedLicensesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetainedLicensesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        
        [SwaggerOperation(Summary = "Get detained license with Id", 
            Description = "Retrieve detained license with Id from database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(DetainedLicense), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var detainedLicense = await _unitOfWork.DetainedLicenses.GetByIdAsync(id);

            if (detainedLicense is null)
            {
                return NotFound($"There isn't any detained license with Id {id}");
            }

            return Ok(detainedLicense);
        }




        [SwaggerOperation(Summary = "Get all detained licenses",
            Description = "Retrieve all detained licenses from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<DetainedLicense>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var detainedLicenses = await _unitOfWork.DetainedLicenses.GetAllAsync();

            if (detainedLicenses is null)
            {
                return NotFound($"There isn't any detained licenses yet.");
            }

            return Ok(detainedLicenses);
        }




        [SwaggerOperation(Summary = "Create detained license",
            Description = "Create detained license in the database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(DetainedLicense), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateDetainedLicenseDTO createDetainedLicenseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isValidLicenseId = await _unitOfWork.Licenses.AnyAsync(l => l.LicenseID == 
                    createDetainedLicenseDTO.LicenseID);

            if (! isValidLicenseId)
            {
                return NotFound($"Thers isn't any license with Id {createDetainedLicenseDTO.LicenseID}");
            }

            DetainedLicense detainedLicense = new DetainedLicense
            {
                LicenseID = createDetainedLicenseDTO.LicenseID,
                DetainDate = createDetainedLicenseDTO.DetainDate,
                FineFees = createDetainedLicenseDTO.FineFees,
                IsReleased = createDetainedLicenseDTO.IsReleased,
                ReleaseApplicationID = createDetainedLicenseDTO.ReleaseApplicationID,
                ReleaseDate = createDetainedLicenseDTO.ReleaseDate
            };

            await _unitOfWork.DetainedLicenses.AddAsync(detainedLicense);
            await _unitOfWork.CommitAsync();

            return Ok(detainedLicense);
        }





        [SwaggerOperation(Summary = "Update detained license", 
            Description = "Update detaiend license in the database.")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(DetainedLicense), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, UpdateDetainedLicenseDTO updateDetainedLicenseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var detainedLicense = await _unitOfWork.DetainedLicenses.GetByIdAsync(id);

            if (detainedLicense is null)
            {
                return NotFound($"There isn't any detained license with Id {id}");
            }

            detainedLicense.IsReleased = updateDetainedLicenseDTO.IsReleased;
            detainedLicense.ReleaseDate = updateDetainedLicenseDTO.ReleaseDate;
            detainedLicense.ReleaseApplicationID = updateDetainedLicenseDTO.ReleaseApplicationID;

            _unitOfWork.DetainedLicenses.Update(detainedLicense);
            await _unitOfWork.CommitAsync();

            return Ok(detainedLicense);
        }





        [SwaggerOperation(Summary = "Delete a detained license",
            Description = "Delete a detained license from the database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(204)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var detainedLicense = await _unitOfWork.DetainedLicenses.GetByIdAsync(id);

            if (detainedLicense is null)
            {
                return NotFound($"There isn't any detained license with Id {id}");
            }

            _unitOfWork.DetainedLicenses.Delete(detainedLicense);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
