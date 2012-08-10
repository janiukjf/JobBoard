/* Author: */

$(function () {
    $('nav ul.top li.main').hoverIntent(function () {
        $(this).find('.sub').slideDown();
    }, function () {
        $(this).find('.sub').slideUp();
    });

    Modernize();
});

function Modernize() {
    // We need to detect HTML5 support
    $.each($('input[type=date]'), function (i, el) {
        $(el).datepicker().get(0).setAttribute('type', 'text');
    });
    $('input[type=datetime]').datepicker();

    if (!Modernizr.input.placeholder) {
        $("input").each(function () {
            if ($(this).val() == "" && $(this).attr("placeholder") != "") {
                $(this).val($(this).attr("placeholder"));
                $(this).focus(function () {
                    if ($(this).val() == $(this).attr("placeholder")) $(this).val("");
                });
                $(this).blur(function () {
                    if ($(this).val() == "") $(this).val($(this).attr("placeholder"));
                });
            }
        });
    }
}