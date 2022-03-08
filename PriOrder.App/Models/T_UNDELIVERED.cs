using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class T_UNDELIVERED
    {
        [Display(Name = "Code")]
        public string DIST_ID { get; set; }

        [Display(Name = "DO")]
        public string DONO { get; set; }

        [Display(Name = "Line")]
        public string DO_LINE_NO { get; set; }

        [Display(Name = "Date")]
        public string DODATE { get; set; }

        [Display(Name = "Item")]
        public string ITEMID { get; set; }

        [Display(Name = "Name")]
        public string ITEMNAME { get; set; }

        [Display(Name = "Qty")]
        public int QTY  { get; set; }

        [Display(Name = "S.Qty")]
        public int SQTY { get; set; }

        [Display(Name = "Total")]
        public int TQTY { get; set; }

        [Display(Name = "Rate")]
        public decimal RATE { get; set; }

        [Display(Name = "Amount")]
        public decimal AMOUNT { get; set; }

        [Display(Name = "Depot")]
        public int DEPOT { get; set; }
    }
}