using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.DetainedLicenseDTOs
{
    public class UpdateDetainedLicenseDTO
    {
        public bool IsReleased { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? ReleaseApplicationID { get; set; }
    }
}
