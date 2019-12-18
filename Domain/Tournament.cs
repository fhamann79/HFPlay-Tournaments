using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Tournament
    {
        [Key]
        public int TournamentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Index("Tournament_Name_Index", IsUnique = true)]
        [Display(Name = "Torneo")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Display(Name = "Es Activo?")]
        public bool IsActive { get; set; }

        [Display(Name = "Orden")]
        public int Order { get; set; }

        // the name of virtual property is somethings
        public virtual ICollection<TournamentGroup> TournamentGroups { get; set; }

        public virtual ICollection<Date> Dates { get; set; }

        public virtual ICollection<Sanction> Sanctions { get; set; }

        public virtual ICollection<TournamentManager> TournamentManagers { get; set; }

    }
}
