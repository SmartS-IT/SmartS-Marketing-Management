using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SmartS_Marketing_Management.Models;

namespace SmartS_Marketing_Management.Connectivity
{
    public class DBConnectivityImpl
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;

        public DBConnectivityImpl()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBString"].ConnectionString); 
        }

        public bool AddNewBSCCodes(BscCodeModel bscCodeModel)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "INSERT_BSC_CODE";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paramEmail = new SqlParameter("@EMAIL", SqlDbType.NVarChar, 30);
                var paramDomain = new SqlParameter("@DOMAIN", SqlDbType.VarChar, 100);
                var paraBscCode = new SqlParameter("@BSC_CODE", SqlDbType.VarChar, 100);
                var paramCreatedBy = new SqlParameter("@CREATED_BY", SqlDbType.Int);
                var paraCreatedOn = new SqlParameter("@CREATED_ON", SqlDbType.DateTime);

                paramEmail.Value = bscCodeModel.Email;
                paramDomain.Value = bscCodeModel.Domain;
                paraBscCode.Value = bscCodeModel.BSC_Code;
                paramCreatedBy.Value = bscCodeModel.CreatedBy;
                paraCreatedOn.Value = bscCodeModel.CreatedOn;

                sqlCommand.Parameters.Add(paramEmail);
                sqlCommand.Parameters.Add(paramDomain);
                sqlCommand.Parameters.Add(paraBscCode);
                sqlCommand.Parameters.Add(paramCreatedBy);
                sqlCommand.Parameters.Add(paraCreatedOn);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery(); 
            }
            catch(Exception e)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
                } 
            return true;
        }

        public DataTable FetchAllBscCodes(out bool status)
        {
            status = true;
            DataTable table = new DataTable("MS_BSC_CODE");
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "SELECTALL_BCS_CODE";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapt = new SqlDataAdapter(sqlCommand);
                sqlConnection.Open();
                adapt.Fill(table); 
            }
            catch (Exception e)
            {
                status = false;
                return table; 
            }
            finally
            {
                sqlConnection.Close();
            }
            return table;
        }

        public bool UpdateBSCCodes(BscCodeModel bscCodeModel)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "UPDATE_MS_BSC_CODE";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paramID= new SqlParameter("@ID", SqlDbType.Int);
                var paramEmail = new SqlParameter("@EMAIL", SqlDbType.NVarChar, 30);
                var paramDomain = new SqlParameter("@DOMAIN", SqlDbType.VarChar, 100);
                var paraBscCode = new SqlParameter("@BSC_CODE", SqlDbType.VarChar, 100);

                paramID.Value = bscCodeModel.ID;
                paramEmail.Value = bscCodeModel.Email;
                paramDomain.Value = bscCodeModel.Domain;
                paraBscCode.Value = bscCodeModel.BSC_Code;

                sqlCommand.Parameters.Add(paramID);
                sqlCommand.Parameters.Add(paramEmail);
                sqlCommand.Parameters.Add(paramDomain);
                sqlCommand.Parameters.Add(paraBscCode);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public bool AddNewTcCodes(TcCodeModel tcCodeModel)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "INSERT_TC_CODE";
                sqlCommand.CommandType = CommandType.StoredProcedure; 
                var paramTcName = new SqlParameter("@NAME", SqlDbType.VarChar, 100);
                var paraTcCode = new SqlParameter("@TC_CODE", SqlDbType.VarChar, 100);
                var paramCreatedBy = new SqlParameter("@CREATED_BY", SqlDbType.Int);
                var paraCreatedOn = new SqlParameter("@CREATED_ON", SqlDbType.DateTime);

                paramTcName.Value = tcCodeModel.TC_Name;
                paraTcCode.Value = tcCodeModel.TC_Code;
                paramCreatedBy.Value = tcCodeModel.CreatedBy;
                paraCreatedOn.Value = tcCodeModel.CreatedOn;

                sqlCommand.Parameters.Add(paramTcName);
                sqlCommand.Parameters.Add(paraTcCode);
                sqlCommand.Parameters.Add(paramCreatedBy);
                sqlCommand.Parameters.Add(paraCreatedOn);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public DataTable FetchAllTcCodes(out bool status)
        {
            status = true;
            DataTable table = new DataTable("MS_TC_CODE");
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "SELECTALL_TC_CODE";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapt = new SqlDataAdapter(sqlCommand);
                sqlConnection.Open();
                adapt.Fill(table);
            }
            catch (Exception e)
            {
                status = false;
                return table;
            }
            finally
            {
                sqlConnection.Close();
            }
            return table;
        }

        public bool UpdateTCCodes(TcCodeModel tcCodeModel)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "UPDATE_MS_TC_CODE";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paramID = new SqlParameter("@ID", SqlDbType.Int);
                var paramName = new SqlParameter("@TC_NAME", SqlDbType.VarChar, 100);
                var paramCode = new SqlParameter("@TC_CODE", SqlDbType.VarChar, 100); 

                paramID.Value = tcCodeModel.ID;
                paramName.Value = tcCodeModel.TC_Name;
                paramCode.Value = tcCodeModel.TC_Code; 

                sqlCommand.Parameters.Add(paramID);
                sqlCommand.Parameters.Add(paramName);
                sqlCommand.Parameters.Add(paramCode); 
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public DataTable FetchAllEverlyticData(string fromDate, string ToDate, int mode, out bool status)
        {
            status = true;
            DataTable table = new DataTable();
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "SELECTALL_APPL_DTLS_EVERLYT";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapt = new SqlDataAdapter(sqlCommand);

                var fromDt = new SqlParameter("@Fromdate", SqlDbType.VarChar, 30);
                var ToDt = new SqlParameter("@Todate", SqlDbType.VarChar, 30);
                var Mode = new SqlParameter("@Mode", SqlDbType.Int);

                fromDt.Value = fromDate;
                ToDt.Value = ToDate;
                Mode.Value = mode;

                sqlCommand.Parameters.Add(fromDt);
                sqlCommand.Parameters.Add(ToDt);
                sqlCommand.Parameters.Add(Mode);

                sqlConnection.Open();
                adapt.Fill(table); 
            }
            catch (Exception e)
            {
                status = false;
                return table;
            }
            finally
            {
                sqlConnection.Close();
            }
            return table;
        }

        public bool InsertJobFunction(string JobFunction, int userID)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "INSERT_MS_JOB_FUNCTION";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paramFuncName = new SqlParameter("@FUNCTION_NAME", SqlDbType.VarChar, 100);
                var paramCreatedBy = new SqlParameter("@CREATED_BY", SqlDbType.Int);
                var paraCreatedOn = new SqlParameter("@CREATED_ON", SqlDbType.DateTime);

                paramFuncName.Value = JobFunction; 
                paramCreatedBy.Value = userID;
                paraCreatedOn.Value = DateTime.Now;

                sqlCommand.Parameters.Add(paramFuncName);
                sqlCommand.Parameters.Add(paramCreatedBy);
                sqlCommand.Parameters.Add(paraCreatedOn);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public bool InsertIndustry(string Industry, int userID)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "INSERT_MS_JOB_INDUSTRY";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paraIndustry = new SqlParameter("@INDUSTRY_NAME", SqlDbType.VarChar, 100);
                var paramCreatedBy = new SqlParameter("@CREATED_BY", SqlDbType.Int);
                var paraCreatedOn = new SqlParameter("@CREATED_ON", SqlDbType.DateTime);

                paraIndustry.Value = Industry;
                paramCreatedBy.Value = userID;
                paraCreatedOn.Value = DateTime.Now;

                sqlCommand.Parameters.Add(paraIndustry);
                sqlCommand.Parameters.Add(paramCreatedBy);
                sqlCommand.Parameters.Add(paraCreatedOn);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public DataTable FetchAllFunctionsAndIndustry(int mode, out bool status)
        {
            status = true;
            DataTable table = new DataTable();
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "SELECTALL_JOB_DETAILS";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paramMode = new SqlParameter("@MODE", SqlDbType.Int);
                paramMode.Value = mode;

                sqlCommand.Parameters.Add(paramMode);

                SqlDataAdapter adapt = new SqlDataAdapter(sqlCommand);
                sqlConnection.Open();
                adapt.Fill(table);
            }
            catch (Exception e)
            {
                status = false;
                return table;
            }
            finally
            {
                sqlConnection.Close();
            }
            return table;
        }

        public DataTable FetchAllApplicationData(string fromDate, string ToDate, string domain, 
            string tcName, string mobile, string freezoneCode, out bool status)
        {
            status = true;
            DataTable table = new DataTable();
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "SELECTALL_APPL_DTLS_EVERLYT";
                sqlCommand.CommandType = CommandType.StoredProcedure; 

                var fromDt = new SqlParameter("@Fromdate", SqlDbType.VarChar, 30);
                var ToDt = new SqlParameter("@Todate", SqlDbType.VarChar, 30);
                var paramDomain = new SqlParameter("@domain", SqlDbType.VarChar, 100);
                var paramTcCode = new SqlParameter("@tcname", SqlDbType.VarChar, 100);
                var paramMob = new SqlParameter("@mobile", SqlDbType.VarChar, 30);
                var paramFreeZone = new SqlParameter("@freezone", SqlDbType.VarChar, 30);
                var paramMode = new SqlParameter("@Mode", SqlDbType.Int);

                fromDt.Value = fromDate;
                ToDt.Value = ToDate;
                paramDomain.Value = domain;
                paramTcCode.Value = tcName;
                paramMob.Value = mobile;
                paramFreeZone.Value = freezoneCode;
                paramMode.Value = 1;

                sqlCommand.Parameters.Add(fromDt);
                sqlCommand.Parameters.Add(ToDt);
                sqlCommand.Parameters.Add(paramDomain);
                sqlCommand.Parameters.Add(paramTcCode);
                sqlCommand.Parameters.Add(paramMob);
                sqlCommand.Parameters.Add(paramFreeZone);
                sqlCommand.Parameters.Add(paramMode);

                sqlConnection.Open();
                SqlDataAdapter adapt = new SqlDataAdapter(sqlCommand);  
                adapt.Fill(table);
            }
            catch (Exception e)
            {
                status = false;
                return table;
            }
            finally
            {
                sqlConnection.Close();
            }
            return table;
        }

        public bool UpdateJobDetails(JobDetails jobFunctions)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "UPDATE_MS_JOB_CATEGORIES";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paraTitle = new SqlParameter("@Title", SqlDbType.VarChar, 100);
                var paraId = new SqlParameter("@Id", SqlDbType.Int);
                var paraFunc = new SqlParameter("@job_function", SqlDbType.Int);
                var paraIndustry = new SqlParameter("@industry", SqlDbType.Int);

                paraTitle.Value = jobFunctions.Title;
                paraId.Value = jobFunctions.Id;
                if(jobFunctions.JobFunction!=0)
                {
                    paraFunc.Value = jobFunctions.JobFunction;
                }
                if (jobFunctions.Industry != 0)
                {
                    paraIndustry.Value = jobFunctions.Industry;
                }  
                sqlCommand.Parameters.Add(paraTitle);
                sqlCommand.Parameters.Add(paraId);
                sqlCommand.Parameters.Add(paraFunc);
                sqlCommand.Parameters.Add(paraIndustry);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public DataTable FetchAllJobDetails(out bool status)
        {
            status = true;
            DataTable table = new DataTable();
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "SELECTALL_JOB_DETAILS";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paramMode = new SqlParameter("@MODE", SqlDbType.Int);
                paramMode.Value = 2;

                sqlCommand.Parameters.Add(paramMode);

                SqlDataAdapter adapt = new SqlDataAdapter(sqlCommand);
                sqlConnection.Open();
                adapt.Fill(table);
            }
            catch (Exception e)
            {
                status = false;
                return table;
            }
            finally
            {
                sqlConnection.Close();
            }
            return table;
        }

        public DataTable FetchUserDetails(string username, out bool status)
        {
            status = true;
            DataTable table = new DataTable();
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "FETCH_USER_DETAILS";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paramUsername = new SqlParameter("@USERNAME", SqlDbType.VarChar, 100);
                paramUsername.Value = username;
                sqlCommand.Parameters.Add(paramUsername);

                SqlDataAdapter adapt = new SqlDataAdapter(sqlCommand);
                sqlConnection.Open();
                adapt.Fill(table);
            }
            catch (Exception e)
            {
                status = false;
                return table;
            }
            finally
            {
                sqlConnection.Close();
            }
            return table;
        }

        public bool InsertDataLog(TableLog tableLog)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "INSERT_MS_DATA_LOG";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paraTbID = new SqlParameter("@TABLE_IDENTIFIER_ID", SqlDbType.Int, 150);
                var paraDtlsId = new SqlParameter("@DTLS_ID", SqlDbType.Int, 150);
                var paraOld = new SqlParameter("@OLD_VALUE", SqlDbType.VarChar, 150);
                var paraNewValue = new SqlParameter("@NEW_VALUE", SqlDbType.VarChar, 150);
                var paraEntryDate = new SqlParameter("@ENTRY_DATE", SqlDbType.DateTime);
                var paraUserId = new SqlParameter("@USER_ID", SqlDbType.Int);

                paraTbID.Value = tableLog.Table_Identity_Id;
                paraDtlsId.Value = tableLog.Dtls_Id;
                paraOld.Value = tableLog.Old_Value;
                paraNewValue.Value = tableLog.New_Value;
                paraEntryDate.Value = tableLog.EntryDate;
                paraUserId.Value = tableLog.UserId;

                sqlCommand.Parameters.Add(paraTbID);
                sqlCommand.Parameters.Add(paraDtlsId);
                sqlCommand.Parameters.Add(paraOld);
                sqlCommand.Parameters.Add(paraNewValue);
                sqlCommand.Parameters.Add(paraEntryDate);
                sqlCommand.Parameters.Add(paraUserId);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }
    }
}