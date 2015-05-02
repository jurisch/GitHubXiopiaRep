using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.BusinessLogic;

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
    }
}