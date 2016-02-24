$('document').ready(function () {
    console.log("document ready!");

    //tooltip para los datetime
    //$('input[type="datetime"]').addClass("tooltipped");
    //$('input[type="datetime"]').attr({
    //    "data-position":"top",
    //    "data-delay": 50,
    //    "data-tooltip": "formato: dd/mm/aaaa hh:mm:ss"
    //    });
    //$('.tooltipped').tooltip({ delay: 50 });

    //inicializar los datetime
    if ($('input[type="datetime"]').length > 0)
    {
        var value = $('input[type="datetime"]').val();
        var widget = '<div class="well">';
        widget += '<div class="input-append datetimepicker">';
        widget += '<input data-format="dd/MM/yyyy hh:mm:ss" type="text" Name="Fecha" id="Fecha" value="'+value+'">';
        widget += '<span class="add-on btn">';
        widget += '<i class="material-icons">perm_contact_calendar</i>';
        widget += '</span></div></div>';
    
        $('input[type="datetime"]').replaceWith(widget);
        $('.datetimepicker').datetimepicker({
            language: 'es-AR',
        });
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