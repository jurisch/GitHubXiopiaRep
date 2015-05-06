using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var settingsViewModel = new SettingsViewModel();
            using (var context = new WorkTimeTrackerDbContext())
            {
                var globalSettings = context.GlobalSettings;
                settingsViewModel.DaysAweek = globalSettings.First().DaysAweek;
                settingsViewModel.HoursAday = globalSettings.First().HoursAday;
                settingsViewModel.HoursAweek = globalSettings.First().HoursAweek;
                settingsViewModel.MonthsAyear = globalSettings.First().MonthsAyear;
                settingsViewModel.VacationDays = globalSettings.First().VacationDays;
            }
            return View(settingsViewModel);
        }

        [HttpPost]
        public ActionResult SaveSettings(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var settings = new List<Setting>
                {
                    new Setting{ DaysAweek = model.DaysAweek,
                        HoursAday = model.HoursAday,
                        HoursAweek = model.HoursAweek,
                        MonthsAyear = model.MonthsAyear,
                        VacationDays = model.VacationDays}
                };
                using (var context = new WorkTimeTrackerDbContext())
                {
                    settings.ForEach(r => context.GlobalSettings.Add(r));
                    context.SaveChanges();
                }
            }
            return View("Index");
        }
    }
}