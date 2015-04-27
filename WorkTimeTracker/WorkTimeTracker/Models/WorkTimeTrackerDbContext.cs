using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WorkTimeTracker.Models
{
    public class WorkTimeTrackerDbContext : DbContext
    {
        public WorkTimeTrackerDbContext() : base("WorkTimeTrackerDb ")
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<WorkTimesEntry> WorkTimeEntries { get; set; }

        public DbSet<WorkTimeTrackerRole> WorkTimeTrackerRoles { get; set; }

    }
}