using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Congressus.Web.Attributes
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        string[] Extensions { get; set; }
        public FileExtensionAttribute(string[] extensions)
        {
            Extensions = extensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Debe seleccionar una imagen.");
            }
            var imagenes = (List<HttpPostedFileBase>)value;
            if (imagenes.First() == null)
            {
                return new ValidationResult("Debe seleccionar al menos una imagen.");
            }
            foreach (var imagen in imagenes)
            {
                if(!Extensions.Any(x => imagen.FileName.EndsWith(x)))
                {
                    return new ValidationResult("Solo se aceptan imagenes con los siguientes formatos: " + string.Join(" ", Extensions));
                }
            }                            
            return null;
        }
    }
}