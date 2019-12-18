using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LeagueCredentialLogo
    {
        [Key]
        public int LeagueCredentialLogoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(256, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Logo Frontal Principal")]
        public string LeagueMainLogo { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Logo Frontal Secundario")]
        public string FrontSecondaryLogo { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Logo Reverso Principal")]
        public string ReverseMainLogo { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Logo Reverso Secundario")]
        public string ReverseSecondaryLogo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(256, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Texto Reverso Principal")]
        public string MainReverseText { get; set; }

        [MaxLength(256, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Texto Reverso Secundario")]
        public string SecondaryReverseText { get; set; }

        [MaxLength(256, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Texto Reverso Alterno")]
        public string AlternateReverseText { get; set; }

        [Display(Name = "Liga")]
        public int LeagueId { get; set; }

        public virtual League League { get; set; }



    }
}
