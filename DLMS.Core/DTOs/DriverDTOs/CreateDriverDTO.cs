using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.DriverDTOs
{
    public class CreateDriverDTO
    {
        public int PersonID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
