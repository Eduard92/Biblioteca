using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace RK.Libraries
{
    public class Helper
    {
        
        public static string Gravatar(string email,int size=50,string rating="g")
        {

            if (email == "" || email == null)
            {
                email = "3b3be63a4c2a439b013787725dfce802";
            }
            else
            {
                MD5 Md5hash = MD5.Create();
                email = GetMd5Hash(Md5hash, email);
            }
            string url = "http://www.gravatar.com/avatar/"+email;

            string extra = "?s="+size.ToString()+"&r="+rating;


            url += HttpUtility.UrlEncode(extra);


            return url;


        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static string UrlBase(HttpContextBase httpcontext)
        {

            /*System.Web.Mvc.UrlHelper UrlHelp;
            var UrlHelper = new UrlHelper(httpcontext.Request)

            
            string url = string.Format("{0}://{1}{2}", httpcontext.Request.Url.Scheme, httpcontext.Request.Url.Authority,  Url.Content("~"));

            return url;*/
            string url_current = "";
            string sheme = httpcontext.Request.Url.Authority;
            string[] bases = sheme.Split(":".ToCharArray());//httpcontext.Request.Url.Scheme,httpcontext.Request.Url.Authority.
            string port = "";//Settings.Get("port");
            //httpcontext.Request.Url.Port
            if (port != "" && httpcontext.Request.Url.Authority != "localhost")
                url_current = string.Format("{0}://{1}:{2}/", httpcontext.Request.Url.Scheme, httpcontext.Request.Url.Authority, port);
            else
                url_current = string.Format("{0}://{1}/", httpcontext.Request.Url.Scheme, httpcontext.Request.Url.Authority);
            return url_current;
        }
        
    }
}