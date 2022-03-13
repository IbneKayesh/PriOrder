using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class T_DSMA_BAL
    {
        [Display(Name = "MR")]
        public decimal MRDO_AMNT { get; set; }
        [Display(Name = "Balance")]
        public decimal DBAL_ABAL { get; set; }
        [Display(Name = "Credit")]
        public decimal DBAL_CBAL { get; set; }
    }
}