/****************
Author: Alex Ninneman
************************/

$(function () {
    var ed_table = $('table').dataTable({ "bJQueryUI": true });

    $('.del').live('click', function () {
        var href = $(this).attr('href');
        var row = $(this).parent().parent().get()[0];
        if (confirm('Are you sure you want to remove this education level?\r\nAny job listings tied to this level will no longer be tied to an education credential.')) {
            $.get(href, function (resp) {
                if (resp.length == 0) {
                    ed_table.fnDeleteRow(row);
                } else {
                    alert(resp);
                }
            });
        }
        return false;
    });
});