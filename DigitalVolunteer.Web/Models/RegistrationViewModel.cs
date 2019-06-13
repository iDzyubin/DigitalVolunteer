using System.ComponentModel.DataAnnotations;

namespace DigitalVolunteer.Web.Models
{
    public class RegistrationViewModel
    {
        [Required( ErrorMessage = "Не указан Email" )]
        [DataType( DataType.EmailAddress )]
        public string Email { get; set; }

        [Required( ErrorMessage = "Не указан пароль" )]
        [MinLength( 6, ErrorMessage = "Пароль должен быть не короче {1} символов" )]
        [DataType( DataType.Password )]
        public string Password { get; set; }

        [Compare( "Password", ErrorMessage = "Пароль введен неверно" )]
        [DataType( DataType.Password )]
        public string ConfirmPassword { get; set; }
    }
}
