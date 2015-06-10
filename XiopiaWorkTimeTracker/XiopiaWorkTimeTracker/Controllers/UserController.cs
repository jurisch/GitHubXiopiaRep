using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using XiopiaWorkTimeTracker.BusinessLogic;
using XiopiaWorkTimeTracker.Models;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;
using XiopiaWorkTimeTracker.Models.ViewModels;

namespace XiopiaWorkTimeTracker.Controllers
{
    public class UserController : Controller
    {
        private UserRepository _usersRepository = null;
        private ProjectsRepository _projectsRepo = null;

        enum ViewArctionResults { Success, MultipleStarts, NotYetStarted }

        public UserController()
        {
            this._usersRepository = new UserRepository();
            this._projectsRepo = new ProjectsRepository();
        }

        // GET: User
        public ActionResult Index()
        {
            var userViewModel = new UserViewModel();
            var users = this._usersRepository.GetAll();
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
            var user = this._usersRepository.Get(model.UserId);
            WorkTimeEntry wtEntry = null;
            WorkTimeEntry startetEntry = null;
            ViewArctionResults result = ViewArctionResults.Success;
            List<WorkTimeEntry> startedEntries = user.GetTodayStartedEntries(); 

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
                var userViewModel = new UserViewModel(userId, month);

                userViewModel.User = this._usersRepository.Get(userId);

                var projectsSelectListItems = new List<SelectListItem>();

                // Add all user projects
                foreach (var pr in _projectsRepo.GetByUserId(userId))
                {
                    projectsSelectListItems.Add(new SelectListItem()
                    {
                        Text = pr.Name,
                        Value = pr.Id.ToString()
                    });
                }

                // Adding user independent default projects
                var defaultProjekt = this._projectsRepo.GetByName("Momentan ohne Projekt");
                projectsSelectListItems.Add(new SelectListItem()
                {
                    Text = defaultProjekt.Name,
                    Value = defaultProjekt.Id.ToString()
                });
                defaultProjekt = this._projectsRepo.GetByName("Weiterbildung");
                projectsSelectListItems.Add(new SelectListItem()
                {
                    Text = defaultProjekt.Name,
                    Value = defaultProjekt.Id.ToString()
                });
                defaultProjekt = this._projectsRepo.GetByName("Business Development");
                projectsSelectListItems.Add(new SelectListItem()
                {
                    Text = defaultProjekt.Name,
                    Value = defaultProjekt.Id.ToString()
                });
                defaultProjekt = this._projectsRepo.GetByName("Personalmanagement");
                projectsSelectListItems.Add(new SelectListItem()
                {
                    Text = defaultProjekt.Name,
                    Value = defaultProjekt.Id.ToString()
                });
                defaultProjekt = this._projectsRepo.GetByName("Vorstellungsgespräche");
                projectsSelectListItems.Add(new SelectListItem()
                {
                    Text = defaultProjekt.Name,
                    Value = defaultProjekt.Id.ToString()
                });

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

		public ActionResult ExportXls(UserViewModel model)
		{
			int userId = Int32.Parse((Session["userId"]).ToString());
			int month = model.CurrentMonth;
	    	var userViewModel = new UserViewModel(userId, month);
			userViewModel.User = this._usersRepository.Get(userId);
			userViewModel.WorkDays = WorkTimesBuilder.GetMonth(month);
			var userEntriesForMonth = userViewModel.User.GetTimeEntriesForMonth(month);

			GridView gv = new GridView();
			gv.DataSource = userEntriesForMonth;
			gv.DataBind();
			Response.ClearContent();
			Response.Buffer = true;
			Response.AddHeader("content-disposition", "attachment; filename=TimeTable_" + userViewModel.User.LastName +"-"+ DateTime.Now + ".xls");
			Response.ContentType = "application/ms-excel";
			Response.Charset = "";
			StringWriter sw = new StringWriter();
			HtmlTextWriter htw = new HtmlTextWriter(sw);
			gv.RenderControl(htw);
			Response.Output.Write(sw.ToString());
			Response.Flush();
			Response.End();

			return RedirectToAction("Index");
		}

		public ActionResult DumpExcel(UserViewModel model)
		{
			int userId = Int32.Parse((Session["userId"]).ToString());
			int month = model.CurrentMonth;
			var userViewModel = new UserViewModel(userId, month);
			userViewModel.User = this._usersRepository.Get(userId);
			userViewModel.WorkDays = WorkTimesBuilder.GetMonth(month);
			var userEntriesForMonth = userViewModel.User.GetTimeEntriesForMonth(month);

			GridView gv = new GridView();
			gv.DataSource = userEntriesForMonth;
			gv.DataBind();

			using (var package = new ExcelPackage())
			{
				package.Workbook.Worksheets.Add("TimeTable "+ userViewModel.User.LastName);
				ExcelWorksheet ws = package.Workbook.Worksheets[1];
				ws.Name = "TimeTable"; //Setting Sheet's name
				ws.View.ShowGridLines = false;

				package.Workbook.Calculate();

				ws.Cells.Style.Font.Size = 9; //Default font size for whole sheet
				ws.Cells.Style.Font.Name = "Arial"; //Default Font name for whole sheet
				ws.Column(1).Width = 5.5;
				ws.Column(2).Width = 5.5;
				ws.Column(3).Width = 8;
				ws.Column(4).Width = 8;
				ws.Column(5).Width = 8;
				ws.Column(15).Width = 2;
				ws.Column(16).Width = 2;
				ws.Column(17).Width = 4;
				ws.Column(18).Width = 30;
				ws.Row(4).Height = 25;
				ws.Row(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

				//Merging cells and create a center heading for out table
				ws.Cells[1, 1].Value = "Stundenaufstellung für "+ userViewModel.User.FirstName + " " + userViewModel.User.LastName; // Heading Name
				ws.Cells[1, 1, 1, 22].Merge = true;	//Merge columns start and end range
				ws.Cells[2, 1, 2, 22].Merge = true;	//Merge columns start and end range
				ws.Cells[3, 1, 3, 22].Merge = true;	//Merge columns start and end range
				ws.Cells[4, 1, 4, 2].Merge = true;	//Merge columns start and end range
				ws.Cells[4, 6, 4, 13].Merge = true;	//Merge columns start and end range
				ws.Cells[4, 1, 4, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center
				ws.Cells[1, 1, 5, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center
				ws.Cells["Q5:Q13"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;	// Aligmnet is center
				ws.Cells["S4:V14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;	// Aligmnet is center
				ws.Cells[1, 1, 5, 14].Style.Font.Bold = true; //Font should be bold
												
				ws.Cells[4, 1, 4, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
				ws.Cells[4, 1, 4, 14].Style.Fill.BackgroundColor.SetColor(Color.Navy);
				ws.Cells[4, 1, 4, 14].Style.Font.Color.SetColor(Color.White);

				ws.Cells[4, 1].Value = "Tag";
				ws.Cells[4, 3].Value = "Kommen";
				ws.Cells[4, 4].Value = "Mittag";
				ws.Cells[4, 5].Value = "Gehen";
				ws.Cells[4, 6].Value = "Zeit pro Projekt";
				ws.Cells[4, 14].Value = "Gesamt";

				ws.Cells[5, 6].Value = ws.Cells["Q5"].Value = "#1";
				ws.Cells[5, 7].Value = ws.Cells["Q6"].Value = "#2";
				ws.Cells[5, 8].Value = ws.Cells["Q7"].Value = "#3";
				ws.Cells[5, 9].Value = ws.Cells["Q8"].Value = "#4";
				ws.Cells[5, 10].Value = ws.Cells["Q9"].Value = "#5";
				ws.Cells[5, 11].Value = ws.Cells["Q10"].Value = "#6";
				ws.Cells[5, 12].Value = ws.Cells["Q11"].Value = "#7";
				ws.Cells[5, 13].Value = ws.Cells["Q12"].Value = "#8";
				ws.Cells["S5:S12"].Style.Numberformat.Format = ws.Cells["U5:U12"].Style.Numberformat.Format = "#,##0.00";
				ws.Cells["S5:S12"].Value = 1;
				ws.Cells["U5:U12"].Value = 0;

				ws.Cells["Q4"].Value = "Projekt / Beschreibung";
				ws.Cells["S4"].Value = "SGL Faktor"; ws.Cells["S4:S12"].Style.Font.Size = 6;
                ws.Cells["T4"].Value = "Stunden geleistet";
				ws.Cells["U4"].Value = "P Faktor"; ws.Cells["U4:U12"].Style.Font.Size = 6;
                ws.Cells["V4"].Value = "P Wert";

				// create a Table with German Holidays
				int days = DateTime.DaysInMonth(model.CurrentYear, month);
				for (int d = 1; d <= days; d++)
				{
					ws.Cells[(d + 5), 1].Value = d;
					ws.Cells[(d + 5), 2].Value = string.Format("{0:ddd}", new DateTime(model.CurrentYear, month, d));
					if (string.Format("{0:ddd}", new DateTime(model.CurrentYear, month, d)).Equals("So") || string.Format("{0:ddd}", new DateTime(model.CurrentYear, month, d)).Equals("Sa"))
					{
						ws.Cells[(d + 5), 1, (d + 5), 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
						ws.Cells[(d + 5), 1, (d + 5), 14].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
						ws.Cells[(d + 5), 1, (d + 5), 14].Style.Font.Color.SetColor(Color.White);
					}

					if (userViewModel.WorkDays[0].AttrGermanHoliday.Count > 0)
					{
						foreach (GermanHoliday holi in userViewModel.WorkDays[0].AttrGermanHoliday)
						{
							if (ws.Cells[(holi.DatumConverted.Value.Day + 5), 2].Comment == null)
							{
								ws.Cells[(holi.DatumConverted.Value.Day + 5), 2].AddComment(holi.Feiertag, "Admin");
							}
							else
							{
								ws.Cells[(holi.DatumConverted.Value.Day + 5), 2].Comment.Text = holi.Feiertag;
								ws.Cells[(holi.DatumConverted.Value.Day + 5), 2].Comment.Author = "Admin";
							}
							ws.Cells[(holi.DatumConverted.Value.Day + 5), 1, (holi.DatumConverted.Value.Day + 5), 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
							ws.Cells[(holi.DatumConverted.Value.Day + 5), 1, (holi.DatumConverted.Value.Day + 5), 14].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
							ws.Cells[(holi.DatumConverted.Value.Day + 5), 1, (holi.DatumConverted.Value.Day + 5), 14].Style.Font.Color.SetColor(Color.White);
						}
					}

					Dictionary<string, int> projects = getPrjk(userEntriesForMonth);
					int r = 5;
					foreach (var project in projects) {
						ws.Cells["R" + r].Value = project.Key.ToString();
						r++;
                    }
                }

				//fill the table with worked times 


				foreach (WorkTimeEntry wtmEntry in userEntriesForMonth) {

					if (wtmEntry.AttrIll)
					{
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 3].Value = " K r a n k !";
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 3, (wtmEntry.WorkDay.Day + 5), 14].Style.Font.Color.SetColor(Color.Red);
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 3, (wtmEntry.WorkDay.Day + 5), 14].Merge = true;	//Merge columns start and end range
					}
					else if (wtmEntry.AttrHoliday)
					{
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 3].Value = " U r l a u b !";
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 3, (wtmEntry.WorkDay.Day + 5), 14].Style.Font.Color.SetColor(Color.Blue);
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 3, (wtmEntry.WorkDay.Day + 5), 14].Merge = true;	//Merge columns start and end range
					}
					else
					{
						
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 3].Value = string.Format("{0:HH:mm}", wtmEntry.WorkStartTime);
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 4].Value = wtmEntry.PauseLength;
//						string converter = @"=TEXT(D"+ (wtmEntry.WorkDay.Day + 5)+@"/1440;""hh: mm"")";
//						ws.Cells[(wtmEntry.WorkDay.Day + 5), 4].Formula = converter;
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 5].Value = string.Format("{0:HH:mm}", wtmEntry.WorkEndTime);
						//ws.Cells[(wtmEntry.WorkDay.Day + 5), 6].Value = wtmEntry.ProjectName;
					}
				}

				//Add Formular in table
				//WENN(UND(ISTZAHL(C11); ISTZAHL(E11)); E11 - C11 - D11; 0)
				//=TEXT(D8/1440;"hh:mm") gibt 00:30
				//=TEXT(((E20*24*60)-(C20*24*60)-(TEXT(D20/1440;"hh:mm")*24*60))/1440;"hh:mm")   passt
				//for (int d = 1; d <= days; d++)
				//{
					//string formular = @"=TEXT(((E6*24*60)-(C6*24*60)-(TEXT(D6/1440;""hh:mm"")*24*60))/1440;""hh:mm"")";
					//ws.Cells["F6"].Formula = formular;
				//}



				// timetable Setting Top/left,right/bottom borders.
				var border = ws.Cells[4, 1, (days + 5), 14].Style.Border;
				border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[4, 1, 4, 14].Style.Border.Top.Style = ExcelBorderStyle.Medium;
				ws.Cells[4, 14, (days + 5), 14].Style.Border.Right.Style = ExcelBorderStyle.Medium;
				ws.Cells[(days + 5), 1, (days + 5), 14].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

				var borderPr = ws.Cells["A5:N5"].Style.Border;
				borderPr.Left.Style = borderPr.Right.Style = ExcelBorderStyle.None;
				
				//projects table Setting Top/left,right/bottom borders.
				var border2 = ws.Cells["Q4:V14"].Style.Border;
				border2.Bottom.Style = border2.Top.Style = border2.Left.Style = border2.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells["Q4:V4"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
				ws.Cells["Q4:Q14"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
				ws.Cells["V4:V14"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
				ws.Cells["Q14:V14"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

				//Summary table Setting Top/left,right/bottom borders.
				var border3 = ws.Cells["Q17:V31"].Style.Border;
				border3.Bottom.Style = border3.Top.Style = border3.Left.Style = border3.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells["Q17:V17"].Style.Border.Top.Style = ws.Cells["Q17:V17"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
				ws.Cells["Q17:Q31"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
				ws.Cells["V17:V31"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
				ws.Cells["Q31:V31"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

				ws.Cells[6, 1, (days + 5), 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center

				var memoryStream = package.GetAsByteArray();
				var fileName = string.Format("TimeTable_{0}-{1:yyyy-MM-dd-HH-mm-ss}.xlsx", userViewModel.User.LastName, DateTime.UtcNow);
				return base.File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

			}
		}

		
		
		private ExcelWorksheet Calculator(ExcelWorksheet ws, WorkTimeEntry wtmEntry) {
			//var projekt;
			//var workStartTimeMin;
			//var workEndTimeMAX;
			//var pauseSum;
			
			return ws;
        }

		private Dictionary<string, int> getPrjk(List<WorkTimeEntry> userEntriesForMonth)
		{
			int prKeyCounter = 6;
			Dictionary<string, int> projectsThisMonth = new Dictionary<string, int>();
			foreach (WorkTimeEntry wtmEntry in userEntriesForMonth)
			{
				if (null != wtmEntry.ProjectName && !projectsThisMonth.ContainsKey(wtmEntry.ProjectName))
				{
					projectsThisMonth.Add(wtmEntry.ProjectName, prKeyCounter);
					prKeyCounter++;
				}
            }
			return projectsThisMonth;
		}

	}
}