using Backend.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Backend.Helpers
{
    public class CombosHelper : IDisposable
    {
        private static DataContextLocal db = new DataContextLocal();
        public static List<League> GetLeagues()
        {

           var leagues = db.Leagues.ToList();

            leagues.Add(new League
            {
                LeagueId = 0,
                Name = "[Seleccione una Liga ...]"
            });
            
            return leagues.OrderBy(l => l.Name).ToList();
        }

        public static List<TournamentGroup> GetGroupsMatch(int tournamentId)
        {

            var groups = db.TournamentGroups
                .Where(g => g.TournamentId == tournamentId)
                .ToList();

            groups.Add(new TournamentGroup
            {
                TournamentGroupId = 0,
                Name = "[Seleccione un grupo ...]"
            });

            return groups.OrderBy(l => l.Name).ToList();
        }


        public static List<Team> GetTeamsMatch(int tournamentGroupId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tournamentTeams = db.TournamentTeams
                .Include(t => t.Team)
                .Where(t => t.TournamentGroupId == tournamentGroupId);
            var teams = ExtractTeams(tournamentTeams);

            teams.Add(new Team
            {
                TeamId = 0,
                Name = "[Seleccione un Equipo...]",
            });

            return teams.OrderBy(t => t.Name).ToList();
        }


        public static List<Team> GetTeams(int leagueId)
        {
            var teams = db.Teams.Where(c => c.LeagueId == leagueId).ToList();

            teams.Add(new Team
            {
                TeamId = 0,
                Name = "[Seleccione un Equipo...]",
            });

            return teams.OrderBy(t => t.Name).ToList();
        }

        private static List<Team> ExtractTeams(IQueryable<TournamentTeam> tournamentTeams)
        {
            var teams = new List<Team>();

            foreach (var item in tournamentTeams)
            {
                teams.Add(new Team
                {
                    Name = item.Team.Name,
                    TeamId = item.Team.TeamId
                });
            }
            return teams;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        //public static List<Company> GetCompanies()
        //{

        //    //creamos variable para el combo
        //    var companies = db.Companies.ToList();

        //    //anadimos mensaje
        //    companies.Add(new Company
        //    {
        //        CompanyId = 0,
        //        Name = "[Seleccione una empresa ...]"
        //    });
        //    //ordenamos por nombre
        //    return companies.OrderBy(c => c.Name).ToList();
        //}

        //public void Dispose()
        //{
        //    db.Dispose();
        //}



        //public static List<Category> GetCategories(int companyId)
        //{

        //    var categories = db.Categories.Where(c => c.CompanyId == companyId).ToList();

        //    categories.Add(new Category
        //    {
        //        CategoryId = 0,
        //        Description = "[Seleccione una categoria ...]"
        //    });

        //    return categories.OrderBy(c => c.Description).ToList();
        //}

        //public static List<Tax> GetTaxes(int companyId)
        //{

        //    var taxes = db.Taxes.Where(t => t.CompanyId == companyId).ToList();

        //    taxes.Add(new Tax
        //    {
        //        TaxId = 0,
        //        Description = "[Seleccione un impuesto ...]"
        //    });

        //    return taxes.OrderBy(t => t.Description).ToList();
        //}

        //public static List<Customer> GetCustomers(int companyId)
        //{

        //    var qry = (from cu in db.Customers
        //               join cc in db.CompanyCustomers on cu.CustomerId equals cc.CustomerId
        //               join co in db.Companies on cc.CompanyId equals co.CompanyId
        //               where co.CompanyId == companyId
        //               select new { cu }).ToList();




        //    var customers = new List<Customer>();



        //    foreach (var item in qry)
        //    {
        //        customers.Add(item.cu);
        //    }

        //    customers.Add(new Customer
        //    {
        //        CustomerId = 0,
        //        FirstName = "[Seleccione un cliente ...]"
        //    });

        //    return customers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName).ToList();
        //}

        //public static List<Product> GetProducts(int companyId)
        //{
        //    var products = db.Products.Where(p => p.CompanyId == companyId).ToList();

        //    products.Add(new Product
        //    {
        //        ProductId = 0,
        //        Description = "[Seleccione un producto...]"
        //    });

        //    return products.OrderBy(p => p.Description).ToList();
        //}

        //public static List<Product> GetProducts(int companyId, bool sw)
        //{
        //    var products = db.Products.Where(p => p.CompanyId == companyId).ToList();
        //    return products.OrderBy(p => p.Description).ToList();
        //}



    }

}