$('document').ready(function () {
            var checkbox = $("#TipoTaller");
            checkbox.change(function () {
                if (checkbox.is(":checked")) {
                    $("#Cupo").prop("disabled", "");
                } else {
                    $("#Cupo").prop("disabled", "false");
                    $("#Cupo").val(0);
                }
            });

            var paperSelect = $('select[name="PaperId"]');
            var orador = $('#orador');
            paperSelect.change(function (e) {
                if(e.target.value=="0"){
                    orador.css('display', 'block');
                } else {
                    $('#orador input').val('');
                    orador.css('display', 'none');
                }
            });

        });