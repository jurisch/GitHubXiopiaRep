﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using XiopiaWorkTimeTracker.BusinessLogic;
using XiopiaWorkTimeTracker.Models;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;
using XiopiaWorkTimeTracker.Models.ViewModels;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class UserController : Controller
    {
        UserRepository usersRepository = null;

        enum ViewArctionResults { Success, MultipleStarts, NotYetStarted }

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

        [HttpPost]
        public JsonResult ActionFromView(ActionsFromViewViewModel model)
        {
            var userRepo = new UserRepository();
            var user = userRepo.Get(model.UserId);
            var wtEntry = new WorkTimeEntry();
            WorkTimeEntry startetEntry = null;
            ViewArctionResults result = ViewArctionResults.Success;
            List<WorkTimeEntry> startedEntries = user.GetTodayStartedEntries(); ;

            bool startet = false;
            if (startedEntries != null)
            {
                if (startedEntries != null)
                {
                    foreach (var ent in startedEntries)
                    {
                        if ((ent.WorkStartTime != null) && (ent.WorkEndTime == null))
                        {
                            startet = true;
                            startetEntry = ent;
                            break;
                        }
                    }
                }
            }

            switch (model.Action)
            {
                case "StartWork":
                    if (!startet)
                    {
                        var projRepo = new ProjectsRepository();
                        wtEntry.WorkDay = DateTime.Now;
                        wtEntry.WorkStartTime = DateTime.Now;
                        wtEntry.ProjectName = projRepo.Get(model.ProjectId).Name;
                        user.AddTimeEntry(wtEntry);
                    }
                    else
                    {
                        result = ViewArctionResults.MultipleStarts;
                    }
                    break;
                case "EndWork":
                    if (startet)
                    {
                        startetEntry.WorkEndTime = DateTime.Now;
                        user.UpdateTimeEntry(startetEntry);
                    }
                    //else if (startedEntries.Count > 1)
                    //{
                    //    result = ViewArctionResults.MultipleStarts;
                    //}
                    else
                    {
                        result = ViewArctionResults.NotYetStarted;
                    }
                    break;
                default:
                    break;
            }
            return Json(result);
        }

        public ActionResult SelectMonthWorkTimes(int month)
        {
            var tmp = Session["userId"];
            if (tmp != null)
            {
                int userId = Int32.Parse(tmp.ToString());
                var projectsRepo = new ProjectsRepository();
                var userViewModel = new UserViewModel();

                userViewModel.User = this.usersRepository.Get(userId);

                var projectsSelectListItems = new List<SelectListItem>();
                foreach (var pr in projectsRepo.GetByUserId(userId))
                {
                    projectsSelectListItems.Add(new SelectListItem()
                    {
                        Text = pr.Name,
                        Value = pr.Id.ToString()
                    });
                }
                userViewModel.UserProjects = projectsSelectListItems;

                if (userViewModel.User != null)
                {
                    userViewModel.WorkDays = WorkTimesBuilder.GetMonth(month);

                    userViewModel.CurrentYear = DateTime.Now.Year.ToString();
                    userViewModel.CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                    var userEntriesForMonth = userViewModel.User.GetTimeEntriesForMonth(month);
                    foreach (var modelViewDayRows in userViewModel.WorkDays)
                    {
                        if (modelViewDayRows.WorkDate.Month == month)
                        {
                            modelViewDayRows.DataRow = new List<WorkTimeRow>();
                            foreach (var dbEntry in userEntriesForMonth)
                            {
                                //var entryList = new List<WorkTimeRow>();
                                if (modelViewDayRows.WorkDate.Day == dbEntry.WorkDay.Day)
                                {
                                    if (dbEntry.WorkStartTime.HasValue)
                                    {
                                        var entry = new WorkTimeRow();
                                        entry.StartTime = dbEntry.WorkStartTime.HasValue ? dbEntry.WorkStartTime.Value.ToShortTimeString() : "";
                                        entry.EndTime = dbEntry.WorkEndTime.HasValue ? dbEntry.WorkEndTime.Value.ToShortTimeString() : "";
                                        entry.Project = dbEntry.ProjectName;
                                        modelViewDayRows.DataRow.Add(entry);
                                    }
                                }
                            }
                        }
                    }
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