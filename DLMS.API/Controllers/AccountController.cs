using DLMS.Core.DTOs.UserDTOs;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;




        [SwaggerOperation(Summary = "Register a new user", 
            Description = "Register a new user in the application.")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(string), 200)]
        [ResponseCache(CacheProfileName = "Any-60")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(NewUserDTO newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber
                };

                IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);

                if (result.Succeeded)
                {
                    return Ok("Succeeded.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Errors", error.Description);
                    }
                }
            }

            return BadRequest(ModelState);
        }





        [SwaggerOperation(Summary = "Login a user",
            Description = "Login a user in the application.")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await _userManager.FindByNameAsync(loginUser.UserName);

                if (user is not null)
                {
                    if (await _userManager.CheckPasswordAsync(user, loginUser.Password))
                    {
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, loginUser.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await _userManager.GetRolesAsync(user);

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            _configuration["JWT:SecurityKey"]));
                        SigningCredentials signingCredentials = new SigningCredentials(key,
                            SecurityAlgorithms.HmacSha256);

                        var jwtToken = new JwtSecurityToken(
                            claims: claims,
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            signingCredentials: signingCredentials,
                            expires: DateTime.Now.AddHours(double.Parse(_configuration["JWT:ExpiresInHours"]))
                            );

                        var token = new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                            Expiration = jwtToken.ValidTo
                        };

                        return Ok(token);
                    }
                }
                
                ModelState.AddModelError("Errors", "UserName or Password is incorrect.");
            }

            return BadRequest(ModelState);
        }
    }
}
