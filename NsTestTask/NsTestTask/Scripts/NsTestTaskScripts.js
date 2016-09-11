////////modals\\\\\\\\\\\\\
function showModalIfError(error) {
    if (error == 'register') {
        $('#regModal').modal('show');
    }
    if (error == 'addTaskError') {
        showModal('add');
    }
    if (error == 'editTaskError') {
        showModal('edit');
    }
    if (error == 'shareTaskError') {
        showModal('share');
    }

};

function showModal(task, param) {
    if (task == 'add') {
        $('#Title').val('');
        $('#Description').val('');
        $('#taskId').remove();
        $('#EditModal').find('.modal-title').empty();
        $('#EditModal').find('.modal-title').append('Add Task');
        $('#modalActionAttr').attr('action', '');
        $('#modalActionAttr').attr('action', 'Home/AddTask');
        $('#EditModal').modal('show');
    }
    if (task == 'edit') {

        $('#EditModal').find('.modal-title').empty();
        $('#EditModal').find('.modal-title').append('Edit Task');
        $('#modalActionAttr').attr('action', '');
        $('#modalActionAttr').attr('action', 'Home/EditTask');
        var id = $(param).attr('id');
        var name = $(param).attr('name');
        var desc = $(param).attr('desc');
        $('#Title').val(name);
        $('#Description').val(desc);
        $('#taskId').remove();
        $('#hiddenId').append('<input class="form-control" type="hidden" name="Id" id="taskId" value="">');
        $('#taskId').val(id);
        $('#EditModal').modal('show');
    }

    if (task == 'share') {
        var id = $(param).attr('id');
        var name = $(param).attr('name');
        $('#shareTaskActionAttr').attr('action', '');
        $('#shareTaskActionAttr').attr('action', 'Home/ShareTask');
        $('#hiddenShareTaskId').val(id);
        $('#shareTaskName').val(name);
        $('#ShareModal').modal('show');
    }
};
//////////////////////////////////////////

/////////////slider class\\\\\\\\\\\\\\\
var sliderObj = new function () {
    var slide = true
    var counter = 0

    this.slide = function (arg) {
        slide = true;
        if (arg == 'start')
            slideStart();
        if (arg == 'stop')
            slideStop();
    }

    slideStart = function () {
        if (counter < 20 && slide == true) {
            setTimeout(ajCall, 2000);
        }
    };
    slideStop = function () {
        slide = false;
    };

    ajCall = function () {
        $.ajax({
            type: "GET",
            async: true,
            url: '/Home/TestStart',

            success: function (data) {
                $("#slider-range-max").slider('value', data);
                $("#amount").val(data);
                $('.console-body').append(data+',');
            },
            error: function (data) {
                var result = false;
            },
        });
        counter++;
        slideStart();
    };
};
////////////////////////////////////////////////////

