using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Congressus.Web.Models;
using Congressus.Web.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Congressus.Web.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private static ApplicationDbContext _instance;
        public DbSet<FormularioBeca> FormulariosBecas { get; set; }
        public DbSet<AreaCientifica> AreasCientificas { get; set; }
        public DbSet<Charla> Charlas { get; set; }
        public DbSet<Paper> Papers { get; set; }
        public DbSet<Revision> Revisiones { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Asistente> Asistentes { get; set; }
        public DbSet<MiembroComite> Miembros { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<ApplicationRole> IdentityRoles { get; set; }
        public DbSet<Evaluacion> Evaluacions { get; set; }
        public DbSet<InscripcionEvento> InscripcionesEvento { get; set; }
        public DbSet<InscripcionCharla> InscripcionesCharla { get; set; }
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false){}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
            //if (_instance == null) _instance = new ApplicationDbContext();
            //return _instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Evento>()
                        .HasMany<MiembroComite>(e => e.Comite)
                        .WithMany(m => m.Eventos)
                        .Map(em =>
                        {
                            em.MapLeftKey("EventoId");
                            em.MapRightKey("MiembroId");
                            em.ToTable("MiembroComiteEventos");
                        });
            base.OnModelCreating(modelBuilder);

        }

        

    }
}