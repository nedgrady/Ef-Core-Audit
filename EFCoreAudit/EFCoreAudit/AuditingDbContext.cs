using EFCoreAudit;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAudit
{
    public class AuditingDbContext : DbContext
    {
        private readonly IUsernameProvider _usernameProvider;

        public DbSet<Employee> Employees { get; set; }

        public AuditingDbContext(DbContextOptions dbContextOptions, IUsernameProvider usernameProvider) : base(dbContextOptions)
        {
            this._usernameProvider = usernameProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .ToTable("Employee", tableBuilder => tableBuilder.IsTemporal());

            // Required e.g. for existing records in the history table that might have been created before the Username was captured
            modelBuilder
                .Entity<Employee>()
                .Property(e => e.Username)
                .HasDefaultValue("Unknown User");

        }

        public override int SaveChanges()
        {
            var thingsThatRequireUsername = ChangeTracker.Entries().Where(entry => entry.Entity is IRequireUsernameToBeCaptured);
            foreach (var entityEntry in thingsThatRequireUsername) // Detects changes automatically
            {
                ((IRequireUsernameToBeCaptured)entityEntry.Entity).Username = _usernameProvider.Username;
            }

            return base.SaveChanges();
        }
    }

    public class Employee : IRequireUsernameToBeCaptured
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Username { get; set; }
    }

}
