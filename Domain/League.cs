using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class League
    {
        [Key]
        public int LeagueId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Index("League_Name_Index", IsUnique = true)]
        [Display(Name = "Liga")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
