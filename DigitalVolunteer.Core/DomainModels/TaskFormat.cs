using System.ComponentModel.DataAnnotations;

namespace DigitalVolunteer.Core.DomainModels
{
    public enum TaskFormat
    {
        [Display( Name = "Можно выполнить удаленно" )]
        Freelance,

        [Display( Name = "Нужно присутствие по адресу" )]
        UpWork
    }
}