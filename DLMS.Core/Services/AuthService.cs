using DLMS.Core.Constants;
using DLMS.Core.DTOs.AuthenticationDTOs;
using DLMS.Core.Helpers;
using DLMS.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<Jwt> jwt,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Jwt _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;


        public async Task<AuthenticationDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return new AuthenticationDTO { Message = "Email is already registerd." };
            }
            
            if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return new AuthenticationDTO { Message = "Username is already registered." };
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhoneNumber = registerDTO.PhoneNumber
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                StringBuilder errors = new StringBuilder();

                foreach (var error in result.Errors)
                {
                    errors.Append($"{error.Description} ");
                }

                return new AuthenticationDTO { Message = errors.ToString() };
            }

            await _userManager.AddToRoleAsync(user, RoleTypes.User);

            JwtSecurityToken jwtSecurityToken = await CreateJwtTokenAsync(user);

            return new AuthenticationDTO
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { RoleTypes.User },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName
            };
        }

        public async Task<AuthenticationDTO> LoginAsync(LoginDTO loginDTO)
        {
            AuthenticationDTO authenticationDTO = new AuthenticationDTO();

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                authenticationDTO.Message = "Email or Password is incorrect.";
                return authenticationDTO;
            }

            JwtSecurityToken jwtSecurityToken = await CreateJwtTokenAsync(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authenticationDTO.IsAuthenticated = true;
            authenticationDTO.Email = user.Email;
            authenticationDTO.UserName = user.UserName;
            authenticationDTO.Roles = rolesList.ToList();
            authenticationDTO.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationDTO.ExpiresOn = jwtSecurityToken.ValidTo;

            return authenticationDTO;
        }

        public async Task<string> AddRoleAsync(AddRoleDTO addRoleDTO)
        {
            var user = await _userManager.FindByIdAsync(addRoleDTO.UserId);
            
            if (user is null || !await _roleManager.RoleExistsAsync(addRoleDTO.RoleName))
            {
                return "Invalid UserId or RoleName.";
            }

            if (await _userManager.IsInRoleAsync(user, addRoleDTO.RoleName))
            {
                return "User already assigned to role.";
            }

            IdentityResult result = await _userManager.AddToRoleAsync(user, addRoleDTO.RoleName);

            return (result.Succeeded) ? string.Empty : "Something went wrong.";
        }

        private async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(roleClaims)
            .Union(userClaims);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecurityKey));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwt.ExpiresInHours),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }
    }
}
