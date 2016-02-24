using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Congressus.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
namespace Congressus.Web.Controllers
{
    [Authorize(Roles="admin")]
    public class UsersController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; set; }
        public RoleManager<ApplicationRole> RoleManager { get; set; }

        public UsersController()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            UserManager = new ApplicationUserManager(store);

            var roleStore = new RoleStore<ApplicationRole>(new ApplicationDbContext());
            RoleManager = new RoleManager<ApplicationRole>(roleStore);  
        }
        

        // GET: Users
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext()) { 
                var users = UserManager.Users.ToList();
                List<UserViewModel> usersVm = new List<UserViewModel>();
                UserViewModel uvm;

                foreach (var user in users)
                {
                    uvm = new UserViewModel()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                    };

                    if (UserManager.IsInRole(user.Id, "presidente"))
                    {
                        uvm.ProfileType = "Presidente";
                    }else if (UserManager.IsInRole(user.Id, "autor"))
                    {
                        uvm.ProfileType = "Autor";
                    }else if (UserManager.IsInRole(user.Id, "MiembroComite"))
                    {
                        uvm.ProfileType = "Miembro Comite";
                    }else if (UserManager.IsInRole(user.Id, "admin"))
                    {
                        uvm.ProfileType = "admin";
                    }
                    usersVm.Add(uvm);
                }
                return View(usersVm);
            }


        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                //db.ApplicationUsers.Add(applicationUser);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                UserManager.Update(applicationUser);
                //db.Entry(applicationUser).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = UserManager.FindById(id);
            UserManager.Delete(applicationUser);
            //db.ApplicationUsers.Remove(applicationUser);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AuthorizeUser()
        {
            var data = new Dictionary<string, object> 
            {
                {"usuarios", UserManager.Users.ToList()},
                {"roles", RoleManager.Roles.ToList()}
            };
            return View(data);
        }

        [HttpPost]
        public ActionResult AuthorizeUser(AuthorizeUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = UserManager.FindById(model.UserId);
            var role = RoleManager.FindById(model.RoleId);

            if (!usuario.Roles.Any(x => x.RoleId == model.RoleId))
            {
                UserManager.AddToRole(model.UserId, role.Name);
            }
            
            
            return RedirectToAction("Index", "Users");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
