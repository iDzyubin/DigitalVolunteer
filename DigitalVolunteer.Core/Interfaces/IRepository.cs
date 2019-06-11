using System;
using System.Collections.Generic;

namespace DigitalVolunteer.Core.Interfaces
{
    public interface IRepository<T>
    {
        void Add( T item );

        void Remove( Guid id );

        void Update( T item );

        T Get( Guid id );

        List<T> Get( Func<T, bool> filter );

        List<T> GetAll();
    }
}