﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.Models;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;
using XiopiaWorkTimeTracker.Models.ViewModels;

namespace XiopiaWorkTimeTracker.Controllers
{
	public class AdminController : Controller
	{
		UserRepository usersRepository = null;

		// GET: Admin
		public ActionResult Index()
		{
			var settingsViewModel = GetModel(null);
			return View(settingsViewModel);
		}

		private SettingsViewModel GetModel(string firstName)
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
				settingsViewModel.GermanStateId = globalSettings.First().GermanStateId;

				UserAndRolesViewModel usrvm = new UserAndRolesViewModel();
				FeierTag feiertagView = new FeierTag();

				usersRepository = new UserRepository();

				List<Employee> users = usersRepository.GetAll();
				List<Employee> searchResult = new List<Employee>();

				if (users != null)
				{
					if (null == firstName)
						settingsViewModel.usrvm = new UserAndRolesViewModel { employees = users };
					else
					{
						foreach (var user in users)
						{
							if (user.FirstName.Contains(firstName))
								searchResult.Add(user);
						}
						settingsViewModel.usrvm = new UserAndRolesViewModel { employees = searchResult };
					}
				}
				
				settingsViewModel.germanHolidays = feiertagView;
			}
			return settingsViewModel;
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

		[HttpGet]
		public ActionResult GetSelectedUserRole(Guid id)
		{
			var settingsViewModel = new SettingsViewModel();
			UserAndRolesViewModel usrvm = new UserAndRolesViewModel();
			usersRepository = new UserRepository();
			var user = usersRepository.GetByGuid(id);
			if (user != null)
			{
				usrvm.User = user;

				// using id's just for checkbox hier. They are not realy id's! just for view. 
				usrvm.UserRoleId = user.HasRole("User") ? 1 : 0;
				usrvm.AccountingRoleId = user.HasRole("Accounting") ? 1 : 0;
				usrvm.AdminRoleId = user.HasRole("Admin") ? 1 : 0;
				usrvm.ProjectSupervisorRoleId = user.HasRole("ProjectSupervisor") ? 1 : 0;
				return Json(usrvm, JsonRequestBehavior.AllowGet);
			}
			else
				return null;

		}


		[HttpGet]
		public PartialViewResult UpdateUserRole(Guid guid, bool user, bool accounting, bool admin, bool projektmanager)
		{
			if (ModelState.IsValid)
			{
				usersRepository = new UserRepository();
				var selectedUser = usersRepository.GetByGuid(guid);
				var rolesRepo = new RolesRepository();

				//Clear all Role from selected user
				if (selectedUser.HasRole("User")) selectedUser.RemoveRole(rolesRepo.GetByName("User").Id);
				if (selectedUser.HasRole("Accounting")) selectedUser.RemoveRole(rolesRepo.GetByName("Accounting").Id);
				if (selectedUser.HasRole("Admin")) selectedUser.RemoveRole(rolesRepo.GetByName("Admin").Id);
				if (selectedUser.HasRole("ProjectSupervisor")) selectedUser.RemoveRole(rolesRepo.GetByName("ProjectSupervisor").Id);

				//add selected Roles to selected user
				if (user) selectedUser.AddRole(rolesRepo.GetByName("User").Id);
				if (accounting) selectedUser.AddRole(rolesRepo.GetByName("Accounting").Id);
				if (admin) selectedUser.AddRole(rolesRepo.GetByName("Admin").Id);
				if (projektmanager) selectedUser.AddRole(rolesRepo.GetByName("ProjectSupervisor").Id);

				usersRepository.SetModified(selectedUser);
				usersRepository.SaveChanges();
			}

			var data = GetModel(null);
			return PartialView(data.usrvm);

		}

		public PartialViewResult SearchPeople(string keyword)
		{
			var data = GetModel(keyword);
			return PartialView(data.usrvm);
		}

		[HttpPost]
		public ActionResult SetVariable(string key, string value)
		{
			Session[key] = value;

			return this.Json(new { success = true });
		}


		[HttpGet]
		public ActionResult UpdateSelectedGermanHoliday(int id, bool festgelegt)
		{
			var holidayrep = new GermanHolidayRepository();
			var selectedHoliday = holidayrep.GetById(id);
			selectedHoliday.Festgelegt = festgelegt;
			holidayrep.SaveChanges();

			return Json(selectedHoliday, JsonRequestBehavior.AllowGet);
		}

	}
}