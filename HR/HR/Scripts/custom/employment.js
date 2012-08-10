var html = '';
$(document).ready(function () {

    html = '<hr class="splitter" />' + $('.repetitive').html();

    $('.noscript').hide();
    $('.input_form').fadeIn();

    $('#addEmployment,#saveEmployment').live('click', function (e) {
        return validateBasic();
    });

    $(document).on('click', '.nav-tabs li a', function (e) {
        e.preventDefault();
        $(this).tab('show');
    });

    $(document).on('click', 'a.del-employer', function (e) {
        var emp = $(this).data('employer') || 'this employer';
        if (confirm('Are you sure you want to remove ' + emp + '?')) {
            return true;
        } else {
            return false;
        }
    });

    var validateBasic = (function () {
        var err_arr = new Array();
        $('.error_box').remove();

        $.each($('input[required],select[required],textarea[required]').get(), function (i, el) {
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
});