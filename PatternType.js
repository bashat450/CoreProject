let patternId = 0;

$(document).on('click', '.edit-pattern', function (e) {
    debugger
    e.preventDefault();
    let id = $(this).data('id');
    let name = $(this).data('name');

    $('#editPatternId').val(id);
    $('#editPatternName').val(name);
    $('#saveButton').text('Update'); // Optional: change button text
});


$(document).on('click', '.delete-pattern', function () {
    patternId = $(this).data('id');
    let name = $(this).data('name');
    $('#deleteItemName').text(name);
    $('#deleteModal').modal('show');
});

$('#confirmDeleteBtn').click(function () {
    let token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '/Master/DeletePattern',
        type: 'POST',
        data: {
            id: patternId
        },
        headers: {
            "RequestVerificationToken": token // ✅ Add token in header
        },
        success: function (response) {
            if (response.success) {
                $('#deleteModal').modal('hide');
                location.reload();
            } else {
                alert("Error deleting");
            }
        },
        error: function () {
            alert("Something went wrong.");
        }
    });
});