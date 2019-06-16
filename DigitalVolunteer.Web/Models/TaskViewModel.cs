using System.Collections.Generic;
using DigitalVolunteer.Core.DataModels;

namespace DigitalVolunteer.Web.Models
{
    public class TaskViewModel
    {
        public TaskSelectorMode SelectorMode { get; set; }
        public List<DigitalTask> Tasks { get; set; }
    }
}