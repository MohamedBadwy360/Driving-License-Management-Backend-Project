using DLMS.Core;
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






    }
}
