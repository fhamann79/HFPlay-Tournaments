using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TeamPlayer
    {
        public int TeamPlayerId { get; set; }

        [Display(Name = "Número")]
        [Range(1, 100,ErrorMessage = "El valor para {0} debe estar entre {1} y {2}.")]
        public int Number { get; set; }

        [Display(Name = "Jugador")]
        public int UserId { get; set; }

        [Display(Name = "Equipo")]
        public int TeamId { get; set; }

        [Display(Name = "¿Es aceptado?")]
        public bool IsAccepted { get; set; }

        [Display(Name = "¿Es activo?")]
        public bool IsActive { get; set; }

        public virtual Team Team { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<MatchTeamPlayer> MatchTeamPlayers { get; set; }
    }
}
