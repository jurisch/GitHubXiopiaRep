using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkTimeTracker.Models
{
    public class UserViewModel
    {
        public List<WorkTimesEntry> WorkTimeEntries { get; set; }
    }
}