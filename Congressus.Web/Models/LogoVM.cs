using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congressus.Web.Models
{
    public class LogoVM
    {
        public int Id { get; set; }
        public HttpPostedFileBase Logo { get; set; }
    }
}