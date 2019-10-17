using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Backend.Models
{
    [NotMapped]
    public class MatchView : Match
    {
        [DataType(DataType.Date) ]
        [Display (Name = "Fecha")]
        public string DateString { get; set; }
        
        [DataType(DataType.Time)]
        [Display(Name = "Hora")]
        public string TimeString { get; set; }

        [Display(Name = "Cara Uno")]
        public HttpPostedFileBase AdverseActFile { get; set; }

        [Display(Name = "Cara Dos")]
        public HttpPostedFileBase BackActFile { get; set; }
    }
}