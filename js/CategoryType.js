
let categoryId = 0;

$(document).on('click', '.edit-category', function (e) {
    debugger
    e.preventDefault();
    let id = $(this).data('id');
    let name = $(this).data('name');

    $('#editCategoryId').val(id);
    $('#editCategoryName').val(name);
    $('#saveButton').text('Update'); // Optional: change button text
});


$(document).on('click', '.delete-category', function () {
    categoryId = $(this).data('id');
    let name = $(this).data('name');
    $('#deleteItemName').text(name);
    $('#deleteModal').modal('show');
});

$('#confirmDeleteBtn').click(function () {
    $.ajax({
        url: '/Master/Delete',
        type: 'POST',
        data: {
            id: categoryId,

        },
        success: function (response) {
            if (response.success) {
                $('#deleteModal').modal('hide');
                location.reload(); // or remove the row using JS
            } else {
                alert("Error deleting");
            }
        },
        error: function () {
            alert("Something went wrong.");
        }
    });
});
