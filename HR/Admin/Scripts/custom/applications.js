/******************
Author: Alex Ninneman @ninnemana
*********************************/
$(function () {
    var index_table = $('.index_table').dataTable({ "bJQueryUI": true });

    /******************************************************
    *               INDEX PAGE JAVASCRIPT                 *
    ******************************************************/

    $('.archive').live('click', function (e) {
        e.preventDefault();
        if (confirm('Are you sure you want to archive this application?')) {
            var path = $(this).attr('href');
            var el = $(this).parent().parent().get()[0];
            $.get(path, { 'ajax': true }, function (resp) {
                if (resp.length == 0) {
                    index_table.fnDeleteRow(el);
                } else {
                    alert('There was an error while archiving the application.');
                }
            });
        }
    });

    /******************************************************
    *             END INDEX PAGE JAVASCRIPT               *
    ******************************************************/
});