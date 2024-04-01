using DLMS.Core;
using DLMS.Core.DTOs.InternationalLicenseDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternationalLicensesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InternationalLicensesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get international license by id", 
            Description = "Get intrnational license by id from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(InternationalLicense), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var internationalLicense = await _unitOfWork.InternationalLicenses.GetByIdAsync(id);

            if (internationalLicense is null)
            {
                return NotFound($"There is not international license woth id {id}");
            }

            return Ok(internationalLicense);
        }





        [SwaggerOperation(Summary = "Get all international licenses", 
            Description = "Get all international licenses from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<InternationalLicense>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var internationalLicenses = await _unitOfWork.InternationalLicenses.GetAllAsync();

            if (internationalLicenses is null)
            {
                return NotFound($"There isn't any international licenses in the database yet.");
            }

            return Ok(internationalLicenses);
        }




        [SwaggerOperation(Summary = "Create an international license",
            Description = "Create an international license in the database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(InternationalLicense), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateInternationalLicenseDTO createInternationalLicenseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isValidApplication = await _unitOfWork.Applications.AnyAsync(a => a.ApplicationID ==
                    createInternationalLicenseDTO.ApplicationID);

            if (!isValidApplication)
            {
                return NotFound($"There isn't application with id {createInternationalLicenseDTO.ApplicationID}");
            }

            bool isValidDriver = await _unitOfWork.Drivers.AnyAsync(d => d.DriverID ==
                    createInternationalLicenseDTO.DriverID);

            if (! isValidDriver)
            {
                return NotFound($"There isn't driver with id {createInternationalLicenseDTO.DriverID}");
            }

            bool isValidLocalLicense = await _unitOfWork.Licenses.AnyAsync(l => l.LicenseID ==
                    createInternationalLicenseDTO.IssuedUsingLocalLicenseID);

            if (! isValidLocalLicense)
            {
                return NotFound($"There isn't any local license with id {createInternationalLicenseDTO.IssuedUsingLocalLicenseID}");
            }

            InternationalLicense internationalLicense = new InternationalLicense
            {
                ApplicationID = createInternationalLicenseDTO.ApplicationID,
                IssuedUsingLocalLicenseID = createInternationalLicenseDTO.IssuedUsingLocalLicenseID,
                DriverID = createInternationalLicenseDTO.DriverID,
                ExpirationDate = createInternationalLicenseDTO.ExpirationDate,
                IssueDate = createInternationalLicenseDTO.IssueDate,
                IsActive = createInternationalLicenseDTO.IsActive
            };

            await _unitOfWork.InternationalLicenses.AddAsync(internationalLicense);
            await _unitOfWork.CommitAsync();

            return Ok(internationalLicense);
        }




        [SwaggerOperation(Summary = "Update an international license",
            Description = "Update an international license in the database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(InternationalLicense), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateInternationalLicenseDTO updateInternationalLicenseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var internationalLicense = await _unitOfWork.InternationalLicenses.GetByIdAsync(id);

            if (internationalLicense is null)
            {
                return NotFound($"There isn't any international license with id {id}");
            }

            bool isValidApplication = await _unitOfWork.Applications.AnyAsync(a => a.ApplicationID ==
                    updateInternationalLicenseDTO.ApplicationID);

            if (!isValidApplication)
            {
                return NotFound($"There isn't application with id {updateInternationalLicenseDTO.ApplicationID}");
            }

            bool isValidDriver = await _unitOfWork.Drivers.AnyAsync(d => d.DriverID ==
                    updateInternationalLicenseDTO.DriverID);

            if (!isValidDriver)
            {
                return NotFound($"There isn't driver with id {updateInternationalLicenseDTO.DriverID}");
            }

            bool isValidLocalLicense = await _unitOfWork.Licenses.AnyAsync(l => l.LicenseID ==
                    updateInternationalLicenseDTO.IssuedUsingLocalLicenseID);

            if (!isValidLocalLicense)
            {
                return NotFound($"There isn't any local license with id {updateInternationalLicenseDTO.IssuedUsingLocalLicenseID}");
            }

            internationalLicense.ApplicationID = updateInternationalLicenseDTO.ApplicationID;
            internationalLicense.DriverID = updateInternationalLicenseDTO.DriverID;
            internationalLicense.IsActive = updateInternationalLicenseDTO.IsActive;
            internationalLicense.IssuedUsingLocalLicenseID = updateInternationalLicenseDTO.IssuedUsingLocalLicenseID;

            _unitOfWork.InternationalLicenses.Update(internationalLicense);
            await _unitOfWork.CommitAsync();

            return Ok(internationalLicense);
        }




        [SwaggerOperation(Summary = "Delete an international license",
            Description = "Delete an international license in the database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(204)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var internationalLicense = await _unitOfWork.InternationalLicenses.GetByIdAsync(id);

            if (internationalLicense is null)
            {
                return NotFound($"There isn't any international license with id {id}");
            }

            _unitOfWork.InternationalLicenses.Delete(internationalLicense);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
