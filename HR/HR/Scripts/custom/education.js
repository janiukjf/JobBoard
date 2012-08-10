$(document).ready(function () {
    $('.noscript').hide();
    $('form.input_form').fadeIn();

    /* Education Listeners  */
    $(document).on('click', 'a.addEducation', function (e) {
        e.preventDefault();
        var edu = new Education();
        edu.edit();
    });

    $(document).on('click', '.edu-form div.modal-footer a.btn-save', function (e) {
        e.preventDefault();

        var edu = new Education();
        edu.id = $('.edu-form #edu_id').val();
        edu.school_type = $('.edu-form #school_type').val();
        edu.name = $('.edu-form #name').val();
        edu.address = $('.edu-form #address').val();
        edu.city = $('.edu-form #city').val();
        edu.state_id = $('.edu-form #state_id').val();
        edu.years_completed = $('.edu-form #years_completed').val();
        edu.gpa = $('.edu-form #gpa').val();
        edu.degree = $('.edu-form #degree').val();
        edu.app_id = $('#app_id').val();
        edu.save();

    });

    $(document).on('click', 'a.edit-edu', function (e) {
        e.preventDefault();
        var edu = new Education(),
            row = $(this).closest('tr'),
            json = unescape($(this).attr('data-json'));

        var obj = JSON.parse(json);
        edu.create(obj);
        edu.edit();
    });

    $(document).on('click', 'a.delete-edu', function (e) {
        e.preventDefault();
        var edu = new Education();
        edu.id = $(this).attr('data-id');
        if (confirm('Are you sure you want to remove this education record?')) {
            edu.remove();
        }
    });
    /* End Education Listeners */

    /* Reference Listeners */

    $(document).on('click', 'a.addReference', function (e) {
        e.preventDefault();
        var ref = new Reference();
        ref.edit();
    });

    $(document).on('click', '.ref-form div.modal-footer a.btn-save', function (e) {
        e.preventDefault();

        var ref = new Reference();
        ref.id = $('.ref-form #ref_id').val();
        ref.name = $('.ref-form #name').val();
        ref.job_title = $('#job_title').val();
        ref.phone = $('.ref-form #phone').val();
        ref.years_known = $('.ref-form #years_known').val();
        ref.app_id = $('#app_id').val();
        ref.save();

    });

    $(document).on('click', 'a.edit-ref', function (e) {
        e.preventDefault();
        var ref = new Reference(),
            row = $(this).closest('tr'),
            json = unescape($(this).attr('data-json'));

        var obj = JSON.parse(json);
        ref.create(obj);
        ref.edit();
    });

    $(document).on('click', 'a.delete-ref', function (e) {
        e.preventDefault();
        var ref = new Reference();
        ref.id = $(this).attr('data-id');
        if (confirm('Are you sure you want to remove this reference?')) {
            ref.remove();
        }
    });
    /* End Reference Listeners */

});

var Reference = function () {
    id = generateGUID(),
    name = '',
    job_title = '',
    phone = '',
    years_known = 0,
    app_id = ''
};

Reference.prototype = {
    create: function (obj) {
        this.id = obj.id || generateGUID();
        this.name = obj.name;
        this.job_title = obj.job_title;
        this.phone = obj.phone;
        this.years_known = parseFloat(obj.years_known);
        this.app_id = obj.app_id;
    }, edit: function () {
        $('.ref-form #ref_id').val(this.id);
        $('.ref-form #name').val(this.name);
        $('.ref-form #job_title').val(this.job_title);
        $('.ref-form #phone').val(this.phone);
        $('.ref-form #years_known').val(this.years_known);

        $('div.modal-body div.alert').remove();
        $('.ref-form').modal('show', {
            backdrop: true,
            keyboard: true
        })
    }, save: function () {
        var json_obj = '';
        try {
            $('div.modal-footer span.msg').remove();
            if (this.id === undefined || this.id.length === 0) {
                this.id = generateGUID();
            }
            var json_obj = JSON.stringify(this),
                error = false;
            $.ajax({
                url: '/Job/SaveReference?reference=' + json_obj,
                type: 'GET',
                dataType: 'json',
                beforeSend: function () {
                    $('div.modal-footer span.msg').text('Saving...');
                }, success: function (data, textStatus, xhr) {
                    $('div.modal-footer span.msg').text('');
                    if (typeof data != 'object') {
                        throw ('There was an error while saving.');
                    }

                    var json_data = data;

                    $('tr#' + json_data.id).remove();

                    var html = '<tr id="' + json_data.id + '">';
                    html += '<td>' + json_data.name + ' - ' + json_data.job_title + '</td>';
                    html += '<td>' + json_data.phone + '</td>';
                    html += '<td>' + json_data.years_known + '</td>';
                    html += '<td>';

                    html += '<div class="btn-group"><a class="btn btn-inverse dropdown-toggle" data-toggle="dropdown" href="#">Action<span class="caret"></span></a>';
                    html += '<ul class="dropdown-menu">';
                    html += '<li>';
                    html += '<a href="#" title="Edit" class="edit-ref" data-json="' + escape(json_data.json) + '" data-id="' + json_data.id + '">Edit</a>';
                    html += '</li>';
                    html += '<li>';
                    html += '<a href="#" title="Delete" class="delete-ref" data-id="' + json_data.id + '">Delete</a>';
                    html += '</li>';
                    html += '</ul></div></td>';

                    html += '</tr>';
                    $('.ref-table').append(html);
                    $('.ref-form').modal('hide');
                }, error: function (xhr, textStatus, errorThrown) {
                    error = true;
                    //throw ('There was an error while saving');
                }, complete: function () {
                    if (error) {
                        $('div.modal-body div.alert').remove();
                        var html = '<div class="alert alert-error">';
                        html += '<a class="close" data-dismiss="alert" href="#">x</a>';
                        var html = '<div class="alert alert-error">';
                        html += '<a class="close" data-dismiss="alert" href="#">x</a>';
                        html += '<h4 class="alert-heading">Error!</h4>';
                        html += '<strong>Oh snap!</strong> Something went wrong while saving.</div>';
                        $('div.modal-body').prepend(html);
                    }
                }
            });
        } catch (err) {
            $('div.modal-body div.alert').remove();
            var html = '<div class="alert alert-error">';
            html += '<a class="close" data-dismiss="alert" href="#">x</a>';
            var html = '<div class="alert alert-error">';
            html += '<a class="close" data-dismiss="alert" href="#">x</a>';
            html += '<h4 class="alert-heading">Error!</h4>';
            html += '<strong>Oh snap!</strong> Something went wrong: ' + err + '</div>';
            $('div.modal-body').prepend(html);
        }
    }, remove: function () {
        var error = false;
        var id = this.id;
        $.ajax({
            url: '/Job/DeleteReference/' + id,
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                $('.ref-table tr#' + id).find('td').animate({ opacity: '0.2' });
            }, success: function (data, textStatus, xhr) {
                if (data == null || data.length == 0) {
                    $('.ref-table tr#' + id).remove();
                }
            }, error: function (xhr, textStatus, errorThrown) {
                error = true;
            }, complete: function () {
                if (error) {
                    alert('There was an error while removing the reference.');
                }
            }
        });
    }
};


var Education = function () {
    id = generateGUID(),
    school_type = '',
    name = '',
    address = '',
    city = '',
    state_id = '',
    years_completed = 0,
    gpa = '0.0',
    degree = '',
    app_id = '',
    json = ''
};

Education.prototype = {
    create: function (obj) {
        this.id = obj.id || generateGUID();
        this.school_type = obj.school_type;
        this.name = obj.name;
        this.address = obj.address;
        this.city = obj.city;
        this.state_id = obj.state_id;
        this.years_completed = parseFloat(obj.years_completed);
        this.gpa = obj.gpa;
        this.degree = obj.degree;
        this.app_id = obj.app_id;
        this.description = obj.description || '';
    }, edit: function () {
        $('.edu-form #edu_id').val(this.id);
        $('.edu-form #school_type').val(this.school_type);
        $('.edu-form #name').val(this.name);
        $('.edu-form #address').val(this.address);
        $('.edu-form #city').val(this.city);
        $('.edu-form #state_id').val(this.state_id);
        $('.edu-form #years_completed').val(this.years_completed);
        $('.edu-form #gpa').val(this.gpa);
        $('.edu-form #degree').val(this.degree);

        $('div.modal-body div.alert').remove();
        $('.edu-form').modal('show', {
            backdrop: true,
            keyboard: true
        }).css({
            width: '980',
            'margin-left': function () {
                return -($(this).width() / 2) - 25;
            }
        });
    }, save: function () {
        var json_obj = '';
        try {
            $('div.modal-footer span.msg').remove();
            if (this.id === undefined || this.id.length === 0) {
                this.id = generateGUID();
            }
            var json_obj = JSON.stringify(this),
                error = false;
            $.ajax({
                url: '/Job/SaveEducation?edu=' + json_obj,
                type: 'GET',
                dataType: 'json',
                beforeSend: function () {
                    $('div.modal-footer span.msg').text('Saving...');
                }, success: function (data, textStatus, xhr) {
                    $('div.modal-footer span.msg').text('');
                    if (typeof data != 'object') {
                        throw ('There was an error while saving.');
                    }

                    var json_data = data;

                    $('tr#' + json_data.id).remove();

                    var html = '<tr id="' + json_data.id + '">';
                    html += '<td>' + json_data.school_type + '</td>';
                    html += '<td>' + json_data.description + '</td>';
                    html += '<td>' + json_data.years_completed + '</td>';
                    html += '<td>' + json_data.gpa + '</td>';
                    html += '<td>' + json_data.degree + '</td>';
                    html += '<td>';

                    html += '<div class="btn-group"><a class="btn btn-inverse dropdown-toggle" data-toggle="dropdown" href="#">Action<span class="caret"></span></a>';
                    html += '<ul class="dropdown-menu">';
                    html += '<li>';
                    html += '<a href="#" title="Edit" class="edit-edu" data-json="' + escape(json_data.json) + '" data-id="' + json_data.id + '">Edit</a>';
                    html += '</li>';
                    html += '<li>';
                    html += '<a href="#" title="Delete" class="delete-edu" data-id="' + json_data.id +'">Delete</a>';
                    html += '</li>';
                    html += '</ul></div></td>';

                    html += '</tr>';
                    $('.edu-table').append(html);
                    $('.edu-form').modal('hide');
                }, error: function (xhr, textStatus, errorThrown) {
                    error = true;
                    //throw ('There was an error while saving');
                }, complete: function () {
                    if (error) {
                        var html = '<div class="alert alert-error">';
                        html += '<a class="close" data-dismiss="alert" href="#">x</a>';
                        var html = '<div class="alert alert-error">';
                        html += '<a class="close" data-dismiss="alert" href="#">x</a>';
                        html += '<h4 class="alert-heading">Error!</h4>';
                        html += '<strong>Oh snap!</strong> Something went wrong while saving.</div>';
                        $('div.modal-body').prepend(html);
                    }
                }
            });
        } catch (err) {
            var html = '<div class="alert alert-error">';
            html += '<a class="close" data-dismiss="alert" href="#">x</a>';
            var html = '<div class="alert alert-error">';
            html += '<a class="close" data-dismiss="alert" href="#">x</a>';
            html += '<h4 class="alert-heading">Error!</h4>';
            html += '<strong>Oh snap!</strong> Something went wrong: ' + err + '</div>';
            $('div.modal-body').prepend(html);
        }

    }, remove: function () {
        var error = false;
        var id = this.id;
        $.ajax({
            url: '/Job/DeleteEducation/' + id,
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                $('.edu-table tr#' + id).find('td').animate({ opacity: '0.2' });
            }, success: function (data, textStatus, xhr) {
                if (data == null || data.length == 0) {
                    $('.edu-table tr#' + id).remove();
                }
            }, error: function (xhr, textStatus, errorThrown) {
                error = true;
            }, complete: function () {
                if (error) {
                    alert('There was an error while removing the education record.');
                }
            }
        });
    }
}

function generateGUID() {
    return "00000000-0000-0000-0000-000000000000";
}