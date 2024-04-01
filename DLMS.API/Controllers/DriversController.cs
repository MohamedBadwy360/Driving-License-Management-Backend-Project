using DLMS.Core;
using DLMS.Core.DTOs.DriverDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DriversController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get driver by Id", 
            Description = "Get driver by Id from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(Driver), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var driver = await _unitOfWork.Drivers.GetByIdAsync(id);

            if (driver is null)
            {
                return NotFound($"There isn't any driver with Id {id}");
            }

            return Ok(driver);
        }




        [SwaggerOperation(Summary = "Get all drivers", 
            Description = "Get all frivers from thw database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<Driver>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var drivers = await _unitOfWork.Drivers.GetAllAsync();

            if (drivers is null)
            {
                return NotFound("There isn't any driver in the database yet.");
            }

            return Ok(drivers);
        }





        [SwaggerOperation(Summary = "Create a driver", 
            Description = "Create a driver in the database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(Driver), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateDriverDTO createDriverDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool validPerson = await _unitOfWork.People.AnyAsync(p => p.PersonID ==
                    createDriverDTO.PersonID);

            if (! validPerson)
            {
                return NotFound($"There isn't any person with Id {createDriverDTO.PersonID}");
            }

            bool isExistingDriver = await _unitOfWork.Drivers.AnyAsync(d => d.PersonID ==
                    createDriverDTO.PersonID);

            if (isExistingDriver)
            {
                return BadRequest($"There is a driver whose person id is {createDriverDTO.PersonID}");
            }

            Driver driver = new Driver
            {
                PersonID = createDriverDTO.PersonID,
                CreatedDate = createDriverDTO.CreatedDate
            };

            await _unitOfWork.Drivers.AddAsync(driver);
            await _unitOfWork.CommitAsync();

            return Ok(driver);
        }





        [SwaggerOperation(Summary = "Update a driver", 
            Description = "Update a driver in the database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(Driver), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateDriverDTO updateDriverDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var driver = await _unitOfWork.Drivers.GetByIdAsync(id);

            if (driver is null)
            {
                return NotFound($"There isn't any driver with id {id}");
            }

            bool validPerson = await _unitOfWork.People.AnyAsync(p => p.PersonID ==
                    updateDriverDTO.PersonID);

            if (!validPerson)
            {
                return NotFound($"There isn't any person with id {updateDriverDTO.PersonID}");
            }

            driver.PersonID = updateDriverDTO.PersonID;
            driver.CreatedDate = updateDriverDTO.CreatedDate;

            _unitOfWork.Drivers.Update(driver);
            await _unitOfWork.CommitAsync();

            return Ok(driver);
        }





        [SwaggerOperation(Summary = "Delete a driver", 
            Description = "Delete a driver by id from the database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(204)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var driver = await _unitOfWork.Drivers.GetByIdAsync(id);

            if (driver is null)
            {
                return NotFound($"There isn't any driver with id {id}");
            }

            _unitOfWork.Drivers.Delete(driver);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
