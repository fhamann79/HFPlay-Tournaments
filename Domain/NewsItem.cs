using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class NewsItem
    {
        [Key]
        public int NewsItemId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(500, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Contenido")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        
        [Display(Name = "Fecha de Publicación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime PublicationDate { get; set; }

        
        [Display(Name = "Fecha de Modificación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime ModificationDate { get; set; }
        
        
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string Picture { get; set; }

        [DataType(DataType.Url)]
        [MaxLength(256, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Video Facebook")]
        public string FacebookVideo { get; set; }

        [Display(Name = "¿Es aprobado?")]
        public bool IsApproved { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

    }
}
