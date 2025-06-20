let materialId = 0;

$(document).on('click', '.edit-material', function (e) {
    debugger
    e.preventDefault();
    let id = $(this).data('id');
    let name = $(this).data('name');

    $('#editMaterialId').val(id);
    $('#editMaterialName').val(name);
    $('#saveButton').text('Update'); // Optional: change button text
});


$(document).on('click', '.delete-material', function () {
    materialId = $(this).data('id');
    let name = $(this).data('name');
    $('#deleteItemName').text(name);
    $('#deleteModal').modal('show');
});

$('#confirmDeleteBtn').click(function () {
    let token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '/Master/DeleteMaterial',
        type: 'POST',
        data: {
            id: materialId
        },
        headers: {
            "RequestVerificationToken": token // ✅ Add token in header
        },
        success: function (response) {
            if (response.success) {
                $('#deleteModal').modal('hide');
                location.reload();
            } else {
                alert("Error deleting: " + response.error); // 👈 show actual error
            }
        },

        error: function () {
            alert("Something went wrong.");
        }
    });
});
