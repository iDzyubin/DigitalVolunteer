using Microsoft.AspNetCore.Identity;

namespace DigitalVolunteer.Core.DomainModels
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
//
//        [Display( Name = "Пароль" )] public string Password { get; set; }
//
//        [Display( Name = "Подтвердите пароль" )]
//        public string PasswordConfirm { get; set; }

//        public string FirstName       { get; set; }
//        public string LastName        { get; set; }
//        public string Email           { get; set; }
//        public string City            { get; set; }
//        public string Password        { get; set; }
//        public string ConfirmPassword { get; set; }
    }
}