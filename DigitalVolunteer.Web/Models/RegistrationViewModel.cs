using System.ComponentModel.DataAnnotations;

namespace DigitalVolunteer.Web.Models
{
    public class RegistrationViewModel
    {
        [Required( ErrorMessage = "Не указан Email" )]
        [EmailAddress( ErrorMessage = "Недействительный e-mail адрес" )]
        public string Email { get; set; }

        [Required( ErrorMessage = "Не указан пароль" )]
        [MinLength( 6, ErrorMessage = "Пароль должен быть не короче {1} символов" )]
        [DataType( DataType.Password )]
        public string Password { get; set; }

        [Compare( "Password", ErrorMessage = "Пароль введен неверно" )]
        [DataType( DataType.Password )]
        public string ConfirmPassword { get; set; }

        [DataType( DataType.Text )]
        public string FirstName { get; set; }

        [DataType( DataType.Text )]
        public string LastName { get; set; }

        [Phone( ErrorMessage = "Недействительный номер телефона" )]
        public string Phone { get; set; }

        public bool IsAdmin { get; set; }
    }
}
