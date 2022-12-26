using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DemoServerless.Entities
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext()
        {
        }

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=DbServerless;Persist Security Info=True;User ID=sa;Password=MyStrongP@ssw0rd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("profile");

                entity.HasIndex(e => e.UserId)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Country)
                    .HasMaxLength(100)
                    .HasColumnName("country");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Profile)
                    .HasForeignKey<Profile>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_profile");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IsVerify)
                    .IsRequired()
                    .HasColumnName("isVerify")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Role)
                    .HasDefaultValue(Role.User)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
