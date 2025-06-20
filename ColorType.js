let colorId = 0;

$(document).on('click', '.edit-color', function (e) {
    debugger
    e.preventDefault();
    let id = $(this).data('id');
    let name = $(this).data('name');

    $('#editColorId').val(id);
    $('#editColorName').val(name);
    $('#saveButton').text('Update'); // Optional: change button text
});


$(document).on('click', '.delete-color', function () {
    colorId = $(this).data('id');
    let name = $(this).data('name');
    $('#deleteItemName').text(name);
    $('#deleteModal').modal('show');
});

$('#confirmDeleteBtn').click(function () {
    let token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '/ColorType/Delete',
        type: 'POST',
        data: {
            id: colorId
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