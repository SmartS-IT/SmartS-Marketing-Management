using SmartS_Marketing_Management.Interfaces; 
using SmartS_Marketing_Management.Services;
using System;  
using System.Web.Mvc; 
using System.Data;
using System.Configuration; 
using System.Text; 
using System.Linq; 

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
            if (System.Web.HttpContext.Current.Session["UserName"] == null)
            {
                return RedirectToAction("LoginView", "UserManagment");
            }  
            return View();
        }

        [HttpPost]
        public JsonResult ExportToExcel(FileDetails file)
        {
            icodeServices = new CodeServicesImpl();
            var fdate = Convert.ToDateTime(file.FromDate);
            var Tdate = Convert.ToDateTime(file.ToDate);
            var data = icodeServices.FetchAllEverlyticData(fdate.ToString("dd-MM-yyyy 00:00:00"), Tdate.ToString("dd-MM-yyyy 23:59:59"), out bool status);

            if (!status)
                return Json(Helper.Helper.ConvertToJsonString(false, "Fetching data failed with exception", null));

            if (data.Rows.Count == 0)
            {
                return Json(Helper.Helper.ConvertToJsonString(false, "Not data found in the date range", null));
            }


            Session["FileData"] = data;
            return Json(Helper.Helper.ConvertToJsonString(true, "", null));
        } 
        public FileResult CreateExcelFile()
        {

            var dt = Session["FileData"] as DataTable;
             
            StringBuilder sb = new StringBuilder();
            //headers
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append(dt.Columns[i]);
                if (i < dt.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(Environment.NewLine);
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sb.Append(value);
                        }
                        else
                        {
                            sb.Append(dr[i].ToString());
                        }
                    }
                    if (i < dt.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append(Environment.NewLine);
            } 

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Everlytic_" + DateTime.Now.ToString("dd_MM_yy_HH_mm_ss") + ".csv");
        }
    }
}