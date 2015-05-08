using System;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models.Repositories;
using XiopiaWorkTimeTracker.Models.ViewModels;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class HomeController : Controller
    {
        UserRepository usersRepository = null;

        public HomeController()
        {
            this.usersRepository = new UserRepository();
        }

        public ActionResult Index()
        {
            var userId = Session["userId"];
            if (userId != null)
            {
                var model = new UserAndRolesViewModel();
                var user = usersRepository.GetById(Int32.Parse(userId.ToString()));
                model.User = user;
                return View(model);
                //return Redirect("/User/SelectMonthWorkTimes/?month=" + DateTime.Now.Month + "&SelectedUser=" + userId);
            }
            else
            {
                return Redirect("/User");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}