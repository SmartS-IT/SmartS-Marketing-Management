using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartS_Marketing_Management.Models
{
    public class BscCodeModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Domain { get; set; }
        public string BSC_Code { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}