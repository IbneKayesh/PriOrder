using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class T_DSMA
    {
        public string APPL_NID { get; set; }
        public string DSTO_NIDN { get; set; }
        public string DIST_TYPE_ID { get; set; }
        public string DIST_TYPE { get; set; }
        public string DSMA_DSID { get; set; }
        public string DSMA_NAME { get; set; }
        public string ADDR1 { get; set; }
        public string CONTACTS { get; set; }
        public string ADDR2 { get; set; }
        public string DIGR_TEXT { get; set; }
        public string DIGR_NAME { get; set; }
        public string SALES_ZONE_ID { get; set; }
        public string SALES_ZONE { get; set; }
        public string DIST_ZONE_ID { get; set; }
        public string DIST_ZONE { get; set; }
        public string ZCONTACTS { get; set; }

        public virtual T_DSMA_BAL T_DSMA_BAL { get; set; }
    }
}