using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.ApplicationTypeDTOs
{
    public class CreateApplicationTypeDTO
    {
        [StringLength(150)]
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get; set; }
    }
}
