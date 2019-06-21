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

        public TaskRepository( MainDb db )
        {
            _db = db;
        }

        public void Add( DigitalTask item, Guid ownerId )
        {
            (item.Id, item.OwnerId) = (Guid.NewGuid(), ownerId);
            _db.Insert( item );
        }

        public void Update( DigitalTask item ) => _db.Update( item );
        public void Remove( Guid id ) => _db.DigitalTasks.Delete( u => u.Id == id );

        // TODO. Переделать позднее.
        public DigitalTask Get( Guid id )
        {
            var task = _db.DigitalTasks.Find( id );
            if( task != null )
            {
                task.Owner = _db.Users.Find( task.OwnerId );
                task.Executor = task.ExecutorId.HasValue
                    ? _db.Users.Find( task.ExecutorId.Value )
                    : null;
            }
            return task;
        }

        public List<DigitalTask> Get( Func<DigitalTask, bool> filter ) => _db.DigitalTasks.Where( filter ).ToList();

        // TODO. Переделать позднее.
        public List<DigitalTask> GetAll()
        {
            var tasks = _db.DigitalTasks.ToList();
            tasks.ForEach( task => task.Owner = _db.Users.Find( task.OwnerId ) );
            return tasks;
        }

        public List<DigitalTask> GetUserTasks( Guid userId )
        {
            return _db.TaskExecutors.Where( t => t.UserId == userId )
                .Select( t => t.Task ).ToList();
        }
        public List<DigitalTask> GetUserTasks( Guid userId, Func<DigitalTask, bool> filter )
        {
            return _db.TaskExecutors.Where( t => t.UserId == userId )
                .Select( t => t.Task ).Where( filter ).ToList();
        }

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


        public void Add( DigitalTask item ) => throw new NotImplementedException();


        public List<DigitalTask> GetAll( Guid categoryId )
        {
            var tasks = _db.DigitalTasks.Where( x => x.CategoryId == categoryId ).ToList();
            tasks.ForEach( task => task.Owner = _db.Users.Find( task.OwnerId ) );
            return tasks;
        }
    }
}