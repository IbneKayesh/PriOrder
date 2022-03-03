using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class T_ITMA
    {
        public string ITMA_ITID { get; set; }
        public string ITMA_NAME { get; set; }
        public decimal ITMA_PRIC { get; set; }
        public string ITMA_GRUP { get; set; }
        public decimal ITMA_FACT { get; set; }
        public string ITMA_CLASS { get; set; }
        public string ITMA_IMGE { get; set; }
        public int ITMA_STOCK { get; set; }
    }
}