using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class MatchTeamPlayerGoal
    {
        [Key]
        public int MatchTeamPlayerGoalId { get; set; }

        [Display(Name = "Minuto")]
        [Range(1, 90, ErrorMessage = "El valor para {0} debe estar entre {1} y {2}.")]
        public int Minute { get; set; }

        [Display(Name = "Jugador")]
        public int MatchTeamPlayerId { get; set; }

        [Display(Name = "Tarjeta")]
        public int GoalTypeId { get; set; }

        public virtual MatchTeamPlayer MatchTeamPlayer { get; set; }

        public virtual GoalType GoalType { get; set; }
    }
}