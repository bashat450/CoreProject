let subCategoryIdToDelete = 0;

$(document).ready(function () {

    // EDIT logic
    $(document).on('click', '.edit-Subcategory', function (e) {
        e.preventDefault();

        let id = $(this).data('id');
        let name = $(this).data('name');
        let categoryId = $(this).data('categoryid');

        // Set form values
        $('#editCategoryId').val(id);
        $('#editCategoryName').val(name);

        // Select the option in dropdown that matches the categoryId
        $('#categoryDropdown').val(categoryId).trigger('change');

        // Change button text
        $('#saveButton').text('Update');
    });


   

    $(document).ready(function () {
        // ✅ Show delete confirmation modal
        $(document).on('click', '.delete-category', function (e) {
            e.preventDefault();
            subCategoryIdToDelete = $(this).data('id');
            const name = $(this).data('name');

            $('#deleteItemName').text(name);
            $('#deleteModal').modal('show');
        });

        // ✅ Confirm delete AJAX
        $('#confirmDeleteBtn').click(function () {
            $.ajax({
                url: '/Master/DeleteSubCategory',
                type: 'POST',
                data: {
                    id: subCategoryIdToDelete
                },
                success: function (response) {
                    if (response.success) {
                        $('#deleteModal').modal('hide');

                        // Remove the row without reload
                        $('a.delete-category[data-id="' + subCategoryIdToDelete + '"]').closest('tr').remove();

                        // Reset ID
                        subCategoryIdToDelete = 0;
                    } else {
                        alert(response.message || "Error deleting sub-category.");
                    }
                },
                error: function () {
                    alert("Something went wrong while deleting.");
                }
            });
        });
    });


    // DELETE: Show modal & store ID
   
});
