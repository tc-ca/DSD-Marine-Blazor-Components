// jQuery date picker
function jqueryDatepicker(id, format, lang) {

    if (lang == "fr") {

    } else {
        lang = "";
    }

    var options = $.extend({},
        $.datepicker.regional[lang],
        {
            dateFormat: format, // show date format mm/dd/yy.
            changeYear: true,
            changeMonth: true,
            onSelect: function (date) {

                var elementRef = $(this)[0];

                var event = new Event('change');

                elementRef.dispatchEvent(event);
            }
        });

    $('input[id$=' + id + ']').datepicker(options);
}