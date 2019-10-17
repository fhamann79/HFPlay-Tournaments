using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SanctionMeliorate
    {
        [Key]
        public int SanctionMeliorateId { get; set; }

        [Display(Name = "Puntaje")]
        public int Value { get; set; }

        [Display(Name = "Tipo")]
        public String Type { get; set; }

        [Display(Name = "Descripción")]
        public String Description { get; set; }

        public int TournamentTeamId { get; set; }

        public virtual TournamentTeam TournamentTeam { get; set; }
    }
}
