using System.Collections.Generic;
using DigitalVolunteer.Core.Models;

namespace DigitalVolunteer.Web.Models
{
    public class ProfileViewModel : UserInfoModel
    {
        public List<TaskTitle> LastTasks { get; set; }
    }
}
