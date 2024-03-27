using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.LocalDrivingLicenseApplicationDTOs
{
    public class CreateLocalDrivingLicenseApplicationDTO
    {
        public int ApplicationID { get; set; }

        public int LicenseClassID { get; set; }
    }
}
