$('document').ready(function () {
    console.log("document ready!");
    //Para validar las fechas con Jquery en formato dd/mm/aaaa
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || parseDate(value, "dd-MM-yyyy") !== null;
    };
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
    $('#agregarMiembro').click(function () {
        $('#agregarMiembroDiv').openModal();
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
});
//Silder eventos
function next() {
    $('.carousel.carousel-slider').carousel('next');
}