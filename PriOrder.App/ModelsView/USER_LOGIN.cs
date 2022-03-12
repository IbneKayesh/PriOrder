using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace PriOrder.App.ModelsView
{
    public class USER_LOGIN
    {
        [Display(Name ="User Id")]
        [Required(ErrorMessage ="{0} is required")]
        public string USER_ID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required")]
        public string USER_PASSWORD { get; set; }


        [Display(Name = "Profile Image")]
        [NotMapped]
        public HttpPostedFileBase USER_IMAGE { get; set; }
    }
}