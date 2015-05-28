using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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
            var user = this.usersRepository.Get(model.UserId);
            WorkTimeEntry wtEntry = null;
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
                        wtEntry = new WorkTimeEntry();
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
                    else
                    {
                        result = ViewArctionResults.NotYetStarted;
                    }
                    break;
                case "ElementChange":
                    if ((model.Value == null) && (model.Element.Equals("DeleteElement")))
                    {
                        wtEntry = user.GetTimeEntryByid(Int32.Parse(model.ElementId));
                        user.DeleteEntry(wtEntry);
                    }
                    else
                    {
                        ResultRowHelper serializer = new JavaScriptSerializer().Deserialize<ResultRowHelper>(model.Value);
                        if (string.IsNullOrEmpty(model.ElementId))
                        {
                            wtEntry = serializer.GetWorkTimeEntry();
                            user.AddTimeEntry(wtEntry);
                        }
                        else
                        {
                            wtEntry = user.GetTimeEntryByid(Int32.Parse(model.ElementId));

                            if (!serializer.AttrHoliday && !serializer.AttrIll && (serializer.StartTime == null) && (serializer.EndTime == null) && (serializer.PauseLength == 0))
                            {
                                user.DeleteEntry(wtEntry);
                            }
                            else
                            {

                                var tempEntry = serializer.GetWorkTimeEntry();

                                wtEntry.WorkStartTime = tempEntry.WorkStartTime;
                                wtEntry.WorkEndTime = tempEntry.WorkEndTime;
                                wtEntry.PauseLength = tempEntry.PauseLength;
                                wtEntry.ProjectName = tempEntry.ProjectName;
                                wtEntry.AttrHoliday = tempEntry.AttrHoliday;
                                wtEntry.AttrIll = tempEntry.AttrIll;
                                user.UpdateTimeEntry(wtEntry);
                            }
                        }
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
                var userViewModel = new UserViewModel(userId, month);

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

                    userViewModel.CurrentYear = DateTime.Now.Year;
                    userViewModel.CurrentMonth = month;

                    var userEntriesForMonth = userViewModel.User.GetTimeEntriesForMonth(month);
                    foreach (var modelViewDayRows in userViewModel.WorkDays)
                    {
                        if (modelViewDayRows.WorkDate.Month == month)
                        {
                            modelViewDayRows.DataRow = new List<WorkTimeRow>();
                            foreach (var dbEntry in userEntriesForMonth)
                            {
                                if (modelViewDayRows.WorkDate.Day == dbEntry.WorkDay.Day)
                                {
                                    var entry = new WorkTimeRow();
                                    if (dbEntry.AttrIll)
                                    {
                                        modelViewDayRows.AttrIll = dbEntry.AttrIll;
                                        modelViewDayRows.DayId = dbEntry.Id;
                                        continue;
                                    }
                                    else if (dbEntry.AttrHoliday)
                                    {
                                        modelViewDayRows.AttrHoliday = dbEntry.AttrHoliday;
                                        modelViewDayRows.DayId = dbEntry.Id;
                                    }
                                    else
                                    {
                                        entry.EntryId = dbEntry.Id;
                                        entry.StartTime = dbEntry.WorkStartTime.Value;
                                        entry.PauseLength = dbEntry.PauseLength.HasValue ? dbEntry.PauseLength.Value : 0;
                                        entry.EndTime = dbEntry.WorkEndTime.HasValue ? dbEntry.WorkEndTime.Value : Convert.ToDateTime("00:00").Date;
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