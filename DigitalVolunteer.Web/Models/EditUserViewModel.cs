using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalVolunteer.Web.Models
{
    public class EditUserViewModel
    {
        public Guid Id { get; set; }

        [DataType( DataType.Text )]
        public string FirstName { get; set; }

        [DataType( DataType.Text )]
        public string LastName { get; set; }

        [Phone( ErrorMessage = "Недействительный номер телефона" )]
        public string Phone { get; set; }

        public bool IsAdmin { get; set; }
    }
}
