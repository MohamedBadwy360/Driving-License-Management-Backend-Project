using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.LicenseClassDTOs
{
    public class CreateLicenseClass
    {
        [StringLength(50)]
        public string ClassName { get; set; }

        [StringLength(500)]
        public string ClassDescription { get; set; }

        /// <summary>
        /// Minmum age allowed to apply for this license
        /// </summary>
        public byte MinimumAllowedAge { get; set; }

        /// <summary>
        /// How many years the licesnse will be valid.
        /// </summary>
        public byte DefaultValidityLength { get; set; }

        public decimal ClassFees { get; set; }
    }
}
