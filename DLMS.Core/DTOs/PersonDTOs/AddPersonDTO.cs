using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.DTOs.PersonDTO
{
    public class AddPersonDTO
    {
        [StringLength(20)]
        public string NationalNo { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string SecondName { get; set; }

        [StringLength(20)]
        public string ThirdName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public byte Gendor { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int NationalityCountryID { get; set; }

        [StringLength(250)]
        public string ImagePath { get; set; }
    }
}
