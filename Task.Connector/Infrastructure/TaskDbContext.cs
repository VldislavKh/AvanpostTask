﻿using Microsoft.EntityFrameworkCore;
using Task.Integration.Data.DbCommon.DbModels;

namespace Task.Connector.Infrastructure;

public class TaskDbContext : DbContext
{
    
    public TaskDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }

    public DbSet<ITRole> Roles { get; set; }

    public DbSet<UserRequestRight> UserRequestRights { get; set; }

    public DbSet<UserITRole> UserITRoles { get; set; }

    public DbSet<RequestRight> RequestRights { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseNpgsql(_connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRequestRight>()
            .HasKey(x => new { x.UserId, x.RightId });

        modelBuilder.Entity<UserITRole>()
            .HasKey(x => new {x.UserId, x.RoleId});
    }
}