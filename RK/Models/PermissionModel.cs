using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataLayer;
using Newtonsoft.Json;
namespace RK.Models
{
    public class PermissionModel
    {
        public int group_id { get; set; }
        [Display(Name = "Grupo")]
        public string group_name { get; set; }
        public List<ModuleModel> list_module_m { get; set; }

        public static Dictionary<string, string[]> GetGroup(int group_id){
        
            rekursosEntities db = new rekursosEntities();
            Dictionary<string, string[]> permissions = new Dictionary<string, string[]>();

            
            var result = db.permissions.Where(w => w.group_id == group_id)
               .ToList();

            if (result != null)
            {
                foreach (var permission in result)
                {

                    var roles = permission.roles == null ? new string[] { } : JsonConvert.DeserializeObject<string[]>(permission.roles);
                    permissions.Add(permission.module, roles);
                }
                
            }

            return permissions;


        }
    }
    public class ModuleModel
    {
        public string slug { get; set; }
        public string name { get; set; }
        public int id_module { get; set; }
        public bool check { get; set; }
        public string[] roles { get; set; }
       
    }
}