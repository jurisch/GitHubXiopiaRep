using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class WorkTimeTrackerDbContext : DbContext
    {
        public WorkTimeTrackerDbContext()
            : base("WorkTimeTrackerDbContext")
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<WorkTimeEntry> WorkTimeEntries { get; set; }

        public DbSet<WorkTimeRole> WorkTimeTrackerRoles { get; set; }

        public DbSet<Setting> GlobalSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}