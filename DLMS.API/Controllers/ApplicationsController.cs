using DLMS.Core;
using DLMS.Core.DTOs.ApplicationDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get an application by Id", 
            Description = "Get an application by Id from database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(Application), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(id);

            if (application is null)
            {
                return NotFound($"No application with Id {id}");
            }

            return Ok(application);
        }





        [SwaggerOperation(Summary = "Get all applications", 
            Description = "Return all applications from database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<Application>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var applications = await _unitOfWork.Applications.GetAllAsync();

            if (applications is null)
            {
                return NotFound("There isn't any application yet.");
            }

            return Ok(applications);
        }




        [SwaggerOperation(Summary = "Create an application", 
            Description = "Create an application and add it to database, " +
            "ApplicationStatus: 1-New 2-Cancelled 3-Completed")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(CreateApplicationDTO), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateApplicationDTO createApplicationDTO)
        {
            var isValidPersonId = await _unitOfWork.People.AnyAsync(p => p.PersonID
                    == createApplicationDTO.ApplicantPersonID);

            if (! isValidPersonId)
            {
                return BadRequest($"No Person with Id {createApplicationDTO.ApplicantPersonID}");
            }

            var isValidApplicationTypeId = await _unitOfWork.ApplicationTypes.AnyAsync(t => t.ApplicationTypeID
                    == createApplicationDTO.ApplicationTypeID);

            if (! isValidApplicationTypeId)
            {
                return BadRequest($"No ApplicationType with Id {createApplicationDTO.ApplicationTypeID}" +
                    $" ApplicationTypeID should be 1 to 7");
            }

            bool isValidApplicationStatus = createApplicationDTO.ApplicationStatus > 0 &&
                 createApplicationDTO.ApplicationStatus < 4;

            if (!isValidApplicationStatus)
            {
                return BadRequest($"Application Status should be 1-New 2-Cancelled 3-Completed");
            }

            Application application = new Application
            {
                ApplicantPersonID = createApplicationDTO.ApplicantPersonID,
                ApplicationDate = createApplicationDTO.ApplicationDate,
                ApplicationStatus =createApplicationDTO.ApplicationStatus,
                ApplicationTypeID = createApplicationDTO.ApplicationTypeID,
                LastStatusDate = createApplicationDTO.LastStatusDate,
                PaidFees = createApplicationDTO.PaidFees
            };

            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.CommitAsync();

            return Ok(createApplicationDTO);
        }





        [SwaggerOperation(Summary = "Update an application", 
            Description = "Update an application in database, ApplicationStatus: 1-New 2-Cancelled 3-Completed")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(Application), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, UpdateApplicationDTO updateApplicationDTO)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(id);

            if (application is null)
            {
                return BadRequest($"There isn't any application with Id {id}");
            }

            bool isValidApplicationTypeId = await _unitOfWork.ApplicationTypes.AnyAsync(t => t.ApplicationTypeID
                    == updateApplicationDTO.ApplicationTypeID);

            if (!isValidApplicationTypeId)
            {
                return BadRequest($"No ApplicationType with Id {updateApplicationDTO.ApplicationTypeID}" +
                    $" ApplicationTypeID should be 1 to 7");
            }

            bool isValidApplicationStatus = updateApplicationDTO.ApplicationStatus > 0 &&
                 updateApplicationDTO.ApplicationStatus < 4;

            if (! isValidApplicationStatus)
            {
                return BadRequest($"Application Status should be 1-New 2-Cancelled 3-Completed");
            }

            application.ApplicationStatus = updateApplicationDTO.ApplicationStatus;
            application.ApplicationTypeID = updateApplicationDTO.ApplicationTypeID;
            application.LastStatusDate = updateApplicationDTO.LastStatusDate;
            application.PaidFees = updateApplicationDTO.PaidFees;

            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CommitAsync();

            return Ok(application);
        }





        [SwaggerOperation(Summary = "Delete an application", 
            Description = "Delete an application from database by Id")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(200)]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var application =await _unitOfWork.Applications.GetByIdAsync(id);

            if (application is null)
            {
                return BadRequest($"There isn't any application with Id {id}");
            }

            _unitOfWork.Applications.Delete(application);
            await _unitOfWork.CommitAsync();

            return Ok();
        }
    }
}
