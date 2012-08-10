/* Author: Alex Ninneman */

$(function () {
    $('nav ul.top li.main').hoverIntent(function () {
        $(this).find('.sub').slideDown();
    }, function () {
        $(this).find('.sub').slideUp();
    });


    // We need to add support for HTML5 stuff
    if (!Modernizr.inputtypes.date) {
        //alert('no html5 support');
    }
});