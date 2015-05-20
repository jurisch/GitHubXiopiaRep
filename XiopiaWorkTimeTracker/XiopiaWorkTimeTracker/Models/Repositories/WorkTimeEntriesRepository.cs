using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class WorkTimeEntriesRepository : Repository<WorkTimeEntry>
    {
        public List<WorkTimeEntry> GetUserEntriesForMonth(int id, int month)
        {
//            return DbSet.Where(e => e.EmployeeId == id).ToList();
            var quiry = from c in DbSet
                            where c.EmployeeId == id
                            select c;
            var list = new List<WorkTimeEntry>();
            foreach(var ent in quiry)
            {
                if(ent.WorkDay.Month == month)
                {
                    list.Add(ent);
                }
            }
            return list;
        }

        public List<WorkTimeEntry> GetUserTodayStartedEntries(int id)
        {
            var list = new List<WorkTimeEntry>();
            var quiry = from c in DbSet
                        where c.EmployeeId == id
                        select c;
            var temp = DateTime.Now;
            foreach (var ent in quiry)
            {
                if ((ent.WorkStartTime.HasValue) && 
                    ((ent.WorkStartTime.Value.Year == temp.Year) && 
                    (ent.WorkStartTime.Value.Month == temp.Month) && 
                    (ent.WorkStartTime.Value.Day == temp.Day)))
                {
                    list.Add(ent);
                }
            }
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

    }
}