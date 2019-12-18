using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TournamentManager
    {
        [Key]
        public int TournamentManagerId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(256, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Dirigente")]
        public int UserId { get; set; }

        [Display(Name = "Torneo")]
        public int TournamentId { get; set; }

        [Display(Name = "¿Es aceptado?")]
        public bool IsAccepted { get; set; }

        [Display(Name = "¿Es activo?")]
        public bool IsActive { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual User User { get; set; }
    }
}
