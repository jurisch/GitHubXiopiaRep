using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class ReportingViewModel
    {
        public ReportingViewModel()
        {
            this.UserList = new List<SelectListItem>();
            this.AllProjects = new List<SelectListItem>();
        }

        public List<SelectListItem> UserList { get; set; }
        public List<SelectListItem> AllProjects { get; set; }
        public int SelectedUser { get; set; }
        public int SelectedProject { get; set; }

        [Required]
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
    }
}