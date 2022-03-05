using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int ITMA_STOCK { get; set; }
                
        [Display(Name = "Product Image")]
        [NotMapped]
        public HttpPostedFileBase ITMA_IMGE { get; set; }
    }
}