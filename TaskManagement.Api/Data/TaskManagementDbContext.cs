using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskModel = TaskManagement.Api.Models.Task;

namespace TaskManagement.Api.Data;

public partial class TaskManagementDbContext : DbContext
{
    public TaskManagementDbContext()
    {
    }

    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaskModel> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // CreatedAt: Set ONLY on insert, NEVER on update
            entity.Property(e => e.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Status: Default is 'Pending'
            entity.Property(e => e.Status).HasDefaultValueSql("'Pending'");

            // UpdatedAt: MySQL handles this automatically via ON UPDATE CURRENT_TIMESTAMP
            // EF Core should NOT touch this column
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
