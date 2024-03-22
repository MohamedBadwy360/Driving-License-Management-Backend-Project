using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.ApplicationDTOs
{
    public class UpdateApplicationDTO
    {
        public int ApplicationTypeID { get; set; }

        /// <summary>
        /// 1-New 2-Cancelled 3-Completed
        /// </summary>
        public byte ApplicationStatus { get; set; }

        public DateTime LastStatusDate { get; set; }

        public decimal PaidFees { get; set; }
    }
}
