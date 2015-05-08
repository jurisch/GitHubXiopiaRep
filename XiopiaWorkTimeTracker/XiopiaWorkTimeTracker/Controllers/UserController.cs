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
            var userViewModel = new UserViewModel();
            userViewModel.CurrentUser = "Anonyme";
            var tmp = Session["userId"];
            if (tmp != null)
            {
                var currentUser = this.usersRepository.GetById(Int32.Parse(tmp.ToString()));
                if (currentUser != null)
                {
                    userViewModel.CurrentUser = currentUser.FirstName + ", " + currentUser.LastName;
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