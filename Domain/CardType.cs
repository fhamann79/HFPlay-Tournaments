using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CardType
    {
        [Key]
        public int CardTypeId { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [MaxLength(128, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        public String Name { get; set; }

        public virtual ICollection<MatchTeamPlayerCard> MatchTeamPlayerCards { get; set; }
    }
}
