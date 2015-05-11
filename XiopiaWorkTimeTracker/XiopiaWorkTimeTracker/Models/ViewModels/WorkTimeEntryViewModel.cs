using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XiopiaWorkTimeTracker.Models.ViewModels
{
    public class WorkTimeEntryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Ressources.Language))]
        public DateTime WorkDay { get; set; }

        [Display(Name = "StartWork", ResourceType = typeof(Ressources.Language))]
        public DateTime WorkStartTime { get; set; }

        [Display(Name = "EndWork", ResourceType = typeof(Ressources.Language))]
        public DateTime WorkEndTime { get; set; }

        [Display(Name = "EndWork", ResourceType = typeof(Ressources.Language))]
        public int PauseLength { get; set; }

        public int EmployeeId { get; set; }
    }
}