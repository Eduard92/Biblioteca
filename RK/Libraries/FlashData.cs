using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RK.Libraries
{
    public class FlashData
    {
        static List<string> items = new List<string>();

        public static void SetFlashData(string key, string message)
        {


            HttpContext context = HttpContext.Current;

            context.Session.Add(key, message);

        }

        public static string GetFlashData(string key, bool remove = true)
        {

            string message = "";
            HttpContext context = HttpContext.Current;
            if (context.Session[key] != null)
            {


                message = (string)context.Session[key];

                if (remove == true)
                    context.Session.Remove(key);
            }




            return message;
        }
    }
}