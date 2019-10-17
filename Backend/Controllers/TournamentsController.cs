using Backend.Helpers;
using Backend.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TournamentsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        private int widthPhotoTournament = 360;

        private int heigthPhotoTournament = 360;

        #region Admin

        public async Task<ActionResult> AddCardToPlayer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var matchTeamPlayer = await db.MatchTeamPlayers.FindAsync(id);

            if (matchTeamPlayer == null)
            {
                return HttpNotFound();
            }

            var matchTeamPlayerCard = new MatchTeamPlayerCard { MatchTeamPlayerId = matchTeamPlayer.MatchTeamPlayerId};

            ViewBag.CardTypeId = new SelectList(db.CardTypes, "CardTypeId", "Name");

            return View(matchTeamPlayerCard);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCardToPlayer(MatchTeamPlayerCard matchTeamPlayerCard)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.MatchTeamPlayerCards.Add(matchTeamPlayerCard);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.CreateOk();
                    return RedirectToAction("DetailsMatchTeamPlayer", new { id = matchTeamPlayerCard.MatchTeamPlayerId });
                    
                }
                
                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());
            }
            ViewBag.CardTypeId = new SelectList(db.CardTypes, "CardTypeId", "Name", matchTeamPlayerCard.CardTypeId);
            return View(matchTeamPlayerCard);
        }

        public async Task<ActionResult> DetailsMatchTeamPlayer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var matchTeamPlayer = await db.MatchTeamPlayers.FindAsync(id);

            if (matchTeamPlayer == null)
            {
                return HttpNotFound();
            }

            return View(matchTeamPlayer);
        }



        public async Task<ActionResult> EditMatchList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var matchTeamPlayers = await db.MatchTeamPlayers.Where(mtp => mtp.MatchTeamId == id).OrderBy(mtp => mtp.TeamPlayer.Number).ToListAsync();
            
            return View(matchTeamPlayers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMatchList(List<MatchTeamPlayer> itemPlayer, int matchId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach(var item in itemPlayer)
                    {
                        db.Entry(item).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    TempData["SuccessMessage"] = MessageHelper.EditListOk();
                    return RedirectToAction("DetailsMatch", new { id = matchId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());
            }

            return View(itemPlayer);
        }

        public async Task<ActionResult> CreateSanction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var santion = new Sanction() { TournamentId = tournament.TournamentId};

            return View(santion);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSanction(Sanction sanction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Sanctions.Add(sanction);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.CreateOk();
                    return RedirectToAction("Details", new { id = sanction.TournamentId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());
            }

            return View(sanction);
        }


        public async Task<ActionResult> EditMatchAct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var match = await db.Matches.FindAsync(id);
            var view = new MatchView
            {
                MatchId = id.Value,
                AdverseAct = match.AdverseAct,
                BackAct = match.BackAct,
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMatchAct(MatchView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var adverseAct = view.AdverseAct;
                    var backAct = view.BackAct;

                    var folder = "~/Content/Act";

                    if (view.AdverseActFile != null)
                    {
                        adverseAct = FileHelpers.UploadPhoto(view.AdverseActFile, folder, widthPhotoTournament, heigthPhotoTournament);
                        adverseAct = string.Format("{0}/{1}", folder, adverseAct);
                    }

                    if (view.BackActFile != null)
                    {
                        backAct = FileHelpers.UploadPhoto(view.BackActFile, folder, widthPhotoTournament, heigthPhotoTournament);
                        backAct = string.Format("{0}/{1}", folder, backAct);
                    }

                    var match = await db.Matches.FindAsync(view.MatchId);
                    match.AdverseAct = adverseAct;
                    match.BackAct = backAct;
                                        
                    db.Entry(match).State = EntityState.Modified;
                    
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.EditOk();
                    return RedirectToAction("DetailsMatch", new { id = match.MatchId });

                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {
                ModelState.AddModelError("", MessageHelper.ExceptionData());

            }


            return View(view);
            
           
        }

        public async Task<ActionResult> EditMatchReports(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var match = await db.Matches.FindAsync(id);
            var view = new MatchView
            {
                MatchId = id.Value,
                ArbitrationReport = match.ArbitrationReport,
                DelegatedReport = match.DelegatedReport,
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMatchReports(MatchView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var match = await db.Matches.FindAsync(view.MatchId);
                    match.ArbitrationReport = view.ArbitrationReport;
                    match.DelegatedReport = view.DelegatedReport;
                    db.Entry(match).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.EditOk();
                    return RedirectToAction("DetailsMatch", new { id = match.MatchId });

                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {
                ModelState.AddModelError("", MessageHelper.ExceptionData());

            }
            
            return View(view);
        }

        public ActionResult DetailsMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }

            if (match.MatchTeams.Count == 0)
            {
                FillMatchTeamPlayers(match);
                
            }

            var matchView = db.Matches.Find(id);

            return View(matchView);
        }

        private void FillMatchTeamPlayers(Match match)
        {
            var matchTeamLocal = new MatchTeam
            {
                MatchId = match.MatchId,
                TeamId = match.Local.TeamId,
                PlayAsLocal = true,
                PresentsBall = false,
            };
            var matchTeamVisitor = new MatchTeam
            {
                MatchId = match.MatchId,
                TeamId = match.Visitor.TeamId,
                PlayAsLocal = false,
                PresentsBall = false,
            };
            db.MatchTeams.Add(matchTeamLocal);
            db.MatchTeams.Add(matchTeamVisitor);
            db.SaveChanges();

            var matchTeamLocalDb = db.MatchTeams
                .Where(mt => mt.MatchId == match.MatchId && mt.TeamId == match.LocalId)
                .FirstOrDefault();
            var matchTeamVisitorDb = db.MatchTeams
                .Where(mt => mt.MatchId == match.MatchId && mt.TeamId == match.VisitorId)
                .FirstOrDefault();

            var teamPlayersLocal = db.TeamPlayers.Where(tp => tp.TeamId == match.LocalId).ToList();

            foreach (var player in teamPlayersLocal)
            {
                var matchTeamPlayerLocal = new MatchTeamPlayer
                {
                    IsHeadline = false,
                    Change = false,
                    MatchTeamId = matchTeamLocalDb.MatchTeamId,
                    TeamPlayerId = player.TeamPlayerId,
                };

                db.MatchTeamPlayers.Add(matchTeamPlayerLocal);
                
            }
            db.SaveChanges();

            var teamPlayersVisitor = db.TeamPlayers.Where(tp => tp.TeamId == match.VisitorId).ToList();

            foreach (var player in teamPlayersVisitor)
            {
                var matchTeamPlayerVisitor = new MatchTeamPlayer
                {
                    IsHeadline = false,
                    Change = false,
                    MatchTeamId = matchTeamVisitorDb.MatchTeamId,
                    TeamPlayerId = player.TeamPlayerId,
                };

                db.MatchTeamPlayers.Add(matchTeamPlayerVisitor);
             
            }
            db.SaveChanges();
        }

        public async Task<ActionResult> DeleteSanctionMeliorateTeam(int? id, bool? saveChangesError = false)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }
            var sanctionMeliorate = await db.SanctionMeliorates.FindAsync(id);
            if (sanctionMeliorate == null)
            {
                return HttpNotFound();
            }
            return View(sanctionMeliorate);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSanctionMeliorateTeam(int id)
        {
            var sanctionMeliorate = await db.SanctionMeliorates.FindAsync(id);
            using (var transacction = db.Database.BeginTransaction())
            {
                try
                {
                    db.SanctionMeliorates.Remove(sanctionMeliorate);
                    await db.SaveChangesAsync();

                    var tournamentTeam = await db.TournamentTeams.FindAsync(sanctionMeliorate.TournamentTeamId);
                    tournamentTeam.Points -= sanctionMeliorate.Value;
                    db.Entry(tournamentTeam).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    //Update Positions
                    var teams = await db.TournamentTeams
                    .Where(tt => tt.TournamentGroupId == tournamentTeam.TournamentGroupId)
                    .ToListAsync();
                    var i = 1;
                    foreach (var team in teams.OrderByDescending(t => t.Points)
                                                .ThenByDescending(t => t.FavorGoals - t.AgainstGoals)
                                                .ThenByDescending(t => t.FavorGoals))
                    {
                        team.Position = i;
                        db.Entry(team).State = EntityState.Modified;
                        i++;
                    }

                    await db.SaveChangesAsync();
                    transacction.Commit();
                    TempData["SuccessMessage"] = MessageHelper.DeleteOk();
                }
                catch (DataException/* dex */)
                {
                    transacction.Rollback();
                                       
                    return RedirectToAction("DeleteSanctionMeliorateTeam", new { id = id, saveChangesError = true });
                }
            }
            

            return RedirectToAction("DetailsSanctionMeliorateTeam", new { id = sanctionMeliorate.TournamentTeamId });
        }

        public ActionResult CreateSanctionMeliorateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentTeam tournamentTeam = db.TournamentTeams.Find(id);
            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }

            var sanctionMeliorate = new SanctionMeliorate();

            sanctionMeliorate.TournamentTeamId = tournamentTeam.TournamentTeamId;
            
            
            return View(sanctionMeliorate);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSanctionMeliorateTeam(SanctionMeliorate sanctionMeliorate)
        {
            if (ModelState.IsValid)
            {
                using (var transacction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var value = sanctionMeliorate.Value;
                        if (value > 0)
                        {
                            sanctionMeliorate.Type = string.Format("{0}", "Bonificación");
                        }
                        else if (value < 0)
                        {
                            sanctionMeliorate.Type = string.Format("{0}", "Sanción");
                        }
                        else
                        {
                            ModelState.AddModelError("", MessageHelper.NotZero());
                            return View(sanctionMeliorate);
                        }
                        
                        db.SanctionMeliorates.Add(sanctionMeliorate);
                        await db.SaveChangesAsync();

                        var tournamentTeam = await db.TournamentTeams.FindAsync(sanctionMeliorate.TournamentTeamId);
                        tournamentTeam.Points += value;
                        db.Entry(tournamentTeam).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        //Update Positions
                        var teams = await db.TournamentTeams
                        .Where(tt => tt.TournamentGroupId == tournamentTeam.TournamentGroupId)
                        .ToListAsync();
                        var i = 1;
                        foreach (var team in teams.OrderByDescending(t => t.Points)
                                                    .ThenByDescending(t => t.FavorGoals - t.AgainstGoals)
                                                    .ThenByDescending(t => t.FavorGoals))
                        {
                            team.Position = i;
                            db.Entry(team).State = EntityState.Modified;
                            i++;
                        }
                        
                        await db.SaveChangesAsync();
                        transacction.Commit();

                        return RedirectToAction("DetailsSanctionMeliorateTeam", new { id = sanctionMeliorate.TournamentTeamId });
                    }
                    catch (Exception ex)
                    {
                        transacction.Rollback();
                        ModelState.AddModelError(string.Empty, ex.Message);
                        return View(sanctionMeliorate);
                    }
                }
                
            }

            
            return View(sanctionMeliorate);
        }


        public async Task<ActionResult> DetailsSanctionMeliorateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentTeam tournamentTeam = await db.TournamentTeams.FindAsync(id);
            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }
            return View(tournamentTeam);
            
        }

        public async Task<ActionResult> CloseMatchAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var match = await db.Matches.FindAsync(id);

            if (match == null)
            {
                return HttpNotFound();
            }

            var status = db.Status.Where(s => s.Name == "Cerrado").FirstOrDefault();

            if(status == null)
            {
                TempData["ErrorMessage"] = MessageHelper.NoStateFind();
                return RedirectToAction("DetailsDate", new { id = match.DateId});
            }

            //if (match.StatusId == status.StatusId)
            //{
            //    TempData["ErrorMessage"] = MessageHelper.MatchIsClose();
            //    return RedirectToAction("DetailsDate", new { id = match.DateId });
            //}

            return View(match);
        }
        




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CloseMatchAdmin(Match match)
        {
            using (var transacction = db.Database.BeginTransaction())
            {
                try
                {
                    
                    var oldMatch = await db.Matches.FindAsync(match.MatchId);
                    var oldLocalGoals = oldMatch.LocalGoals;
                    var oldVisitorGoals = oldMatch.VisitorGoals;
                    oldMatch.LocalGoals = match.LocalGoals;
                    oldMatch.VisitorGoals = match.VisitorGoals;
                    var statusMatch = GetStatus(match.LocalGoals.Value, match.VisitorGoals.Value);
                    
                    var status = db.Status.Where(s => s.Name == "Cerrado").FirstOrDefault();

                    if (status == null)
                    {
                        TempData["ErrorMessage"] = MessageHelper.NoStateFind();
                        return RedirectToAction("DetailsDate", new { id = match.DateId });
                    }
                        
                    if (oldMatch.StatusId != status.StatusId) //if match is not close
                    {
                        // Update match

                        oldMatch.StatusId = status.StatusId;

                        db.Entry(oldMatch).State = EntityState.Modified;
                                               

                        // Update tournaments statatistics
                        var local = await db.TournamentTeams
                            .Where(tt => tt.TournamentGroupId == oldMatch.TournamentGroupId &&
                                            tt.TeamId == oldMatch.LocalId)
                            .FirstOrDefaultAsync();

                        var visitor = await db.TournamentTeams
                            .Where(tt => tt.TournamentGroupId == oldMatch.TournamentGroupId &&
                                            tt.TeamId == oldMatch.VisitorId)
                            .FirstOrDefaultAsync();

                        local.MatchesPlayed++;
                        local.FavorGoals += oldMatch.LocalGoals.Value;
                        local.AgainstGoals += oldMatch.VisitorGoals.Value;

                        visitor.MatchesPlayed++;
                        visitor.FavorGoals += oldMatch.VisitorGoals.Value;
                        visitor.AgainstGoals += oldMatch.LocalGoals.Value;

                        if (statusMatch == 1)
                        {
                            local.MatchesWon++;
                            local.Points += 3;
                            visitor.MatchesLost++;
                        }
                        else if (statusMatch == 2)
                        {
                            visitor.MatchesWon++;
                            visitor.Points += 3;
                            local.MatchesLost++;
                        }
                        else
                        {
                            local.MatchesTied++;
                            visitor.MatchesTied++;
                            local.Points++;
                            visitor.Points++;
                        }

                        db.Entry(local).State = EntityState.Modified;
                        db.Entry(visitor).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                    }
                    else // if match is close
                    {
                        // Update match

                        db.Entry(oldMatch).State = EntityState.Modified;

                        var statusOldMatch = GetStatus(oldLocalGoals.Value, oldVisitorGoals.Value);

                        // Update tournaments statatistics
                        var local = await db.TournamentTeams
                            .Where(tt => tt.TournamentGroupId == oldMatch.TournamentGroupId &&
                                            tt.TeamId == oldMatch.LocalId)
                            .FirstOrDefaultAsync();

                        var visitor = await db.TournamentTeams
                            .Where(tt => tt.TournamentGroupId == oldMatch.TournamentGroupId &&
                                            tt.TeamId == oldMatch.VisitorId)
                            .FirstOrDefaultAsync();

                        local.FavorGoals -= oldLocalGoals.Value;
                        local.FavorGoals += oldMatch.LocalGoals.Value;
                        local.AgainstGoals -= oldVisitorGoals.Value;
                        local.AgainstGoals += oldMatch.VisitorGoals.Value;
                        visitor.FavorGoals -= oldVisitorGoals.Value;
                        visitor.FavorGoals += oldMatch.VisitorGoals.Value;
                        visitor.AgainstGoals -= oldLocalGoals.Value;
                        visitor.AgainstGoals += oldMatch.LocalGoals.Value;

                        if (statusOldMatch == 1)
                        {
                            if( statusMatch == 1)
                            {
                                
                            }
                            else if (statusMatch == 2)
                            {
                                local.MatchesWon--;
                                local.MatchesLost++;
                                local.Points -= 3;
                                visitor.MatchesWon++;
                                visitor.MatchesLost--;
                                visitor.Points += 3;
                                
                                
                            }
                            else
                            {
                                local.MatchesWon--;
                                local.MatchesTied++;
                                local.Points -= 2;
                                visitor.Points++;
                                visitor.MatchesLost--;
                                visitor.MatchesTied++;
                            }
                            
                        }
                        else if (statusOldMatch == 2)
                        {
                            if (statusMatch == 1)
                            {
                                local.Points += 3;
                                local.MatchesLost--;
                                local.MatchesWon++;
                                visitor.MatchesWon--;
                                visitor.MatchesLost++;
                                visitor.Points -= 3;
                                
                            }
                            else if (statusMatch == 2)
                            {

                            }
                            else
                            {
                                local.Points++;
                                local.MatchesLost--;
                                local.MatchesTied++;
                                visitor.Points -= 2;
                                visitor.MatchesWon--;
                                visitor.MatchesTied++;
                            }
                            
                        }
                        else
                        {
                            if(statusMatch == 1)
                            {
                                local.Points += 2;
                                local.MatchesTied--;
                                local.MatchesWon++;
                                visitor.Points--;
                                visitor.MatchesTied--;
                                visitor.MatchesLost++;
                            }
                            else if (statusMatch == 2)
                            {
                                local.Points--;
                                local.MatchesTied--;
                                local.MatchesLost++;
                                visitor.Points += 2;
                                visitor.MatchesWon++;
                                visitor.MatchesTied--;
                            }
                            else
                            {

                            }
                            
                        }

                        db.Entry(local).State = EntityState.Modified;
                        db.Entry(visitor).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        
                    }

                    // Update positions
                    var teams = await db.TournamentTeams
                        .Where(tt => tt.TournamentGroupId == oldMatch.TournamentGroupId)
                        .ToListAsync();
                    var i = 1;
                    foreach (var team in teams.OrderByDescending(t => t.Points)
                                                .ThenByDescending(t => t.FavorGoals - t.AgainstGoals)
                                                .ThenByDescending(t => t.FavorGoals))
                    {
                        team.Position = i;
                        db.Entry(team).State = EntityState.Modified;
                        i++;
                    }

                    //// Update predictions
                    //var predictions = await db.Predictions.Where(p => p.MatchId == oldMatch.MatchId).ToListAsync();
                    //foreach (var prediction in predictions)
                    //{
                    //    var points = 0;
                    //    if (prediction.LocalGoals == oldMatch.LocalGoals &&
                    //        prediction.VisitorGoals == oldMatch.VisitorGoals)
                    //    {
                    //        points = 3;
                    //    }
                    //    else
                    //    {
                    //        var statusPrediction = GetStatus(prediction.LocalGoals, prediction.VisitorGoals);
                    //        if (statusMatch == statusPrediction)
                    //        {
                    //            points = 1;
                    //        }
                    //    }

                    //    if (points != 0)
                    //    {
                    //        prediction.Points = points;
                    //        db.Entry(prediction).State = EntityState.Modified;
                    //    }

                    //    // Update user
                    //    var user = await db.Users.FindAsync(prediction.UserId);
                    //    user.Points += points;
                    //    db.Entry(user).State = EntityState.Modified;

                    //    // Update points in groups
                    //    var groupUsers = await db.GroupUsers.Where(gu => gu.UserId == user.UserId &&
                    //                                            gu.IsAccepted &&
                    //                                            !gu.IsBlocked)
                    //                                            .ToListAsync();
                    //    foreach (var groupUser in groupUsers)
                    //    {
                    //        groupUser.Points += points;
                    //        db.Entry(groupUser).State = EntityState.Modified;
                    //    }
                    //}

                    await db.SaveChangesAsync();
                    transacction.Commit();
                    return RedirectToAction("DetailsDate", new { id = oldMatch.DateId });
                }
                catch (Exception ex)
                {
                    transacction.Rollback();
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(match);
                }
            }

        }

        public async Task<ActionResult> DeleteMatch(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }
            Match match = await db.Matches.FindAsync(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteMatch(int id)
        {
            Match match = await db.Matches.FindAsync(id);
            try
            {
                db.Matches.Remove(match);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("DeleteMatch", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("DetailsDate", new { id = match.DateId });
        }

        // GET: Matches/Edit/5
        public async Task<ActionResult> EditMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = await db.Matches.FindAsync(id);

            if (match == null)
            {
                return HttpNotFound();
            }

            var view = ToMatchView(match);
            var date = await db.Dates.FindAsync(match.DateId);
            ViewBag.StadiumId = new SelectList(db.Stadia, "StadiumId", "Name", match.StadiumId);
            ViewBag.LocalId = new SelectList(CombosHelper.GetTeamsMatch(match.TournamentGroupId), "TeamId", "Name", match.LocalId);
            ViewBag.VisitorId = new SelectList(CombosHelper.GetTeamsMatch(match.TournamentGroupId), "TeamId", "Name", match.VisitorId);
            ViewBag.TournamentGroupId = new SelectList(CombosHelper
                .GetGroupsMatch(date.TournamentId), "TournamentGroupId", "Name", match.TournamentGroupId);
            ViewBag.StatusId = new SelectList(db.Status.OrderBy(s => s.Name), "StatusId", "Name", match.StatusId);
            return View(view);
        }

        

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMatch(MatchView view)
        {
            try
            {
                if (ModelState.IsValid && view.LocalId != view.VisitorId)
                {
                    var match = ToMatch(view);
                    db.Entry(match).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.EditOk();
                    return RedirectToAction("DetailsDate", new { id = match.DateId });

                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());

            }

            
            var date = await db.Dates.FindAsync(view.DateId);

            ViewBag.StadiumId = new SelectList(db.Stadia, "StadiumId", "Name", view.StadiumId);
            ViewBag.LocalId = new SelectList(CombosHelper.GetTeamsMatch(view.TournamentGroupId), "TeamId", "Name", view.LocalId);
            ViewBag.VisitorId = new SelectList(CombosHelper.GetTeamsMatch(view.TournamentGroupId), "TeamId", "Name", view.VisitorId);
            ViewBag.TournamentGroupId = new SelectList(CombosHelper
                .GetGroupsMatch(date.TournamentId), "TournamentGroupId", "Name", view.TournamentGroupId);
            ViewBag.StatusId = new SelectList(db.Status.OrderBy(s => s.Name), "StatusId", "Name", view.StatusId);
            return View(view);
        }


        public async Task<ActionResult> CreateMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }

            ViewBag.StadiumId = new SelectList(db.Stadia.OrderBy(s => s.Name), "StadiumId", "Name");
            ViewBag.LocalId = new SelectList(CombosHelper.GetTeamsMatch(0).OrderBy(t => t.Name), "TeamId", "Name");
            ViewBag.VisitorId = new SelectList(CombosHelper.GetTeamsMatch(0).OrderBy(t => t.Name), "TeamId", "Name");
            ViewBag.TournamentGroupId = new SelectList(CombosHelper
                .GetGroupsMatch(date.TournamentId).OrderBy(g => g.Name), "TournamentGroupId", "Name");
            var view = new MatchView { DateId = date.DateId, };
            return View(view);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMatch(MatchView view)
        {
            try
            {
                if (ModelState.IsValid && view.LocalId != view.VisitorId)
                {
                    var estado = string.Format("No iniciado");  // Todo: Verificar que status tenga valores
                    var status = db.Status.Where(s => s.Name == estado).FirstOrDefault();
                    Match match = ToMatch(view);
                    match.StatusId = status.StatusId;
                    db.Matches.Add(match);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.CreateOk();
                    return RedirectToAction("DetailsDate", new { id = match.DateId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());
            }

            var date = await db.Dates.FindAsync(view.DateId);

            ViewBag.StadiumId = new SelectList(db.Stadia, 
                "StadiumId", "Name", view.StadiumId);
            ViewBag.LocalId = new SelectList(CombosHelper.GetTeamsMatch(view.TournamentGroupId), 
                "TeamId", "Name", view.LocalId);
            ViewBag.VisitorId = new SelectList(CombosHelper.GetTeamsMatch(view.TournamentGroupId), 
                "TeamId", "Name", view.VisitorId);
            ViewBag.TournamentGroupId = new SelectList(CombosHelper
                .GetGroupsMatch(date.TournamentId), "TournamentGroupId", "Name",view.TournamentGroupId);
            
            return View(view);
        }

        

        public async Task<ActionResult> DetailsDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }

            return View(date);
        }


        public async Task<ActionResult> DeleteTeam(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }

            TournamentTeam tournamentTeam = await db.TournamentTeams.FindAsync(id);

            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }
            return View(tournamentTeam);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var tournamentTeam = await db.TournamentTeams.FindAsync(id);
            try
            {
                db.TournamentTeams.Remove(tournamentTeam);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("DeleteTeam", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("DetailsGroup", new { id = tournamentTeam.TournamentGroupId});
        }

        public async Task<ActionResult> EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentTeam tournamentTeam = await db.TournamentTeams.FindAsync(id);
            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }

            var view = ToTournamentTeamView(tournamentTeam);

            ViewBag.LeagueId = new SelectList(CombosHelper.GetLeagues(), "LeagueId", "Name", tournamentTeam.Team.LeagueId);
            ViewBag.TeamId = new SelectList(CombosHelper.GetTeams(tournamentTeam.Team.LeagueId), "TeamId", "Name",tournamentTeam.Team.TeamId);
            
            return View(view);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TournamentTeamView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tournamentTeam = ToTournamentTeam(view);
                    db.Entry(tournamentTeam).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.EditOk();
                    return RedirectToAction("DetailsGroup", new { id = tournamentTeam.TournamentGroupId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());

            }
            

            ViewBag.LeagueId = new SelectList(CombosHelper.GetLeagues(), "LeagueId", "Name",view.LeagueId);
            ViewBag.TeamId = new SelectList(CombosHelper.GetTeams(view.LeagueId), "TeamId", "Name",view.TeamId);

            return View(view);
        }

        

        public async Task<ActionResult> CreateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentGroup = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.LeagueId = new SelectList(CombosHelper.GetLeagues(), "LeagueId", "Name");
            ViewBag.TeamId = new SelectList(CombosHelper.GetTeams(0), "TeamId", "Name");
            
            var view = new TournamentTeamView { TournamentGroupId = tournamentGroup.TournamentGroupId,};
            return View(view);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TournamentTeamView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tournamentTeam = ToTournamentTeam(view);
                    db.TournamentTeams.Add(tournamentTeam);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.CreateOk();
                    return RedirectToAction("DetailsGroup", new { id = tournamentTeam.TournamentGroupId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());
            }

            ViewBag.LeagueId = new SelectList(CombosHelper.GetLeagues(), "LeagueId", "Name", view.LeagueId);
            ViewBag.TeamId = new SelectList(CombosHelper.GetTeams(0), "TeamId", "Name", view.TeamId);
            
            return View(view);
        }

        public async Task<ActionResult> DetailsGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentGroups = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroups == null)
            {
                return HttpNotFound();
            }

            return View(tournamentGroups);
        }

        public async Task<ActionResult> DeleteDate(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }

            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }

            return View(date);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDate(int id)
        {
            Date date = await db.Dates.FindAsync(id);
            try
            {
                
                db.Dates.Remove(date);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (Exception)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("DeleteDate", new { id = id, saveChangesError = true });

            }
            return RedirectToAction("Details",new { id = date.TournamentId  });
        }

        public async Task<ActionResult> EditDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }
            
            return View(date);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDate(Date date)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(date).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.EditOk();
                    return RedirectToAction("Details", new { id = date.TournamentId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());

            }
                        
            return View(date);
        }

        public async Task<ActionResult> CreateDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var Date = new Date { TournamentId = tournament.TournamentId };

            return View(Date);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDate(Date date)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Dates.Add(date);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.CreateOk();
                    return RedirectToAction("Details", new { id = date.TournamentId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());
            }

            return View(date);
        }

        public async Task<ActionResult> DeleteGroup(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }
            var tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }
            return View(tournamentGroup);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            var tournamentGroup = await db.TournamentGroups.FindAsync(id);
            try
            {
                db.TournamentGroups.Remove(tournamentGroup);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("DeleteGroup", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Details", new { id = tournamentGroup.TournamentId});
        }

        public async Task<ActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentGroup = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }
            
            return View(tournamentGroup);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditGroup(TournamentGroup tournamentGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tournamentGroup).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.EditOk();
                    return RedirectToAction("Details", new { id = tournamentGroup.TournamentId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());

            }
                        
            return View(tournamentGroup);
        }

        public async Task<ActionResult> CreateGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var view = new TournamentGroup { TournamentId = tournament.TournamentId };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroup(TournamentGroup tournamentGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.TournamentGroups.Add(tournamentGroup);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.CreateOk();
                    return RedirectToAction("Details", new { id = tournamentGroup.TournamentId });
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                ModelState.AddModelError("", MessageHelper.ExceptionData());
            }
            
            ViewBag.TournamentId = new SelectList(db.Tournaments, "TournamentId", "Name", tournamentGroup.TournamentId);
            return View(tournamentGroup);
        }

        public async Task<ActionResult> Index()
        {
            return View(await db.Tournaments.ToListAsync());

        }

        // GET: Tournaments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TournamentView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Logos";

                    if (view.LogoFile != null)
                    {
                        pic = FileHelpers.UploadPhoto(view.LogoFile, folder, widthPhotoTournament, heigthPhotoTournament);
                        pic = string.Format("{0}/{1}", folder, pic);
                    }

                    var tournament = ToTournament(view);
                    tournament.Logo = pic;

                    db.Tournaments.Add(tournament);
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.CreateOk();
                    return RedirectToAction("Index");
                    
                }
                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());

            }

            

            
            return View(view);
        }

       

        

        // GET: Tournaments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var view = ToView(tournament);

            return View(view);
        }

        

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TournamentView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pic = view.Logo;
                    var folder = "~/Content/Logos";

                    if (view.LogoFile != null)
                    {
                        pic = FileHelpers.UploadPhoto(view.LogoFile, folder, widthPhotoTournament, heigthPhotoTournament);
                        pic = string.Format("{0}/{1}", folder, pic);
                    }

                    var tournament = ToTournament(view);
                    tournament.Logo = pic;

                    db.Entry(tournament).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.EditOk();
                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMessage = MessageHelper.ModelNotValid();
            }
            catch (DataException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", MessageHelper.ExceptionData());

            }

            

            return View(view);
        }

        // GET: Tournaments/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Tournament tournament = await db.Tournaments.FindAsync(id);
            try
            {
                db.Tournaments.Remove(tournament);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        

        [ValidateInput(false)]
        public ActionResult TournamentGridViewPartial()
        {
            var model = db.Tournaments.ToList();
            var viewModel = ToTournamentViewModel(model);
            return PartialView("_TournamentGridViewPartial", viewModel.ToList());
        }



        #endregion


        #region Methods

        private int GetStatus(int localGoals, int visitorGoals)
        {
            if (localGoals > visitorGoals)
            {
                return 1; // Local win
            }

            if (visitorGoals > localGoals)
            {
                return 2; // Visitor win
            }

            return 3; // Draw
        }


        private List<TournamentViewModel> ToTournamentViewModel(List<Tournament> model)
        {
            var view = new List<TournamentViewModel>();

            foreach (var item in model)
            {
                view.Add(new TournamentViewModel
                {
                    DateCount = item.Dates.Count,
                    GroupCount = item.TournamentGroups.Count,
                    IsActive = item.IsActive,
                    Name = item.Name,
                    Order = item.Order,
                    Logo = item.Logo,
                    TournamentId = item.TournamentId,
                    Dates = item.Dates,
                    TournamentGroups = item.TournamentGroups,
                });
            }

            return view;
        }

        private TournamentView ToView(Tournament tournament)
        {
            return new TournamentView
            {
                IsActive = tournament.IsActive,
                Logo = tournament.Logo,
                Name = tournament.Name,
                Order = tournament.Order,
                TournamentGroups = tournament.TournamentGroups,
                TournamentId = tournament.TournamentId,
            };
        }

        private Tournament ToTournament(TournamentView view)
        {
            return new Tournament
            {
                IsActive = view.IsActive,
                Logo = view.Logo,
                Name = view.Name,
                Order = view.Order,
                TournamentGroups = view.TournamentGroups,
                TournamentId = view.TournamentId,
            };
        }


        private Match ToMatch(MatchView view)
        {
            return new Match
            {
                Date = view.Date,
                DateTime = Convert.ToDateTime(string.Format("{0} {1}", view.DateString, view.TimeString)),
                DateId = view.DateId,
                Local = view.Local,
                LocalGoals = view.LocalGoals,
                LocalId = view.LocalId,
                MatchId = view.MatchId,
                Status = view.Status,
                StatusId = view.StatusId,
                TournamentGroup = view.TournamentGroup,
                TournamentGroupId = view.TournamentGroupId,
                Visitor = view.Visitor,
                VisitorGoals = view.VisitorGoals,
                VisitorId = view.VisitorId,
                StadiumId = view.StadiumId,
                Stadium = view.Stadium,
            };
        }


        private TournamentTeam ToTournamentTeam(TournamentTeamView view)
        {
            return new TournamentTeam
            {
                TeamId = view.TeamId,
                Team = view.Team,
                TournamentTeamId = view.TournamentTeamId,
                AgainstGoals = view.AgainstGoals,
                FavorGoals = view.FavorGoals,
                MatchesLost = view.MatchesLost,
                MatchesPlayed = view.MatchesPlayed,
                MatchesTied = view.MatchesTied,
                MatchesWon = view.MatchesWon,
                Points = view.Points,
                Position = view.Position,
                TournamentGroup = view.TournamentGroup,
                TournamentGroupId = view.TournamentGroupId,
            };

        }

        private TournamentTeamView ToTournamentTeamView(TournamentTeam tournamentTeam)
        {
            return new TournamentTeamView
            {
                TeamId = tournamentTeam.TeamId,
                Team = tournamentTeam.Team,
                LeagueId = tournamentTeam.Team.LeagueId,
                AgainstGoals = tournamentTeam.AgainstGoals,
                FavorGoals = tournamentTeam.FavorGoals,
                MatchesLost = tournamentTeam.MatchesLost,
                MatchesPlayed = tournamentTeam.MatchesPlayed,
                MatchesTied = tournamentTeam.MatchesTied,
                MatchesWon = tournamentTeam.MatchesWon,
                Points = tournamentTeam.Points,
                Position = tournamentTeam.Position,
                TournamentGroup = tournamentTeam.TournamentGroup,
                TournamentGroupId = tournamentTeam.TournamentGroupId,
                TournamentTeamId = tournamentTeam.TournamentTeamId,

            };
        }

        private MatchView ToMatchView(Match match)
        {
            return new MatchView
            {
                Stadium = match.Stadium,
                Date = match.Date,
                DateId = match.DateId,
                DateString = match.DateTime.ToString("yyyy-MM-dd"),
                DateTime = match.DateTime,
                Local = match.Local,
                LocalGoals = match.LocalGoals,
                LocalId = match.LocalId,
                MatchId = match.MatchId,
                StadiumId = match.StadiumId,
                Status = match.Status,
                StatusId = match.StatusId,
                TimeString = match.DateTime.ToString("H:mm"),
                TournamentGroup = match.TournamentGroup,
                TournamentGroupId = match.TournamentGroupId,
                Visitor = match.Visitor,
                VisitorGoals = match.VisitorGoals,
                VisitorId = match.VisitorId,

            };
        }

        #endregion


    }


}
