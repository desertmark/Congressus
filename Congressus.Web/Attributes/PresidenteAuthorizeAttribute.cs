using Congressus.Web.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace Congressus.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PresidenteAuthorizeAttribute : AuthorizeAttribute
    {
        private int eventoId = 0;
        private IPrincipal User { get; set; }
        private ApplicationDbContext _db = new ApplicationDbContext();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            
            User = filterContext.HttpContext.User;
            if (Roles.Contains("presidente") && !User.IsInRole("admin"))
            {
                var userId = filterContext.HttpContext.User.Identity.GetUserId();
                if (filterContext.HttpContext.Request.HttpMethod == "GET")
                    //Buscar evento id en la ruta
                    eventoId = int.Parse(filterContext.HttpContext.Request.Params["EventoId"] ?? filterContext.RouteData.Values["Id"]?.ToString() ?? "0");
                else
                    //Buscar evento id en el body
                    eventoId = int.Parse(filterContext.HttpContext.Request.Form["EventoId"] ?? filterContext.HttpContext.Request.Form["Id"] ?? "0");
            }
            base.OnAuthorization(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            base.AuthorizeCore(httpContext);
            if (!User.IsInRole("admin")) { 
                if (eventoId == 0)
                    return true;
                var evento = _db.Eventos.FirstOrDefault(e => e.Id == eventoId);
                if (evento.Presidente.UsuarioId != User.Identity.GetUserId())
                    return false;
            }
            return true;
        }


    }
}