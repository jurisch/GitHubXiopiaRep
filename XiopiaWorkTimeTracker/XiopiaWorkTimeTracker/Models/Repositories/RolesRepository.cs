using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class RolesRepository : Repository<WorkTimeRole>
    {
        public WorkTimeRole GetByName(string name)
        {
            return DbSet.Where(r => r.Name.Equals(name)).First();
        }
    }
}