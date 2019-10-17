using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Sanction
    {
        [Key]
        public int SanctionId { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        [MaxLength(512, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        public string Description { get; set; }

        [Display(Name = "Multa")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, 10000000, ErrorMessage = "El valor para {0} debe estar entre {1} y {2}.")]
        public decimal PenaltyFee { get; set; }

        [Display(Name = "Número de Partidos")]
        [Range(0, 500, ErrorMessage = "El valor para {0} debe estar entre {1} y {2}.")]
        public int NumberOfMatchs { get; set; }

        [Display(Name = "Número de Meses")]
        [Range(0, 500, ErrorMessage = "El valor para {0} debe estar entre {1} y {2}.")]
        public int NumberOfMonths { get; set; }

        [Display(Name = "Número de Años")]
        [Range(0, 50, ErrorMessage = "El valor para {0} debe estar entre {1} y {2}.")]
        public int NumberOfYears { get; set; }

        [Display(Name = "Torneo")]
        public int TournamentId { get; set; }

        public virtual ICollection<CardSanction> CardSanctions { get; set; }

        public virtual Tournament Tournament { get; set; }

    }
}