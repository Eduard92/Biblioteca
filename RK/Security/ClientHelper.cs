using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Web.Security;
using System.Net.Http.Headers;

namespace RK.Security
{
    public class ClientHelper
    {

         // Basic auth
        public static HttpClient GetClient(string username,string password)
        {
                var authValue = new  AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(username+":"+password)));

                var client = new HttpClient(){
                    DefaultRequestHeaders = { Authorization = authValue}
                    //Set some other client defaults like timeout / BaseAddress
                };
           
                return client;
        }

        // Auth with bearer token
        public static HttpClient GetClient(string token)
        {
                var authValue = new AuthenticationHeaderValue("Bearer", token);

                var client = new HttpClient(){
                    DefaultRequestHeaders = { Authorization = authValue}
                    //Set some other client defaults like timeout / BaseAddress
                };

            
                return client;
        }
    }
}