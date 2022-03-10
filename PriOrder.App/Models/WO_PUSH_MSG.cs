using System;
using System.ComponentModel.DataAnnotations;

namespace PriOrder.App.Models
{
    public class WO_PUSH_MSG
    {
        [Display(Name = "Id")]
        public string MSG_ID { get; set; }

        [Display(Name = "Category")]
        public string CAT_ID { get; set; }


        public string DIGR_TEXT { get; set; }
        public string SALES_ZONE_ID { get; set; }
        public string BASE_ID { get; set; }
        public string DIST_ID { get; set; }


        [Display(Name = "Body")]
        public string BODY_TEXT { get; set; }

        [Display(Name = "Date")]
        public DateTime CREATE_DATE { get; set; }

        [Display(Name = "New")]
        public string NEW_MSG { get; set; }
    }
}