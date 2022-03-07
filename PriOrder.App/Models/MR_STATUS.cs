using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class MR_STATUS
    {
        [Display(Name = "MR Number")]
        public string MRDO_MRNO { get; set; }

        [Display(Name = "Dist Id")]
        public string MRDO_DIST { get; set; }

        [Display(Name = "Date")]
        public string MRDO_DATE { get; set; }

        [Display(Name = "Bank")]
        public string MRDO_SORC { get; set; }

        [Display(Name = "Cheque")]
        public string MRDO_CHEK { get; set; }

        [Display(Name = "Amount")]
        public decimal MRDO_AMNT { get; set; }
    }
}