function doPost(id, first, last, role) {
    // Get all the classes
    var classList = $(id).attr('class').split(/\s+/);
    var action = 'authorize';
    if (classList.includes('active')) {
        action = 'revoke';
        $(id).removeClass('active');
    }
    else {
        $(id).addClass('active');
    }

    // If there is "active" class before hitting, revoking role action
    Swal.fire({
        html: 'Are you sure to ' + '<strong>' + action.toUpperCase() +
            '</strong>' + ' ' + '<strong>' + role + '</strong>' + ' to: ' +
            '<br>' + '<strong>' +
            first + ' ' + last + ' ?' + '</strong>',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            var myObject = JSON.stringify({
                'First': first,
                'Last': last,
                'Action': action,
                'Role': role
            });
            // Actual post to controller
            $.ajax({
                url: '/Admin/onPostRoleChangeAsync',
                data: myObject,
                contentType: 'application/json',
                type: 'POST',
                success: function (data) {
                    Swal.fire(
                        'Success!',
                        'Your changes has been saved.',
                        'success'
                    );
                    console.log(data);
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Failed!',
                        'Your cannot remove the last admin',
                        'error'
                    );
                    $(id).addClass('active');
                }
            })
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then(function (success) {
        // When Cancel clicked, revert back the active class.
        if (success.value) {
            if (classList.includes('active')) {
                $(id).removeClass('active');
            }
            else {
                $(id).addClass('active');
            }
        }
    });
};
