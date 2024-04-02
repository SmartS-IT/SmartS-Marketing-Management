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

        public bool InsertJobFunction(string jobFunction, int userID)
        {
            try
            {
                return dBConnectivityImpl.InsertJobFunction(jobFunction, userID);

            }
            catch
            {
                return false;
            }
        }

        public bool InsertJobIndustry(string industry, int userID)
        {
            try
            {
                return dBConnectivityImpl.InsertIndustry(industry, userID);

            }
            catch
            {
                return false;
            }
        }

        public Tuple<List<JobFunction>, List<JobIndustry>> FetchAllJobFunctions(int mode, out bool status)
        {
            status = true; 
            try
            {
                var dt = dBConnectivityImpl.FetchAllFunctionsAndIndustry(mode, out status);
                if (mode == 1)
                {
                    var Ind = Helper.Helper.ConvertToList<JobIndustry>(dt);
                    return new Tuple<List<JobFunction>, List<JobIndustry>>(null, Ind); 
                }
                else
                {
                    var functions = Helper.Helper.ConvertToList<JobFunction>(dt);
                    return new Tuple<List<JobFunction>, List<JobIndustry>>(functions, null);
                }
            }
            catch
            {
                status = false;
            }
            return null;
        }

        public bool UpdateJobDetails(JobDetails jobFunctions)
        {
            try
            {
                return dBConnectivityImpl.UpdateJobDetails(jobFunctions);

            }
            catch
            {
                return false;
            }
        }

        public List<JobDetails> FetchAllJobDetails(out bool status)
        {
            status = true;
            var jobFunc = new List<JobDetails>();

            try
            {
                var dt = dBConnectivityImpl.FetchAllJobDetails(out status);
                jobFunc = Helper.Helper.ConvertToList<JobDetails>(dt);
            }
            catch
            {
                status = false;
            }
            return jobFunc;
        }

        public List<UserDetails> FetchUserDetails(string username, out bool status)
        {
            status = true;
            var userDetails = new List<UserDetails>();

            try
            {
                var dt = dBConnectivityImpl.FetchUserDetails(username, out status);
                userDetails = Helper.Helper.ConvertToList<UserDetails>(dt);
            }
            catch
            {
                status = false;
            }
            return userDetails;
        }

        public bool InsertDataLog(TableLog listTableLogs)
        { 
            try
            {
                return dBConnectivityImpl.InsertDataLog(listTableLogs);

            }
            catch
            {
                return false;
            }
        }
    }
}