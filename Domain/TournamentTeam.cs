using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TournamentTeam
    {
        [Key]
        public int TournamentTeamId { get; set; }

        [Index("TournamentTeam_TournamentGroupId_TeamId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Grupo")]
        public int TournamentGroupId { get; set; }

        [Index("TournamentTeam_TournamentGroupId_TeamId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "Equipo")]
        public int TeamId { get; set; }

        [Display(Name = "Partidos Jugados")]
        public int MatchesPlayed { get; set; }

        [Display(Name = "Partidos Ganados")]
        public int MatchesWon { get; set; }

        [Display(Name = "Partidos Perdidos")]
        public int MatchesLost { get; set; }

        [Display(Name = "Partidos Empatados")]
        public int MatchesTied { get; set; }

        [Display(Name = "Goles a Favor")]
        public int FavorGoals { get; set; }

        [Display(Name = "Goles en Contra")]
        public int AgainstGoals { get; set; }

        public int Points { get; set; }

        public int Position { get; set; }

        [NotMapped]
        public int DifferenceGoals  { get { return FavorGoals - AgainstGoals; } }

        public virtual TournamentGroup TournamentGroup { get; set; }

        public virtual Team Team { get; set; }

        public virtual List<SanctionMeliorate> SanctionMeliorates { get; set; }
    }

}
