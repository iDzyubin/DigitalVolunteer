﻿using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Models;

namespace DigitalVolunteer.Core.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _tasks;


        public TaskService( ITaskRepository tasks )
        {
            _tasks = tasks;
        }


        public List<TaskTitle> GetUserTaskTitles( Guid userId, int count )
        {
            return _tasks.GetUserTasks( userId )
                .OrderByDescending( t => t.StartDate )
                .Take( count )
                .Select( t => new TaskTitle { Id = t.Id, Title = t.Title } )
                .ToList();
        }


        public List<DigitalTask> GetCompletedTasks( Guid userId )
        {
            return _tasks.GetUserTasks( userId, t => t.Status == DigitalTaskStatus.Completed );
        }


        public int GetCreatedTasksCount( Guid userId )
        {
            return _tasks.Get( t => t.OwnerId == userId ).Count;
        }


        /// <summary>
        /// Добавить исполнителя на задание.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        public void OfferTask( Guid userId, Guid taskId )
        {
            var task = _tasks.Get(taskId);
            task.ExecutorId = userId;
            _tasks.Update( task );
        }


        /// <summary>
        /// Убрать исполнителя с задания.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        public void CancelOffer( Guid userId, Guid taskId )
        {
            var task = _tasks.Get(taskId);
            if( userId != task.ExecutorId ) return;

            task.ExecutorId = null;
            _tasks.Update( task );
        }
    }
}
