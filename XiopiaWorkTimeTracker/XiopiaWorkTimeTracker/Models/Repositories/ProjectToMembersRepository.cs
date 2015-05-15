using System;
using System.Collections.Generic;
using System.Linq;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class ProjectToMembersRepository : Repository<ProjectToMembersMapping>
    {
        public List<Employee> GetMembersByProjectGuid(Guid guid)
        {
            var members = new List<Employee>();
            var usersRepo = new UserRepository();
            var mappings = DbSet.Where(u => u.ProjectGuid == guid).ToList();
            foreach (var m in mappings)
            {
                members.Add(usersRepo.GetByGuid(m.MemberGuid));
            }
            return members;
        }

        public List<Project> GetProjectsByUserGuid(Guid guid)
        {
            var projects = new List<Project>();
            var projRepo = new ProjectsRepository();
            var userMappings = DbSet.Where(m => m.MemberGuid == guid);
            foreach (var pr in userMappings)
            {
                projects.Add(projRepo.GetByGuid(pr.ProjectGuid));
            }
            return projects;
        }
    }
}