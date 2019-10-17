using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(75, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Nombres")]
        public string FirtsName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(75, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(10, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [RegularExpression(@"^[\d]{0,10}$", ErrorMessage = "Ingresar 10 dígitos")]
        [Index("Player_IdentificationCard_Index", IsUnique = true)]
        [Display(Name = "Cédula")]
        public string IdentificationCard { get; set; }

        [MaxLength(100, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime Birthdate { get; set; }
        
        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        
    }
}
