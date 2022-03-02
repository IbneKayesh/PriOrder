using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class DISTRIBUTORS
    {
        public int DISTRIBUTOR_ID { get; set; }
        public string DISTRIBUTOR_PASSWORD { get; set; }
        public string DISTRIBUTOR_NAME { get; set; }
        public string DISTRIBUTOR_ADRESS { get; set; }
        public string MOBILE_NUMBER { get; set; }
        public string HO_NUMBER { get; set; }
        public decimal DISTRIBUTOR_BALANCE { get; set; }
    }
}