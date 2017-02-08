namespace Congressus.Web.Migrations
{
    using Models.Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<Context.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context.ApplicationDbContext context)
        {
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
                    AreaCientifica = "Informatica",
                    Path = "",
                    Descripcion = "un paper"
                },
                new Paper()
                {
                    Nombre = "Defusion del CIAA",
                    Fecha = DateTime.Now,
                    AreaCientifica = "Informatica",
                    Path = "",
                    Descripcion = "un paper"
                },
                new Paper()
                {
                    Nombre = "Arduino: Ventajas y desventajas",
                    Fecha = DateTime.Now,
                    AreaCientifica = "Informatica",
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
