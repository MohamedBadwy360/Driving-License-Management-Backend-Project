using DLMS.Core;
using DLMS.Core.DTOs.PersonDTO;
using DLMS.Core.Models;
using DLMS.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PeopleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [SwaggerOperation(Summary = "Get person by Id", Description = "Get person by Id from the database")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var person = await _unitOfWork.People.GetByIdAsync(id);

            if (person is not null)
            {
                return Ok(person);
            }
            else
            {
                return NotFound($"Person with Id {id} is not found.");
            }
        }



        [SwaggerOperation(Summary = "Get list of people", Description = "Get list of people from the database")]
        [ProducesResponseType(typeof(IEnumerable<Person>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var people = await _unitOfWork.People.GetAllAsync();

            if (people is not null)
            {
                return Ok(people);
            }
            else
            {
                return NotFound("There isn't any person yet.");
            }
        }



        [SwaggerOperation(Summary = "Add person", Description = "Add person to the database")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(AddPersonDTO), 200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost]
        public async Task<IActionResult> AddAsync(AddPersonDTO addPersonDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isValidCountryId = await _unitOfWork.Countries.AnyAsync(country => country.CountryID
                    == addPersonDTO.NationalityCountryID);

            if (!isValidCountryId)
            {
                return BadRequest($"No Country was found with Id {addPersonDTO.NationalityCountryID}");
            }

            var isNotValidNationalNo = await _unitOfWork.People.AnyAsync(p => p.NationalNo 
                    == addPersonDTO.NationalNo);

            if (isNotValidNationalNo)
            {
                return BadRequest($"There is another person wiht NationalNo {addPersonDTO.NationalNo}");
            }

            if (addPersonDTO.Gendor < 0 || addPersonDTO.Gendor > 1)
            {
                return BadRequest("Incorrect Gendor");
            }


            Person person = new Person
            {
                FirstName = addPersonDTO.FirstName,
                LastName = addPersonDTO.LastName,
                SecondName = addPersonDTO.SecondName,
                ThirdName = addPersonDTO.ThirdName,
                Gendor = addPersonDTO.Gendor,
                Address = addPersonDTO.Address,
                NationalityCountryID = addPersonDTO.NationalityCountryID,
                DateOfBirth = addPersonDTO.DateOfBirth,
                Email = addPersonDTO.Email,
                ImagePath = addPersonDTO.ImagePath,
                NationalNo = addPersonDTO.NationalNo,
                Phone = addPersonDTO.Phone
            };

            await _unitOfWork.People.AddAsync(person);
            await _unitOfWork.CommitAsync();

            return Ok(addPersonDTO);
        }



        [SwaggerOperation(Summary = "Update person", Description = "Update person in the database.")]
        [ProducesResponseType(typeof(UpdatePersonDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, UpdatePersonDTO updatePersonDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = await _unitOfWork.People.GetByIdAsync(id);

            if (person is null)
            {
                return BadRequest($"No person with Id {id}");
            }

            person.Address = updatePersonDTO.Address;
            person.Phone = updatePersonDTO.Phone;
            person.Email = updatePersonDTO.Email;

            _unitOfWork.People.Update(person);
            await _unitOfWork.CommitAsync();

            return Ok(updatePersonDTO);
        }




        [SwaggerOperation(Summary = "Delete person", Description = "Delete person from the database")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(200)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var person = await _unitOfWork.People.GetByIdAsync(id);

            if (person is null)
            {
                return BadRequest($"No person found with Id {id}");
            }

            _unitOfWork.People.Delete(person);
            await _unitOfWork.CommitAsync();

            return Ok();
        }
    }
}
