using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class DISTRIBUTOR_MASTER
    {
        public string DIST_ID { get; set; }
        public string DIST_NAME { get; set; }
        public string DIGR_TEXT { get; set; }
        public DateTime OPDATE { get; set; }
        public Nullable<DateTime> CLDATE { get; set; }
        public string CANCELLED { get; set; }
    }
}