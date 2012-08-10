/****************
    Author: Alex Ninneman @ninnemana
    **********************************/

$(function () {
    var cat_table = $('table').dataTable({ "bJQueryUI": true });

    // handle delete action in index page
    $('.del').live('click', function () {
        var path = $(this).attr('href');
        var row = $(this).parent().parent().get()[0];
        if (confirm('Are you sure you want to remove this category?\r\n This will cause all job listings in this category to become uncategorized.')) {
            $.get(path, function (resp) {
                if (resp.length == 0) {
                    cat_table.fnDeleteRow(row);
                } else {
                    alert('There was an error while removing the record.');
                }
            });
        }
        return false;
    });
});