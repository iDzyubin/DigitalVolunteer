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

        public List<DigitalTask> GetMyTasks( Guid userId, TaskSelectorMode selectorMode )
        {
            switch( selectorMode )
            {
                case TaskSelectorMode.All:
                    return _db.DigitalTasks.Where( x => x.OwnerId == userId || x.ExecutorId == userId ).ToList();
                case TaskSelectorMode.Executor:
                    return _db.DigitalTasks.Where( x => x.ExecutorId == userId ).ToList();
                case TaskSelectorMode.Owner:
                    return _db.DigitalTasks.Where( x => x.OwnerId == userId ).ToList();
            }
            return new List<DigitalTask>();
        }

        public void Add( DigitalTask item ) => throw new NotImplementedException();
    }
}