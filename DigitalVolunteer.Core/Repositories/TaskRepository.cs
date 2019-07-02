using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using LinqToDB;

namespace DigitalVolunteer.Core.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly MainDb _db;


        public TaskRepository( MainDb db ) => _db = db;


        public void Add( DigitalTask item, Guid ownerId )
        {
            (item.Id, item.OwnerId) = (Guid.NewGuid(), ownerId);
            _db.Insert( item );
        }


        /// <summary>
        /// Обновление информации по задаче.
        /// </summary>
        /// <param name="item"></param>
        public void Update( DigitalTask item )
            => _db.Update( item );


        /// <summary>
        /// Удаление задачи.
        /// </summary>
        /// <param name="id"></param>
        public void Remove( Guid id )
            => _db.DigitalTasks.Delete( u => u.Id == id );


        /// <summary>
        /// Получить задачу по id.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public DigitalTask Get( Guid taskId )
            => _db.DigitalTasks.FirstOrDefault( x => x.Id == taskId );


        /// <summary>
        /// Добавить задачу.
        /// </summary>
        /// <param name="item"></param>
        public void Add( DigitalTask item )
            => throw new NotImplementedException();


        /// <summary>
        /// Получить все задачи.
        /// </summary>
        /// <returns></returns>
        public List<DigitalTask> GetAll()
            => throw new NotImplementedException();


        /// <summary>
        /// Вернуть детальную информацию по задаче.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public DigitalTask GetTaskDetails( Guid taskId )
        {
            var tasks = _db.DigitalTasks.AsQueryable();
            if( !tasks.Any( x => x.Id == taskId ) ) return null;

            if( tasks.First( x => x.Id == taskId ).ExecutorId.HasValue )
            {
                var res1 = (from task in _db.DigitalTasks
                            where task.Id == taskId
                            join owner in _db.Users on task.OwnerId equals owner.Id
                            join executor in _db.Users on task.ExecutorId equals executor.Id
                            select new {task, owner, executor}).First();
                (res1.task.Owner, res1.task.Executor) = (res1.owner, res1.executor);
                return res1.task;
            }
            else
            {
                var res2 = (from task in _db.DigitalTasks
                            where task.Id == taskId
                            join owner in _db.Users on task.OwnerId equals owner.Id
                            select new {task, owner}).First();
                res2.task.Owner = res2.owner;
                return res2.task;
            }
        }


        /// <summary>
        /// Получить все задачи в соответсвие с фильтром.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<DigitalTask> Get( Func<DigitalTask, bool> filter )
            => _db.DigitalTasks.Where( filter ).ToList();


        /// <summary>
        /// Выбрать все задачи для показа карточек заданий.
        /// </summary>
        /// <param name="categoryId">Категория задачи</param>
        /// <returns></returns>
        public List<DigitalTask> GetAll( Guid? categoryId )
        {
            var tasks = _db.DigitalTasks.AsQueryable();
            if( categoryId.HasValue ) tasks = tasks.Where( t => t.CategoryId == categoryId );

            return ( from task in tasks
                     join owner in _db.Users on task.OwnerId equals owner.Id
                     select new DigitalTask
                     {
                         Id = task.Id,
                         Title = task.Title,
                         TaskFormat = task.TaskFormat,
                         StartDate = task.StartDate,
                         EndDate = task.EndDate,
                         Owner = owner
                     } ).ToList();
        }


        /// <summary>
        /// Выбрать задачи выбранного пользователя.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DigitalTask> GetUserTasks( Guid userId )
            => _db.TaskExecutors.Where( x => x.UserId == userId ).Select( x => x.Task ).ToList();


        // TODO. Expression tree.
        /// <summary>
        /// Выбрать задачи выбранного пользователя по критерию.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<DigitalTask> GetUserTasks( Guid userId, Func<DigitalTask, bool> filter )
        {
            return _db.TaskExecutors.Where( t => t.UserId == userId )
                .Select( t => t.Task ).Where( filter ).ToList();
        }

        public List<DigitalTask> GetMyTasks( Guid userId, TaskSelectorMode selectorMode, Guid? categoryId )


        /// <summary>
        /// Вернуть список задач пользователя,
        /// в зависимости от выбранных критериев.
        /// </summary>
        /// <param name="userId">Пользователь</param>
        /// <param name="selectorMode">Параметр выборки</param>
        /// <param name="categoryId">Категория задачи</param>
        /// <returns></returns>
        public List<DigitalTask> GetMyTasks( Guid userId, TaskSelectorMode selectorMode, Guid? categoryId )
        {
            var tasks = _db.DigitalTasks.AsQueryable();
            if( categoryId.HasValue ) tasks = tasks.Where( x => x.CategoryId == categoryId );

            switch( selectorMode )
            {
                case TaskSelectorMode.All:
                    tasks = tasks.Where( x => x.OwnerId == userId || x.ExecutorId == userId ); break;
                case TaskSelectorMode.Executor:
                    tasks = tasks.Where( x => x.ExecutorId == userId ); break;
                case TaskSelectorMode.Owner:
                    tasks = tasks.Where( x => x.OwnerId == userId ); break;
            }
            var joined = tasks.Join( _db.Users, t => t.OwnerId, u => u.Id, ( task, user ) => new { Task = task, Owner = user } ).ToList();
            joined.ForEach( j => j.Task.Owner = j.Owner );
            return joined.Select( j => j.Task ).ToList();
        }
    }
}