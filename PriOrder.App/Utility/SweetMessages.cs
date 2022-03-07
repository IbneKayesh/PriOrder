using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Utility
{
    public static class SweetMessages
    {
        public static string Success(string _text)
        {
            return "swal({title: 'Success!',text: '" + _text + "!',icon: 'success',button: 'Ok!'});";
        }
        public static string SuccessPop(string _text)
        {
            return "swal({position: 'top-end',icon: 'success', title: '" + _text + "', showConfirmButton: false,timer: 1000})";
        }

        public static string Failed(string _text)
        {
            return "swal({title: 'Error!',text: '" + _text + "!',icon: 'error',button: 'Ok!'});";
        }
    }
}