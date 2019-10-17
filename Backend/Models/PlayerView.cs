using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    [NotMapped]
    public class PlayerView : Player
    {
        [Display(Name = "Foto")]
        public HttpPostedFileBase PhotoFile { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public string DateString { get; set; }
    }
}