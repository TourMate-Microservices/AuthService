using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TourMate.AuthService.Repositories.Models;

namespace TourMate.AuthService.Repositories.Context;

public partial class TourMateAuthContext : DbContext
{
    public TourMateAuthContext()
    {
    }

    public TourMateAuthContext(DbContextOptions<TourMateAuthContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RefreshToken");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.ExpireAt)
                .HasColumnType("datetime")
                .HasColumnName("expireAt");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsRevoked).HasColumnName("isRevoked");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshToken_Account");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
