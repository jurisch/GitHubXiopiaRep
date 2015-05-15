using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class ActionsFromViewViewModel
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public int ProjectId { get; set; }
    }
}