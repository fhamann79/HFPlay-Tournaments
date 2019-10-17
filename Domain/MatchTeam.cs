using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MatchTeam
    {
        [Key]
        public int MatchTeamId { get; set; }

        public bool PlayAsLocal { get; set; }

        [Display(Name = "Presenta Balón")]
        public bool PresentsBall { get; set; }

        [Index("MatchTeam_MatchId_TeamId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Grupo")]
        public int MatchId { get; set; }

        [Index("MatchTeam_MatchId_TeamId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "Grupo")]
        public int TeamId { get; set; }

        public virtual Match Match { get; set; }

        public virtual Team Team { get; set; }

        public virtual ICollection<MatchTeamPlayer> MatchTeamPlayers { get; set; }
    }
}
