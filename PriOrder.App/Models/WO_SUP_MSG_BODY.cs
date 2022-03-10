using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string CREATE_USER { get; set; } = "0";

        [Display(Name = "Date")]
        public DateTime CREATE_DATE { get; set; }

        public bool IS_ACTIVE { get; set; }

        [NotMapped]
        public SUP_MSG_REPL SUP_MSG_REPL { get; set; }
    }
}