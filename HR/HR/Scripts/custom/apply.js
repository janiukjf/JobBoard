$(document).ready(function () {

    $('.noscript').hide();
    $('.input_form').fadeIn();

    $('label[for=referred_by] input[type=radio]').live('click', function () {
        if ($(this).attr('id') == 'other') {
            $(this).parent().append('<input type="text" name="referred_by" placeholder="Enter other.." />');
        } else {
            $(this).parent().find('input[type=text]').remove();
        }
    });

    if (!$('#isFelon_yes').is(':checked')) {
        $('label[for=convictionsExplanation],label[for=previousExplanation]').hide();
    }

    $('#previousEmployee_yes').change(function () {
        $('label[for=previousExplanation]').slideDown();
    });

    $('#previousEmployee_no').change(function () {
        $('#previousExplanation').val('');
        $('label[for=previousExplanation]').hide();
    });

    $('#isFelon_yes').change(function () {
        $('label[for=convictionsExplanation]').slideDown();
    });

    $('#isFelon_no').change(function () {
        var dates = $('label[for=convictionsExplanation]').find('input').get();
        $.each(dates, function (i, input) {
            $(input).val('');
        });
        $('label[for=convictionsExplanation]').hide();
    });

    $('#addConviction').live('click', function () {
        var count = $('label[for=convictionsExplanation]').find('input[type=text]').length;
        var html = '<input type="date" name="conviction_date" id="conviction_date' + (count + 1) + '" placeholder="Enter the date you were convicted" style="margin:10px 0px" />';
        html += '<input type="text" name="conviction" id="conviction' + (count + 1) + '" placeholder="Enter the conviction" />';
        $('label[for=convictionsExplanation]').find('.helper').before(html);
        //$('input[name=conviction_date]').datepicker();
        Modernize();
    });

    $('#btnSubmit').live('click', function (e) {
        return validateBasic();
    });

    var validateBasic = (function () {
        var err_arr = new Array();
        $('.error_box').remove();

        $.each($('input[required],select[required]').get(), function (i, el) {
            if ($(el).val().length == 0 || $(el).val() == 0) {
                $(el).addClass('err');
                err_arr.push($(el).attr('title'));
            }
        });

        if (err_arr.length > 0) {
            var html = '<div class="error_box">';
            html += '<span>The following errors have occurred:';
            html += '<ul>';
            for (i = 0; i < err_arr.length; i++) {
                html += '<li>' + err_arr[i] + '</li>';
            }
            html += '</ul>';
            html += '</div>';
            $('.input_form h3').after(html);
            return false;
        }
        return true;
    });

    /*$('.moveright').live('click', function () {
    var parent = $(this).parent();
    var next_page = $(this).attr('id').split('_')[0];
    $(parent).animate({
    left: '-150%'
    }, 400, function () {
    $(parent).css('left', '-150%');
    $(parent).appendTo('.input_form');
    $(next_page).animate({
    left: 'auto'
    }, 500, function () {
    $(next_page).css('left', 'auto');
    });
    });
    });
    $('.moveleft').live('click', function () {
    var parent = $(this).parent();
    var prev_page = $(this).attr('id').split('_')[0];
    $(parent).animate({
    left: '150%'
    }, 400, function () {
    $(parent).css('left', '150%');
    $(parent).appendTo('.input_form');
    $('#' + prev_page).animate({
    left: 'auto'
    }, 500, function () {
    $('#' + prev_page).css('left', 'auto');
    });
    });
    });

    window.onbeforeunload = function () {
    return;
    }*/
});