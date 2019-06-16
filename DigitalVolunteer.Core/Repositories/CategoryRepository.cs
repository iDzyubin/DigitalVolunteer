using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.DataModels;
using DigitalVolunteer.Core.Interfaces;
using LinqToDB;

namespace DigitalVolunteer.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MainDb _db;


        public CategoryRepository( MainDb db ) => _db = db;


        public void Add( Category item )
        {
            item.Id = Guid.NewGuid(); ;
            _db.Insert( item );
        }


        public Category Get( Guid id ) => _db.Categories.Find( id );


        public List<Category> Get( Func<Category, bool> filter ) => _db.Categories.Where( filter ).ToList();


        public List<Category> GetAll() => _db.Categories.ToList();


        public void Remove( Guid id ) => _db.Categories.Delete( x => x.Id == id );


        public void Update( Category item ) => _db.Update( item );
    }
}
