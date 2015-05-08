using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.BusinessLogic;
using System.ComponentModel.DataAnnotations;

namespace XiopiaWorkTimeTracker.Models
{
    public class UserViewModel
    {
        public int SelectedUser { get; set; }
        public string CurrentUser { get; set; }
        public string CurrentYear { get; set; }
        public string CurrentMonth { get; set; }
        public List<WorkTimeRow> WorkTimeRows { get; set; }
        public List<SelectListItem> UserList = new List<SelectListItem>();

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