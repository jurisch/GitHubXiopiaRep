using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

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

		public DbSet<GermanHoliday> GermanHolidays { get; set; }

		public DbSet<GermanState> GermanStates { get; set; }

		public DbSet<HolidayTyp> HolidayTyps { get; set; }

		public DbSet<WorkTimeEntry> WorkTimeEntries { get; set; }

        public DbSet<WorkTimeRole> WorkTimeTrackerRoles { get; set; }

        public DbSet<Setting> GlobalSettings { get; set; }

        public DbSet<UserToRoleMapping> UserToRoleMappings { get; set; }

        public DbSet<ProjectToMembersMapping> ProjectToMembersMappings { get; set; }

		public DbSet<HolidayToStateMapping> HolidayToStateMappings { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Employee>().HasMany(e => e.WorkTimeRoles)
            //    .WithMany(e => e.Employees);
        }

    }
}