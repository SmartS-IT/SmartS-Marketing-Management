using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartS_Marketing_Management.Models
{
    public class TableLog
    {
        public int Id { get;  set; }
        public int Table_Identity_Id { get; set; } 
        public int Dtls_Id { get; set; }  
        public string Old_Value { get; set; }
        public string New_Value { get; set; } 
        public DateTime EntryDate { get; set; } 
        public int UserId { get; set; }
    }
}