using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
	public class UserToTimeEntrieRepository : Repository<UserToTimeEntriesMapping>
	{
		public UserToTimeEntriesMapping GetById(int id)
		{
			return DbSet.Where(u => u.Id == id).First();
		}

		public List<WorkTimeEntry> GetByUserId(int uId)
		{
			var repo = new WorkTimeEntriesRepository();
            var entries = new List<WorkTimeEntry>();
            var mappings = DbSet.Where(ac => ac.UserId.Equals(uId)).ToList();
			foreach (var m in mappings)
			{
				entries.Add(repo.Get(m.Id));
            }
			return entries;
		}

		public void AddTimeEntrie(int userId, int timeeEntryId)
		{
			var usertoentrie = DbSet.Find(userId);
		}
	}
}