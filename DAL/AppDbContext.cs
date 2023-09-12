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
        //public DbSet<Voting> Votings { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public AppDbContext() => Database.EnsureCreated();
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
         { 
            Database.EnsureCreated();
         }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Data Source=codeCupDb.db";

            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
        }
    }
}