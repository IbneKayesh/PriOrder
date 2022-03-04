using System.ComponentModel.DataAnnotations;

namespace PriOrder.App.ModelsView
{
    public class USER_LOGIN_INFO
    {
        [Display(Name ="Id")]
        public string DIST_ID { get; set; }

        [Display(Name = "Name")]
        public string DIST_NAME { get; set; }

        [Display(Name = "Group")]
        public string DIST_GROUP { get; set; }

        [Display(Name = "Mobile")]
        public string DIST_MOBILE { get; set; }

        [Display(Name = "Balance")]
        public string DIST_BALANCE { get; set; }
    }
}