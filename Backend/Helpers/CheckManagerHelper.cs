using Backend.Models;
using Domain;
using System;
using System.Linq;
using System.Data;
namespace Backend.Helpers
{
    public class CheckResponse
    {
        public League League { get; set; }
        public bool Response { get; set; }
    }

    public static class CheckManagerHelper
    {
        private static DataContextLocal db = new DataContextLocal();

        public static CheckResponse CheckLeagueManager(string userASPId, int leagueId)
        {
            var checkResponse = new CheckResponse();

            try
            {
                var qry = (from l in db.Leagues
                           where l.LeagueId == leagueId
                           join lm in db.LeagueManagers on l.LeagueId equals lm.LeagueId
                           join u in db.Users on lm.UserId equals u.UserId
                           where u.UserASPId == userASPId
                           select new { l }).FirstOrDefault();

                var league = db.Leagues.Find(qry.l.LeagueId);

                if (league == null)
                {
                    checkResponse.League = null;
                    checkResponse.Response = false;
                    return checkResponse;
                }

                checkResponse.League = league;
                checkResponse.Response = true;

                return checkResponse;
            }
            catch (NullReferenceException)
            {
                checkResponse.League = null;
                checkResponse.Response = false;
                return checkResponse;
            }
        }

 


    }
}