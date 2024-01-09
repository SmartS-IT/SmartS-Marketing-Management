using SmartS_Marketing_Management.Interfaces; 
using SmartS_Marketing_Management.Services;
using System;  
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO; 
using System.Data;
using System.Configuration;

namespace SmartS_Marketing_Management.Controllers
{
    public class HomeController : Controller
    {

        ICodeServices icodeServices;
        public struct FileDetails
        {
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string FilePath { get; set; }
        }
        public HomeController()
        { 
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult EverlyticView()
        { 
            TempData["DefaultFilePath"] = ConfigurationManager.AppSettings["DefaultFilePath"];
            return View();
        }

        [HttpPost]
        public ActionResult ExportToExcel(FileDetails file)
        {
            icodeServices = new CodeServicesImpl();
            var fdate = Convert.ToDateTime(file.FromDate);
            var Tdate = Convert.ToDateTime(file.ToDate);
            var data = icodeServices.FetchAllEverlyticData(fdate.ToString("dd-MM-yyyy 00:00:00"), Tdate.ToString("dd-MM-yyyy 23:59:59"), out bool status);
            if (!status)
                return Json(Helper.Helper.ConvertToJsonString(false,"Fetching data failed with exception", null));

            if(data.Rows.Count == 0)
            {
                return Json(Helper.Helper.ConvertToJsonString(false, "Not data found in the date range", null));
            }

            status = CreateExcelFile(data, file.FilePath, out string filename);
            if (status)
            {
               return Json(Helper.Helper.ConvertToJsonString(status, "File saved successfully in the location :-"+ filename, null));
            }
            return Json(status);
        }

        public bool CreateExcelFile(DataTable dt, string filePath, out string filename)
        {
            filename = filePath + @"/Everlytic_" + DateTime.Now.ToString("dd-MM-yy-HH-mm-ss")+ ".xlsx";
            try 
            { 
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Everlytic"); 
                    wb.SaveAs(filename); 
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}