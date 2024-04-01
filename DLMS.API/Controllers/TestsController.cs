using DLMS.Core;
using DLMS.Core.DTOs.TestDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get test by id",
            Description = "Get test by id from database, TestResult: 0 - Fail 1-Pass")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(Test), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var test = await _unitOfWork.Tests.GetByIdAsync(id);

            if (test is null)
            {
                return NotFound($"There isn't any test with id {id}");
            }

            return Ok(test);
        }




        [SwaggerOperation(Summary = "Get all tests", 
            Description = "Get all tests from database, TestResult: 0 - Fail 1-Pass")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<Test>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var tests = await _unitOfWork.Tests.GetAllAsync();

            if (tests is null)
            {
                return NotFound($"There isn't any test in database yet.");
            }

            return Ok(tests);
        }





        [SwaggerOperation(Summary = "Create a test",
            Description = "Create a test in database, TestResult: 0 - Fail 1-Pass")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(Test), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateTestDTO createTestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isValidTestAppointment = await _unitOfWork.TestAppointments.AnyAsync(t => t.TestAppointmentID
                == createTestDTO.TestAppointmentID);

            if (! isValidTestAppointment)
            {
                return NotFound($"There isn't any test appointment with id {createTestDTO.TestAppointmentID}");
            }

            bool isTestAppointmentAlreadyExists = await _unitOfWork.Tests.AnyAsync(t => t.TestAppointmentID ==
                createTestDTO.TestAppointmentID);

            if (isTestAppointmentAlreadyExists)
            {
                return BadRequest($"Test appointment already exist.");
            }

            Test test = new Test
            {
                TestAppointmentID = createTestDTO.TestAppointmentID,
                TestResult = createTestDTO.TestResult,
                Notes = createTestDTO.Notes
            };

            await _unitOfWork.Tests.AddAsync(test);
            await _unitOfWork.CommitAsync();

            return Ok(test);
        }





        [SwaggerOperation(Summary = "Update a test",
            Description = "Update a test in database, TestResult: 0 - Fail 1-Pass")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(Test), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateTestDTO updateTestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var test = await _unitOfWork.Tests.GetByIdAsync(id);

            if (test is null)
            {
                return NotFound($"There isn't any test with id {id}");
            }

            test.TestResult = updateTestDTO.TestResult;
            test.Notes = updateTestDTO.Notes;

            _unitOfWork.Tests.Update(test);
            await _unitOfWork.CommitAsync();

            return Ok(test);
        }





        [SwaggerOperation(Summary = "Delete a test",
            Description = "Delete a test from database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(204)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var test = await _unitOfWork.Tests.GetByIdAsync(id);

            if (test is null)
            {
                return NotFound($"There isn't any test with id {id}");
            }

            _unitOfWork.Tests.Delete(test);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
