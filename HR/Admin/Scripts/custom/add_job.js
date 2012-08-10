/******************
Author: Alex Ninneman @ninnemana
*********************************/
var cat_list = new Array();
var shift_list = new Array();
var contact_list = new Array();
$(function () {
    var cat_table = $('.cat_table').dataTable({ "bJQueryUI": true });
    var shift_table = $('.shift_table').dataTable({ "bJQueryUI": true });
    var contact_table = $('.contact_table').dataTable({ "bJQueryUI": true });
    var index_table = $('.index_table').dataTable({ "bJQueryUI": true });

    /******************************************************
    *               INDEX PAGE JAVASCRIPT                 *
    ******************************************************/

    $('.delete').live('click', function (e) {
        e.preventDefault();
        if (confirm('Are you sure you want to remove this job posting?')) {
            var path = $(this).attr('href');
            var el = $(this).parent().parent().get()[0];
            $.get(path, { 'ajax': true }, function (resp) {
                if (resp.length == 0) {
                    index_table.fnDeleteRow(el);
                } else {
                    alert('There was an error while deleting the job posting.');
                }
            });
        }
    });

    /******************************************************
    *             END INDEX PAGE JAVASCRIPT               *
    ******************************************************/

    /******************************************************
    *               ADD PAGE JAVASCRIPT                   *
    ******************************************************/
    $('#add_cat_select').live('change', function () {
        var val = $(this).val();
        var cat = $(this).find('option[value=' + val + ']').text();
        if (val != 0 && cat_list.indexOf(val) == -1) {
            cat_table.fnAddData([
                cat + '<input type="hidden" name="cats" value="' + val + '" />',
                '<a href="javascript:void(0)" class="add_del_cat">Delete</a>'
            ]);
            cat_list.push(val);
        } else {
            alert('You cannot add this job to the same category more than once.');
        }
        $(this).val(0);
    });

    $('.add_del_cat').live('click', function () {
        if (confirm('Are you sure you want to remove the association is this job category?')) {
            var row = $(this).parent().parent().get()[0];
            cat_table.fnDeleteRow(row);
        }
    });

    $('#add_shift_select').live('change', function () {
        var val = $(this).val();
        var shift = $(this).find('option[value=' + val + ']').text();
        if (val != 0 && shift_list.indexOf(val) == -1) {
            shift_table.fnAddData([
                shift + '<input type="hidden" name="shifts" value="' + val + '" />',
                '<a href="javascript:void(0)" class="add_del_shift">Delete</a>'
            ]);
            shift_list.push(val);
        } else {
            alert('You cannot add this shift to the same job more than once.');
        }
        $(this).val(0);
    });

    $('.add_del_shift').live('click', function () {
        if (confirm('Are you sure you want to remove this shift from this job?')) {
            var row = $(this).parent().parent().get()[0];
            shift_table.fnDeleteRow(row);
        }
    });

    $('#add_contact_select').live('change', function () {
        var val = $(this).val();
        var contact = $(this).find('option[value=' + val + ']').text();
        if (val != 0 && contact_list.indexOf(val) == -1) {
            contact_table.fnAddData([
                contact + '<input type="hidden" name="contacts" value="' + val + '" />',
                '<a href="javascript:void(0)" class="add_del_contact">Delete</a>'
            ]);
            contact_list.push(val);
        } else {
            alert('You cannot add this notification contact to the same job more than once.');
        }
        $(this).val(0);
    });

    $('.add_del_contact').live('click', function () {
        if (confirm('Are you sure you want to remove this notification contact from this job?')) {
            var row = $(this).parent().parent().get()[0];
            contact_table.fnDeleteRow(row);
        }
    });

    /******************************************************
    *             END ADD PAGE JAVASCRIPT                 *
    ******************************************************/

    /******************************************************
    *               EDIT PAGE JAVASCRIPT                   *
    ******************************************************/
    $(document).on('click', '#btnDuplicate', function (e) {
        e.preventDefault();
        var href = $(this).attr('href');
        if (confirm('Are you sure you want to duplicate this job posting?')) {
            window.location.href = href;
        }
    });

    $('#edit_cat_select').live('change', function () {
        var val = $(this).val();
        var cat = $(this).find('option[value=' + val + ']').text();
        var job_id = $('form').attr('action').split('/')[4];
        if (job_id.length > 0 && cat_list.indexOf(val) == -1) {
            $.getJSON('/Admin/Jobs/AddCategory', { 'job_id': job_id, 'cat_id': val }, function (resp) {
                if (resp.id != undefined && resp.id != null) {
                    cat_table.fnAddData([
                            cat,
                            '<a href="javascript:void(0)" id="' + resp.cat + '" class="edit_del_cat">Delete</a>'
                        ]);
                    cat_list.push(val);
                } else {
                    alert('There was error while adding the category.');
                }
            });
        } else {
            alert('You cannot add this job to the same category more than once.');
        }
        $(this).val(0);
    });

    $('.edit_del_cat').live('click', function () {
        if (confirm('Are you sure you want to remove the association is this job category?')) {
            var cat_id = $(this).attr('id');
            var job_id = $('form').attr('action').split('/')[4];
            var row = $(this).parent().parent().get()[0];
            if (cat_id.length > 0 && job_id.length > 0) {
                $.get('/Admin/Jobs/DeleteCategory', { 'cat_id': cat_id, 'job_id': job_id }, function (resp) {
                    if (resp.length == 0) {
                        cat_list.pop(cat_id);
                        cat_table.fnDeleteRow(row);
                    } else {
                        alert('There was an error while processing your request.');
                    }
                });
            } else {
                alert('There was an error while processing your request.');
            }
        }
        return false;
    });

    $('#edit_shift_select').live('change', function () {
        var val = $(this).val();
        var shift = $(this).find('option[value=' + val + ']').text();
        var job_id = $('form').attr('action').split('/')[4];
        if (job_id.length > 0 && shift_list.indexOf(val) == -1) {
            $.getJSON('/Admin/Jobs/AddShift', { 'job_id': job_id, 'shift_id': val }, function (resp) {
                if (resp.id != undefined && resp.id != null) {
                    shift_table.fnAddData([
                            shift,
                            '<a href="javascript:void(0)" id="' + resp.shift + '" class="edit_del_shift">Delete</a>'
                        ]);
                    shift_list.push(val);
                } else {
                    alert('There was error while adding the shift.');
                }
            });
        } else {
            alert('You cannot add this shift to the same job more than once.');
        }
        $(this).val(0);
    });

    $('.edit_del_shift').live('click', function () {
        if (confirm('Are you sure you want to remove this shift from this job?')) {
            var shift_id = $(this).attr('id');
            var job_id = $('form').attr('action').split('/')[4];
            var row = $(this).parent().parent().get()[0];
            if (shift_id.length > 0 && job_id.length > 0) {
                $.get('/Admin/Jobs/DeleteShift', { 'shift_id': shift_id, 'job_id': job_id }, function (resp) {
                    if (resp.length == 0) {
                        shift_list.pop(shift_id);
                        shift_table.fnDeleteRow(row);
                    } else {
                        alert('There was an error while processing your request.');
                    }
                });
            } else {
                alert('There was an error while processing your request.');
            }
        }
        return false;
    });

    $('#edit_contact_select').live('change', function () {
        var val = $(this).val();
        var contact = $(this).find('option[value=' + val + ']').text();
        var job_id = $('form').attr('action').split('/')[4];
        if (job_id.length > 0 && contact_list.indexOf(val) == -1) {
            $.getJSON('/Admin/Jobs/AddContact', { 'job_id': job_id, 'contact_id': val }, function (resp) {
                if (resp.id != undefined && resp.id != null) {
                    contact_table.fnAddData([
                            contact,
                            '<a href="javascript:void(0)" id="' + resp.Contact.id + '" class="edit_del_contact">Delete</a>'
                        ]);
                    contact_list.push(val);
                } else {
                    alert('There was error while adding the notification contact.');
                }
            });
        } else {
            alert('You cannot add this notification contact to the same job more than once.');
        }
        $(this).val(0);
    });

    $('.edit_del_contact').live('click', function () {
        if (confirm('Are you sure you want to remove this notification contact from this job?')) {
            var contact_id = $(this).attr('id');
            var job_id = $('form').attr('action').split('/')[4];
            var row = $(this).parent().parent().get()[0];
            if (contact_id.length > 0 && job_id.length > 0) {
                $.get('/Admin/Jobs/DeleteContact', { 'contact_id': contact_id, 'job_id': job_id }, function (resp) {
                    if (resp.length == 0) {
                        contact_list.pop(contact_id);
                        contact_table.fnDeleteRow(row);
                    } else {
                        alert('There was an error while processing your request.');
                    }
                });
            } else {
                alert('There was an error while processing your request.');
            }
        }
        return false;
    });

    /******************************************************
    *             END EDIT PAGE JAVASCRIPT                *
    ******************************************************/
});