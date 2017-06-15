$("document").ready(function () {
            init();

        });

        function init() {
            //AreaCientifica aparece al checkear PresentaTrabajo
            $("#PresentaTrabajo").on("change", presentaTrabajoChange);
            presentaTrabajoChange();
            //Segun el valor de CategoriaId, se muestra la vista parcial de Graduado o DemasCategorias.
            $("#CategoriaId").on("change", categoriaIdChange);
            $("#SeccionAlumnoGrado").css("display", "none");
            $("#SeccionDemasCategorias").css("display", "none");
        }

        function presentaTrabajoChange() {
            var checkBox = $("#PresentaTrabajo");
            var areaCientifica = $("#AreaCientifica").parent().parent();
            var select = $("#AreaCientifica");
            if (checkBox.prop('checked')) {
                areaCientifica.css("display", "block");
                checkBox.val("true");
            } else {
                checkBox.val("false");
                areaCientifica.css("display", "none");
                select.val(0);
            }
        }
        function categoriaIdChange(e) {
            var alumno = $("#SeccionAlumnoGrado");
            var demas = $("#SeccionDemasCategorias");
            if (e.target.value == 1) {
                alumno.css("display", "block");
                demas.css("display", "none");
                resetDemas();
            } else {
                alumno.css("display", "none");
                demas.css("display", "block");
                resetAlumno();
            }
        }

        function resetAlumno() {
            var universidad = $("#Universidad");
            var carrera = $("#Carrera");
            var porcentajeCarrera = $("#PorcentajeCarrera");
            var promedioParcial = $("#PromedioParcial");

            universidad.val("");
            carrera.val("");
            porcentajeCarrera.val(0);
            promedioParcial.val(0);
        }

        function resetDemas() {
            var TituloGrado = $("#TituloGrado");
            var AñoGrado = $("#A_oGrado");
            var TituloPosgrado = $("#TituloPosgrado");
            var AñoPosgrado = $("#A_oPosgrado");
            var PorcentajePosgrado = $("#PorcentajePosgrado");
            var DirectorPosgrado = $("#DirectorPosgrado");
            var LugarTrabajoPosgrado = $("#LugarTrabajoPosgrado");
            var PosicionActual = $("#PosicionActual");
            var Puesto = $("#Puesto");

            TituloGrado = TituloGrado.val("");
            AñoGrado = AñoGrado.val(0);
            TituloPosgrado = TituloPosgrado.val("");
            AñoPosgrado = AñoPosgrado.val(0);
            PorcentajePosgrado = PorcentajePosgrado.val(0);
            DirectorPosgrado = DirectorPosgrado.val("");
            LugarTrabajoPosgrado = LugarTrabajoPosgrado.val("");
            PosicionActual = PosicionActual.val("");
            Puesto = Puesto.val("");


        }