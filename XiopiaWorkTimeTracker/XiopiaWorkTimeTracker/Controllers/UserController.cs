using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.BusinessLogic;
using XiopiaWorkTimeTracker.Models;
using XiopiaWorkTimeTracker.Models.Database;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            
            var userViewModel = new UserViewModel();
            userViewModel.SelectedUser = 0;

            using (var context = new WorkTimeTrackerDbContext())
            {
                var users = context.Employees;
                if(users != null)
                {
                   foreach(var cur in users.ToList())
                   {
                       userViewModel.UserList.Add(new SelectListItem() { Value = cur.Id.ToString(), Text = cur.FirstName + " " + cur.LastName });
                   }
                }
            }

            return View(userViewModel);
        }

        public ActionResult SelectMonthWorkTimes(int month, int SelectedUser)
        {
            var userViewModel = new UserViewModel();
            userViewModel.CurrentUser = "Anonyme";
            using (var context = new WorkTimeTrackerDbContext())
            {
                var currentUser = context.Employees.Where(u => u.Id == SelectedUser).FirstOrDefault();
                if (currentUser != null)
                {
                    var settingsFromDb = context.GlobalSettings;
                    int fd = 1, ld = 5;
                    //String fds = settingsFromDb.Where(s => s.Name.Equals("WeekFirstDay")).FirstOrDefault().Value;
                    //if (fds != null)
                    //{
                    //    Int32.TryParse(fds, out fd);
                    //}
                    //String lds = settingsFromDb.Where(s => s.Name.Equals("WeekLastDay")).FirstOrDefault().Value;
                    //if (lds != null)
                    //{
                    //    Int32.TryParse(lds, out ld);
                    //}
                    SettingsModel settingsModel = new SettingsModel() { FirstWorkDayOfWeek = fd, LastWorkDayOfWeek = ld };
                    userViewModel.CurrentUser = currentUser.FirstName + ", " + currentUser.LastName;
                    userViewModel.SelectedUser = SelectedUser;
                    userViewModel.WorkTimeRows = WorkTimesBuilder.GetMonth(month, settingsModel);
                    userViewModel.CurrentYear = DateTime.Now.Year.ToString();
                    userViewModel.CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                }
            } 
            return View(userViewModel);
        }

        public ActionResult StartRecordingNow()
        {
            return PartialView("StartRecordingNowDialog");
        }

        [HttpPost]
        public ActionResult SaveStartRecording()
        {
            return RedirectToAction("Index");
        }
    }
}