function ToggleDnone() {
    if ($('#showBtn').text() === 'show comment') {
        $('#showBtn').text('hide comment');
    }
    else {
        $('#showBtn').text('show comment');
    }

    if ($('[name="comment"]').hasClass('d-none')) {
        $('[name="comment"]').removeClass('d-none');
    }
    else {
        $('[name="comment"]').addClass('d-none');
    }
}


function ChangeBG(role, isNull) {
    if (role == 'Instructor') {
        $('#editor1').addClass('bg-white');

        if (isNull === 'True') {
            CKEDITOR.instances.editor1.setData('');
        }
    }
}

function getDate() {
    var date = new Date();
    var day = date.getDate();       // yields date
    var month = date.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
    var year = date.getFullYear();  // yields year
    var hour = date.getHours();     // yields hours
    var minute = date.getMinutes(); // yields minutes
    var second = date.getSeconds(); // yields seconds

    // After this construct a string with the above results as below
    return year + "/" + month + "/" + day + " " + hour + ':' + minute + ':' + second;
}

function AddNote(fullname, cid) {
    var time = getDate();

    Swal.fire({
        text: 'Are you sure to UPDATE this note? ',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            var text = CKEDITOR.instances.editor1.getData();
            var note = JSON.stringify({
                'Text': text,
                'PostDate': time,
                'ProfessorFullName': fullname,
                'FId': cid
            });
            console.log(note);
            // Actual post to controller
            $.ajax({
                url: '/Courses/onPostSubmitNoteAsync',
                data: note,
                contentType: 'application/json',
                type: 'POST',
                success: function (data) {
                    Swal.fire(
                        'Success!',
                        'Your changes has been saved.',
                        'success'
                    );
                    $('#Approval').removeClass();
                    $('#Approval').addClass('badge badge-info d-inline');
                    $('#Approval').html('Pending Approval');
                    console.log(data);
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Failed!',
                        'Somthing went wrong, please try again with less words!',
                        'error'
                    );
                }
            })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then(function (success) {
        if (success.value) {
            $('#LastUpdateTime').html('&nbsp;' + 'on ' + time);
            $('#LastUpdateName').text('Last updated by : ' + fullname);
            $('#editor1').removeClass('bg-white');
        }
    });
}

function ApproveNote(noteId) {
    Swal.fire({
        text: 'Are you sure to APPROVE this note? ',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            // Actual post to controller
            $.ajax({
                url: '/Courses/onPostApproveNoteAsync?CNId=' + noteId,
                contentType: 'application/json',
                type: 'POST',
                success: function (data) {
                    Swal.fire(
                        'Success!',
                        'Your changes has been saved.',
                        'success'
                    );
                    $('#Approval').removeClass();
                    $('#Approval').addClass('badge badge-success d-inline');
                    $('#Approval').html('Approved');
                    $('#ApproveNoteBtn').attr('disabled', true);
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Failed!',
                        'Somthing went wrong, please try again with less words!',
                        'error'
                    );
                }
            })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then(function (success) {
        if (success.value) {
        }
    });
}

function AddComment(editor, commentTime, LId, isProfessor, fullname) {
    Swal.fire({
        text: 'Are you sure to UPDATE this note? ',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            console.log(editor);
            var date = getDate();
            var text = CKEDITOR.instances[editor].getData();
            var note = JSON.stringify({
                'Text': text,
                'PostDate': date,
                'IsProfessor': isProfessor,
                'ProfessorFullName': fullname,
                'FId': LId
            });
            console.log(note);
            // Actual post to controller
            $.ajax({
                url: '/Courses/onPostLOSNoteAsync',
                data: note,
                contentType: 'application/json',
                type: 'POST',
                success: function (data) {
                    Swal.fire(
                        'Success!',
                        'Your changes has been saved.',
                        'success'
                    );
                    console.log(data);
                    if (isProfessor === 'False') {
                        $('#' + editor).addClass('text-red');
                    }
                    else {
                        $('#' + editor).removeClass('text-red');
                    }
                    $('#' + commentTime).text('Last edit: ' + date);
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Failed!',
                        'Somthing went wrong, please try again with less words!',
                        'error'
                    );
                }
            })
        },
        allowOutsideClick: () => !Swal.isLoading()
    });
}

async function Upload(type, id,rate) {
    const { value: file } = await Swal.fire({
        title: 'Select File (pdf or zip)',
        input: 'file',
        inputAttributes: {
            accept: 'application/pdf,application/zip',
            'aria-label': 'Upload your profile picture'
        }
    })

    if (file) {
        console.log(file);
        var fileData = new FormData();
        fileData.append(file.name, file);
        fileData.append("LId", id);
        if (type == 'Example') {
            fileData.append("Rate", rate);
        }
        $.ajax({
            url: '/Courses/OnPostUploadAsync',
            type: 'post',
            processData: false,
            contentType: false,
            data: fileData,
            success: function (response) {
                Swal.fire(
                    'Upload Success',
                    'success'
                );
                $('#Ass' + id).removeClass("isDisabled");
            }
        });
    }
}

async function UploadExample(id) {
    const { value: fruit } = await Swal.fire({
        title: 'Select which example to upload',
        html:
              '<h3> Type </h3>'
            + '<select id="selection" class="swal2-select" style="display: flex;">'
            + '<option value="good">Good</option> '
            + '<option value="average" >Average</option> ' 
            + '<option value="poor">Poor</option> ' 
            + '</select > '
            + '<h3> File (pdf or zip) </h3>'
            + '<input id="fileInput" type="file" accept="application/pdf,application/zip"'
            + 'aria- label="Upload your file" class= "swal2-file" placeholder = "" style = "display: flex;" > ',
        showCancelButton: true,
        preConfirm: () => {
            var file = document.getElementById('fileInput').files[0];
            var fileData = new FormData();
            fileData.append(file.name, file);
            fileData.append("LId", id);
            fileData.append("Rate", document.getElementById('selection').value);

            $.ajax({
                url: '/Courses/OnPostUploadExampleAsync',
                type: 'post',
                processData: false,
                contentType: false,
                data: fileData,
                success: function (response) {
                    Swal.fire(
                        'Upload Success',
                        'success'
                    );
                    $('#Example' + id).removeClass("isDisabled");
                }
            });
        }
    })

}

