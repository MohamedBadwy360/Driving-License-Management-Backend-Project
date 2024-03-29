using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.TestTypeDTOs
{
    public class CreateTestTypeDTO
    {
        [StringLength(100)]
        public string TestTypeTitle { get; set; }

        [StringLength(500)]
        public string TestTypeDescription { get; set; }

        public decimal TestTypeFees { get; set; }
    }
}
