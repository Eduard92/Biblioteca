using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RK.Models
{
    public class EmpleadoModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "No. empleado")]
        public string id_empleado { get; set; }
        
        [Display(Name = "Centro")]
        public int id_centro { get; set; }
        [Display(Name = "Area")]
        public int id_area { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Required]
        [Display(Name = "Apellido paterno")]
        public string apellido_paterno { get; set; }
        [Display(Name = "Apellido materno")]
        public string apellido_materno { get; set; }
         [Display(Name = "Cargo")]
        public string cargo { get; set; }
        [Display(Name = "Fecha nacimiento")]
        public DateTime fecha_nacimiento { get; set; }
        [Display(Name = "Correo electrónico")]
        public string email { get; set; }
        [Display(Name = "RFC")]
        public string rfc { get; set; }
        [Display(Name = "CURP")]
        public string curp { get; set; }
        [Display(Name = "Domicilio")]
        public string direccion { get; set; }
        [Display(Name = "Teléfono")]
        public string telefono { get; set; }
        [Display(Name = "Tipo mando")]
        public string tipo_mando { get; set; }
       
        [Display(Name = "Activo")]
        public int activo { get; set; }
        [Display(Name = "Fotografia")]
        public string fotografia { get; set; }


        public string camera { get; set; }



    }
}