using System;
using System.Collections.Generic;
using DigitalVolunteer.Core.DomainModels;

namespace DigitalVolunteer.Core.Repositories
{
    public class MoqTaskRepository : ITaskRepository
    {
        public void Add( Task item )
        {
            throw new NotImplementedException();
        }

        public void Remove( Guid id )
        {
            throw new NotImplementedException();
        }

        public void Update( Task item )
        {
            throw new NotImplementedException();
        }

        public Task Get( Guid id )
        {
            throw new NotImplementedException();
        }

        public List<Task> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}