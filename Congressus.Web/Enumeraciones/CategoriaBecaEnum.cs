using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Congressus.Web.Enumeraciones
{
    public enum CategoriaBecaEnum
    {
        [Display(Name="Alumno de grado")]
        AlumnoGrado = 1,
        Graduado = 2,
        [Display(Name = "Estudiante de maestría")]
        EstudianteMaestría = 3,
        [Display(Name = "Estudiante de doctorado")]
        EstudianteDoctorado = 4
    }
}