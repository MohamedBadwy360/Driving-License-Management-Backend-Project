using DLMS.Core;
using DLMS.Core.DTOs.ApplicationTypeDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get application type by Id", 
            Description = "Return application type from database by Id")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(ApplicationType), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var applicationType = await _unitOfWork.ApplicationTypes.GetByIdAsync(id);

            if (applicationType is null)
            {
                return NotFound($"There isn't any application type with Id {id}");
            } 

            return Ok(applicationType);
        }





        [SwaggerOperation(Summary = "Get all application types", 
            Description = "Retrieve all application types from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<ApplicationType>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var applications = await _unitOfWork.ApplicationTypes.GetAllAsync();

            if (applications is null)
            {
                return NotFound($"There isn't any ApplicationType yet.");
            }

            return Ok(applications);
        }




        [SwaggerOperation(Summary = "Create an application type", 
            Description = "Create an application type and add it to database")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(ApplicationType), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateApplicationTypeDTO createApplicationTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isNotValidApplicationTypeTitle = await _unitOfWork.ApplicationTypes.AnyAsync(
                t => t.ApplicationTypeTitle == createApplicationTypeDTO.ApplicationTypeTitle);

            if (isNotValidApplicationTypeTitle)
            {
                return BadRequest($"Not valid application type title. " +
                    $"There is another application type with this title {createApplicationTypeDTO.ApplicationTypeTitle}.");
            }

            ApplicationType applicationType = new ApplicationType
            {
                ApplicationTypeTitle = createApplicationTypeDTO.ApplicationTypeTitle,
                ApplicationFees = createApplicationTypeDTO.ApplicationFees
            };

            await _unitOfWork.ApplicationTypes.AddAsync(applicationType);
            await _unitOfWork.CommitAsync();

            return Ok(applicationType);
        }





        [SwaggerOperation(Summary = "Update an application type",
            Description = "Update an application type in database by Id")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(ApplicationType), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateApplicationTypeDTO updateApplicationTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationType = await _unitOfWork.ApplicationTypes.GetByIdAsync(id);

            if (applicationType is null)
            {
                return BadRequest($"There isn't any application type with Id {id}");
            }

            applicationType.ApplicationTypeTitle = updateApplicationTypeDTO.ApplicationTypeTitle;
            applicationType.ApplicationFees = updateApplicationTypeDTO.ApplicationFees;

            _unitOfWork.ApplicationTypes.Update(applicationType);
            await _unitOfWork.CommitAsync();

            return Ok(applicationType);
        }




        [SwaggerOperation(Summary = "Delete an application type", 
            Description = "Delete an application type from database with Id.")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var applicationType = await _unitOfWork.ApplicationTypes.GetByIdAsync(id);

            if (applicationType is null)
            {
                return BadRequest($"There isn't any application type with Id {id}");
            }

            _unitOfWork.ApplicationTypes.Delete(applicationType);
            await _unitOfWork.CommitAsync();

            return Ok();
        }
    }
}
