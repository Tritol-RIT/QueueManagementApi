﻿

using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Core.Entities;

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
    }
}
