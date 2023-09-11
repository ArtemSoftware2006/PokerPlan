using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Новая_папка.Models;

namespace Новая_папка
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public AppDbContext() => Database.EnsureCreated();
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
         { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Data Source=codeCupDb.db";

            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));
        }
    }
}