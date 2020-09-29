using System;
using Microsoft.EntityFrameworkCore;
using RAP.Persistence.Primary.Entities;

namespace RAP.Persistence.Primary
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<PostRecurrenceEntity> PostRecurrences { get; set; }
        public DbSet<RedditUserEntity> RedditUsers { get; set; }
    }
}
