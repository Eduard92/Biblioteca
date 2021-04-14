using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace RK.Libraries
{
    public class Settings
    {

        public Settings()
        {

        }

        public static string Get(string slug)
        {
            rekursosEntities db = new rekursosEntities();

            var settings = db.settings.Where(w => w.slug == slug).SingleOrDefault();
            if (settings != null)
            {
                string value = settings.@default;

                if (settings.value != null)
                {
                    value = settings.value;
                }
                return value;
            }
            return "";


        }
        
    }
}