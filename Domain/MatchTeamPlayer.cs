using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MatchTeamPlayer
    {
        [Key]
        public int MatchTeamPlayerId { get; set; }

        [Display(Name = "¿Es Titular?")]
        public bool IsHeadline { get; set; }

        [Display(Name = "Cambio")]
        public bool Change { get; set; }

        [Display(Name = "Equipo")]
        public int MatchTeamId { get; set; }

        [Display(Name = "Jugador")]
        public int TeamPlayerId { get; set; }

        public virtual MatchTeam MatchTeam { get; set; }

        public virtual TeamPlayer TeamPlayer { get; set; }

        public virtual ICollection<MatchTeamPlayerCard> MatchTeamPlayerCards { get; set; }

        public virtual ICollection<MatchTeamPlayerGoal> MatchTeamPlayerGoals { get; set; }
    }
}
