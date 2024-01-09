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
    public class MiscellaneousSearchController : Controller
    {
        public struct FetchDetails
        {
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string Domain { get; set; }

            public string TcName { get; set; }
            public string Modile { get; set; }

            public string FreeZoneCode { get; set; }
        }

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

        // GET: MiscellaneousSearch
        public ActionResult ApplicationSearch()
        {
            return View();
        }

        public JsonResult CodeSelectionChanged(string Codes)
        {
            icodeServices = new CodeServicesImpl();
            if (Codes.ToUpper() == "1")
            {
                var data = icodeServices.FetchAllBscCodes(out bool status);
                if (!status)
                    return Json(Helper.Helper.ConvertToJsonString(status, "Failed to load Codes", null));

                var codes = data?.Select(x => x.Domain.ToLower());
                return Json(Helper.Helper.ConvertToJsonString(status, "Successfully updated", codes));
            }
            else
            {
                var data = icodeServices.FetchAllTcCodes(out bool status);
                if (!status)
                    return Json(Helper.Helper.ConvertToJsonString(status, "Failed to load Codes", null));

                var codes = data?.Select(x => x.TC_Name.ToLower());
                return Json(Helper.Helper.ConvertToJsonString(status, "Successfully updated", codes));
            }
        }

        public JsonResult FetchAllCustomerCode(FetchDetails dtls)
        {
            icodeServices = new CodeServicesImpl();
            var fdate = Convert.ToDateTime(dtls.FromDate);
            var Tdate = Convert.ToDateTime(dtls.ToDate);
            var data= icodeServices.FetchAllApplicationData(fdate.ToString("dd-MM-yyyy 00:00:00"), Tdate.ToString("dd-MM-yyyy 23:59:59"),
                                                          dtls.Domain,dtls.TcName,dtls.Modile, dtls.FreeZoneCode, out bool status);
            if(!status)
            {
                return Json(Helper.Helper.ConvertToJsonString(status, "Failed to fetch the data", ""));
            } 
            return Json(Helper.Helper.ConvertToJsonString(status, "", data));
        }
    }
}