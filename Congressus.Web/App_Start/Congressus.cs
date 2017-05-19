using Congressus.Web.Context;
using Congressus.Web.Models;
using Congressus.Web.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Congressus.Web.App_Start
{
    public class Congressus
    {
        public static void start(ApplicationDbContext context) {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-AR");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
            //Creacion de los Roles
            var roleStore = new RoleStore<ApplicationRole>(context);
            var RoleManager = new RoleManager<ApplicationRole>(roleStore);

            if (!context.Roles.Any(r => r.Name == "admin"))
            {
                var AdminRole = new ApplicationRole("admin");
                RoleManager.Create(AdminRole);
            }
            if (!context.Roles.Any(r => r.Name == "autor"))
            {
                var AutorRole = new ApplicationRole("autor");
                RoleManager.Create(AutorRole);
            }
            if (!context.Roles.Any(r => r.Name == "asistente"))
            {
                var asistenteRole = new ApplicationRole("asistente");
                RoleManager.Create(asistenteRole);
            }
            if (!context.Roles.Any(r => r.Name == "MiembroComite"))
            {
                var MiembroRole = new ApplicationRole("MiembroComite");
                RoleManager.Create(MiembroRole);
            }
            if (!context.Roles.Any(r => r.Name == "presidente"))
            {
                var presidente = new ApplicationRole("presidente");
                RoleManager.Create(presidente);
            }

            //Creacion de los usuarios
            var store = new UserStore<ApplicationUser>(context);
            var UserManager = new ApplicationUserManager(store);

            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var AdminUser = new ApplicationUser { UserName = "admin@admin.com" };
                AdminUser.Email = AdminUser.UserName;
                UserManager.Create(AdminUser, "Password@123");
                UserManager.AddToRole(AdminUser.Id, "admin");
            }

            if (!context.Users.Any(u => u.UserName == "fer@fer.com"))
            {
                var AutorUser = new ApplicationUser { UserName = "fer@fer.com" };
                AutorUser.Email = AutorUser.UserName;
                UserManager.Create(AutorUser, "Password@123");
                UserManager.AddToRole(AutorUser.Id, "autor");
            }


            //Creacion de papers
            context.Papers.AddOrUpdate(p => p.Nombre,
                new Paper()
                {
                    Nombre = "Microsoft Student Partner",
                    Fecha = DateTime.Now,
                    AreaCientifica = new AreaCientifica() { Descripcion = "Informatica" },
                    Path = "",
                    Descripcion = "un paper"
                },
                new Paper()
                {
                    Nombre = "Defusion del CIAA",
                    Fecha = DateTime.Now,
                    AreaCientifica = new AreaCientifica() { Descripcion = "Informatica" },
                    Path = "",
                    Descripcion = "un paper"
                },
                new Paper()
                {
                    Nombre = "Arduino: Ventajas y desventajas",
                    Fecha = DateTime.Now,
                    AreaCientifica = new AreaCientifica() { Descripcion = "Informatica" },
                    Path = "",
                    Descripcion = "un paper"
                }
            );
            context.SaveChanges();
            //Creacion de Autores
            context.Autores.AddOrUpdate(p => p.Nombre,
                new Autor()
                {
                    Nombre = "Fernando Asulay",
                    Papers = new List<Paper>() { context.Papers.First() },
                    Usuario = UserManager.FindByName("fer@fer.com")
                }
             );
        }
    }
}