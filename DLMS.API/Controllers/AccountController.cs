using DLMS.Core.DTOs.AuthenticationDTOs;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        private readonly IAuthService _authService;




        [SwaggerOperation(Summary = "Register a new user", 
            Description = "Register a new user in the application.")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(AuthenticationDTO), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authenticationDTO = await _authService.RegisterAsync(registerDTO);

            if (!authenticationDTO.IsAuthenticated)
            {
                return BadRequest(authenticationDTO.Message);
            }

            return Ok(authenticationDTO);
        }





        [SwaggerOperation(Summary = "Login a user",
            Description = "Login a user in the application.")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(AuthenticationDTO), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthenticationDTO authenticatedDTO = await _authService.LoginAsync(loginDTO);
            
            if (!authenticatedDTO.IsAuthenticated)
            {
                return BadRequest(authenticatedDTO.Message);
            }

            return Ok(authenticatedDTO);
        }




        [SwaggerOperation(Summary = "Add role to user",
            Description = "Add role to user in the application.")]
        [ProducesResponseType(typeof(AddRoleDTO), 200)]
        [ProducesResponseType(400)]
        [ResponseCache(CacheProfileName = "NoCache")]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync(AddRoleDTO addRoleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string result = await _authService.AddRoleAsync(addRoleDTO);

            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }

            return Ok(addRoleDTO);
        }
    }
}
