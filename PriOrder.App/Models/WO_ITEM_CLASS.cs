using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_ITEM_CLASS
    {
        [Display(Name = "Class Id")]
        [Required(ErrorMessage = "{0} is required")]
        public string ITEM_CLASS_ID { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string ITEM_CLASS_NAME { get; set; }

        [Display(Name = "Items")]
        public int ITEMS_COUNT { get; set; }

        [Display(Name = "Category Id")]
        [Required(ErrorMessage = "{0} is required")]
        public string ITEM_TYPE_ID { get; set; }

        [Display(Name = "Class Image")]
        [NotMapped]
        public HttpPostedFileBase ITEM_CLASS_IMAGE { get; set; }
    }
}