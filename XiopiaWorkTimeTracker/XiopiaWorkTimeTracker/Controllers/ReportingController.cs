using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models.Repositories;
using XiopiaWorkTimeTracker.Models.ViewModels;
using Ressources;
using System;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class ReportingController : Controller
    {

        private UserRepository _usersRepository = null;
        private ProjectsRepository _projectsRepo = null;

        public ReportingController()
        {
            _usersRepository = new UserRepository();
            _projectsRepo = new ProjectsRepository();
        }
        
        // GET: Reporting
        public ActionResult Index()
        {
            ReportingViewModel viewModel = new ReportingViewModel();
            viewModel.From = DateTime.Now;
            viewModel.Until = DateTime.Now;
            return View(FillLists(viewModel));
        }

        [HttpPost]
        public ActionResult Search(ReportingViewModel model)
        {
            if(model.From > model.Until)
            {
                ModelState.AddModelError("Until", Language.ErrorUntilBeforeFrom);
            }
            if(ModelState.IsValid)
            {

            }
            return View("Index", FillLists(model));
        }

        private ReportingViewModel FillLists(ReportingViewModel model)
        {
            var users = this._usersRepository.GetAll();
            if (users != null)
            {
                model.UserList.Add(new SelectListItem() { Value = "0", Text = Language.AllUsers, Selected = true });
                foreach (var user in users)
                {
                    model.UserList.Add(new SelectListItem() { Value = user.Id.ToString(), Text = user.FirstName + " " + user.LastName });
                }
            }
            var projects = this._projectsRepo.GetAll();
            model.AllProjects.Add(new SelectListItem() { Value = "0", Text = Language.AllProjects, Selected = true });
            foreach (var project in projects)
            {
                model.AllProjects.Add(new SelectListItem() { Value = project.Id.ToString(), Text = project.Name });
            }
            return model;
        }
    }
}