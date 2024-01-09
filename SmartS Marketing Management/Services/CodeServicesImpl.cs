using SmartS_Marketing_Management.Connectivity;
using SmartS_Marketing_Management.Interfaces;
using SmartS_Marketing_Management.Models;
using SmartS_Marketing_Management.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SmartS_Marketing_Management.Services
{
    public class CodeServicesImpl: ICodeServices
    {
        DBConnectivityImpl dBConnectivityImpl;

        public CodeServicesImpl()
        {
            dBConnectivityImpl = new DBConnectivityImpl();
        }
        public bool AddNewBSCCodes(BscCodeModel bscCodeModel)
        {
            try
            {
                return dBConnectivityImpl.AddNewBSCCodes(bscCodeModel);
            }
            catch
            {
                return false;
            }
        }

        public List<BscCodeModel> FetchAllBscCodes(out bool status)
        {

            status = true;
            var bscCodes = new List<BscCodeModel>();
            try
            {
                var dt = dBConnectivityImpl.FetchAllBscCodes(out status);
                bscCodes = Helper.Helper.ConvertToList<BscCodeModel>(dt);
               
            }
            catch
            {
                status = false;
            }
            return bscCodes;
        }

        public bool UpdateBscCode(BscCodeModel bscCodeModel)
        {
            try
            {
                return dBConnectivityImpl.UpdateBSCCodes(bscCodeModel); 

            }
            catch
            {
                return false;
            } 
        }

        public bool AddNewTCCodes(TcCodeModel tcCodeModel)
        {
            try
            {
                return dBConnectivityImpl.AddNewTcCodes(tcCodeModel);
            }
            catch
            {
                return false;
            }
        }

        public List<TcCodeModel> FetchAllTcCodes(out bool status)
        {
            status = true;
            var tcCodes = new List<TcCodeModel>();
            try
            {
                var dt = dBConnectivityImpl.FetchAllTcCodes(out status);
                tcCodes = Helper.Helper.ConvertToList<TcCodeModel>(dt);

            }
            catch
            {
                status = false;
            }
            return tcCodes;
        }

        public bool UpdateTcCode(TcCodeModel tcCodeModel)
        { 
            try
            {
                return dBConnectivityImpl.UpdateTCCodes(tcCodeModel);

            }
            catch
            {
                return false;
            }
        }

        public DataTable FetchAllEverlyticData(string fDate, string tDate, out bool status)
        {
            status = true;
            var dt = new DataTable();
            try
            {
                  dt = dBConnectivityImpl.FetchAllEverlyticData(fDate, tDate, out status); 

            }
            catch
            {
                status = false;
            }
            return dt;
        }

        public List<CustomerInfoModel> FetchAllApplicationData(string fromDate, string ToDate, string domain,
            string tcName, string mobile, string freezoneCode, out bool status)
        {
            status = true;
            var customerInfos = new List<CustomerInfoModel>();
            try
            {
                var dt = dBConnectivityImpl.FetchAllApplicationData(fromDate, ToDate, domain, tcName,mobile, freezoneCode,out status);
                customerInfos = Helper.Helper.ConvertToList<CustomerInfoModel>(dt);
            }
            catch
            {
                status = false;
            }
            return customerInfos;
        }

        public bool InsertJobFunction(string jobFunction)
        {
            try
            {
                return dBConnectivityImpl.InsertJobFunction(jobFunction);

            }
            catch
            {
                return false;
            }
        }

        public bool InsertJobIndustry(string industry)
        {
            try
            {
                return dBConnectivityImpl.InsertIndustry(industry);

            }
            catch
            {
                return false;
            }
        }

        //public List<T> FetchAllJobFunctions<T>(int mode, out bool status)
        //{
        //    status = true;
        //    List<JobFunction> functions;
        //    try
        //    {
        //        var dt = dBConnectivityImpl.FetchAllTcCodes(out status);
        //        if(mode==1)
        //        {
        //            functions =  Helper.Helper.ConvertToList<JobFunction>(dt);
        //            return functions;
        //        }
        //        else
        //        {
        //            return (JobIndustry)Helper.Helper.ConvertToList<JobIndustry>(dt).ToList();
        //        }

        //    }
        //    catch
        //    {
        //        status = false;
        //    }
        //    return null;
        //}
    }
}