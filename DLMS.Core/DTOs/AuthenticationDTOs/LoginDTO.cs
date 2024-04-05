using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.AuthenticationDTOs
{
    public class LoginDTO
    {
        [Required, MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
