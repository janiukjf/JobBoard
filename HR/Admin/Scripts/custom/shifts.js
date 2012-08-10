/****************
Author: Jessica Janiuk
************************/

$(function () {
    var shift_table = $('table').dataTable({ "bJQueryUI": true });

    $('.del').live('click', function () {
        var href = $(this).attr('href');
        var row = $(this).parent().parent().get()[0];
        if (confirm('Are you sure you want to remove this shift?')) {
            $.get(href, function (resp) {
                if (resp.length == 0) {
                    shift_table.fnDeleteRow(row);
                } else {
                    alert(resp);
                }
            });
        }
        return false;
    });
});