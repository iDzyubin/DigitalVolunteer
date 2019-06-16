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


        public void Add( Task item, Guid ownerId )
        {
            (item.Id, item.OwnerId) = (Guid.NewGuid(), ownerId);
            _db.Insert( item );
        }


        public void Remove( Guid id ) => _db.Tasks.Delete( x => x.Id == id );


        public void Update( Task item ) => _db.Update( item );


        // TODO. Переделать позднее.
        public Task Get( Guid id )
        {
            var task = _db.Tasks.Find( id );
            task.Owner = _db.Users.Find( task.OwnerId );
            return task;
        }


        public List<Task> Get( Func<Task, bool> filter ) => _db.Tasks.Where( filter ).ToList();


        // TODO. Переделать позднее.
        public List<Task> GetAll()
        {
            var tasks = _db.Tasks.ToList();
            tasks.ForEach( task => task.Owner = _db.Users.Find( task.OwnerId ) );
            return tasks;
        }


        public void Add( Task item ) => throw new NotImplementedException();
    }
}