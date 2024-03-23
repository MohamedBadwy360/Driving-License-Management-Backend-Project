using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.DetainedLicenseDTOs
{
    public class CreateDetainedLicenseDTO
    {
        public int LicenseID { get; set; }

        public DateTime DetainDate { get; set; }

        public decimal FineFees { get; set; }

        public bool IsReleased { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? ReleaseApplicationID { get; set; }
    }
}
