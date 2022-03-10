using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class SUP_MSG_REPL
    {
        [Required]
        public string replyId { get; set; }
        [Required]
        public string messagesText { get; set; }
        public int closereply { get; set; }
    }
}