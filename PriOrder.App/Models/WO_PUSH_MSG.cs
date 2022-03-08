using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_PUSH_MSG
    {
        [Display(Name = "Id")]
        public string MSG_ID { get; set; }

        [Display(Name = "Category")]
        public string CAT_ID { get; set; }

        [Display(Name = "Body")]
        public string BODY_TEXT { get; set; }

        [Display(Name = "Read")]
        public int IS_READ { get; set; }

        [Display(Name = "Date")]
        public DateTime CREATE_DATE { get; set; }
    }
}