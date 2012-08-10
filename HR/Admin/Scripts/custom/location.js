/****************
    Author: Alex Ninneman
    ************************/

$(function () {
    var loc_table = $('table').dataTable({ "bJQueryUI": true });

    $('.del').live('click', function () {
        var href = $(this).attr('href');
        var row = $(this).parent().parent().get()[0];
        if (confirm('Are you sure you want to remove this location?\r\nAny job listings tied to this location will no longer be tied to a location.')) {
            $.get(href, function (resp) {
                if (resp.length == 0) {
                    loc_table.fnDeleteRow(row);
                } else {
                    alert(resp);
                }
            });
        }
        return false;
    });
});