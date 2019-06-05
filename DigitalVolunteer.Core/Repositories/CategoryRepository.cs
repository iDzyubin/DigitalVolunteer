using System;
using System.Collections.Generic;
using System.Linq;
using DigitalVolunteer.Core.Contexts;
using DigitalVolunteer.Core.DomainModels;

namespace DigitalVolunteer.Core.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository( ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
        }

        public void Add( Category item )
        {
            _dbContext.Add( item );
            _dbContext.SaveChanges();
        }

        public void Remove( Guid id )
        {
            _dbContext.Remove( Get( id ) );
            _dbContext.SaveChanges();
        }

        public void Update( Category item )
        {
            _dbContext.Update( item );
            _dbContext.SaveChanges();
        }

        public Category Get( Guid id ) 
            => _dbContext.Categories.Find( id );

        public List<Category> GetAll()
            => _dbContext.Categories.ToList();
    }
}