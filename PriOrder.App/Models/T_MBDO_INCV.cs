using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class T_MBDO_INCV
    {
        [Display(Name = "Npv")]
        public decimal INCV { get; set; } = 0;

        [Display(Name = "SE")]
        public decimal SE { get; set; } = 0;

        [Display(Name = "Spv")]
        public decimal SPV { get; set; } = 0;

        [Display(Name = "Net")]
        [NotMapped]
        public decimal NET { get; set; } = 0;
    }
}