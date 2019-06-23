using System;
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


        /// <summary>
        /// Вернуть заголовки задач.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<TaskTitle> GetUserTaskTitles( Guid userId, int count )
            => _tasks.GetUserTasks( userId )
                .OrderByDescending( t => t.StartDate )
                .Take( count )
                .Select( t => new TaskTitle { Id = t.Id, Title = t.Title } )
                .ToList();

        /// <summary>
        /// Вернуть заголовки задач.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<TaskTitle> GetAllUserTaskTitles( Guid? userId )
            => userId.HasValue
                ? _tasks.GetMyTasks( userId.Value, TaskSelectorMode.Owner, null )
                    .Where( x => x.TaskState != DigitalTaskState.Closed )
                    .OrderByDescending( t => t.StartDate )
                    .Select( t => new TaskTitle { Id = t.Id, Title = t.Title } )
                    .ToList()
                : new List<TaskTitle>();


        /// <summary>
        /// Вернуть выполненные задачи пользователя.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DigitalTask> GetCompletedTasks( Guid userId )
            => _tasks.GetUserTasks( userId, t => t.Status == DigitalTaskStatus.Completed );


        /// <summary>
        /// Вернуть число созданных задач.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetCreatedTasksCount( Guid userId )
            => _tasks.Get( t => t.OwnerId == userId ).Count;


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
        /// Вернуть текущее состояние задачи.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public DigitalTaskState GetDigitalTaskState( Guid taskId )
            => _db.DigitalTasks.Find( taskId ).TaskState;


        /// <summary>
        /// Предложить услуги по выполнению задачи.
        /// </summary>
        /// <param name="taskId"> Id задачи </param>
        /// <param name="userId"> Id потенциального исполнителя </param>
        public void OfferHelp( Guid taskId, Guid userId )
        {
            if( IsDigitalTaskOwner( taskId, userId ) ) return;

            _db.DigitalTasks
                .Where( t => t.Id == taskId )
                .Set( t => t.TaskState, DigitalTaskState.Unconfirmed )
                .Set( t => t.ExecutorId, userId )
                .Update();
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

            _db.DigitalTasks
                .Where( t => t.Id == taskId )
                .Set( t => t.TaskState, x => DigitalTaskState.Created )
                .Set( t => t.ExecutorId, x => null )
                .Update();
        }


        /// <summary>
        /// На какое состояние изменить.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="newTaskState"></param>
        private void ChangeTaskState( Guid taskId, DigitalTaskState newTaskState )
            => _db.DigitalTasks
                .Where( t => t.Id == taskId )
                .Set( x => x.TaskState, x => newTaskState )
                .Update();
    }
}
