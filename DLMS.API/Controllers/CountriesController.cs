using DLMS.Core;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get country by Id", Description = "Retrieve country from database by Id")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(Country), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var country = await _unitOfWork.Countries.GetByIdAsync(id);

            if (country is null)
            {
                return BadRequest($"There isn't any country with this Id {id}");
            }

            return Ok(country);
        }




        [SwaggerOperation(Summary = "Get all countries")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var countries = await _unitOfWork.Countries.GetAllAsync();

            if (countries is null)
            {
                return NotFound($"There isn't any country yet.");
            }

            return Ok(countries);
        }
    }
}
