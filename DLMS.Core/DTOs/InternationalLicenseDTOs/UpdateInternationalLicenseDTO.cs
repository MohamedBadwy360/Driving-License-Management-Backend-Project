using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.InternationalLicenseDTOs
{
    public class UpdateInternationalLicenseDTO
    {
        public int ApplicationID { get; set; }

        public int DriverID { get; set; }

        public int IssuedUsingLocalLicenseID { get; set; }

        public bool IsActive { get; set; }
    }
}
