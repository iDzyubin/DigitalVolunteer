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

        public void Add( DigitalTask item ) => _db.Insert( item );
        public void Update( DigitalTask item ) => _db.Update( item );
        public void Remove( Guid id ) => _db.DigitalTasks.Delete( u => u.Id == id );
        public DigitalTask Get( Guid id ) => _db.DigitalTasks.Find( id );
        public List<DigitalTask> Get( Func<DigitalTask, bool> filter ) => _db.DigitalTasks.Where( filter ).ToList();
        public List<DigitalTask> GetAll() => _db.DigitalTasks.ToList();

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
    }
}
