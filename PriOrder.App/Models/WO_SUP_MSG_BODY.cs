using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_SUP_MSG_BODY
    {
        public string SUP_NUMBER { get; set; }

        [Display(Name = "Body")]
        public string BODY_TEXT { get; set; }

        [Display(Name = "Support By")]
        public string SUP_BY { get; set; } = "0";

        [Display(Name = "Date")]
        public DateTime CREATE_DATE { get; set; }
    }
}