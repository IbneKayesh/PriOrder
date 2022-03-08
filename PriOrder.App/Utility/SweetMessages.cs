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
            return "Swal.fire({title: 'Success!',text: '" + _text + "!',icon: 'success',button: 'Ok!'});";
        }
        public static string SuccessPop(string _text)
        {
            return "Swal.fire({position: 'top-end',icon: 'success', title: '" + _text + "', showConfirmButton: false,timer: 1000})";
        }

        public static string Failed(string _text)
        {
            return "Swal.fire({title: 'Error!',text: '" + _text + "!',icon: 'error',button: 'Ok!'});";
        }
        public static string Info(string _text)
        {
            return "Swal.fire({title: 'Info!',text: '" + _text + "!',icon: 'info',button: 'Ok!'});";
        }
        public static string Help(string _text)
        {
            return "Swal.fire({title: 'Help!',text: '" + _text + "!',icon: 'question',button: 'Ok!'});";
        }
    }
}