using DLMS.Core;
using DLMS.Core.DTOs.LicenseDTO;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LicensesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get license by id",
            Description = "Get license by id from database, " +
            "IssueReason: 1-FirstTime, 2-Renew, 3-Replacement for Damaged, 4- Replacement for Lost.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(License), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var license = await _unitOfWork.Licenses.GetByIdAsync(id);
            
            if (license is null)
            {
                return NotFound($"There isn't any license with id {id}");
            }

            return Ok(license);
        }





        [SwaggerOperation(Summary = "Get all licenses", 
            Description = "Get all licenses from database, " +
            "IssueReason: 1-FirstTime, 2-Renew, 3-Replacement for Damaged, 4- Replacement for Lost.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<License>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var licenses = await _unitOfWork.Licenses.GetAllAsync();

            if (licenses is null)
            {
                return NotFound($"There isn't any license in database yet.");
            }

            return Ok(licenses);
        }





        [SwaggerOperation(Summary = "Create a license",
            Description = "Create a license in database, " +
            "IssueReason: 1-FirstTime, 2-Renew, 3-Replacement for Damaged, 4- Replacement for Lost.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(License), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLicenseDTO createLicenseDTO)
        {
            bool isValidApplication = await _unitOfWork.Applications.AnyAsync(a => a.ApplicationID ==
                    createLicenseDTO.ApplicationID);

            if (! isValidApplication)
            {
                return NotFound($"There isn't any application with id {createLicenseDTO.ApplicationID}");
            }

            bool isApplicationInLicenseTable = await _unitOfWork.Licenses.AnyAsync(l =>
                    l.ApplicationID == createLicenseDTO.ApplicationID);

            if (isApplicationInLicenseTable)
            {
                return BadRequest($"The application is already used.");
            }

            bool isValidDriver = await _unitOfWork.Drivers.AnyAsync(d => d.DriverID ==
                    createLicenseDTO.DriverID);

            if (! isValidDriver)
            {
                return NotFound($"There isn't any driver with id {createLicenseDTO.DriverID}");
            }


            bool isValidLicenseCalss = await _unitOfWork.LicenseClasses.AnyAsync(lc => lc.LicenseClassID ==
                    createLicenseDTO.LicenseClass);

            if (! isValidLicenseCalss)
            {
                return NotFound($"There isn't any license class with id {createLicenseDTO.LicenseClass}");
            }

            bool isValidIssueReason = (createLicenseDTO.IssueReason >  0) && (createLicenseDTO.IssueReason < 5);

            if (! isValidIssueReason)
            {
                return BadRequest($"Incorrect issue reason.");
            }

            License license = new License
            {
                ApplicationID = createLicenseDTO.ApplicationID,
                LicenseClass = createLicenseDTO.LicenseClass,
                DriverID = createLicenseDTO.DriverID,
                ExpirationDate = createLicenseDTO.ExpirationDate,
                IsActive = createLicenseDTO.IsActive,
                IssueReason = createLicenseDTO.IssueReason,
                Notes = createLicenseDTO.Notes,
                PaidFees = createLicenseDTO.PaidFees
            };

            await _unitOfWork.Licenses.AddAsync(license);
            await _unitOfWork.CommitAsync();

            return Ok(license);
        }





        [SwaggerOperation(Summary = "Update a license",
            Description = "Update a license in database, " +
            "IssueReason: 1-FirstTime, 2-Renew, 3-Replacement for Damaged, 4- Replacement for Lost.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(License), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateLicenseDTO updateLicenseDTO)
        {
            var license = await _unitOfWork.Licenses.GetByIdAsync(id);

            if (license is null)
            {
                return NotFound($"There isn't any license with id {id}");
            }

            bool isValidApplication = await _unitOfWork.Applications.AnyAsync(a => a.ApplicationID ==
                    updateLicenseDTO.ApplicationID);

            if (!isValidApplication)
            {
                return NotFound($"There isn't any application with id {updateLicenseDTO.ApplicationID}");
            }

            bool isValidDriver = await _unitOfWork.Drivers.AnyAsync(d => d.DriverID ==
                    updateLicenseDTO.DriverID);

            if (!isValidDriver)
            {
                return NotFound($"There isn't any driver with id {updateLicenseDTO.DriverID}");
            }


            bool isValidLicenseCalss = await _unitOfWork.LicenseClasses.AnyAsync(lc => lc.LicenseClassID ==
                    updateLicenseDTO.LicenseClass);

            if (!isValidLicenseCalss)
            {
                return NotFound($"There isn't any license class with id {updateLicenseDTO.LicenseClass}");
            }

            bool isValidIssueReason = (updateLicenseDTO.IssueReason > 0) && (updateLicenseDTO.IssueReason < 5);

            if (!isValidIssueReason)
            {
                return BadRequest($"Incorrect issue reason.");
            }

            license.ApplicationID = updateLicenseDTO.ApplicationID;
            license.LicenseClass = updateLicenseDTO.LicenseClass;
            license.DriverID = updateLicenseDTO.DriverID;
            license.IsActive = updateLicenseDTO.IsActive;
            license.IssueReason = updateLicenseDTO.IssueReason;
            license.PaidFees = updateLicenseDTO.PaidFees;

            _unitOfWork.Licenses.Update(license);
            await _unitOfWork.CommitAsync();

            return Ok(license);
        }





        [SwaggerOperation(Summary = "Delete a license",
                    Description = "Delete a license in database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(204)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var license = await _unitOfWork.Licenses.GetByIdAsync(id);

            if (license is null)
            {
                return NotFound($"There isn't any license with id {id}");
            }

            _unitOfWork.Licenses.Delete(license);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
