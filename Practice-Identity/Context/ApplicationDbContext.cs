using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Practice_Identity.Context
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public class ApplicationDbContext : IdentityDbContext<IdentityUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            public DbSet<User> Users => Set<User>();
        }
    }
}
