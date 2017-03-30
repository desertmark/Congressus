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
        bool Multiple { get; set; }
        public FileExtensionAttribute(string[] extensions, bool multiple = true)
        {
            Extensions = extensions;
            Multiple = multiple;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Debe seleccionar un archivo.");
            }
            if (value is List<HttpPostedFileBase>)
            {
                var archivos = (List<HttpPostedFileBase>)value;
                if (archivos.First() == null)
                {
                    return new ValidationResult("Debe seleccionar al menos un archivo.");
                }
                foreach (var archivo in archivos)
                {
                    if (!Extensions.Any(x => archivo.FileName.EndsWith(x)))
                    {
                        return new ValidationResult("Solo se aceptan archivos con los siguientes formatos: " + string.Join(" ", Extensions));
                    }
                }
            }else if(value is HttpPostedFileBase)
            {
                var archivo = (HttpPostedFileBase)value;
                if (!Extensions.Any(x => archivo.FileName.EndsWith(x)))
                {
                    return new ValidationResult("Solo se aceptan archivos con los siguientes formatos: " + string.Join(" ", Extensions));
                }
            }                          
            return ValidationResult.Success;
        }
    }
}