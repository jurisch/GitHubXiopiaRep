using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.BusinessLogic;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models
{
    public class UserViewModel
    {
        public Employee User { get; set; }
        public string CurrentYear { get; set; }
        public string CurrentMonth { get; set; }

        public List<DateRowModel> WorkDays { get; set; }

        public List<SelectListItem> UserList = new List<SelectListItem>();

        public List<SelectListItem> UserProjects { get; set; }

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