//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;
//using Congressus.Web.Models;
//using Congressus.Web.Models.Entities;

//namespace Congressus.Web.Context
//{
//    public class ModelDbContext : DbContext
//    {
//        public DbSet<Paper> Papers { get; set; }
//        public DbSet<Revision> Revisiones { get; set; }
//        public DbSet<Autor> Autores { get; set; }
//        public DbSet<MiembroComite> Miembros { get; set; }
//        public DbSet<ApplicationRole> IdentityRoles { get; set; }
//        public DbSet<Evento> Eventos { get; set; }

//        public ModelDbContext()
//            : base("DefaultConnection")
//        {

//        }

//        public static ModelDbContext Create()
//        {
//            return new ModelDbContext();
//        }
//    }
//}