using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
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

			holidayrep.SetModified(selectedHoliday);
			holidayrep.SaveChanges();

            holidayrep.UpdateHolidayOnCacheById(id);

			return Json(selectedHoliday, JsonRequestBehavior.AllowGet);
		}


		public void ExportXls(FeierTag model)
		{

			WebGrid grid = new WebGrid(source: model.feiertage, canPage: false, canSort: false);
			string gridData = grid.GetHtml(mode: WebGridPagerModes.All,
							tableStyle: "webgrid-table",
							headerStyle: "webgrid-header",
							footerStyle: "webgrid-footer",
							alternatingRowStyle: "webgrid-alternating-row",
							selectedRowStyle: "webgrid-selected-row",
							rowStyle: "webgrid-row-style",
							fillEmptyRows: false,
							columns: grid.Columns(
							 grid.Column("Id", header: "Id", style: "Id"),
							 grid.Column("feiertag", header: "Feiertag"),
							 grid.Column("datumConverted", header: "Datum", format: (item) => string.Format("{0:dd.MM.yyyy}", item.datumConverted)),
							 grid.Column("art", header: "Art"),
							 grid.Column("Länder", "Bundesland", format: (item) =>
							 {
								 if (item.Länder == null)
								 {
									 return String.Empty;
								 }
								 else
								 {
									 string tmp = "<div style=''>";
									 foreach (var land in item.Länder)
									 {
										 tmp += land + "<br/>";
									 }
									 tmp += "</div>";
									 return new HtmlString(tmp);
								 }
							 }),
							 grid.Column("festgelegt", header: "Festgelegt", style: "center festgelegt")
						)).ToString();

			Response.ClearContent();
			Response.AddHeader("content-disposition", "attachment; filename=AllHolidays.xls");
			Response.ContentType = "application/excel";
			Response.Write(gridData);
			Response.End();

		}

		public ActionResult Export(FeierTag model)
		{

			GridView gv = new GridView();
			gv.DataSource = model.feiertage.ToList();
			gv.DataBind();
			Response.ClearContent();
			Response.Buffer = true;
			Response.AddHeader("content-disposition", "attachment; filename=AllHolidays-" + DateTime.Now + ".xls");
			Response.ContentType = "application/ms-excel";
			Response.Charset = "";
			StringWriter sw = new StringWriter();
			HtmlTextWriter htw = new HtmlTextWriter(sw);
			gv.RenderControl(htw);
			Response.Output.Write(sw.ToString());
			Response.Flush();
			Response.End();

			return RedirectToAction("Index", "Admin");
		}

		public ActionResult DumpExcel(FeierTag model)
		{
			using (var package = new ExcelPackage())
			{
				package.Workbook.Worksheets.Add("AllHolidays");
				ExcelWorksheet ws = package.Workbook.Worksheets[1];
				ws.Name = "AllHolidays"; //Setting Sheet's name
				ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
				ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

				//Merging cells and create a center heading for out table
				ws.Cells[1, 1].Value = "All German Golidays"; // Heading Name
				ws.Cells[1, 1, 1, 10].Merge = true;	//Merge columns start and end range
				ws.Cells[1, 1, 1, 10].Style.Font.Bold = true; //Font should be bold
				ws.Cells[1, 1, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Aligmnet is center

				for (var i = 1; i < 11; i++)
				{
					for (var j = 2; j < 45; j++)
					{
						var cell = ws.Cells[j, i];

						//Setting Value in cell
						cell.Value = i * (j - 1);
					}
				}

				var chart = ws.Drawings.AddChart("chart1", eChartType.AreaStacked);
				//Set position and size
				chart.SetPosition(0, 630);
				chart.SetSize(800, 600);

				// Add the data series. 
				var series = chart.Series.Add(ws.Cells["A2:A46"], ws.Cells["B2:B46"]);

				var memoryStream = package.GetAsByteArray();
				var fileName = string.Format("MyData-{0:yyyy-MM-dd-HH-mm-ss}.xlsx", DateTime.UtcNow);
				return base.File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

			}
		}

		public ActionResult ExportPdf(FeierTag model)
		{

			WebGrid grid = new WebGrid(source: model.feiertage, canPage: false, canSort: false);
			string gridData = grid.GetHtml(mode: WebGridPagerModes.All,
							tableStyle: "webgrid-table",
							headerStyle: "webgrid-header",
							footerStyle: "webgrid-footer",
							alternatingRowStyle: "webgrid-alternating-row",
							selectedRowStyle: "webgrid-selected-row",
							rowStyle: "webgrid-row-style",
							fillEmptyRows: false,
							columns: grid.Columns(
							 grid.Column("Id", header: "Id", style: "Id"),
							 grid.Column("feiertag", header: "Feiertag"),
							 grid.Column("datumConverted", header: "Datum", format: (item) => string.Format("{0:dd.MM.yyyy}", item.datumConverted)),
							 grid.Column("art", header: "Art"),
							 grid.Column("Länder", "Bundesland", format: (item) =>
							 {
								 if (item.Länder == null)
								 {
									 return String.Empty;
								 }
								 else
								 {
									 string tmp = "<div style=''>";
									 foreach (var land in item.Länder)
									 {
										 tmp += land + "<br/>";
									 }
									 tmp += "</div>";
									 return new HtmlString(tmp);
								 }
							 }),
							 grid.Column("festgelegt", header: "Festgelegt", style: "center festgelegt")
						)).ToString();

			    //string exportData = String.Format("<html><head>{0}</head><body>{1}</body></html>", "<style>table{ border-spacing: 10px; border-collapse: separate; }</style>", gridData);
				string exportData = gridData;
				//var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);

				//PdfDocument document = new PdfDocument();
				//document.Info.Title = "Created with PDFsharp";
				//// Create an empty page 
				//PdfPage page = document.AddPage();
				//// Get an XGraphics object for drawing 
				//XGraphics gfx = XGraphics.FromPdfPage(page);
				//// Create a font 
				//XFont font = new XFont("Verdana", 11, XFontStyle.BoldItalic);
				//// Draw the text 
				//gfx.DrawString(exportData, font, XBrushes.Black,
				//new XRect(0, 0, page.Width, page.Height),
				//XStringFormats.Center);
				//// Save the document... 
				//const string filename = "AllHolidays.pdf";
				//document.Save(filename);
				//// ...and start a viewer. 
				//Process.Start(filename);

			//	var pdfModel = new GeneratePDFModel();
				return new Rotativa.ViewAsPdf("GeneratePDF", model) { FileName = "TestViewAsPdf.pdf" };

		}


	}
}