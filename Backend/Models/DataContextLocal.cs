using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class DataContextLocal : DataContext
    {
        public System.Data.Entity.DbSet<Domain.Date> Dates { get; set; }

        public System.Data.Entity.DbSet<Domain.TournamentTeam> TournamentTeams { get; set; }

        public System.Data.Entity.DbSet<Domain.UserType> UserTypes { get; set; }

        public System.Data.Entity.DbSet<Domain.User> Users { get; set; }

        public System.Data.Entity.DbSet<Domain.Status> Status { get; set; }

        public System.Data.Entity.DbSet<Domain.Match> Matches { get; set; }

        public System.Data.Entity.DbSet<Domain.Stadium> Stadia { get; set; }

        public System.Data.Entity.DbSet<Domain.Player> Players { get; set; }

        public System.Data.Entity.DbSet<Domain.TeamPlayer> TeamPlayers { get; set; }

        public System.Data.Entity.DbSet<Domain.TeamManager> TeamManagers { get; set; }

        public System.Data.Entity.DbSet<Domain.NewsItem> NewsItems { get; set; }

        public System.Data.Entity.DbSet<Domain.SanctionMeliorate> SanctionMeliorates { get; set; }

        public System.Data.Entity.DbSet<Domain.MatchTeam> MatchTeams { get; set; }

        public System.Data.Entity.DbSet<Domain.MatchTeamPlayer> MatchTeamPlayers { get; set; }

        public System.Data.Entity.DbSet<Domain.MatchTeamPlayerCard> MatchTeamPlayerCards { get; set; }

        public System.Data.Entity.DbSet<Domain.MatchTeamPlayerGoal> MatchTeamPlayerGoals { get; set; }

        public System.Data.Entity.DbSet<Domain.CardType> CardTypes { get; set; }

        public System.Data.Entity.DbSet<Domain.CardSanction> CardSanctions { get; set; }

        public System.Data.Entity.DbSet<Domain.GoalType> GoalTypes { get; set; }

        public System.Data.Entity.DbSet<Domain.Sanction> Sanctions { get; set; }
    }
}