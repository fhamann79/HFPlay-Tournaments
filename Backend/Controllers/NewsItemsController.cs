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
using Microsoft.AspNet.Identity;

namespace Backend.Controllers
{
    [Authorize]
    public class NewsItemsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        private int widthPhotoNews = 600;

        private int heigthPhotoNews = 450;


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var newsItem = db.NewsItems.Where(ni => ni.NewsItemId == id).FirstOrDefault();
                newsItem.IsApproved = true;
                newsItem.ModificationDate = DateTime.UtcNow;
                db.Entry(newsItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = MessageHelper.ApproveOk();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> NotApprove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var newsItem = db.NewsItems.Where(ni => ni.NewsItemId == id).FirstOrDefault();
                newsItem.IsApproved = false;
                newsItem.ModificationDate = DateTime.UtcNow;
                db.Entry(newsItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = MessageHelper.NotApproveOk();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var newsItems = db.NewsItems.Include(n => n.User);
            return View(await newsItems.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: NewsItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = await db.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            return View(newsItem);
        }

        // GET: NewsItems/Create
        public ActionResult Create()
        {
            var newsItem = new NewsItemView
            {
                UserId = 0,
                PublicationDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow,
            };
            return View(newsItem);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewsItemView view)
        {
            if (view.PictureFile == null)
            {
                ModelState.AddModelError(String.Empty, MessageHelper.RequiredImage());
                return  View(view);
            }
            if (ModelState.IsValid)
                {

                    try
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/NewsItems";
                        var anonim = "foto-anonimo.jpg";

                        if (view.PictureFile != null)
                        {
                            pic = FileHelpers.UploadPhoto(view.PictureFile, folder, widthPhotoNews, heigthPhotoNews);
                            pic = string.Format("{0}/{1}", folder, pic);
                        }
                        else
                        {
                            pic = string.Format("{0}/{1}", folder, anonim);
                        }
                        var userASPId = User.Identity.GetUserId();
                        var user = db.Users.Where(u => u.UserASPId == userASPId).FirstOrDefault();
                        var newsItem = ToNewsItem(view);
                        newsItem.UserId = user.UserId;
                        newsItem.Picture = pic;
                        newsItem.PublicationDate = DateTime.UtcNow;
                        newsItem.ModificationDate = DateTime.UtcNow;
                        newsItem.IsApproved = false;
                        if (!string.IsNullOrEmpty(newsItem.FacebookVideo))
                        {
                            newsItem.FacebookVideo = "https://www.facebook.com/plugins/video.php?href=" + newsItem.FacebookVideo + "%2F&show_text=0&width=560"; 
                        }
                        db.NewsItems.Add(newsItem);
                        await db.SaveChangesAsync();
                        TempData["SuccessMessage"] = MessageHelper.NewsCreateOk();
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {

                        if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("datetime2"))
                        {
                            ModelState.AddModelError(String.Empty, MessageHelper.ExceptionDateTimeOutOfRange());
                        }
                        else if (ex.Message.Contains("DateTime "))
                        {
                            ModelState.AddModelError(String.Empty, MessageHelper.ExceptionDateNotValid());
                        }
                        else if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                        {
                            ModelState.AddModelError(String.Empty, MessageHelper.ExceptionReference());
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, ex.Message);
                        }
                    }
                } 
            

            
            return View(view);
        }

        private NewsItem ToNewsItem(NewsItemView view)
        {
            return new NewsItem
            {
                Picture = view.Picture,
                Content = view.Content,
                IsApproved = view.IsApproved,
                ModificationDate = view.ModificationDate,
                NewsItemId = view.NewsItemId,
                PublicationDate = view.PublicationDate,
                Title = view.Title,
                User = view.User,
                UserId = view.UserId,
                FacebookVideo = view.FacebookVideo,
                
            };
        }

        [Authorize(Roles = "Admin")]
        // GET: NewsItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = await db.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", newsItem.UserId);
            return View(newsItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "NewsItemId,Title,Content,PublicationDate,ModificationDate,Picture,IsApproved,UserId")] NewsItem newsItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", newsItem.UserId);
            return View(newsItem);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = await db.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            return View(newsItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NewsItem newsItem = await db.NewsItems.FindAsync(id);
            db.NewsItems.Remove(newsItem);
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
