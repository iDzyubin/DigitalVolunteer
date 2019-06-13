using System;

namespace DigitalVolunteer.Core.Models
{
    public class UserInfoModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
