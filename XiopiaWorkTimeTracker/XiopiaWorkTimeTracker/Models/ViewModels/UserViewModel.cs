using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.BusinessLogic;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.ViewModels;

namespace XiopiaWorkTimeTracker.Models
{
    public class UserViewModel
    {
        private OverviewViewModel overview = null;

        public UserViewModel()
        {

        }

        public UserViewModel(int userId)
        {
            overview = new OverviewViewModel(userId);
        }

        public Employee User { get; set; }
        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }

        public List<DateRowModel> WorkDays { get; set; }

        public List<SelectListItem> UserList = new List<SelectListItem>();

        public List<SelectListItem> UserProjects { get; set; }

        public OverviewViewModel Overview {
            get
            {
                return overview;
            }
        }

        [Display(Name = "Date", ResourceType = typeof(Ressources.Language))]
        public DateTime WorkDay { get; set; }

        [Display(Name = "StartWork", ResourceType = typeof(Ressources.Language))]
        public DateTime WorkStartTime { get; set; }

        [Display(Name = "EndWork", ResourceType = typeof(Ressources.Language))]
        public DateTime WorkEndTime { get; set; }

        [Display(Name = "EndWork", ResourceType = typeof(Ressources.Language))]
        public int PauseLength { get; set; }

        public string Project { get; set; }

    }
}