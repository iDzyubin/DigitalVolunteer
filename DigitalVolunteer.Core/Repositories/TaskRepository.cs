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
            item.Id = Guid.NewGuid();
            item.OwnerId = ownerId;
            _db.Insert( item );
        }


        public void Remove( Guid id )  => _db.Tasks.Delete( x => x.Id == id );


        public void Update( Task item ) => _db.Update( item );


        public Task Get( Guid id ) => _db.Tasks.Find( id );


        public List<Task> Get( Func<Task, bool> filter ) => _db.Tasks.Where( filter ).ToList();


        public List<Task> GetAll() => _db.Tasks.ToList();


        public void Add( Task item ) => throw new NotImplementedException();
    }
}