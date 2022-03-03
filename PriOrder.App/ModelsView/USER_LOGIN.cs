using System.ComponentModel.DataAnnotations;

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
    }
}