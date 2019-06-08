using System;

namespace DigitalVolunteer.Core.DomainModels
{
    public class Executor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Rating Rating { get; set; }
    }
}