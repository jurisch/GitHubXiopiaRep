using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class UserToRoleRepository : Repository<UserToRoleMapping>
    {
        //public List<UserToRoleMapping> GetByUserId(int id)
        //{
        //    return DbSet.Where(u => u.EmployeeId == id).ToList();
        //}
    }
}