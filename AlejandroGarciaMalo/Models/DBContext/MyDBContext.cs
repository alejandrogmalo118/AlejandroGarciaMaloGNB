using AlejandroGarciaMalo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Models.DBContext
{
    /// <summary>
    /// DBContext for Sqlite database in directory of the program
    /// </summary>
    public class MyDbContext : DbContext
    {
        /// <summary>
        /// Table Rates
        /// </summary>
        public DbSet<Rate> Rates { get; set; }

        /// <summary>
        /// Table Transactions
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        public MyDbContext()
        {
            Database.EnsureCreated();
        }

        public MyDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseSqlite("Filename=GNBDatabase.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });


            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Rate>(e =>
            {
                e.HasIndex(i => new { i.From, i.To }).IsUnique(true);
            });

            modelBuilder.Entity<Transaction>();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
