using System;

namespace DigitalVolunteer.Core.Models
{
    public class UserInfoModel
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public int TasksCreated { get; set; }
        public int TasksCompleted { get; set; }
        public string FavoriteCategory { get; set; }
    }
}
