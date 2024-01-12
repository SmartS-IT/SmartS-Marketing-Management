using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartS_Marketing_Management.Models
{
    public class JobDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int JobFunction { get; set; }
        public int Industry { get; set; }
        public string FunctionName { get; set; }
        public string IndustryName { get; set; }
    }
}