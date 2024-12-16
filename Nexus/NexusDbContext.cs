using Microsoft.EntityFrameworkCore;
using Nexus.Models;

namespace Nexus
{
    public class NexusDbContext : DbContext
    {
        IConfiguration _configuration;
        public NexusDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("Nexus");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee Entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(64);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasOne<AdminEmployee>()
                    .WithOne()
                    .HasForeignKey<Employee>(e => e.UpdatedBy)
                    .IsRequired(false);
            });

            // RentalShopEmployee Entity
            modelBuilder.Entity<RentalShopEmployee>(entity =>
            {
                entity.HasKey(r => r.RentalShopEmployeeId);
            });

            // AdminEmployee Entity
            modelBuilder.Entity<AdminEmployee>(entity =>
            {
                entity.HasKey(a => a.AdminEmployeeId);
                entity.HasOne<Employee>()
                    .WithOne()
                    .HasForeignKey<AdminEmployee>(a => a.AdminEmployeeId);
            });

            // TechnicalEmployee Entity
            modelBuilder.Entity<TechnicalEmployee>(entity =>
            {
                entity.HasKey(t => t.TechnicalEmployeeId);
                entity.HasOne<Employee>()
                    .WithOne()
                    .HasForeignKey<TechnicalEmployee>(t => t.TechnicalEmployeeId);
            });
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<RentalShopEmployee> RentalShopEmployees { get; set; }
        public DbSet<AdminEmployee> AdminEmployees { get; set; }
        public DbSet<TechnicalEmployee> TechnicalEmployees { get; set; }
        public DbSet<RentalShop> RentalShops { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
