using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class T_MBDO
    {
        public string ORDERID { get; set; }
        public string ORDATE { get; set; }
        public int MBDO_NOTETYPE { get; set; }
        public int LINE { get; set; }
        public int SP_NOTE { get; set; }
        public int CODE { get; set; }
        public string NAME { get; set; }
        public decimal PRICE { get; set; }
        public int FACTOR { get; set; }
        public int QTY { get; set; }
        public decimal TOTAL { get; set; }
        public decimal SP_RATE { get; set; }
        public decimal PRO_RATE { get; set; }
        public string MBDO_NOTE { get; set; }
        public decimal INCP { get; set; }
    }
}