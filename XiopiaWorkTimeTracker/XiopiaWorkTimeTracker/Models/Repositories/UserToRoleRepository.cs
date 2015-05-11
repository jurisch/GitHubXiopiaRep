using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class UserToRoleRepository : Repository<UserToRoleMapping>
    {
        public List<UserToRoleMapping> GetByUserGuid(Guid guid)
        {
            return DbSet.Where(u => u.EmployeeGuid == guid).ToList();
        }
    }
}