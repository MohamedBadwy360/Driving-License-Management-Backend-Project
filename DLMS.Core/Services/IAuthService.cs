using DLMS.Core.DTOs.AuthenticationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.Services
{
    public interface IAuthService
    {
        Task<AuthenticationDTO> RegisterAsync(RegisterDTO registerDTO);

        Task<AuthenticationDTO> LoginAsync(LoginDTO loginDTO);

        Task<string> AddRoleAsync(AddRoleDTO addRoleDTO);
    }
}
