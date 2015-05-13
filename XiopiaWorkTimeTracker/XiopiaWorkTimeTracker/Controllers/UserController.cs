using System;
using System.Globalization;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.BusinessLogic;
using XiopiaWorkTimeTracker.Models;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class UserController : Controller
    {
        UserRepository usersRepository = null;

        public UserController()
        {
            this.usersRepository = new UserRepository();
        }

        // GET: User
        public ActionResult Index()
        {
            var userViewModel = new UserViewModel();
            var users = this.usersRepository.GetAll();
            if (users != null)
            {
                foreach (var user in users)
                {
                    userViewModel.UserList.Add(new SelectListItem() { Value = user.Id.ToString(), Text = user.FirstName + " " + user.LastName });
                }
            }

            return View(userViewModel);
        }

        public ActionResult SelectMonthWorkTimes(int month)
        {
            var tmp = Session["userId"];
            if (tmp != null)
            {
                var userViewModel = new UserViewModel();
                userViewModel.User = this.usersRepository.Get(Int32.Parse(tmp.ToString()));
                if (userViewModel.User != null)
                {
                    userViewModel.WorkTimeRows = WorkTimesBuilder.GetMonth(month);
                    userViewModel.CurrentYear = DateTime.Now.Year.ToString();
                    userViewModel.CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                }
                return View(userViewModel);
            }
            else
            {
                return Redirect("/User");
            }
        }

        [HttpPost]
        public ActionResult SaveStartRecording()
        {
            return RedirectToAction("Index");
        }
    }
}