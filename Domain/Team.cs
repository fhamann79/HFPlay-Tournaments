using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Index("Team_Name_LeagueId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Equipo")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(3, ErrorMessage = "El tamaño para el campo {0} debe ser de {1} caracteres", MinimumLength = 3)]
        [Index("Team_Initials_LeagueId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Iniciales")]
        public string Initials { get; set; }

        [Display(Name = "Liga")]
        [Index("Team_Name_LeagueId_Index", IsUnique = true, Order = 2)]
        [Index("Team_Initials_LeagueId_Index", IsUnique = true, Order = 2)]
        public int LeagueId { get; set; }

        public virtual League League { get; set; }

        public virtual ICollection<User> Fans { get; set; }

        public virtual ICollection<Match> Locals { get; set; }

        public virtual ICollection<Match> Visitors { get; set; }

        public virtual ICollection<TournamentTeam> TournamentTeams { get; set; }

        public virtual ICollection<TeamPlayer> TeamPlayers { get; set; }

        public virtual ICollection<TeamManager> TeamManagers { get; set; }

        public virtual ICollection<MatchTeam> MatchTeams { get; set; }

    }
}
