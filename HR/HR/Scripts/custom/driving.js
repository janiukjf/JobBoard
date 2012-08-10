$(document).ready(function () {
    $('label.desc').hide();
    $('.noscript').hide();
    $('form.input_form').fadeIn();

    $(document).on('click', '.descriptors', function () {
        if ($(this).val() == 1) {
            $(this).closest('label').next('label').slideDown();
        } else {
            $(this).closest('label').next('label').slideUp().find('textarea').val('');
        }
    });

    $.each($('.descriptors').get(), function (i, desc) {
        if ($(desc).is(':checked') && $(desc).val() == 1) {
            $(desc).closest('label').next('label').slideDown();
        }
    });
});