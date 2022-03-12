using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.ModelsView
{
    public class USER_PASSWORD
    {
        [Required]
        public string USER_PASSWORD_OLD { get; set; }
        [Required]
        public string USER_PASSWORD_NEW { get; set; }
        [Required]
        public string USER_PASSWORD_CONFIRM { get; set; }
    }
}