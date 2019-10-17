using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CardSanction
    {
        [Key]
        public int CardSanctionId { get; set; }

        [Index("CardSanction_MatchTeamPlayerCardId_SanctionId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Grupo")]
        public int MatchTeamPlayerCardId { get; set; }

        [Index("CardSanction_MatchTeamPlayerCardId_SanctionId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "Grupo")]
        public int SanctionId { get; set; }

        public virtual Sanction Sanction { get; set; }

        public virtual MatchTeamPlayerCard MatchTeamPlayerCard { get; set; }
    }
}
