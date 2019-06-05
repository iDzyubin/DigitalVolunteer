using DigitalVolunteer.Core.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace DigitalVolunteer.Core.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        
        public ApplicationDbContext( DbContextOptions options ) : base( options )
        {
        }
    }
}