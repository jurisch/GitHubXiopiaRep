using Ressources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class Project
    {
        public Project()
        {
            this.Guid = Guid.NewGuid();
        }

        public int Id { get; set; }
        public Guid Guid { get; private set; }

        [Display(Name = "ProjectName", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errProjName", ErrorMessageResourceType = typeof(Language))]
        public string Name { get; set; }

        [Display(Name = "ProjectResponsible", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errProjectResponsible", ErrorMessageResourceType = typeof(Language))]
        public string ProjectResponsible { get; set; }

        public List<int> MemberIds { get; set; }

        //public List<Employee> Members
        //{
        //    get
        //    {
        //        var projectsRepo = new ProjectToMembersRepository();
        //        return projectsRepo.GetMembersByProjectGuid(this.Guid);
        //    }
        //}

        public void AddMember(int memberId)
        {
            var usersRepo = new UserRepository();
            var user = usersRepo.Get(memberId);
            var projectToMembersRepo = new ProjectToMembersRepository();
            projectToMembersRepo.Add(new ProjectToMembersMapping() { MemberGuid = user.Guid, ProjectGuid = this.Guid });
            projectToMembersRepo.SaveChanges();
        }

    }
}