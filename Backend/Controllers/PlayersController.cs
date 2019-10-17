using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Backend.Models;
using Domain;
using Backend.Helpers;

namespace Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlayersController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        private int widthPhotoPlayer = 240;

        private int heigthPhotoPlayer = 360;

        // GET: Players
        public async Task<ActionResult> Index()
        {
            return View(await db.Players.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = await db.Players.FindAsync(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PlayerView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Photos";
                var anonim = "foto-anonimo.jpg";

                if (view.PhotoFile != null)
                {
                    pic = FileHelpers.UploadPhoto(view.PhotoFile, folder, widthPhotoPlayer, heigthPhotoPlayer);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                else
                {
                    pic = string.Format("{0}/{1}", folder, anonim);
                }

                var player = ToPlayer(view);
                player.Photo = pic;

                db.Players.Add(player);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private Player ToPlayer(PlayerView view)
        {
            return new Player
            {
                Birthdate = view.Birthdate,
                //Birthdate = Convert.ToDateTime(string.Format("{0}", view.DateString)),
                Email = view.Email,
                FirtsName = view.FirtsName,
                IdentificationCard = view.IdentificationCard,
                LastName = view.LastName,
                Photo = view.Photo,
                PlayerId = view.PlayerId,
            };
        }

        // GET: Players/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = await db.Players.FindAsync(id);

            if (player == null)
            {
                return HttpNotFound();
            }

            var view = ToPlayerView(player);

            return View(view);
        }

        private PlayerView ToPlayerView(Player player)
        {
            return new PlayerView
            {
                Birthdate = player.Birthdate,
                DateString = player.Birthdate.ToString("yyyy-MM-dd"),
                Email = player.Email,
                FirtsName = player.FirtsName,
                IdentificationCard = player.IdentificationCard,
                LastName = player.LastName,
                Photo = player.Photo,
                PlayerId = player.PlayerId,
            };
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PlayerView view)
        {
            if (ModelState.IsValid)
            {

                var pic = view.Photo;
                var folder = "~/Content/Photos";
                

                if (view.PhotoFile != null)
                {
                    pic = FileHelpers.UploadPhoto(view.PhotoFile, folder, widthPhotoPlayer, heigthPhotoPlayer);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var player = ToPlayer(view);
                player.Photo = pic;

                db.Entry(player).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        // GET: Players/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = await db.Players.FindAsync(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Player player = await db.Players.FindAsync(id);
            db.Players.Remove(player);
            await db.SaveChangesAsync();
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
    }
}