using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class SUPPORT_MESSAGES
    {
        public int ID { get; set; }
        public string MESSAGES_DATE_TIME { get; set; }
        public string MESSAGES_BODY { get; set; }
        public string REPLIED_BY { get; set; } = "C";
        public bool IS_READ { get; set; }
    }
}