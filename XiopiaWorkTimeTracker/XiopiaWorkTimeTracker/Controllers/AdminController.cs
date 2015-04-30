using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var settingsModel = new SettingsViewModel();
            return View(settingsModel);
        }
    }
}