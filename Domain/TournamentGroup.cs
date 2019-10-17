using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class TournamentGroup
    {
        [Key]
        public int TournamentGroupId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Index("TournamentGroup_Name_TournamentId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Grupo")]
        public string Name { get; set; }

        [Display(Name = "Torneo")]
        [Index("TournamentGroup_Name_TournamentId_Index", IsUnique = true, Order = 2)]
        public int TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual ICollection<TournamentTeam> TournamentTeams { get; set; }

        public virtual ICollection<Match> Matches { get; set; }

    }
}
