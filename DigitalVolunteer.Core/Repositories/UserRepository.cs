using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace DigitalVolunteer.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDb _db;
        private readonly ILogger _logger;

        public UserRepository( MainDb db, ILogger<UserRepository> logger )
        {
            _db = db;
            _logger = logger;
        }

        public void Add( User item ) => _db.Insert( item );
        public void Update( User item ) => _db.Update( item );
        public void Remove( Guid id ) => _db.Users.Delete( u => u.Id == id );
        public User Get( Guid id ) => _db.Users.Find( id );
        public User GetByEmail( string email ) => Get( u => u.Email == email ).FirstOrDefault();
        public List<User> Get( Func<User, bool> filter ) => _db.Users.Where( filter ).ToList();
        public List<User> GetAll() => _db.Users.ToList();
    }
}