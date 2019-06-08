using System;

namespace DigitalVolunteer.Web.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TaskFormat { get; set; }
        public string Shedule { get; set; }
        public AccountModel Owner { get; set; }
    }
}