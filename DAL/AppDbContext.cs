using Domain.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public AppDbContext() => Database.EnsureCreated();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
         { 
            Database.EnsureCreated();
         }
    }
}