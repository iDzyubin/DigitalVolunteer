using DigitalVolunteer.Core.DomainModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitalVolunteer.Core.Contexts
{
    public class ApplicationUserDbContext : IdentityDbContext<User>
    {
        public ApplicationUserDbContext( DbContextOptions options ) : base( options )
        {
            Database.EnsureCreated();
        }
    }
}