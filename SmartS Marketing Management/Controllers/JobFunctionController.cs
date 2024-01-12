using SmartS_Marketing_Management.Interfaces;
using SmartS_Marketing_Management.Models;
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
        private List<JobFunction> _jobFunction;
        public List<JobFunction> JobFunctions
        {
            get { return _jobFunction; }
            set
            {
                _jobFunction = value;
            }
        }

        private List<JobIndustry> _jobInd;
        public List<JobIndustry> JobIndustry
        {
            get { return _jobInd; }
            set
            {
                _jobInd = value;
            }
        }

        private List<JobDetails> _jobDetails;
        public List<JobDetails> JobDetails
        {
            get { return _jobDetails; }
            set
            {
                _jobDetails = value;
            }
        }

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
                var data = icodeServices.FetchAllJobFunctions(0, out status);
                if (status && data.Item1.Count > 0)
                {
                    var items = data.Item1.Where(x => x.FunctionName.ToUpper().Contains(Function.ToUpper())).Select(y => y);
                    if (items.Count() > 0)
                    {
                        return Json(Helper.Helper.ConvertToJsonString(false, "Function is already exist", null));
                    } 
                }

                status = icodeServices.InsertJobFunction(Function);
                if (!status)
                    return Json(Helper.Helper.ConvertToJsonString(status, "Error occured to save job function..!!", null));
            }
            if (!string.IsNullOrWhiteSpace(Industry) && !string.IsNullOrEmpty(Industry))
            {
                var data = icodeServices.FetchAllJobFunctions(1, out status);
                if (status && data.Item2.Count > 0)
                {
                    var items = data.Item2.Where(x => x.IndustryName.ToUpper().Contains(Industry.ToUpper())).Select(y => y);
                    if (items.Count() > 0)
                    {
                        return Json(Helper.Helper.ConvertToJsonString(false, "Industry is already exist", null));
                    }
                }

                status = icodeServices.InsertJobIndustry(Industry);
                if (!status)
                    return Json(Helper.Helper.ConvertToJsonString(status, "Error occured to save job function..!!", null));
            } 

            return Json(Helper.Helper.ConvertToJsonString(status, errorString, null));
        }

        public JsonResult FetchJobFunctionOrIndustry(int mode)
        {
            bool status = false;
            icodeServices = new CodeServicesImpl();
            var data = icodeServices.FetchAllJobFunctions(mode, out status); 
            if(!status)
            {
                return Json(Helper.Helper.ConvertToJsonString(true, "Failed to fetch job details", null));
            }

            if(mode == 0)
            {
                return Json(Helper.Helper.ConvertToJsonString(true, "", data.Item1 ));
            }
            return Json(Helper.Helper.ConvertToJsonString(true, "", data.Item2));

        }

        public JsonResult UpdateJobDetails(JobDetails jobFunctions)
        {
            bool status = false;
            icodeServices = new CodeServicesImpl();
            status = icodeServices.UpdateJobDetails(jobFunctions);
            if (!status)
            {
                return Json(Helper.Helper.ConvertToJsonString(false, "Failed to insert job details", null));
            } 
            return Json(Helper.Helper.ConvertToJsonString(true, "Updated Successfully","")); 
        }

        public JsonResult FetchAllJobDetails()
        {
            bool status = false;
            icodeServices = new CodeServicesImpl();
            var data = icodeServices.FetchAllJobDetails(out status);
            if (!status)
            {
                return Json(Helper.Helper.ConvertToJsonString(false, "Failed to insert job details", null));
            }
            return Json(Helper.Helper.ConvertToJsonString(true, "", data));
        }

        public JsonResult SearchJobs(string searchData)
        {
            icodeServices = new CodeServicesImpl();
            JobDetails = icodeServices.FetchAllJobDetails(out bool status);
            if (!string.IsNullOrEmpty(searchData) && status)
            {
                var items = JobDetails.Where(x => x.Title.ToUpper().Contains(searchData.ToUpper())).Select(y => y);
                return Json(items);
            }
            return Json(JobDetails);
        }

        public JsonResult NewJobSearch(bool IsChecked)
        {
            icodeServices = new CodeServicesImpl();
            JobDetails = icodeServices.FetchAllJobDetails(out bool status);
            if (IsChecked)
            {
                var items = JobDetails.Where(x => string.IsNullOrEmpty(x.FunctionName) || string.IsNullOrEmpty(x.IndustryName)).Select(y => y);
                return Json(items);
            }
            return Json(JobDetails);
        }
    }
}