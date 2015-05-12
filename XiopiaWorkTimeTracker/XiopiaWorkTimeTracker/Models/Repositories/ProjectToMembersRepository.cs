﻿using System.Collections.Generic;
using System.Linq;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class ProjectToMembersRepository : Repository<ProjectToMembersMapping>
    {
        public List<Employee> GetMembersByProjectId(int id)
        {
            var members = new List<Employee>();
            var usersRepo = new UserRepository();
            var mappings = DbSet.Where(u => u.ProjectId == id).ToList();
            foreach (var m in mappings)
            {
                members.Add(usersRepo.Get(m.MemberId));
            }
            return members;
        }
    }
}