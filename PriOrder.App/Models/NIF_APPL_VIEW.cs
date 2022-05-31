using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class NIF_APPL_VIEW
    {
        public NIF_APPL NIF_APPL { get; set; }
        public List<NIF_NOMI> NIF_NOMI { get; set; }
    }
}