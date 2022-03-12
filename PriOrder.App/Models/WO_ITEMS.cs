using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Models
{
    public class WO_ITEMS
    {
        [Display(Name = "Item Code")]
        [Required(ErrorMessage = "{0} is required")]
        public string ITEM_ID { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required")]
        public decimal ITEM_RATE { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string ITEM_NAME { get; set; }

        [Display(Name = "Unit")]
        [Required(ErrorMessage = "{0} is required")]
        public string ITEM_UNIT { get; set; }

        [Display(Name = "Factor")]
        [Required(ErrorMessage = "{0} is required")]
        public int ITEM_FACTOR { get; set; }


        public int NIST_STOCK { get; set; }
        public int WHOC_AVG { get; set; }
        public int NEW_STOCK { get; set; }

        [Display(Name = "Item Image")]
        [NotMapped]
        public HttpPostedFileBase ITEMS_IMAGE { get; set; }

        public virtual List<SelectListItem> WO_NOTE { get; set; }
    }
}