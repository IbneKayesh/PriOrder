using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace PriOrder.App.Models
{
    public class NIF_IMGS
    {
        [Required]
        public string APPL_NID { get; set; }
        [Required]
        public int ITEM_IMAGE_TYPE { get; set; }
        [Display(Name = "Image")]
        [NotMapped]
        public HttpPostedFileBase ITEM_IMAGE { get; set; }
    }
}