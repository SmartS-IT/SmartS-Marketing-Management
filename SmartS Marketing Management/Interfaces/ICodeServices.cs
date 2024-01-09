using SmartS_Marketing_Management.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartS_Marketing_Management.Interfaces
{
    interface ICodeServices
    {
        bool AddNewBSCCodes(BscCodeModel bscCodeModel);

        List<BscCodeModel> FetchAllBscCodes(out bool status);

        bool UpdateBscCode(BscCodeModel bscCodeModel);

        bool AddNewTCCodes(TcCodeModel bscCodeModel);

        List<TcCodeModel> FetchAllTcCodes(out bool status);

        bool UpdateTcCode(TcCodeModel bscCodeModel);

        DataTable FetchAllEverlyticData(string fDate, string tDate, out bool status);

        bool InsertJobFunction(string jobFunction);

        bool InsertJobIndustry(string industry);

        List<CustomerInfoModel> FetchAllApplicationData(string fromDate, string ToDate, string domain,
            string tcName, string mobile, string freezoneCode, out bool status);
    }
}
