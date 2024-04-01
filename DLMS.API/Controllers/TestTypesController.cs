using DLMS.Core;
using DLMS.Core.DTOs.TestTypeDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get a test type",
            Description = "Get a test type by id from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(TestType), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var testType = await _unitOfWork.TestTypes.GetByIdAsync(id);

            if (testType is null)
            {
                return NotFound($"There isn't any test type with id {id}");
            }

            return Ok(testType);
        }





        [SwaggerOperation(Summary = "Get all test types",
            Description = "Get all test types from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(TestType), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var testTypes = await _unitOfWork.TestTypes.GetAllAsync();

            if (testTypes is null)
            {
                return NotFound($"There isn't any test type in database yet.");
            }

            return Ok(testTypes);
        }





        [SwaggerOperation(Summary = "Create a test type", 
            Description = "Create a test type in database")]
        [ProducesResponseType(typeof(TestType), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateTestTypeDTO createTestTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isTestTitleAlreadyExists = await _unitOfWork.TestTypes.AnyAsync(t => t.TestTypeTitle ==
                createTestTypeDTO.TestTypeTitle);

            if (isTestTitleAlreadyExists)
            {
                return BadRequest($"Test type already exists.");
            }

            TestType testType = new TestType
            {
                TestTypeDescription = createTestTypeDTO.TestTypeDescription,
                TestTypeTitle = createTestTypeDTO.TestTypeTitle,
                TestTypeFees = createTestTypeDTO.TestTypeFees
            };

            await _unitOfWork.TestTypes.AddAsync(testType);
            await _unitOfWork.CommitAsync();

            return Ok(testType);
        }





        [SwaggerOperation(Summary = "Update a test type",
            Description = "Update a test type in database")]
        [ProducesResponseType(typeof(TestType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(string), 404)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateTestTypeDTO updateTestTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var testType = await _unitOfWork.TestTypes.GetByIdAsync(id);

            if (testType is null)
            {
                return NotFound($"There isn't any test type with id {id}");
            }

            testType.TestTypeDescription = updateTestTypeDTO.TestTypeDescription;
            testType.TestTypeFees = updateTestTypeDTO.TestTypeFees;
            testType.TestTypeTitle = updateTestTypeDTO.TestTypeTitle;

            _unitOfWork.TestTypes.Update(testType);
            await _unitOfWork.CommitAsync();

            return Ok(testType);
        }





        [SwaggerOperation(Summary = "Delete a test type",
            Description = "Delete a test type from database")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 404)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var testType = await _unitOfWork.TestTypes.GetByIdAsync(id);

            if (testType is null)
            {
                return NotFound($"There isn't any test type eith id {id}");
            }

            _unitOfWork.TestTypes.Delete(testType);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
