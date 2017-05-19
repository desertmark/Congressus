(function ($) {
    console.log("hola");
    $("select[data-ajax=true]").on("change", function (evt) {    

        var select = $(evt.target);

        var targetId = select.attr('data-ajax-update');
        var url = select.attr('data-ajax-url');
        var method = select.attr('data-ajax-method');
        var $targetElement = $(targetId);

        $.ajax({
            method: method,
            url: url,
            data: { value: select.val() },
            success: function (areas) {

                console.log(areas);
                       var options = areas.map((area) => {
                           return '<option value="' + area + '">' + area + '</option>';
                       }).join("");
                       
                       $targetElement.html(options);
            },
            fail: function (err) {
                $targetElement.html("");
            }
        

        });

        //asyncRequest(this, 
        //    { 
        //        type: "GET", 
        //        data: [{ name: $(this).attr('name'), value: $(this).val()}] 
        //        });
    });


}(jQuery));