using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class LeagueCredentialLogoView : LeagueCredentialLogo
    {
        [Display(Name = "Logo Principal")]
        public HttpPostedFileBase LeagueMainLogoView { get; set; }

        [Display(Name = "Logo Frontal Secundario")]
        public HttpPostedFileBase FrontSecondaryLogoView { get; set; }

        [Display(Name = "Logo Reverso Principal")]
        public HttpPostedFileBase ReverseMainLogoView { get; set; }

        [Display(Name = "Logo Reverso Secundario")]
        public HttpPostedFileBase ReverseSecondaryLogoView { get; set; }
    }
}