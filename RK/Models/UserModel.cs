using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RK.Models
{
    public class UserModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string username { get; set; }
        [Required]
        [Display(Name = "Grupo")]
        public int group_id { get; set; }
        [Required]
        [Display(Name = "Correo electrónico")]
        public string email { get; set; }
        [Required]
        [Display(Name = "Nombre a mostrar")]
        public string display_name { get; set; }
        [Display(Name = "Activo")]
        public int active { get; set; }
        [Display(Name = "Contraseña")]
        public string password { get; set; }

        public string fotografia { get; set; }

        [Display(Name = "Antigua contraseña")]
        public string old_password { get; set; }
        [Display(Name = "Nueva contraseña")]
        public string new_password { get; set; }
        [Display(Name = "Repetir la contraseña")]
        public string retype_password { get; set; }
    }
}