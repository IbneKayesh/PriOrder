using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class T_BANKCOL
    {
        [Display(Name = "Bank Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string BNAME { get; set; }

        [Display(Name = "Account Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string ANAME { get; set; }

        [Display(Name = "Account Number")]
        [Required(ErrorMessage = "{0} is required")]
        public string ANUMBER { get; set; }

        [Display(Name = "Branch Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string BRNAME { get; set; }
    }
}