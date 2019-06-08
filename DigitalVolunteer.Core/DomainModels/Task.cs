using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalVolunteer.Core.DomainModels
{
    public class Task
    {
        public Guid Id { get; set; }

        [Display( Name = "Наименование задачи" )]
        public string Title { get; set; }

        [Display( Name = "Опишите задачу, которую необходимо выполнить" )]
        public string Description { get; set; }

        [Display( Name = "Контактный телефон" )]
        public string ContactPhone { get; set; }

        [Display( Name = "Дата начала" )]    
        public DateTime?  StartDate  { get; set; }
        
        [Display( Name = "Дата окончания" )] 
        public DateTime?  EndDate    { get; set; }
        
        public TaskFormat TaskFormat { get; set; }

        [Display( Name = "Получать email уведомления о новых предложениях" )]
        public bool HasPushNotification { get; set; }

        [Display( Name = "Показывать мое задание только исполнителям" )]
        public bool IsOnlyForExecutors { get; set; }

        public Account Owner    { get; set; }
        public Account Executor { get; set; }
    }
}