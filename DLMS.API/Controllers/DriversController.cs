using DLMS.Core;
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
    }
}
