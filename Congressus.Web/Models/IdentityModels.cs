using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Congressus.Web.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Congressus.Web.Models
{
    // Puede agregar datos del perfil del usuario agregando más propiedades a la clase ApplicationUser. Para más información, visite http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {

        }
        public ApplicationRole(string roleName)
            : base(roleName)
        {

        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Paper> Papers { get; set; }
        public DbSet<Revision> Revisiones { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<MiembroComite> Miembros { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<ApplicationRole> IdentityRoles { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
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

        public System.Data.Entity.DbSet<Congressus.Web.Models.Entities.Evaluacion> Evaluacions { get; set; }
    }
}