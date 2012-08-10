/***************
    Author: Alex Ninneman @ninnemana
    *************/

jQuery(document).ready(function () {
    var contact_table = $('table').dataTable({ "bJQueryUI": true });

    $('.change_pass').click(function () {
        $('#old_pass').remove();
        $('#old_pass_helper').remove();
        var html = '<span id="old_pass_helper" class="helper">Enter the old password</span>';
        html += '<input type="password" name="old_pass" id="old_pass" placeholder="Enter the old password" style="margin-bottom:10px" />';
        $('#password').before(html);
        $('#old_pass').focus();
    });

    $('#old_pass').live('keyup', function () {
        var old_pass = $(this).val();
        var pass = $('#password').val();
        log(old_pass + ':' + pass);
        if (old_pass === pass) {
            $('#password').removeAttr('disabled');
            $('#old_pass').remove();
            $('#old_pass_helper').remove();
            $('#password').focus();
        }
    });

    // handle delete action in index page
    $('.del').live('click', function () {
        var path = $(this).attr('href');
        var row = $(this).parent().parent().get()[0];
        if (confirm('Are you sure you want to remove this contact?\r\nWe will temporarily mark you as the contact person for any listings this user is assigned to.')) {
            $.get(path, function (resp) {
                if (resp.length == 0) {
                    contact_table.fnDeleteRow(row);
                } else {
                    alert('There was an error while removing the record.');
                }
            });
        }
        return false;
    });
});