using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.ViewModels;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Projects
        public ActionResult Index()
        {
			//using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext()) {
			//	List<Project> projectmodel = accdb.Projects.ToList();
			//	return View(projectmodel);
			//}

			using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext())
			{
				var allprojects = accdb.Projects.ToList();
				var allemployes = accdb.Employees.ToList();
				var viewModel = new ProjectViewModel(allprojects, allemployes);

				return View(viewModel);
			}

		}

		public ActionResult CreateNewProject(){
			using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext())
			{
				var projectModel = new Project() {

					Employees = accdb.Employees.ToList()
				};
				return View(projectModel);
			}
		}

		[HttpPost]
		public ActionResult CreateNewProject(Project model)
		{
			try
			{
				using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext())
				{
					var Project = accdb.Projects.ToList();
					if (!Project.Exists(p => p.Name == model.Name))
					{
						Project newProject = new Project();
						newProject.Name = model.Name;
						newProject.ProjectResponsibleId = model.ProjectResponsibleId;
						accdb.Projects.Add(newProject);
						accdb.SaveChanges();
						return RedirectToAction("Index");
					}
					else
					{
						return View();
					}

				}
			}
			catch
			{
				return View();
			}
		}




		public ActionResult EditProject(int? Id)
		{
			try
			{
				using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext())
				{
					var Project = accdb.Projects.ToList();
					return View(accdb.Projects.Find(Id));
				}

			}
			catch
			{
				return View();
			}
		}
		[HttpPost]
		public ActionResult EditProject(int? Id, Project project)
		{
			try
			{
				using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext())
				{
					accdb.Entry(project).State = System.Data.Entity.EntityState.Modified;
					accdb.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch
			{
				return View();
			}

		}



		public ActionResult DeleteProject(int? Id)
		{
			using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext())
			{
				return View(accdb.Projects.Find(Id));
			}
		}
		[HttpPost]
		public ActionResult DeleteProject(int? Id, Project project)
		{
			try
			{
				using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext())
				{
					accdb.Entry(project).State = System.Data.Entity.EntityState.Deleted;
					accdb.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch
			{
				return View();
			}
		}

		[HttpPost]
		public JsonResult GetAllEmployeesJson(int? number)
		{
			using (WorkTimeTrackerDbContext accdb = new WorkTimeTrackerDbContext())
			{
				var employee = accdb.Employees.ToList();
				if (null != employee)
					return Json(employee, JsonRequestBehavior.AllowGet);
				return null;
			}

		}


	}
}