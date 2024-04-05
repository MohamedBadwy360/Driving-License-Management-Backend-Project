using DLMS.Core.DTOs.TestAppointmentDTOs;

namespace DLMS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestAppointmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestAppointmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [SwaggerOperation(Summary = "Get a test appointment by id",
            Description = "Get a test appointment by id from database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(TestAppointment), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var testAppointment = await _unitOfWork.TestAppointments.GetByIdAsync(id);

            if (testAppointment is null)
            {
                return NotFound($"There isn't any test appointment with id {id}");
            }

            return Ok(testAppointment);
        }




        [SwaggerOperation(Summary = "Get all test appointments",
            Description = "Get all tests appointmentfrom database.")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(IEnumerable<TestAppointment>), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var testAppointments = await _unitOfWork.TestAppointments.GetAllAsync();

            if (testAppointments is null)
            {
                return NotFound($"There isn't any test appointment in database yet.");
            }

            return Ok(testAppointments);
        }





        [SwaggerOperation(Summary = "Create a test appointment",
            Description = "Create a test appointment in database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(TestAppointment), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateTestAppointmentDTO createTestAppointmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isValidTestType = await _unitOfWork.TestAppointments.AnyAsync(tt => tt.TestTypeID == 
            createTestAppointmentDTO.TestTypeID);

            if (! isValidTestType)
            {
                return NotFound($"There isn't any test type with id {createTestAppointmentDTO.TestTypeID}");
            }

            bool isValidLocalDrivingLicenseApplication = await _unitOfWork.LocalDrivingLicenseApplications
                .AnyAsync(l => l.LocalDrivingLicenseApplicationID 
                == createTestAppointmentDTO.LocalDrivingLicenseApplicationID);

            if (!isValidLocalDrivingLicenseApplication)
            {
                return NotFound($"There isn't any local driving license application with id {createTestAppointmentDTO.LocalDrivingLicenseApplicationID}");
            }

            bool isValidRetakeTestApplication = await _unitOfWork.Applications.AnyAsync(a => a.ApplicationID
                == createTestAppointmentDTO.RetakeTestApplicationID);

            if ((!isValidRetakeTestApplication) 
                && (createTestAppointmentDTO.RetakeTestApplicationID is not null))
            {
                return NotFound($"There isn't any application with id {createTestAppointmentDTO.RetakeTestApplicationID}");
            }

            bool isRetakeTestApplicationAlreadyExists = await _unitOfWork.TestAppointments.AnyAsync(t =>
                t.RetakeTestApplicationID == createTestAppointmentDTO.RetakeTestApplicationID);

            if (isRetakeTestApplicationAlreadyExists 
                && createTestAppointmentDTO.RetakeTestApplicationID is not null)
            {
                return BadRequest($"retake test application already exists.");
            }

            TestAppointment testAppointment = new TestAppointment
            {
                RetakeTestApplicationID = createTestAppointmentDTO.RetakeTestApplicationID,
                TestTypeID = createTestAppointmentDTO.TestTypeID,
                AppointmentDate = createTestAppointmentDTO.AppointmentDate,
                IsLocked = createTestAppointmentDTO.IsLocked,
                LocalDrivingLicenseApplicationID = createTestAppointmentDTO.LocalDrivingLicenseApplicationID,
                PaidFees = createTestAppointmentDTO.PaidFees
            };

            await _unitOfWork.TestAppointments.AddAsync(testAppointment);
            await _unitOfWork.CommitAsync();

            return Ok(testAppointment);
        }





        [SwaggerOperation(Summary = "Update a test appointment",
            Description = "Update a test appointment in database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(TestAppointment), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateTestAppointmentDTO updateTestAppointmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var testAppointment = await _unitOfWork.TestAppointments.GetByIdAsync(id);

            if (testAppointment is null)
            {
                return NotFound($"There isn't any test appointment with id {id}");
            }

            bool isValidTestType = await _unitOfWork.TestAppointments.AnyAsync(tt => tt.TestTypeID ==
            updateTestAppointmentDTO.TestTypeID);

            if (!isValidTestType)
            {
                return NotFound($"There isn't any test type with id {updateTestAppointmentDTO.TestTypeID}");
            }

            bool isValidLocalDrivingLicenseApplication = await _unitOfWork.LocalDrivingLicenseApplications
                .AnyAsync(l => l.LocalDrivingLicenseApplicationID
                == updateTestAppointmentDTO.LocalDrivingLicenseApplicationID);

            if (!isValidLocalDrivingLicenseApplication)
            {
                return NotFound($"There isn't any local driving license application with id {updateTestAppointmentDTO.LocalDrivingLicenseApplicationID}");
            }

            bool isValidRetakeTestApplication = await _unitOfWork.Applications.AnyAsync(a => a.ApplicationID
                == updateTestAppointmentDTO.RetakeTestApplicationID);

            if (!isValidRetakeTestApplication)
            {
                return NotFound($"There isn't any application with id {updateTestAppointmentDTO.RetakeTestApplicationID}");
            }

            testAppointment.LocalDrivingLicenseApplicationID = updateTestAppointmentDTO.LocalDrivingLicenseApplicationID;
            testAppointment.RetakeTestApplicationID = updateTestAppointmentDTO.RetakeTestApplicationID;
            testAppointment.AppointmentDate = updateTestAppointmentDTO.AppointmentDate;
            testAppointment.IsLocked = updateTestAppointmentDTO.IsLocked;
            testAppointment.PaidFees = updateTestAppointmentDTO.PaidFees;
            testAppointment.TestTypeID = updateTestAppointmentDTO.TestTypeID;

            _unitOfWork.TestAppointments.Update(testAppointment);
            await _unitOfWork.CommitAsync();

            return Ok(testAppointment);
        }




        [Authorize(Roles = RoleTypes.Admin)]
        [SwaggerOperation(Summary = "Delete a test appointment",
            Description = "Delete a test appointment from database")]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(204)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var testAppointment = await _unitOfWork.TestAppointments.GetByIdAsync(id);

            if (testAppointment is null)
            {
                return NotFound($"There isn't any test appointment with id {id}");
            }

            _unitOfWork.TestAppointments.Delete(testAppointment);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
