using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        [Display(Name = "Fecha")]
        public int DateId { get; set; }

        [Display(Name = "Fecha y hora")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:f}")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Informe Arbitral")]
        [MaxLength(512, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        public string ArbitrationReport { get; set; }

        [Display(Name = "Informe Delegado")]
        [MaxLength(512, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        public string DelegatedReport { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Acta Anverso")]
        public string AdverseAct { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Acta Reverso")]
        public string BackAct { get; set; }

        [NotMapped]
        public String DateTimeEc { get { return DateTime.AddHours(-5).ToString("f", CultureInfo.CreateSpecificCulture("es-EC")); } }

        [Display(Name = "Local")]
        public int LocalId { get; set; }

        [Display(Name = "Visitante")]
        public int VisitorId { get; set; }

        [Display(Name = "Goles de local")]
        public int? LocalGoals { get; set; }

        [Display(Name = "Goles de visitante")]
        public int? VisitorGoals { get; set; }

        [Display(Name = "Estado")]
        public int StatusId { get; set; }

        [Display(Name = "Estadio")]
        public int StadiumId { get; set; }

        [Display(Name = "Grupo")]
        public int TournamentGroupId { get; set; }

        public virtual Date Date { get; set; }

        public virtual Team Local { get; set; }

        public virtual Team Visitor { get; set; }

        public virtual Status Status { get; set; }

        public virtual Stadium Stadium { get; set; }

        public virtual TournamentGroup TournamentGroup { get; set; }

        public virtual ICollection<Prediction> Predictions { get; set; }

        public virtual ICollection<MatchTeam> MatchTeams { get; set; }
    }

}
