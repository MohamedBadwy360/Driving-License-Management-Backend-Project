using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.TestAppointmentDTOs
{
    public class UpdateTestAppointmentDTO
    {
        public int TestTypeID { get; set; }

        public int LocalDrivingLicenseApplicationID { get; set; }

        public DateTime AppointmentDate { get; set; }

        public decimal PaidFees { get; set; }

        public bool IsLocked { get; set; }

        public int? RetakeTestApplicationID { get; set; }
    }
}
