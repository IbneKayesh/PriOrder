using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_ITEM_TYPE
    {
        [Display(Name = "Category Id")]
        [Required(ErrorMessage = "{0} is required")]
        public string ITEM_TYPE_ID { get; set; }

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string ITEM_TYPE_NAME { get; set; }

        [Display(Name = "Classes")]
        public int ITEM_CLASS_COUNT { get; set; }

        [Display(Name = "Category Image")]
        [NotMapped]
        public HttpPostedFileBase ITEM_TYPE_IMAGE { get; set; }
    }
}