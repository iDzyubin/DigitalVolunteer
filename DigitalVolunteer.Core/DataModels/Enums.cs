using System.ComponentModel.DataAnnotations;

namespace DigitalVolunteer.Core.DataModels
{
    public enum UserStatus
    {
        Unknown = 0,
        Unconfirmed = 1,
        Confirmed = 2,
        Deleted = 3
    }
    
    public enum DigitalTaskStatus
    {
        Open = 0,
        Completed = 1,
        Canceled = 2
    }

    public enum DigitalTaskFormat
    {
        [Display( Name = "Удаленная работа" )]
        Freelance = 0,
        [Display( Name = "Присутствие на месте" )]
        UpWork = 1
    }

    public enum TaskSelectorMode
    {
        [Display( Name = "Все задания" )]
        All = 0,
        [Display( Name = "Я исполнитель" )]
        Executor = 1,
        [Display( Name = "Я заказчик" )]
        Owner = 2
    }
}
