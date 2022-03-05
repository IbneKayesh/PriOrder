using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_ITEM_CATEGORY
    {
        [Display(Name = "Category Id")]
        [Required(ErrorMessage = "{0} is required")]
        public string CATEGORY_ID { get; set; }

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string CATEGORY_NAME { get; set; }

        [Display(Name = "Classes")]
        public int ITEM_CLASS_COUNT { get; set; }

        [Display(Name = "Items")]
        public int ITEM_COUNT { get; set; }

        [Display(Name = "Category Image")]
        [NotMapped]
        public HttpPostedFileBase CATEGORY_IMAGE { get; set; }
    }
}