$('document').ready(function () {
    console.log("document ready!");
    if($.validator!=null){
    //Para validar las fechas con Jquery en formato dd/mm/aaaa
        $.validator.methods.date = function (value, element) {
            return this.optional(element) || parseDate(value, "dd-MM-yyyy") !== null;
        };
    }
    var campos = $('input[type="datetime"]');
    //inicializar los datetime
    if (campos.length > 0)
    {
        for (var i in campos) {

            var value = campos[i].value;
            var name = campos[i].name;

            var widget = '<div class="well">';
            widget += '<div class="input-append datetimepicker">';
            widget += '<input data-format="dd/MM/yyyy hh:mm:ss" type="text" Name="' + name + '" id="' + name + '" value="' + value + '">';
            widget += '<span class="add-on btn">';
            widget += '<i class="material-icons">perm_contact_calendar</i>';
            widget += '</span></div></div>';

            $('input[name="'+name+'"]').replaceWith(widget);
            $('.datetimepicker').datetimepicker({
                language: 'es-AR',
            });
        }
    }

    //textareas
    $('textarea').addClass('materialize-textarea');
    $('textarea').trigger('autoresize');

    //parallax
    $('.button-collapse').sideNav();
    $('.parallax').parallax();


    //views/Miembros/Administrar/AgregarMiembro Modal
    $('.modal').modal();
    $('#agregarMiembro').click(function () {
        $('#agregarMiembroDiv').modal('open');
    });

    //INPUT VALUES TO UPPER CASE
    $('input[type="text"]').focusout(function (e) {
        var upperVal = $(e.currentTarget).val().toUpperCase();
        $(e.currentTarget).val(upperVal);
    });
    $('textarea').focusout(function (e) {
        var upperVal = $(e.currentTarget).val().toUpperCase();
        $(e.currentTarget).val(upperVal);
    });

    //CHIPS
    var chips = $(".chips");
    chips.addClass("tooltipped");
    chips.attr("data-position","right")
    chips.attr("data-tooltip", "Apretar ENTER luego de ingresar cada valor.");
    $('.tooltipped').tooltip({ delay: 50 });

    $('.chips').material_chip();
    
    $('.chips').on('chip.delete', function (e, chip) {
        if ($('.chipHidden').val().includes(";")) {
            $('.chipHidden').val(
                $('.chipHidden').val().replace(';' + chip.tag, "")
            );
        } else {
            $('.chipHidden').val(
                $('.chipHidden').val().replace(chip.tag, "")
            );
        }

    });
    $('.chips').on('chip.add', function (e, chip) {
        if ($('.chipHidden').val() == "") {
            $('.chipHidden').val($('.chipHidden').val() + chip.tag);
        }
        else {
            $('.chipHidden').val($('.chipHidden').val() + ';' + chip.tag);
        }
    });

    var valores = $('.chipHidden').val();
    console.log(valores);
    if (valores != "" && valores != null) {
        var valoresArray = valores.split(';');
        var data = [];
        for (var i = 0; i < valoresArray.length; i++) {
            data.push({ tag: valoresArray[i] });
        }
        console.log(data);
        $('.chips').material_chip({ data: data });
    }

    
});
//Silder eventos
function next() {
    $('.carousel-slider').carousel('next');
    $('.carousel-normal').carousel('next');
}

