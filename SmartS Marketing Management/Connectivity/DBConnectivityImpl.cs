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

                paramEmail.Value = bscCodeModel.Email;
                paramDomain.Value = bscCodeModel.Domain;
                paraBscCode.Value = bscCodeModel.BSC_Code;

                sqlCommand.Parameters.Add(paramEmail);
                sqlCommand.Parameters.Add(paramDomain);
                sqlCommand.Parameters.Add(paraBscCode);
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

                paramTcName.Value = tcCodeModel.TC_Name;
                paraTcCode.Value = tcCodeModel.TC_Code; 

                sqlCommand.Parameters.Add(paramTcName);
                sqlCommand.Parameters.Add(paraTcCode);
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

        public DataTable FetchAllEverlyticData(string fromDate, string ToDate, out bool status)
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

                fromDt.Value = fromDate;
                ToDt.Value = ToDate; 

                sqlCommand.Parameters.Add(fromDt);
                sqlCommand.Parameters.Add(ToDt); 

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

        public bool InsertJobFunction(string JobFunction)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "INSERT_MS_JOB_FUNCTION";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paramFuncName = new SqlParameter("@FUNCTION_NAME", SqlDbType.VarChar, 100); 

                paramFuncName.Value = JobFunction; 

                sqlCommand.Parameters.Add(paramFuncName); 
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

        public bool InsertIndustry(string Industry)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "INSERT_MS_JOB_INDUSTRY";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var paraIndustry = new SqlParameter("@INDUSTRY_NAME", SqlDbType.VarChar, 100);

                paraIndustry.Value = Industry;

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
                paraFunc.Value = jobFunctions.JobFunction;
                paraIndustry.Value = jobFunctions.Industry;

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
    }
}