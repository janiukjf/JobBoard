var html = '';
$(function () {

    html = '<hr class="splitter" />' + $('.repetitive').html();

    $('.noscript').hide();
    $('.input_form').fadeIn();

    $(document).on('click', '.nav-tabs li a', function (e) {
        e.preventDefault();
        $(this).tab('show');
    });

});