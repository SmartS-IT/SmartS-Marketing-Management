using SmartS_Marketing_Management.Interfaces;
using SmartS_Marketing_Management.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartS_Marketing_Management.Controllers
{
    public class JobFunctionController : Controller
    {

        ICodeServices icodeServices;

        // GET: JobFunction
        public ActionResult JobFunction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertFunctionAndIndustry(string Function, string Industry)
        {
            string errorString = "Saved successfully..!!";
            bool status = false;

            icodeServices = new CodeServicesImpl();
            if(!string.IsNullOrWhiteSpace(Function) && !string.IsNullOrEmpty(Function))
            {
                status = icodeServices.InsertJobFunction(Function);
                if (!status)
                    errorString = "Error occured to save job function..!!";
            }
            if (!string.IsNullOrWhiteSpace(Industry) && !string.IsNullOrEmpty(Industry))
            {
                status = icodeServices.InsertJobIndustry(Industry);
                if (!status)
                    errorString = "Error occured to save job industry..!!";
            } 

            return Json(Helper.Helper.ConvertToJsonString(true, errorString, null));
        }
    }
}