using Domain;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class IndexHomeViewModel
    {
        public User User { get; set; }
        public IPagedList<NewsItem> NewItems { get; set; }
        public List<Tournament> Tournaments { get; set; }
        public List<TournamentTeam> TournamentTeams { get; set; }
    }
}