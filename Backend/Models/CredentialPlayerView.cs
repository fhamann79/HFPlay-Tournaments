using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class CredentialPlayerView
    {
        public TeamPlayer TeamPlayer { get; set; }
        public LeagueCredentialLogo LeagueCredentialLogo { get; set; }
    }
}