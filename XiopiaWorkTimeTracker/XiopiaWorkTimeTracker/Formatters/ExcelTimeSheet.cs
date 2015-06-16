using OfficeOpenXml;
using OfficeOpenXml.Style;
using Ressources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using XiopiaWorkTimeTracker.Models;
using XiopiaWorkTimeTracker.Models.Database;
using XiopiaWorkTimeTracker.Models.Repositories;

namespace XiopiaWorkTimeTracker.Formatters
{
	public class ExcelTimeSheet
	{
		private UserViewModel userViewModel;
		private bool all;

		public ExcelTimeSheet(UserViewModel userViewModel)
		{
			this.userViewModel = userViewModel;
		}

		public Byte[] DumpExcel()
		{
			using (var package = new ExcelPackage())
			{
				for (int month = 1; month < 13; month++)
				{
					FillWorksheet(package, month);
				}
				return package.GetAsByteArray();
			}
		}

		public Byte[] DumpExcel(int month)
		{
			using (var package = new ExcelPackage())
			{
				FillWorksheet(package, month);
				return package.GetAsByteArray();
			}
		}
		private void FillWorksheet(ExcelPackage package, int month)
        {
			var userEntriesForMonth = userViewModel.User.GetTimeEntriesForMonth(month);

			string myMonat = string.Format("{0:MMMM}", new DateTime(userViewModel.CurrentYear, month, 1));
			ExcelWorksheet ws = package.Workbook.Worksheets.Add(myMonat);
			ws.View.ShowGridLines = false;

			ws.Cells.Style.Font.Size = 9; //Default font size for whole sheet
			ws.Cells.Style.Font.Name = "Arial";	//Default Font name for whole sheet
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
			ws.Cells[1, 1].Value = string.Format("Stundenaufstellung für den Monat {0} {1}", myMonat ,userViewModel.CurrentYear); // Heading Name
			ws.Cells[2, 1].Value = userViewModel.User.FirstName + " " + userViewModel.User.LastName;
            ws.Cells[1, 1, 1, 22].Merge = true;	//Merge columns start and end range
			ws.Cells[2, 1, 2, 22].Merge = true;	//Merge columns start and end range
			ws.Cells[3, 1, 3, 22].Merge = true;	//Merge columns start and end range
			ws.Cells[4, 1, 4, 2].Merge = true;	//Merge columns start and end range
			ws.Cells[4, 6, 4, 13].Merge = true;	//Merge columns start and end range
			ws.Cells[4, 1, 4, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center
			ws.Cells[1, 1, 5, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center
			ws.Cells["Q5:Q13"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;	// Aligmnet is center
			ws.Cells["S4:V14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;	// Aligmnet is center
			ws.Cells["A1:N5"].Style.Font.Bold = true; //Font should be bold

			ExcelRange range = ws.Cells[4, 1, 4, 14];
			range.Style.Fill.PatternType = ExcelFillStyle.Solid;
			range.Style.Fill.BackgroundColor.SetColor(Color.Navy);
			range.Style.Font.Color.SetColor(Color.White);

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
			ws.Cells["S4"].Value = "SGL" + Environment.NewLine + "Faktor"; ws.Cells["S4:S12"].Style.Font.Size = 6; ws.Cells["S4"].Style.WrapText = true;
			ws.Cells["T4"].Value = "Stunden Geleistet"; ws.Cells["T4"].Style.WrapText = true;
			ws.Cells["U4"].Value = "P" + Environment.NewLine + "Faktor"; ws.Cells["U4:U12"].Style.Font.Size = 6; ws.Cells["U4"].Style.WrapText = true;
			ws.Cells["V4"].Value = "P" + Environment.NewLine + "Wert"; ws.Cells["V4"].Style.WrapText = true;

			// create a Table with German Holidays
			Dictionary<string, int> projects = new Dictionary<string, int>();
			int days = DateTime.DaysInMonth(userViewModel.CurrentYear, month);
			Dictionary<Int32, string> holidays = new Dictionary<int, String>();

			GermanHolidayRepository germanholidayrep = new GermanHolidayRepository();
			Dictionary<int, GermanHoliday> germanHolidays = germanholidayrep.AllHolidays;
			var context = new WorkTimeTrackerDbContext();
			int LandId = context.GlobalSettings.First().GermanStateId;

			foreach (var id in germanholidayrep.AllHolidaysByMonthAccepted[month])
			{
				String Name;
				if(germanholidayrep.TryGetFeiertag(germanHolidays[id].DatumConverted.Value, LandId, out Name))
					holidays.Add(germanHolidays[id].DatumConverted.Value.Day, Name);
			}

			for (int d = 1; d <= days; d++)
			{
				ws.Cells[(d + 5), 1].Value = d;
				ws.Cells[(d + 5), 2].Value = string.Format("{0:ddd}", new DateTime(userViewModel.CurrentYear, month, d));
				if (string.Format("{0:ddd}", new DateTime(userViewModel.CurrentYear, month, d)).Equals("So") || string.Format("{0:ddd}", new DateTime(userViewModel.CurrentYear, month, d)).Equals("Sa") || holidays.ContainsKey(d))
				{
					ws.Cells["A" + (d + 5) + ":N" + (d + 5)].Style.Fill.PatternType = ExcelFillStyle.Solid;
					ws.Cells["A" + (d + 5) + ":N" + (d + 5)].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
				}

				if (holidays.ContainsKey(d))
				{
					if (ws.Cells[(d + 5), 2].Comment == null)
					{
						ws.Cells[(d + 5), 2].AddComment(holidays[d], "Admin");
					}
					else
					{
						ws.Cells[(d + 5), 2].Comment.Text = holidays[d];
						ws.Cells[(d + 5), 2].Comment.Author = "Admin";
					}
				}

				projects = getPrjk(userEntriesForMonth);
				int r = 5;
				foreach (var project in projects)
				{
					ws.Cells["R" + r].Value = project.Key.ToString();
					r++;
				}

				var c = ws.Cells["N" + (d + 5)];
				c.Formula = "=SUM(F" + (d + 5) + ":M" + (d + 5) + ")";
				c.Calculate();
				c.Style.Numberformat.Format = "[HH]:mm";

			}

			int curentDay = 0;
			//fill the table with worked times 
			foreach (WorkTimeEntry wtmEntry in userEntriesForMonth)
			{
				if (wtmEntry.AttrIll)
				{
					ws.Cells[(wtmEntry.WorkDay.Day + 5), 3].Value = " K r a n k !";
					ws.Cells[(wtmEntry.WorkDay.Day + 5), 3, (wtmEntry.WorkDay.Day + 5), 13].Style.Font.Color.SetColor(Color.Red);
					ws.Cells[(wtmEntry.WorkDay.Day + 5), 3, (wtmEntry.WorkDay.Day + 5), 13].Merge = true;	//Merge columns start and end range
				}
				else if (wtmEntry.AttrHoliday)
				{
					ws.Cells[(wtmEntry.WorkDay.Day + 5), 3].Value = " U r l a u b !";
					ws.Cells[(wtmEntry.WorkDay.Day + 5), 3, (wtmEntry.WorkDay.Day + 5), 13].Style.Font.Color.SetColor(Color.Blue);
					ws.Cells[(wtmEntry.WorkDay.Day + 5), 3, (wtmEntry.WorkDay.Day + 5), 13].Merge = true;	//Merge columns start and end range
				}
				else
				{
					List<string> times = GetCalculatedUserEntrieForDay(userEntriesForMonth, wtmEntry.WorkDay.Day);
					if (curentDay != wtmEntry.WorkDay.Day)
					{
						curentDay = wtmEntry.WorkDay.Day;

						ws.Cells[(wtmEntry.WorkDay.Day + 5), 3].Value = string.Format("{0:HH:mm}", DateTime.Parse(times[0]));
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 4].Value = string.Format("{0:HH:mm}", DateTime.Parse(Math.Floor(Decimal.Parse(times[1]) / 60) + ":" + Int32.Parse(times[1]) % 60));
						ws.Cells[(wtmEntry.WorkDay.Day + 5), 5].Value = string.Format("{0:HH:mm}", DateTime.Parse(times[2]));
					}

					foreach (var project in projects)
					{
						if (project.Key.Equals(wtmEntry.ProjectName))
						{
							DateTime pausetime = DateTime.Parse(Math.Floor((decimal)wtmEntry.PauseLength.Value / 60) + ":" + (wtmEntry.PauseLength.Value % 60));
							var span = (wtmEntry.WorkEndTime.Value - wtmEntry.WorkStartTime.Value) - new TimeSpan(pausetime.Hour, pausetime.Minute, pausetime.Second);
							DateTime oldValue;
							if (ws.Cells[(wtmEntry.WorkDay.Day + 5), project.Value].Value == null || !DateTime.TryParse(ws.Cells[(wtmEntry.WorkDay.Day + 5), project.Value].Value.ToString(), out oldValue))
							{
								ws.Cells[(wtmEntry.WorkDay.Day + 5), project.Value].Value = span;
							}
							else
							{
								ws.Cells[(wtmEntry.WorkDay.Day + 5), project.Value].Value = new TimeSpan(oldValue.Hour, oldValue.Minute, oldValue.Second) + span;
							}
							ws.Cells[(wtmEntry.WorkDay.Day + 5), project.Value].Style.Numberformat.Format = "HH:mm";
						}
					}

				}
			}

			// Add all Sum formula
			for (int p = 6; p <= 14; p++)
			{
				var sm = ws.Cells[(days + 6), p];
				var str = ws.Cells[6, p];
				var end = ws.Cells[(days + 5), p];
				var copy = ws.Cells["T" + (p - 1)];
				var pWert = ws.Cells["V" + (p - 1)];
				sm.Formula = "=SUM(" + str + ":" + end + ")";
				copy.Formula = "=SUM(" + str + ":" + end + ")";
				pWert.Formula = "=ROUND(DAY(F" + (days + 6) + ") * 24 + HOUR(F" + (days + 6) + ") + MINUTE(F" + (days + 6) + ") / 60, 2) * U" + (p - 1);
				sm.Calculate();
				copy.Calculate();
				pWert.Calculate();
				sm.Style.Numberformat.Format = "[HH]:mm";
				copy.Style.Numberformat.Format = "[HH]:mm";
				pWert.Style.Numberformat.Format = "[HH]:mm"; ;
			}

			ws.Cells[(days + 6), 6, (days + 6), 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


			// timetable Setting Top/left,right/bottom borders.
			var border = ws.Cells[4, 1, (days + 6), 14].Style.Border;
			border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
			ws.Cells[4, 1, 4, 14].Style.Border.Top.Style = ExcelBorderStyle.Medium;
			ws.Cells[4, 14, (days + 6), 14].Style.Border.Right.Style = ExcelBorderStyle.Medium;
			ws.Cells[(days + 6), 1, (days + 6), 14].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
			ws.Cells[(days + 6), 1, (days + 6), 14].Style.Font.Bold = true;

			var borderPr = ws.Cells["A5:N5"].Style.Border;
			borderPr.Left.Style = borderPr.Right.Style = ExcelBorderStyle.None;

			//projects table Setting Top/left,right/bottom borders / Merges / Bolds.
			var border2 = ws.Cells["Q4:V14"].Style.Border;
			border2.Bottom.Style = border2.Top.Style = border2.Left.Style = border2.Right.Style = ExcelBorderStyle.Thin;
			ws.Cells["Q4:V4"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
			ws.Cells["Q4:Q14"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
			ws.Cells["V4:V14"].Style.Border.Right.Style = ws.Cells["Q4:Q14"].Style.Border.Right.Style = ws.Cells["S4:S14"].Style.Border.Right.Style =
			ws.Cells["T4:T14"].Style.Border.Right.Style = ws.Cells["U4:U14"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
			ws.Cells["Q14:V14"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
			ws.Cells["Q13:R14"].Merge = true; ws.Cells["S13:S14"].Merge = true; ws.Cells["T13:T14"].Merge = true; ws.Cells["V13:V14"].Merge = true; ws.Cells["U13:U14"].Merge = true;
			ws.Cells["T13"].Style.Font.Bold = ws.Cells["V13"].Style.Font.Bold = ws.Cells["T4"].Style.Font.Bold = ws.Cells["V4"].Style.Font.Bold = true;

			//Summary table Setting Top/left,right/bottom borders.
			var border3 = ws.Cells["Q17:V31"].Style.Border;
			border3.Bottom.Style = border3.Top.Style = border3.Left.Style = border3.Right.Style = ExcelBorderStyle.Thin;
			ws.Cells["Q17:V17"].Style.Border.Top.Style = ws.Cells["Q17:V17"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
			ws.Cells["Q17:Q31"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
			ws.Cells["V17:V31"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
			ws.Cells["Q31:V31"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

			ws.Cells[6, 1, (days + 5), 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;	// Aligmnet is center
		}



		public List<string> GetCalculatedUserEntrieForDay(List<WorkTimeEntry> userEntriesForMonth, int day)
		{
			List<WorkTimeEntry> entryForDay = new List<WorkTimeEntry>();
			List<string> calculatedTimesForDay = new List<string>();
			foreach (WorkTimeEntry timeEntry in userEntriesForMonth)
			{
				if (null != timeEntry && timeEntry.WorkDay.Day == day)
				{
					entryForDay.Add(timeEntry);
				}
			}
			if (entryForDay.Count > 0)
			{
				var startv = entryForDay[0].WorkStartTime.Value;
				var pausev = 0;
				var endv = entryForDay[0].WorkStartTime != null ? entryForDay[0].WorkStartTime.Value : startv;

				foreach (var entry in entryForDay)
				{
					if (entry.WorkStartTime.Value < startv) startv = entry.WorkStartTime.Value;
					if (entry.WorkEndTime.Value > endv) endv = entry.WorkEndTime.Value;
					if (null != entry.PauseLength) pausev += entry.PauseLength.Value;
				}
				calculatedTimesForDay.Add(startv.ToString());
				calculatedTimesForDay.Add(pausev.ToString());
				calculatedTimesForDay.Add(endv.ToString());

			}
			return calculatedTimesForDay;
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