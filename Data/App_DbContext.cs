using Microsoft.EntityFrameworkCore;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Data
{
    public class App_DbContext :DbContext
    {
        public App_DbContext(DbContextOptions<App_DbContext>options):base(options)
        {

        }

        //to create sp during creation of tables

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>().MapToStoredProcedures();
        //}

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
