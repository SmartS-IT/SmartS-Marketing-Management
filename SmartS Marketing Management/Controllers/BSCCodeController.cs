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
    public class BSCCodeController : Controller
    {
        ICodeServices icodeServices;
        private List<BscCodeModel> _bscCodes;  
        public List<BscCodeModel> bscCodeModels
        {
            get { return _bscCodes; }
            set
            {
                _bscCodes = value;
            }
        }

        public ActionResult BscUpdate()
        {
            if (System.Web.HttpContext.Current.Session["UserName"] == null)
            {
                return RedirectToAction("LoginView", "UserManagment");
            }
            else
            { 
                return View();
            }
        }

        public BSCCodeController()
        {
            
        } 

        [HttpPost]
        public ActionResult InsertNewBSCCode(BscCodeModel data)
        {
            bscCodeModels = FetchAllData(out _);
            var items = bscCodeModels.Where(x => x.Domain.ToUpper() == data.Domain.ToUpper()).Select(y => y);
            if (items.Count() > 0)
            {
                return Json(Helper.Helper.ConvertToJsonString(false, "Domain is already exist", null));
            }

            data.CreatedBy = Convert.ToInt32(Session["User_Id"]);
            data.CreatedOn = DateTime.Now;

            icodeServices = new CodeServicesImpl();
            var status = icodeServices.AddNewBSCCodes(data);
            if (!status)
            {
                return Json(Helper.Helper.ConvertToJsonString(status, "Failed to add code", null));
            }
            return Json(Helper.Helper.ConvertToJsonString(true, "Successfully added", null));
        }

        public JsonResult FetchAllBSCCode()
        {
            if (bscCodeModels==null ||  bscCodeModels.Count == 0)
            {
                bscCodeModels = FetchAllData(out bool status);
                if (status)
                {
                    var jsonResult= Json(bscCodeModels, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            } 
            return Json(bscCodeModels);
        } 

        public JsonResult SearchBSCDomain(string searchData)
        {
            bscCodeModels = FetchAllData(out bool status);
            if (!string.IsNullOrEmpty(searchData))
            { 
                var items = bscCodeModels.Where(x => x.Domain.ToUpper().Contains(searchData.ToUpper())).Select(y => y);
                return Json(items);
            }
            return Json(bscCodeModels);
        }

        public JsonResult NewDomainSearch(bool IsChecked)
        {
            bscCodeModels = FetchAllData(out bool status);
            if (IsChecked)
            {
                var items = bscCodeModels.Where(x => string.IsNullOrEmpty(x.BSC_Code)).Select(y => y);
                return Json(items);
            }
            return Json(bscCodeModels);
        }


        private List<BscCodeModel> FetchAllData(out bool status)
        {
            status = false;
            icodeServices = new CodeServicesImpl();
            return icodeServices.FetchAllBscCodes(out status);
        }

        public ActionResult UpdateBscCode(BscCodeModel data)
        {
            icodeServices = new CodeServicesImpl();
            bscCodeModels = FetchAllData(out bool s);
            var status = false;
            var dt = bscCodeModels.Where(x => x.ID == data.ID);
            if(dt.Any())
            {
                var listLog = new List<TableLog>();
                if(dt.FirstOrDefault().Domain.ToUpper() != data.Domain.ToUpper())
                {
                    listLog.Add(new TableLog()
                    {
                        Table_Identity_Id = 1,
                        Dtls_Id = data.ID,
                        Old_Value = dt.FirstOrDefault().Domain,
                        New_Value = data.Domain,
                        EntryDate = DateTime.Now,
                        UserId = Convert.ToInt32(Session["User_Id"])
                    });
                }

                if (dt.FirstOrDefault().BSC_Code.ToUpper() != data.BSC_Code.ToUpper())
                {
                    listLog.Add(new TableLog()
                    {
                        Table_Identity_Id = 1,
                        Dtls_Id = data.ID,
                        Old_Value = dt.FirstOrDefault().BSC_Code,
                        New_Value = data.BSC_Code,
                        EntryDate = DateTime.Now,
                        UserId = Convert.ToInt32(Session["User_Id"])
                    });
                }

                foreach(var logs in listLog)
                {
                    status = icodeServices.InsertDataLog(logs);
                    if(!status)
                    {
                        return Json(Helper.Helper.ConvertToJsonString(status, "Failed to update log", null));
                    }
                }
            } 

            status = icodeServices.UpdateBscCode(data);
            if(!status)
            {
                return Json(Helper.Helper.ConvertToJsonString(status, "Failed to update code", null));
            }
            return Json(Helper.Helper.ConvertToJsonString(status, "Successfully updated", null));
        } 
    }
}