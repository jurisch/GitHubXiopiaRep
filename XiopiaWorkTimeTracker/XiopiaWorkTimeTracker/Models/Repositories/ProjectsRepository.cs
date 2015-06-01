using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.Repositories
{
    public class ProjectsRepository : Repository<Project>
    {
        private UserRepository _userRepo = null;
        private ProjectToMembersRepository _projectToMemberRepo = null;

        public ProjectsRepository()
        {
            _userRepo = new UserRepository();
            _projectToMemberRepo = new ProjectToMembersRepository();
        }

        public Project GetByGuid(Guid guid)
        {
            return DbSet.Where(g => g.Guid == guid).First();
        }

        public List<Project> GetByUserId(int id)
        {
            var projectsList = new List<Project>();
            var userGuid = this._userRepo.Get(id).Guid;

            return this._projectToMemberRepo.GetProjectsByUserGuid(userGuid);
        }

        public Project GetByName(string name)
        {
            return DbSet.Where(n => n.Name.Equals(name)).First();
        }
    }
}