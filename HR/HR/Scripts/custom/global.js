/* Author: */
var compatibleWarning;
$(function () {
    $('nav ul.top li.main').hoverIntent(function () {
        $(this).find('.sub').slideDown();
    }, function () {
        $(this).find('.sub').slideUp();
    });
    /*if ($.browser.msie && ($.browser.version == '7.0' && navigator.userAgent.indexOf('Trident') != -1) || (document.documentMode != undefined && (Number($.browser.version) != Number(document.documentMode)))) {
    alert("You're in compatibility mode");
    }*/
    $.CompatibilityMode(compatibleWarning);

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

compatibleWarning = function () {
    $('body').append('<div id="compatible"></div>');
    $('#compatible').append('<div class="bg"></div>');
    $('#compatible').append('<div class="warning"><h2>WARNING!! Internet Explorer Compatibility Mode Detected!!</h2><p>You currently are using internet explorer with compatibility mode enabled. Please turn off compatibility mode to continue using CURT Manufacturing\'s HR site.</p><p>Thank you,<br />CURT Manufacturing Ecommerce Team</p></div>');
    adjustWindow();
    $(window).resize(adjustWindow);
    $(window).scroll(adjustWindow);
}

adjustWindow = function () {
    var divheight = $('#compatible .warning').outerHeight();
    var windowheight = $(window).height();
    var docscrollTop = $(document).scrollTop();
    $('#compatible .warning').css('top', (docscrollTop + ((windowheight / 2) - (divheight / 2))) + 'px');
}