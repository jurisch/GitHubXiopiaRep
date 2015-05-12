using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;
using XiopiaWorkTimeTracker.Models.ViewModels;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Projects
        public ActionResult Index()
        {
            var projectsRepo = new ProjectsRepository();
            var userRepo = new UserRepository();
            var allprojects = projectsRepo.GetAll();
            var allemployes = userRepo.GetAll();
            var viewModel = new ProjectViewModel(allprojects, allemployes);
            return View(viewModel);
        }

        public ActionResult CreateNewProject()
        {
            var userRepo = new UserRepository();
            var projectModel = new Project();
            return View(projectModel);
        }

        [HttpPost]
        public ActionResult CreateNewProject(Project model)
        {
            if (ModelState.IsValid)
            {
                var projectsRepo = new ProjectsRepository();
                var newProject = new Project();
                newProject.Name = model.Name;
                newProject.ProjectResponsible = model.ProjectResponsible;
                projectsRepo.Add(newProject);
                projectsRepo.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult EditProject(int Id)
        {
            var projectsRepo = new ProjectsRepository();
            return View(projectsRepo.Get(Id));
        }

        [HttpPost]
        public ActionResult EditProject(int? Id, Project project)
        {
            var projectsRepo = new ProjectsRepository();
            projectsRepo.SetModified(project);
            projectsRepo.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteProject(int Id)
        {
            var projectsRepo = new ProjectsRepository();
            return View(projectsRepo.Get(Id));
        }

        [HttpPost]
        public ActionResult DeleteProject(int? Id, Project project)
        {
            var projectsRepo = new ProjectsRepository();
            projectsRepo.SetDeleted(project);
            projectsRepo.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetAllEmployeesJson(int? number)
        {
            var userRepo = new UserRepository();
            var employee = userRepo.GetAll();
            if (null != employee)
                return Json(employee, JsonRequestBehavior.AllowGet);
            return null;
        }


    }
}