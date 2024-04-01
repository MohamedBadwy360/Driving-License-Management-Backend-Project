using DLMS.Core;
using DLMS.Core.DTOs.LicenseClassDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseClassesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LicenseClassesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }





        [SwaggerOperation(Summary = "Get a license class by id",
            Description = "Get a license class by id.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(LicenseClass), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var licenseClass = await _unitOfWork.LicenseClasses.GetByIdAsync(id);

            if (licenseClass is null)
            {
                return NotFound($"There isn't any license class with id {id}");
            }

            return Ok(licenseClass);
        }





        [SwaggerOperation(Summary = "Get all license classes", 
            Description = "Get all license classes from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<LicenseClass>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var licenseClasses = await _unitOfWork.LicenseClasses.GetAllAsync();

            if (licenseClasses is null)
            {
                return NotFound($"There isn't any license class in database yet.");
            }

            return Ok(licenseClasses);
        }





        [SwaggerOperation(Summary = "Create a license class",
            Description = "Create a license class in database, " +
            "MinimumAllowedAge: Minmum age allowed to apply for this license, " +
            "DefaultValidityLength: How many years the licesnse will be valid.")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(LicenseClass), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLicenseClass createLicenseClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isClassNameExistInDatabase = await _unitOfWork.LicenseClasses.AnyAsync(lc =>
                    lc.ClassName == createLicenseClass.ClassName);

            if (isClassNameExistInDatabase)
            {
                return BadRequest($"This class name '{createLicenseClass.ClassName}' already exists in database.");
            }

            LicenseClass licenseClass = new LicenseClass
            {
                ClassName = createLicenseClass.ClassName,
                ClassDescription = createLicenseClass.ClassDescription,
                ClassFees = createLicenseClass.ClassFees,
                DefaultValidityLength = createLicenseClass.DefaultValidityLength,
                MinimumAllowedAge = createLicenseClass.MinimumAllowedAge
            };

            await _unitOfWork.LicenseClasses.AddAsync(licenseClass);
            await _unitOfWork.CommitAsync();

            return Ok(licenseClass);
        }





        [SwaggerOperation(Summary = "Update a license class",
            Description = "Update a license class in database, " +
            "MinimumAllowedAge: Minmum age allowed to apply for this license, " +
            "DefaultValidityLength: How many years the licesnse will be valid.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(LicenseClass), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateLicenseClassDTO updateLicenseClassDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var licenseClass = await _unitOfWork.LicenseClasses.GetByIdAsync(id);

            if (licenseClass is null)
            {
                return NotFound($"There isn't license class with id {id}");
            }

            licenseClass.DefaultValidityLength = updateLicenseClassDTO.DefaultValidityLength;
            licenseClass.ClassDescription = updateLicenseClassDTO.ClassDescription;
            licenseClass.ClassFees = updateLicenseClassDTO.ClassFees;
            licenseClass.ClassName = updateLicenseClassDTO.ClassName;
            licenseClass.MinimumAllowedAge = updateLicenseClassDTO.MinimumAllowedAge;

            _unitOfWork.LicenseClasses.Update(licenseClass);
            await _unitOfWork.CommitAsync();

            return Ok(licenseClass);
        }




        [SwaggerOperation(Summary = "Delete a license class",
            Description = "Delete a license class from database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(LicenseClass), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var licenseClass = await _unitOfWork.LicenseClasses.GetByIdAsync(id);

            if (licenseClass is null)
            {
                return NotFound($"There isn't any license class with id {id}");
            }

            _unitOfWork.LicenseClasses.Delete(licenseClass);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
