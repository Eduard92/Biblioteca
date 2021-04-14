using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using RK.Models;
using Newtonsoft.Json;
using System.Data;
using RK.Core;
namespace RK.Controllers
{
    public class PermissionsController : AdminController
    {
        private rekursosEntities db = new rekursosEntities();
        public PermissionsController():base("permissions")
        {
        }
        //
        // GET: /Permissions/

        public ActionResult Index()
        {
            return View(db.groups.ToList());
           
        }

        //
        // GET: /Permissions/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Permissions/Create

        public ActionResult Save(int id=0)
        {
            groups group = db.groups.Find(id);
            Dictionary<string, string[]> list_roles = new Dictionary<string, string[]>();

            PermissionModel permission_m = new PermissionModel() { group_name = group.name, group_id = group.id };

            permission_m.list_module_m = new List<ModuleModel>();

            foreach (var module in db.modules.Where(w => w.enabled == 1))
            {
                var permision_exists = db.permissions.Where(w => w.group_id == id)
                                                     .Where(w => w.module == module.slug)
                                                     .SingleOrDefault();

                switch (module.slug)
                {
                    case "comisiones":
                        list_roles.Add(module.slug, new string[] { "create", "update", "delete", "extra" });
                        break;

                    default:
                        list_roles[module.slug] = new string[] { "create", "update", "delete" };
                        break;
                }
                //Response.Write(permision_exists);
                permission_m.list_module_m.Add(new ModuleModel { id_module = module.id, slug = module.slug, name = module.name, check = (permision_exists == null ? false : true), roles = permision_exists != null && permision_exists.roles != null ? JsonConvert.DeserializeObject<string[]>(permision_exists.roles) : new string[] { } });
            }

            ViewBag.Roles = list_roles;
            return View(permission_m);
        }
        [HttpPost]
        public ActionResult Save(PermissionModel permissions_m)
        {
            List<string> items = new List<string>();
            foreach (var module in permissions_m.list_module_m)
            {


                if (module.slug != null)
                {


                    var permission = db.permissions.Where(w => w.group_id == permissions_m.group_id)
                                              .Where(w => w.module == module.slug).SingleOrDefault();


                    if (permission == null)
                    {
                        db.permissions.Add(new permissions { group_id = permissions_m.group_id, module = module.slug, roles = module.roles != null ? System.Web.Helpers.Json.Encode(module.roles) : null });
                        db.SaveChanges();
                    }
                    else
                    {
                        //db.permissions.e(new permission { group_id = permissions_m.group_id, module = module.slug, roles = System.Web.Helpers.Json.Encode(module.roles) });
                        permissions permission_update = (permissions)permission;

                        permission_update.roles = module.roles != null ? System.Web.Helpers.Json.Encode(module.roles) : null;
                        db.Entry(permission_update).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    items.Add(module.slug);
                }

            }
            List<permissions> list_delete = new List<permissions>();

            if (items.Count > 0)
            {
                list_delete = db.permissions.Where(w => w.group_id == permissions_m.group_id)
                                            .Where(w => !items.Contains(w.module))
                                            .ToList();


            }
            else
            {
                list_delete = db.permissions.Where(w => w.group_id == permissions_m.group_id)
                                            .ToList();
            }
            if (list_delete != null)
            {

                foreach (var enty in list_delete.ToList())
                {

                    db.permissions.Remove(enty);
                    db.SaveChanges();
                }
            }


            //return Content("");
            return RedirectToAction("Save", "Permissions", new { id = permissions_m.group_id });
        }
        

        

    }
}
