using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Security;
using System.Web.Security;
using RK.Security;

namespace RK.Core
{
    public class BasicAuthenticationAttribute:AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //base.OnAuthorization(actionContext);
            var headers = actionContext.Request.Headers;
            if (actionContext.Request.Headers.Authorization == null)
            {
                
                PutUnauthorizedResult(actionContext, "No hay autorización");
            }
            else
            {
                try
                {
                    var userPwd = Encoding.UTF8.GetString(Convert.FromBase64String(headers.Authorization.Parameter));

                    var credArray = userPwd.Split(":".ToCharArray());
                    var userName = credArray[0];
                    var password = credArray[1];

                    if (Ion_Auth.Login(userName, password)==false)
                    {
                      
                        PutUnauthorizedResult(actionContext, "No existe usuario");
                    }
                    

                   
                }
                catch(Exception ex)
                {
                    PutUnauthorizedResult(actionContext, ex.Message);
                }
            }
            
        }
        private static void PutUnauthorizedResult(HttpActionContext actionContext, string msg)
        {

            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)

            {

                Content = new StringContent(msg)

            };

        }
    }
}