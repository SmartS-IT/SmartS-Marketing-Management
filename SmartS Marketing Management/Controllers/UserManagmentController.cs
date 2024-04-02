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
    public class UserManagmentController : Controller
    {

        ICodeServices icodeServices;

        // GET: LoginView
        public ActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckAuthentication(string username, string password)
        {
            icodeServices = new CodeServicesImpl();
            var  userDetails =  icodeServices.FetchUserDetails(username, out bool status);
            if(status && userDetails.Count()>0)
            {
                if(userDetails.FirstOrDefault().IsValid!=1)
                {
                    return Json(Helper.Helper.ConvertToJsonString(false, "User is not active", ""));
                }
                else if(password== userDetails.FirstOrDefault().Password)
                {
                    System.Web.HttpContext.Current.Session["UserName"] = username;
                    System.Web.HttpContext.Current.Session["Name"] = userDetails.FirstOrDefault().Name;
                    System.Web.HttpContext.Current.Session["User_Id"] = userDetails.FirstOrDefault().Id;
                    return Json(Helper.Helper.ConvertToJsonString(true, "", "/BSCCode/BscUpdate"));
                }
                else
                {
                    return Json(Helper.Helper.ConvertToJsonString(false, "Invalid Username or Password", ""));
                }
                
            }
            return Json(Helper.Helper.ConvertToJsonString(false, "Invalid Username or Password", ""));
        }

        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.Session["UserName"] = null;
            System.Web.HttpContext.Current.Session["Name"] = null;
            System.Web.HttpContext.Current.Session["User_Id"] = null;
            return RedirectToAction("LoginView", "UserManagment");
        }
    }
}