using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MProjectWeb.ViewModels
{
    public partial class usuarios
    {



        [Display(Name = "ID")]
        public long id_usuario { get; set; }
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string e_mail { get; set; }
        
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        
        [Display(Name = "Apellido")]
        public string apellido { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string pass { get; set; }  //variable para registrar la clave de usuario

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Compare("pass", ErrorMessage = "La clave actual no corresponde a su clave")]
        public string passChan { get; set; } //corresponde a la clave actual a ser cambiada

        [Display(Name = "clave actual")]
        [DataType(DataType.Password)]
        public string newPass { get; set; } //nueva clave


        [Display(Name = "Repetir clave")]
        [DataType(DataType.Password)]
        [Compare("newPass",ErrorMessage ="Las claves deben ser iguales.")]
        public string repeatPass { get; set; } //variable para repetir la clave

        [Display(Name = "Cargo")]
        [DataType(DataType.Text)]
        public string cargo { get; set; }
        
        [Display(Name = "Entidad")]
        public string entidad { get; set; }
        
        [Display(Name = "Genero")]
        public string genero { get; set; }

        [Display(Name = "Imagen")]
        public string imagen { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile file { get; set; }

        [Display(Name = "Telefono")]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string telefono { get; set; }

        public string path { get; set; }


        public bool administrador { get; set; }
        public bool aux { get; set; }

        public bool disponible { get; set; }
    }
}
