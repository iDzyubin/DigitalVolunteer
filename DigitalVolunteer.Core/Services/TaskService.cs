using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using DigitalVolunteer.Core.Models;
<<<<<<< Updated upstream
=======
using LinqToDB;
using Microsoft.AspNetCore.Mvc.Rendering;
>>>>>>> Stashed changes

namespace DigitalVolunteer.Core.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _tasks;
<<<<<<< Updated upstream
        public TaskService( ITaskRepository taskRepository )
=======
        private readonly MainDb _db;


        public TaskService( ITaskRepository tasks, MainDb db )
>>>>>>> Stashed changes
        {
            _tasks = taskRepository;
        }
        public List<TaskTitle> GetUserTaskTitles( Guid userId, int count )
        {
            return _tasks.GetUserTasks( userId )
                .OrderByDescending( t => t.StartDate )
                .Take( count )
                .Select( t => new TaskTitle { Id = t.Id, Title = t.Title } )
                .ToList();
<<<<<<< Updated upstream
=======


        /// <summary>
        /// Вернуть заголовки задач.
        /// </summary>
        /// <param name="userId"></param>
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
        /// Проверяем, имеется ли у задания исполнитель.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool HasDigitalTaskExecutor( Guid taskId )
            => _tasks.Get( taskId ).ExecutorId.HasValue;


        /// <summary>
        /// Вернуть текущее состояние задачи.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public DigitalTaskState GetDigitalTaskState( Guid taskId )
            => _db.DigitalTasks.Find( taskId ).TaskState;


        /// <summary>
        /// Предложить заказчику услуги по выполнению задачи.
        /// </summary>
        /// <param name="taskId"> Id задачи </param>
        /// <param name="userId"> Id потенциального исполнителя </param>
        public void OfferHelp( Guid taskId, Guid userId )
        {
            if( IsDigitalTaskOwner( taskId, userId ) ) return;
            ChangeTaskState( taskId, DigitalTaskState.WaitingOwnerConfirmation, userId );
        }


        /// <summary>
        /// Предложить задачу потенциальному исполнителю.
        /// </summary>
        /// <param name="taskId"> Id задачи </param>
        /// <param name="userId"> Id заказчика </param>
        public void OfferTask( Guid taskId, Guid userId )
        {
            if( IsDigitalTaskOwner( taskId, userId ) ) return;
            ChangeTaskState( taskId, DigitalTaskState.WaitingExecutorConfirmation, userId );
        }


        /// <summary>
        /// Принять предложение по выполнению задачи.
        /// 1. Если заказчик принимает предложение от потенциального исполнителя.
        /// 2. Потенциальный исполнитель принимает предложение от заказчика.
        /// </summary>
        /// <param name="taskId"> Id задачи </param>
        /// <param name="userId"> Id либо владельца задачи, либо потенциального исполнителя </param>
        public void ConfirmOffer( Guid taskId, Guid userId )
        {
            if( !IsDigitalTaskOwner( taskId, userId ) && !IsDigitalTaskExecutor( taskId, userId ) ) return;
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
>>>>>>> Stashed changes
        }
        public List<DigitalTask> GetCompletedTasks( Guid userId )
        {
            return _tasks.GetUserTasks( userId, t => t.Status == DigitalTaskStatus.Completed );
        }
        public int GetCreatedTasksCount( Guid userId )
        {
<<<<<<< Updated upstream
            return _tasks.Get( t => t.OwnerId == userId ).Count;
        }
=======
            if( !IsDigitalTaskOwner( taskId, userId ) && !IsDigitalTaskExecutor( taskId, userId ) ) return;
            ChangeTaskState( taskId, DigitalTaskState.Created, null );
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

        private void ChangeTaskState( Guid taskId, DigitalTaskState newTaskState, Guid? executorId )
            => _db.DigitalTasks
                .Where( t => t.Id == taskId )
                .Set( t => t.TaskState, x => newTaskState )
                .Set( t => t.ExecutorId, x => executorId )
                .Update();


        /// <summary>
        /// Вернуть список всех задач с минимальной информацией о задачах.
        /// </summary>
        /// <returns></returns>
        public SelectList GetTaskSelectList( Guid? userId )
            => new SelectList( GetAllUserTaskTitles( userId ), "Id", "Title" );
>>>>>>> Stashed changes
    }
}
