﻿using Microsoft.EntityFrameworkCore;
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
    public DbSet<SetPasswordToken> SetPasswordTokens { get; set; }
    public DbSet<Category> Categories { get; set; }

    // Override OnModelCreating if we need it
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Model configurations...

        modelBuilder.Entity<SetPasswordToken>()
            .HasIndex(x => x.Token);

        modelBuilder.Entity<Exhibit>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Exhibits)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull); // Configure the foreign key to be set to null upon deletion of the Category
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AuditableInterceptor());

        base.OnConfiguring(optionsBuilder);
    }
}
