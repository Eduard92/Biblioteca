using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RK.Models
{
    public class AreaModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Required]
        [Display(Name = "Centro")]
        public int id_centro { get; set; }

            
    }
}