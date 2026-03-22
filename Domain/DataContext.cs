using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Domain
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UsersMap());
            modelBuilder.Configurations.Add(new MatchesMap());
        }

        public DbSet<League> Leagues { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentGroup> TournamentGroups { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        public System.Data.Entity.DbSet<Domain.MatchTeamPlayerCard> MatchTeamPlayerCards { get; set; }

        public System.Data.Entity.DbSet<Domain.CardType> CardTypes { get; set; }

        public System.Data.Entity.DbSet<Domain.MatchTeamPlayer> MatchTeamPlayers { get; set; }
    }
}
