using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_SUP_MSG
    {
        public string SUP_NUMBER { get; set; }
        public string SUP_TYPE { get; set; }
        public string DIST_ID { get; set; }
        public bool IS_ACTIVE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string CLOSED_BY { get; set; }
        public string CLOSED_NOTE { get; set; }
    }
}