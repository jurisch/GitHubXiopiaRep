using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class UserAndRolesViewModel
    {
        public Employee User { get; set; }

        public int UserRoleId { get; set; }

        public int AdminRoleId { get; set; }

        public int AccountingRoleId { get; set; }

        public int ProjectSupervisorRoleId { get; set; }
    }
}