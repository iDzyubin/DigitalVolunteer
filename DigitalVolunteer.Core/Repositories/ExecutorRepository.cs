using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;

namespace DigitalVolunteer.Core.Repositories
{
    public class ExecutorRepository : IExecutorRepository
    {
        private readonly MainDb _db;


        public ExecutorRepository( MainDb db ) => _db = db;


        public void Add( User item ) 
            => throw new NotImplementedException();


        public void Remove( Guid id ) 
            => throw new NotImplementedException();


        public void Update( User item ) 
            => throw new NotImplementedException();


        public User Get( Guid id )
            => _db.Users.FirstOrDefault( x => x.IsExecutor && x.Id == id );


        public List<User> Get( Func<User, bool> filter )
            => _db.Users.Where( x => x.IsExecutor && filter.Invoke( x ) ).ToList();


        public List<User> GetAll()
            => _db.Users.Where( x => x.IsExecutor ).ToList();
    }
}