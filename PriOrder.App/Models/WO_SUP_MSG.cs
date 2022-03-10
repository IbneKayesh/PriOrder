using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_SUP_MSG
    {
        [Display(Name = "Support Id")]
        [Required(ErrorMessage = "{0} is required")]
        public string SUP_NUMBER { get; set; }

        [Display(Name = "Support Type")]
        [Required(ErrorMessage = "{0} is required")]
        public string CTYP_TYPE { get; set; }

        public string DIST_ID { get; set; }
        public string CLOSED_NOTE { get; set; }

        public bool IS_ACTIVE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }

    }
}