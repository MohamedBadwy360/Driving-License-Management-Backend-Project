using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.LicenseDTO
{
    public class CreateLicenseDTO
    {
        public int ApplicationID { get; set; }

        public int DriverID { get; set; }

        public int LicenseClass { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string? Notes { get; set; }

        public decimal PaidFees { get; set; }

        public bool IsActive { get; set; }

        public byte IssueReason { get; set; }
    }
}
