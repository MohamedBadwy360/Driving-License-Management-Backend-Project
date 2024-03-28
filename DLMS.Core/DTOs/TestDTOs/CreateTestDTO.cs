using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.TestDTOs
{
    public class CreateTestDTO
    {
        public int TestAppointmentID { get; set; }

        /// <summary>
        /// 0 - Fail 1-Pass
        /// </summary>
        public bool TestResult { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
