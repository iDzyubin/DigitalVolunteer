using System;

namespace DigitalVolunteer.Core.DomainModels
{
    public class Account
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }

        public Rating Rating { get; set; } = new Rating{LikeCount = 100, DislikeCount = 10};
    }
}