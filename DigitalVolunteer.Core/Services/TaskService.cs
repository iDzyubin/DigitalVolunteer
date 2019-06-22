﻿using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Models;
using LinqToDB;

namespace DigitalVolunteer.Core.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _tasks;
        private readonly MainDb _db;

        public TaskService( ITaskRepository tasks, MainDb db )
        {
            _tasks = tasks;
            _db = db;
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


        /// <summary>
        /// Проверяем, является ли пользователь владельцем задания.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsDigitalTaskOwner( Guid taskId, Guid? userId )
            => userId.HasValue && _tasks.Get( taskId ).OwnerId == userId.Value;


        /// <summary>
        /// Проверяем, является ли пользователь исполнителем задания.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsDigitalTaskExecutor( Guid taskId, Guid? userId )
            => userId.HasValue && _tasks.Get( taskId ).ExecutorId == userId.Value;


        /// <summary>
        /// Проверяем, является ли у задания исполнитель.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool HasIsDigitalTaskExecutor( Guid taskId )
            => _tasks.Get( taskId ).ExecutorId.HasValue;


        /// <summary>
        /// Предложить услуги по выполнению задачи.
        /// </summary>
        /// <param name="taskId"> Id задачи </param>
        /// <param name="userId"> Id потенциального исполнителя </param>
        public void OfferHelp( Guid taskId, Guid userId )
        {
            if( IsDigitalTaskOwner( taskId, userId ) ) return;
            ChangeTaskState( taskId, DigitalTaskState.Unconfirmed );
        }


        /// <summary>
        /// Принять предложение по выполнению задачи.
        /// </summary>
        /// <param name="taskId"> Id задачи </param>
        /// <param name="userId"> Id владельца задачи </param>
        public void ConfirmOffer( Guid taskId, Guid userId )
        {
            if( !IsDigitalTaskOwner( taskId, userId ) ) return;
            ChangeTaskState( taskId, DigitalTaskState.Confirmed );
        }


        /// <summary>
        /// Подтвердить выполнение задачи.
        /// </summary>
        /// <param name="taskId"> Id задачи </param>
        /// <param name="userId"> Id исполнителя задачи </param>
        public void ConfirmComplete( Guid taskId, Guid userId )
        {
            if( !IsDigitalTaskExecutor( taskId, userId ) ) return;
            ChangeTaskState( taskId, DigitalTaskState.Completed );
        }


        /// <summary>
        /// Завершение задачи.
        /// </summary>
        /// <param name="taskId"> Id задачи </param>
        /// <param name="userId"> Id владельца задачи </param>
        public void CloseTask( Guid taskId, Guid userId )
        {
            if( !IsDigitalTaskOwner( taskId, userId ) ) return;
            ChangeTaskState( taskId, DigitalTaskState.Closed );
        }


        /// <summary>
        /// Отмена задачи.
        /// </summary>
        /// <param name="taskId"> Id задачи</param>
        /// <param name="userId"> Id исполнителя или заказчика </param>
        public void CancelTask( Guid taskId, Guid userId )
        {
            var isExecutor = IsDigitalTaskExecutor( taskId, userId );
            var isOwner    = IsDigitalTaskOwner( taskId, userId );
            if( !isOwner && !isExecutor ) return;

            ChangeTaskState( taskId, DigitalTaskState.Created );
        }


        /// <summary>
        /// На какое состояние изменить.
        /// </summary>
        /// <param name="newTaskState"></param>
        private void ChangeTaskState( Guid taskId, DigitalTaskState newTaskState )
            => _db.DigitalTasks
                .Where( t => t.Id == taskId )
                .Set( x => x.TaskState, x => newTaskState );
    }
}
