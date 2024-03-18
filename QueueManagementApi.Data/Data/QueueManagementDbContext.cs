

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Infrastructure.Interceptors;

namespace QueueManagementApi.Infrastructure.Data;

public class QueueManagementDbContext : DbContext
{
    public QueueManagementDbContext(DbContextOptions<QueueManagementDbContext> options) : base(options)
    {
    }

    // DbSets for entities
    public DbSet<Exhibit> Exhibits { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<Visitor> Visitors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<ExhibitImage> ExhibitImages { get; set; }
    public DbSet<Insurance> Insurances { get; set; }

    // Override OnModelCreating if we need it
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Model configurations...

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(e => e.Exhibit) // Specifies the navigation property to the Exhibit entity
                .WithMany(x => x.Users) // Exhibit does not have a navigation property back to User
                .HasForeignKey(e => e.ExhibitId) // Specifies the foreign key property in the User entity
                .IsRequired(false); // Makes the foreign key optional
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AuditableInterceptor());

        base.OnConfiguring(optionsBuilder);
    }
}
