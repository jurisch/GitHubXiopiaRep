using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class ProjectsRepository : Repository<Project>
    {
        public Project GetByGuid(Guid guid)
        {
            return DbSet.Where(g => g.Guid == guid).First();
        }

        public List<Project> GetByUserId(int id)
        {
            var projectsList = new List<Project>();
            var userRepo = new UserRepository();
            var userGuid = userRepo.Get(id).Guid;

            var userToProjMapRepo = new ProjectToMembersRepository();
            return userToProjMapRepo.GetProjectsByUserGuid(userGuid);
        }
    }
}