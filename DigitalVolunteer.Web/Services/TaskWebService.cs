using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.Extensions;
using DigitalVolunteer.Core.Repositories;
using DigitalVolunteer.Web.Models;

namespace DigitalVolunteer.Web.Services
{
    public class TaskWebService
    {
        private readonly ITaskRepository _taskRepository;


        public TaskWebService( ITaskRepository taskRepository )
        {
            _taskRepository = taskRepository;
        }


        public IEnumerable<TaskModel> GetTaskList()
            => from task in _taskRepository.GetAll()
               select new TaskModel
               {
                   Id         = task.Id,
                   Title      = task.Title,
                   TaskFormat = task.TaskFormat.GetDisplayName(),
                   Owner      = new AccountModel { Id = task.Owner.Id, Name = task.Owner.GetShortName() },
                   Shedule    = GetScheduleInformation( task.StartDate, task.EndDate )
               };


        private string GetScheduleInformation( DateTime? start, DateTime? end )
        {
            var schedule = String.Empty;
            if( start.HasValue && end.HasValue )
            {
                schedule = $"{start.Value.ConvertToString()} - {end.Value.ConvertToString()}";
            }
            else if( start.HasValue )
            {
                schedule = $"Начать {start.Value.ConvertToString()}";
            }
            else if( end.HasValue )
            {
                schedule = $"Закончить до {end.Value.ConvertToString()}";
            }

            return schedule;
        }
    }
}