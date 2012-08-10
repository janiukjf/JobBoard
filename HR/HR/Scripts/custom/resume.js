$(function () {

    $('.noscript').hide();
    $('.input_form').fadeIn();

    $(document).on('click', '.deleteFile', function (e) {
        turl = $(this).attr('href');
        if (confirm('Are you sure you want to delete this file?')) {
            window.location.href = turl;
        }
    });
});