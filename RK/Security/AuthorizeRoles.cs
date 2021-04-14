using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.AccessControl;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using RK.Models;
using RK.Libraries;

namespace RK.Security
{

    public class AuthorizeRoles:AuthorizeAttribute
    {
        public string AccessLevel { get; set; }
        public string ModuleLevel { get; set; }
        public string RoleLevel { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var IsAuthorized = base.AuthorizeCore(httpContext);
            try
            {
                if (!IsAuthorized)
                {
                    return false;
                }

                if (ModuleLevel != null)
                {


                    var user = Ion_Auth.GetUser(httpContext.User.Identity.Name);



                    var permissions = PermissionModel.GetGroup((int)user.group_id);

                    if (user.groups.name == "admin") return true;


                    if (permissions.ContainsKey(ModuleLevel) == false)
                    {
                        FlashData.SetFlashData("error", "No tienes permiso para ver esta sección.");
                        httpContext.Response.Redirect("/");
                        return false;
                    }
                    if (RoleLevel != null && permissions[ModuleLevel].Contains(RoleLevel) == false)
                    {
                        FlashData.SetFlashData("error", "No tienes permiso para realizar esta acción.");
                        httpContext.Response.Redirect("/");
                        return false;
                    }

                }
                return true;
            }
            catch(Exception ex)
            {

            }
            return false;
        }
    }
}