using Ressources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Models.Database
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ProjectName", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errProjName", ErrorMessageResourceType = typeof(Language))]
        public string Name { get; set; }

        [Display(Name = "ProjectResponsible", ResourceType = typeof(Language))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errProjectResponsible", ErrorMessageResourceType = typeof(Language))]
        public string ProjectResponsible { get; set; }

        public List<Employee> Members
        {
            get
            {
                var projectsRepo = new ProjectToMembersRepository();
                return projectsRepo.GetMembersByProjectId(this.Id);
            }
        }

        public void AddMember(int memberId)
        {
            var projectToMembersRepo = new ProjectToMembersRepository();
            projectToMembersRepo.Add(new ProjectToMembersMapping() { MemberId = memberId, ProjectId = Id });
            projectToMembersRepo.SaveChanges();
        }

    }
}