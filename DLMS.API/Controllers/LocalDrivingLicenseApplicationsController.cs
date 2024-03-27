using DLMS.Core;
using DLMS.Core.DTOs.LocalDrivingLicenseApplicationDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalDrivingLicenseApplicationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocalDrivingLicenseApplicationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get local driving license application by id",
            Description = "Get local driving license application with id from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(LocalDrivingLicenseApplication), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var localDrivingLicenseApplication = await _unitOfWork.LocalDrivingLicenseApplications
                .GetByIdAsync(id);

            if (localDrivingLicenseApplication is null)
            {
                return NotFound($"There isn't any local driving license application with id {id}");
            }

            return Ok(localDrivingLicenseApplication);
        }




        [SwaggerOperation(Summary = "Get all local driving license applications",
            Description = "Get all local driving license applications from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<LocalDrivingLicenseApplication>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var localDrivingLicenseApplications = await _unitOfWork.LocalDrivingLicenseApplications
                .GetAllAsync();

            if (localDrivingLicenseApplications is null)
            {
                return NotFound("There isn't any local driving license application in database yet.");
            }

            return Ok(localDrivingLicenseApplications);
        }




        [SwaggerOperation(Summary = "Create a local driving license application",
            Description = "Create a local driving license application in database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(LocalDrivingLicenseApplication), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLocalDrivingLicenseApplicationDTO 
            createLocalDrivingLicenseApplicationDTO)
        {
            bool isValidApplication = await _unitOfWork.Applications.AnyAsync(a =>
                a.ApplicationID == createLocalDrivingLicenseApplicationDTO.ApplicationID);

            if (! isValidApplication)
            {
                return NotFound($"There isn't any application with id {createLocalDrivingLicenseApplicationDTO.ApplicationID}");
            }

            bool isApplicationAlreadyExists = await _unitOfWork.LocalDrivingLicenseApplications.AnyAsync(l =>
                l.ApplicationID == createLocalDrivingLicenseApplicationDTO.ApplicationID);

            if (isApplicationAlreadyExists)
            {
                return BadRequest($"Application already exists.");
            }

            bool isValidLicenseClass = await _unitOfWork.LicenseClasses.AnyAsync(lc => lc.LicenseClassID ==
                createLocalDrivingLicenseApplicationDTO.LicenseClassID);

            if (! isValidLicenseClass)
            {
                return NotFound($"There isn't any license class with id {createLocalDrivingLicenseApplicationDTO.LicenseClassID}");
            }

            LocalDrivingLicenseApplication localDrivingLicenseApplication = new LocalDrivingLicenseApplication
            {
                LicenseClassID = createLocalDrivingLicenseApplicationDTO.LicenseClassID,
                ApplicationID = createLocalDrivingLicenseApplicationDTO.ApplicationID
            };

            await _unitOfWork.LocalDrivingLicenseApplications.AddAsync(localDrivingLicenseApplication);
            await _unitOfWork.CommitAsync();

            return Ok(localDrivingLicenseApplication);
        }




        [SwaggerOperation(Summary = "Update a local driving license application",
            Description = "Update a local driving license application in database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(LocalDrivingLicenseApplication), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateLocalDrivingLicenseApplicationDTO 
               updateLocalDrivingLicenseApplicationDTO)
        {
            var localDrivingLicenseApplication = await _unitOfWork.LocalDrivingLicenseApplications
                .GetByIdAsync(id);

            if (localDrivingLicenseApplication is null)
            {
                return NotFound($"There isn't any local driving license application with id {id}");
            }

            bool isValidApplication = await _unitOfWork.Applications.AnyAsync(a =>
                a.ApplicationID == updateLocalDrivingLicenseApplicationDTO.ApplicationID);

            if (!isValidApplication)
            {
                return NotFound($"There isn't any application with id {updateLocalDrivingLicenseApplicationDTO.ApplicationID}");
            }

            bool isValidLicenseClass = await _unitOfWork.LicenseClasses.AnyAsync(lc => lc.LicenseClassID ==
                updateLocalDrivingLicenseApplicationDTO.LicenseClassID);

            if (!isValidLicenseClass)
            {
                return NotFound($"There isn't any license class with id {updateLocalDrivingLicenseApplicationDTO.LicenseClassID}");
            }

            localDrivingLicenseApplication.ApplicationID = updateLocalDrivingLicenseApplicationDTO.ApplicationID;
            localDrivingLicenseApplication.LicenseClassID = updateLocalDrivingLicenseApplicationDTO.LicenseClassID;

            _unitOfWork.LocalDrivingLicenseApplications.Update(localDrivingLicenseApplication);
            await _unitOfWork.CommitAsync();

            return Ok(localDrivingLicenseApplication);
        }





        [SwaggerOperation(Summary = "Delete a local driving license application",
            Description = "Delete a local driving license application from database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(204)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var localDrivingLicenseApplication = await _unitOfWork.LocalDrivingLicenseApplications
                .GetByIdAsync(id);

            if (localDrivingLicenseApplication is null)
            {
                return NotFound($"There isn't any local driving license application with id {id}");
            }

            _unitOfWork.LocalDrivingLicenseApplications.Delete(localDrivingLicenseApplication);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
