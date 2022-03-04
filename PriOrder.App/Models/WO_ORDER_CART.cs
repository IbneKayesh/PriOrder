using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_ORDER_CART
    {
        public string DSMA_DSID { get; set; }
        public string ITMA_ITID { get; set; }

        [NotMapped]
        public string ITMA_NAME { get; set; }

        public int ITEM_QTY { get; set; }

        [NotMapped]
        public decimal ITMA_PRIC { get; set; }
        public string NOTE_ID { get; set; }
        public string NOTE_TEXT { get; set; }
    }
}