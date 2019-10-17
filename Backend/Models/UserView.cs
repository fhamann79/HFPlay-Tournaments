using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    [NotMapped]
    public class UserView : User
    {
        [Display(Name = "Foto")]
        public HttpPostedFileBase PhotoFile { get; set; }

        [Display(Name = "Liga Favorita")]
        public int FavoriteLeagueId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "El tamaño para el campo {0} debe mínimo {1} caracteres")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "El Password y la confirmación deben ser idénticos")]
        [Display(Name = "Confirmar Contraseña")]
        public string PasswordConfirm { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Nacimiento")]
        public string BirthDateString { get; set; }
    }
}