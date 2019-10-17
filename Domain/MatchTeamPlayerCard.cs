using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MatchTeamPlayerCard
    {
        [Key]
        public int MatchTeamPlayerCardId { get; set; }

        [Display(Name = "Minuto")]
        [Range(1, 90, ErrorMessage = "El valor para {0} debe estar entre {1} y {2}.")]
        public int Minute { get; set; }

      
        [Display(Name = "Jugador")]
        public int MatchTeamPlayerId { get; set; }

        [Display(Name = "Tarjeta")]
        public int CardTypeId { get; set; }

        public virtual MatchTeamPlayer MatchTeamPlayer { get; set; }

        public virtual CardType CardType { get; set; }

        public virtual ICollection<CardSanction> CardSanctions { get; set; }


    }
}
