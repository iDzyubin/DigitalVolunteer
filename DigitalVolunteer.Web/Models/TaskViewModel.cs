using System.Collections.Generic;
using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Web.Models
{
    public class TaskViewModel
    {
        public TaskSelectorMode SelectorMode { get; set; }
        public List<Task> Tasks { get; set; }
    }
}