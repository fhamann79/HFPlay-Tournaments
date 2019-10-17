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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using PagedList;

namespace Backend.Controllers
{

    public class UsersController : Controller
    {
        #region Properties
        private DataContextLocal db = new DataContextLocal();

        private static ApplicationDbContext userContext = new ApplicationDbContext();

        private int widthPhotoUser = 240;

        private int heigthPhotoUser = 360;

        #endregion

      


        public ActionResult UpdateId()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            try
            {
                var users = db.Users.Where(u => u.UserASPId == null).ToList();

                foreach (var user in users)
                {
                    var userASPId = userManager.FindByEmail(user.Email);

                    if (userASPId != null)
                    {
                        user.UserASPId = userASPId.Id;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges(); 
                    }

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);

            }
            return RedirectToAction("Index");
        }

        [Authorize]

        [Authorize(Roles = "Admin")]

        public ActionResult AddRole()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(AddRoleViewModel view)
        {
            UsersHelper.AddRoleASP(view.Email, view.Role);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> EditManageUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userASPId = User.Identity.GetUserId();

            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.UserASPId != userASPId)
            {
                return HttpNotFound();
            }

            var view = ToUserView(user);

            ViewBag.FavoriteLeagueId = new SelectList(CombosHelper.GetLeagues().OrderBy(l => l.Name), "LeagueId", "Name", user.FavoriteTeam.LeagueId);
            ViewBag.FavoriteTeamId = new SelectList(CombosHelper.GetTeams(user.FavoriteTeam.LeagueId), "TeamId", "Name", user.FavoriteTeam.TeamId);

            return View(view);
        }

        private UserView ToUserView(User user)
        {
            return new UserView
            {
                UserTypeId = user.UserTypeId,
                FavoriteTeam = user.FavoriteTeam,
                Address = user.Address,
                Birthdate = user.Birthdate,
                BirthDateString = user.Birthdate.ToString("yyyy-MM-dd"),
                Email = user.Email,
                FavoriteLeagueId = user.FavoriteTeam.LeagueId,
                FavoriteTeamId = user.FavoriteTeamId,
                FirstName = user.FirstName,
                GroupUsers = user.GroupUsers,
                IdentificationCard = user.IdentificationCard,
                IsVerified = user.IsVerified,
                LastName = user.LastName,
                NickName = user.NickName,
                Phone = user.Phone,
                Picture = user.Picture,
                Points = user.Points,
                Predictions = user.Predictions,
                TeamPlayers = user.TeamPlayers,
                UserASPId = user.UserASPId,
                UserGroups = user.UserGroups,
                UserId = user.UserId,
                UserType = user.UserType,
                Password = "isNull",
                PasswordConfirm = "isNull",

            };
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditManageUser(UserView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pic = view.Picture;
                    var folder = "~/Content/Photos";
                    var anonim = "foto-anonimo.jpg";

                    if (view.PhotoFile != null)
                    {
                        pic = FileHelpers.UploadPhoto(view.PhotoFile, folder, widthPhotoUser, heigthPhotoUser);
                        pic = string.Format("{0}/{1}", folder, pic);
                    }


                    if (string.IsNullOrEmpty(pic))
                    {
                        pic = string.Format("{0}/{1}", folder, anonim);
                    }

                    var user = ToUser(view);
                    user.Picture = pic;         //Set value of pic
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.UserEditOk();
                    return RedirectToAction("Manage", "Users", new { });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                ex.InnerException.InnerException != null &&
                ex.InnerException.InnerException.Message.Contains("User_IdentificationCard_Index"))
                {
                    ModelState.AddModelError(String.Empty, MessageHelper.ExceptionIdentificationCardIndex());
                }
                else if (ex.InnerException != null &&
                ex.InnerException.InnerException != null &&
                ex.InnerException.InnerException.Message.Contains("User_NickName_Index"))
                {
                    ModelState.AddModelError(String.Empty, MessageHelper.ExceptionNickNameIndex());
                }
                else if (ex.InnerException != null &&
                ex.InnerException.InnerException != null &&
                ex.InnerException.InnerException.Message.Contains("User_Email_Index"))
                {
                    ModelState.AddModelError(String.Empty, MessageHelper.ExceptionEmailIndex());
                }
                else if (ex.InnerException != null &&
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

            ViewBag.FavoriteLeagueId = new SelectList(CombosHelper.GetLeagues().OrderBy(l => l.Name), "LeagueId", "Name", view.FavoriteLeagueId);
            ViewBag.FavoriteTeamId = new SelectList(CombosHelper.GetTeams(view.FavoriteLeagueId), "TeamId", "Name", view.FavoriteTeamId);

            return View(view);
        }

        public async Task<ActionResult> Manage()
        {
            var userASPId = User.Identity.GetUserId();
            var users = db.Users.Where(u => u.UserASPId == userASPId).Include(u => u.FavoriteTeam).Include(u => u.UserType);
            return View(await users.ToListAsync());
        }

        public ActionResult Register()
        {
            ViewBag.FavoriteLeagueId = new SelectList(CombosHelper.GetLeagues().OrderBy(l => l.Name), "LeagueId", "Name");
            ViewBag.FavoriteTeamId = new SelectList(CombosHelper.GetTeams(0), "TeamId", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserView view)
        {
            if (ModelState.IsValid)
            {
                var userASP = new ApplicationUser { Email = view.Email, UserName = view.Email, };

                var userContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var userFind = userManager.FindByEmail(view.Email);


                if (userFind != null)
                {
                    ViewBag.ErrorMessage = MessageHelper.UserExits();
                    ViewBag.FavoriteLeagueId = new SelectList(CombosHelper.GetLeagues().OrderBy(l => l.Name), "LeagueId", "Name", view.FavoriteLeagueId);
                    ViewBag.FavoriteTeamId = new SelectList(CombosHelper.GetTeams(view.FavoriteLeagueId), "TeamId", "Name", view.FavoriteTeamId);
                    return View(view);
                }

                using (db)
                {
                    using (var transaction = userContext.Database.BeginTransaction())
                    {
                        try
                        {

                            var pic = string.Empty;
                            var folder = "~/Content/Photos";
                            //var anonim = "foto-anonimo.jpg";

                            if (view.PhotoFile != null)
                            {
                                pic = FileHelpers.UploadPhoto(view.PhotoFile, folder, widthPhotoUser, heigthPhotoUser);
                                pic = string.Format("{0}/{1}", folder, pic);
                            }
                            else
                            {
                                ViewBag.ErrorMessage = MessageHelper.RequiredImage();
                                ViewBag.FavoriteLeagueId = new SelectList(CombosHelper.GetLeagues().OrderBy(l => l.Name), "LeagueId", "Name", view.FavoriteLeagueId);
                                ViewBag.FavoriteTeamId = new SelectList(CombosHelper.GetTeams(view.FavoriteLeagueId), "TeamId", "Name", view.FavoriteTeamId);
                                return View(view);
                                
                            }

                            var user = ToUser(view);
                            user.Picture = pic;
                            user.Points = 0;
                            user.IsVerified = false;
                            var userTypeId = db.UserTypes.Where(ut => ut.Name == "Local").FirstOrDefault();
                            if (userTypeId != null)
                            {
                                user.UserTypeId = userTypeId.UserTypeId;
                            }
                            else
                            {
                                user.UserTypeId = 1;
                            }

                            var result = await userManager.CreateAsync(userASP, view.Password);

                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(userASP.Id, "User");
                                user.UserASPId = userASP.Id;
                                db.Users.Add(user);
                                await db.SaveChangesAsync();
                                transaction.Commit();

                                string code = await userManager.GenerateEmailConfirmationTokenAsync(userASP.Id);

                                var callbackUrl = Url.Action("ConfirmEmail",
                                            "Account",
                                            new { userId = userASP.Id, code = code },
                                            protocol: Request.Url.Scheme);

                                await userManager.SendEmailAsync(userASP.Id,
                                            "Confirmar cuenta HFPlay",
                                            "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");

                                TempData["SuccessMessage"] = MessageHelper.UserCreateOk();
                                
                                return RedirectToAction("Login", "Account", new { }); 
                            }

                            ViewBag.ErrorMessage = MessageHelper.UserCreateFail();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            if (ex.InnerException != null &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException.Message.Contains("User_IdentificationCard_Index"))
                            {
                                ModelState.AddModelError(String.Empty, MessageHelper.ExceptionIdentificationCardIndex());
                            }
                            else if (ex.InnerException != null &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException.Message.Contains("User_NickName_Index"))
                            {
                                ModelState.AddModelError(String.Empty, MessageHelper.ExceptionNickNameIndex());
                            }
                            else if (ex.InnerException != null &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException.Message.Contains("User_Email_Index"))
                            {
                                ModelState.AddModelError(String.Empty, MessageHelper.ExceptionEmailIndex());
                            }
                            else if (ex.InnerException != null &&
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
                }
            }
            ModelState.AddModelError(String.Empty, MessageHelper.ModelNotValid());
            ViewBag.FavoriteLeagueId = new SelectList(CombosHelper.GetLeagues().OrderBy(l => l.Name), "LeagueId", "Name", view.FavoriteLeagueId);
            ViewBag.FavoriteTeamId = new SelectList(CombosHelper.GetTeams(view.FavoriteLeagueId), "TeamId", "Name", view.FavoriteTeamId);

            return View(view);

        }

        


        [Authorize(Roles = "Admin")]
        // GET: Users
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.PlayerSortParm = String.IsNullOrEmpty(sortOrder) ? "player_desc" : "";
            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "firstName_desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "lastName_desc" : "LastName";
            ViewBag.IdentificationCardSortParm = sortOrder == "IdentificationCard" ? "identificationCard_desc" : "IdentificationCard";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            ViewBag.NickNameSortParm = sortOrder == "NickName" ? "nickName_desc" : "NickName";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var users = from u in db.Users
                           select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.FirstName.ToUpper().Contains(searchString.ToUpper()) 
                                    || u.LastName.ToUpper().Contains(searchString.ToUpper())
                                    || u.IdentificationCard.ToUpper().Contains(searchString.ToUpper())
                                    || u.Email.ToUpper().Contains(searchString.ToUpper())
                                    || u.NickName.ToUpper().Contains(searchString.ToUpper())
                                    || u.TeamPlayers.FirstOrDefault().Team.Name.ToUpper().Contains(searchString.ToUpper())
                                    || u.TeamPlayers.FirstOrDefault().Number.ToString().ToUpper().Contains(searchString.ToUpper())
                                    || u.TeamManagers.FirstOrDefault().Team.Name.ToUpper().Contains(searchString.ToUpper())
                                    );
            }

            switch (sortOrder)
            {
                case "FirstName":
                    users = users.OrderBy(u => u.FirstName);
                    break;
                case "firstName_desc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;
                case "LastName":
                    users = users.OrderBy(u => u.LastName);
                    break;
                case "lastName_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "IdentificationCard":
                    users = users.OrderBy(u => u.IdentificationCard);
                    break;
                case "identificationCard_desc":
                    users = users.OrderByDescending(u => u.IdentificationCard);
                    break;
                case "Email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "NickName":
                    users = users.OrderBy(u => u.NickName);
                    break;
                case "nickName_desc":
                    users = users.OrderByDescending(u => u.NickName);
                    break;
                case "player_desc":
                    users = users.OrderByDescending(u => u.TeamPlayers.FirstOrDefault().Team.Name).ThenByDescending(u => u.TeamPlayers.FirstOrDefault().Number);
                    break;
                default:
                    users = users.OrderBy(u => u.TeamPlayers.FirstOrDefault().Team.Name).ThenBy(u => u.TeamPlayers.FirstOrDefault().Number);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));

            
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.FavoriteLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name");
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.UserTypeId = new SelectList(db.UserTypes.OrderBy(ut => ut.Name), "UserTypeId", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Photos";
                    var anonim = "foto-anonimo.jpg";

                    if (view.PhotoFile != null)
                    {
                        pic = FileHelpers.UploadPhoto(view.PhotoFile, folder, widthPhotoUser, heigthPhotoUser);
                        pic = string.Format("{0}/{1}", folder, pic);
                    }
                    else
                    {
                        pic = string.Format("{0}/{1}", folder, anonim);
                    }

                    var user = ToUser(view);
                    user.Picture = pic;         //Set value of pic
                    user.Points = 0;
                    user.IsVerified = false;
                    var userTypeId = db.UserTypes.Where(ut => ut.Name == "Local").FirstOrDefault();
                    if (userTypeId != null)
                    {
                        user.UserTypeId = userTypeId.UserTypeId;
                    }
                    else
                    {
                        user.UserTypeId = 1;
                    }

                    db.Users.Add(user);
                    UsersHelper.CreateUserASP(view.Email, "User", view.Password);  //Create Asp User
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.

                ModelState.AddModelError("", MessageHelper.ExceptionData());
                ModelState.AddModelError("", dex.Message);
            }

            ViewBag.FavoriteLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name", view.FavoriteLeagueId);
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", view.FavoriteTeamId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", view.UserTypeId);
            return View(view);
        }



        private User ToUser(UserView view)
        {
            return new User
            {
                Address = view.Address,
                Email = view.Email,
                FavoriteTeam = view.FavoriteTeam,
                FavoriteTeamId = view.FavoriteTeamId,
                FirstName = view.FirstName,
                GroupUsers = view.GroupUsers,
                LastName = view.LastName,
                NickName = view.NickName,
                Phone = view.Phone,
                Picture = view.Picture,
                Points = view.Points,
                Predictions = view.Predictions,
                UserGroups = view.UserGroups,
                UserId = view.UserId,
                UserType = view.UserType,
                UserTypeId = view.UserTypeId,
                Birthdate = Convert.ToDateTime(string.Format("{0}", view.BirthDateString)),
                IdentificationCard = view.IdentificationCard,
                IsVerified = view.IsVerified,
                TeamPlayers = view.TeamPlayers,
                UserASPId = view.UserASPId,

            };
        }


        [Authorize(Roles = "Admin")]
        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var view = ToUserView(user);

            ViewBag.FavoriteLeagueId = new SelectList(CombosHelper.GetLeagues().OrderBy(l => l.Name), "LeagueId", "Name", user.FavoriteTeam.LeagueId);
            ViewBag.FavoriteTeamId = new SelectList(CombosHelper.GetTeams(user.FavoriteTeam.LeagueId), "TeamId", "Name", user.FavoriteTeam.TeamId);

            return View(view);
            
        }

        [Authorize(Roles = "Admin")]
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserView view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pic = view.Picture;
                    
                    var folder = "~/Content/Photos";
                    var anonim = "foto-anonimo.jpg";

                    if (view.PhotoFile != null)
                    {
                        pic = FileHelpers.UploadPhoto(view.PhotoFile, folder, widthPhotoUser, heigthPhotoUser);
                        pic = string.Format("{0}/{1}", folder, pic);
                    }


                    if (string.IsNullOrEmpty(pic))
                    {
                        pic = string.Format("{0}/{1}", folder, anonim);
                    }

                   

                    var user = ToUser(view);
                    user.Picture = pic;         //Set value of pic
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["SuccessMessage"] = MessageHelper.UserEditOk();
                    return RedirectToAction("Index", "Users", new { });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                ex.InnerException.InnerException != null &&
                ex.InnerException.InnerException.Message.Contains("User_IdentificationCard_Index"))
                {
                    ModelState.AddModelError(String.Empty, MessageHelper.ExceptionIdentificationCardIndex());
                }
                else if (ex.InnerException != null &&
                ex.InnerException.InnerException != null &&
                ex.InnerException.InnerException.Message.Contains("User_NickName_Index"))
                {
                    ModelState.AddModelError(String.Empty, MessageHelper.ExceptionNickNameIndex());
                }
                else if (ex.InnerException != null &&
                ex.InnerException.InnerException != null &&
                ex.InnerException.InnerException.Message.Contains("User_Email_Index"))
                {
                    ModelState.AddModelError(String.Empty, MessageHelper.ExceptionEmailIndex());
                }
                else if (ex.InnerException != null &&
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

            ViewBag.FavoriteLeagueId = new SelectList(CombosHelper.GetLeagues().OrderBy(l => l.Name), "LeagueId", "Name", view.FavoriteLeagueId);
            ViewBag.FavoriteTeamId = new SelectList(CombosHelper.GetTeams(view.FavoriteLeagueId), "TeamId", "Name", view.FavoriteTeamId);

            return View(view);
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUserAdmin(string message, int? id, bool? saveChangesError = false )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = message;
            }

            User user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);

            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUserAdmin(int id)
        {
            
            var userContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            using (db)
            {
                using (var transaction = userContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await db.Users.FindAsync(id);
                        db.Users.Remove(user);
                        await db.SaveChangesAsync();
                        var userASP = userManager.FindById(user.UserASPId);
                        var result = userManager.Delete(userASP);
                        transaction.Commit();
                        TempData["SuccessMessage"] = MessageHelper.DeleteOk();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        if (ex.InnerException != null &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException.Message.Contains("REFERENCE") &&
                            ex.InnerException.InnerException.Message.Contains("TeamManagers"))
                        {
                            return RedirectToAction("DeleteUserAdmin",
                            new { message = MessageHelper.UserTeamManagersReference(), id = id, saveChangesError = true });
                         }
                        else if (ex.InnerException != null &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException.Message.Contains("REFERENCE") &&
                            ex.InnerException.InnerException.Message.Contains("TeamPlayers"))
                        {
                            return RedirectToAction("DeleteUserAdmin",
                            new { message = MessageHelper.UserTeamPlayersReference(), id = id, saveChangesError = true });
                        }
                        else if (ex.InnerException != null &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException.Message.Contains("REFERENCE") &&
                            ex.InnerException.InnerException.Message.Contains("NewsItems"))
                        {
                            return RedirectToAction("DeleteUserAdmin",
                            new { message = MessageHelper.UserNewsItemsReference(), id = id, saveChangesError = true });
                        }
                        else if (ex.InnerException != null &&
                            ex.InnerException.InnerException != null &&
                            ex.InnerException.InnerException.Message.Contains("REFERENCE") )
                        {
                            return RedirectToAction("DeleteUserAdmin",
                            new { message = MessageHelper.UserReference(), id = id, saveChangesError = true });
                        }
                        else
                        {
                            return RedirectToAction("DeleteUserAdmin",
                            new { message = ex.Message, id = id, saveChangesError = true });
                        }

                        
                    }

                }
            }
                        
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [Authorize(Roles = "Admin")]
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
