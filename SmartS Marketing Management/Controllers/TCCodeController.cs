﻿using SmartS_Marketing_Management.Interfaces;
using SmartS_Marketing_Management.Models;
using SmartS_Marketing_Management.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartS_Marketing_Management.Controllers
{
    public class TCCodeController : Controller
    {
        ICodeServices icodeServices;
        private List<TcCodeModel> _tcCodes; 

        public List<TcCodeModel> tcCodeModels
        {
            get { return _tcCodes; }
            set
            {
                _tcCodes = value;
            }
        }    
        public ActionResult TcCodeUpdate()
        {
            if (System.Web.HttpContext.Current.Session["UserName"] == null)
            {
                return RedirectToAction("LoginView", "UserManagment");
            }

            ViewData["Page"] = "TCUPDATE"; 
            return View();
        }  

        public TCCodeController()
        {
        }

        private List<TcCodeModel> FetchAllTcData(out bool status)
        {
            status = false;
            icodeServices = new CodeServicesImpl();
            return icodeServices.FetchAllTcCodes(out status);
        } 

        [HttpPost]
        public ActionResult InsertNewTCCode(TcCodeModel data)
        {
            tcCodeModels = FetchAllTcData(out _);
            var items = tcCodeModels.Where(x => x.TC_Name.ToUpper()== data.TC_Name.ToUpper()).Select(y => y);
            if (items?.Count() > 0)
            {
                return Json(Helper.Helper.ConvertToJsonString(false, "Name is already exist", null));
            }

            data.CreatedBy = Convert.ToInt32(Session["User_Id"]);
            data.CreatedOn = DateTime.Now;
            icodeServices = new CodeServicesImpl();
            var status = icodeServices.AddNewTCCodes(data);
            if (!status)
            {
                return Json(Helper.Helper.ConvertToJsonString(status, "Failed to insert code", null));
            }
            return Json(Helper.Helper.ConvertToJsonString(true, "Successfully added", null));
        }

        public JsonResult FetchAllTCCode()
        {
            tcCodeModels = FetchAllTcData(out bool status);
            if (status)
            {
                return Json(tcCodeModels, JsonRequestBehavior.AllowGet);
            }
            return Json("");
        }

        public ActionResult UpdateTcCode(TcCodeModel data)
        {
            icodeServices = new CodeServicesImpl();
            tcCodeModels = FetchAllTcData(out _);
            var status = false;
            var dt = tcCodeModels.Where(x => x.ID == data.ID);
            if (dt.Any())
            {
                var listLog = new List<TableLog>();
                if (dt.FirstOrDefault().TC_Name.ToUpper() != data.TC_Name.ToUpper())
                {
                    listLog.Add(new TableLog()
                    {
                        Table_Identity_Id = 2,
                        Dtls_Id = data.ID,
                        Old_Value = dt.FirstOrDefault().TC_Name,
                        New_Value = data.TC_Name,
                        EntryDate = DateTime.Now,
                        UserId = Convert.ToInt32(Session["User_Id"])
                    });
                }

                if (dt.FirstOrDefault().TC_Code.ToUpper() != data.TC_Code.ToUpper())
                {
                    listLog.Add(new TableLog()
                    {
                        Table_Identity_Id = 2,
                        Dtls_Id = data.ID,
                        Old_Value = dt.FirstOrDefault().TC_Code,
                        New_Value = data.TC_Code,
                        EntryDate = DateTime.Now,
                        UserId = Convert.ToInt32(Session["User_Id"])
                    });
                }

                foreach (var logs in listLog)
                {
                    status = icodeServices.InsertDataLog(logs);
                    if (!status)
                    {
                        return Json(Helper.Helper.ConvertToJsonString(status, "Failed to update log", null));
                    }
                }
            }

            status = icodeServices.UpdateTcCode(data);
            if (!status)
            {
                return Json(Helper.Helper.ConvertToJsonString(status, "Failed to update code", null));
            }
            return Json(Helper.Helper.ConvertToJsonString(true, "Successfully updated", null));
        }

        public JsonResult SearchTCName(string searchData)
        {
            tcCodeModels = FetchAllTcData(out bool status);
            if (!string.IsNullOrEmpty(searchData))
            {
                var items = tcCodeModels.Where(x => x.TC_Name.ToUpper().Contains(searchData.ToUpper())).Select(y => y);
                return Json(items);
            }
            return Json(tcCodeModels);
        }

        public JsonResult NewTCNameSearch(bool IsChecked)
        {
            tcCodeModels = FetchAllTcData(out bool status);
            if (IsChecked && status)
            {
                var items = tcCodeModels.Where(x => string.IsNullOrEmpty(x.TC_Code)).Select(y => y);
                return Json(items);
            }
            return Json(tcCodeModels);
        }
    }
}