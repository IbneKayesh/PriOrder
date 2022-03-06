using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Models
{
    public class WO_ORDER_CART
    {
        public string ITEM_ID { get; set; }        
        public string ITEM_NAME { get; set; }
        public decimal ITEM_RATE { get; set; }
        public string ITEM_UNIT { get; set; }
        public int ITEM_FACTOR { get; set; }
        public int ITEM_QTY { get; set; }
        public string NOTE_ID { get; set; }
        public string NOTE_TEXT { get; set; }
        public string NOTES { get; set; }
        public virtual List<SelectListItem> WO_NOTE { get; set; }
    }
}