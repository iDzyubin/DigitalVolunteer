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

    public enum TaskFormat
    {
        [Display( Name = "Можно выполнить удаленно" )]
        Freelance = 0,
        [Display( Name = "Нужно присутствие по адресу" )]
        UpWork = 1
    }
}
