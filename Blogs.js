$(document).ready(function () {
    // 🔍 Search filter
    $("#search").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#blogsTable tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // 🔘 Select all checkboxes (if used)
    $("#selectAll").on("click", function () {
        $("input[type='checkbox']").prop('checked', this.checked);
    });

    // ✅ Publish toggle via AJAX
    $('.publish-toggle').on('change', function () {
        var checkbox = $(this);
        var blogId = checkbox.data('id');
        var isChecked = checkbox.is(':checked');

        $.ajax({
            url: '/Blogs/TogglePublish',
            type: 'POST',
            data: { id: blogId, published: isChecked },
            success: function () {
                var status = checkbox.closest('td').find('.publish-status');
                if (isChecked) {
                    status.text('Published').removeClass('text-danger').addClass('text-success');
                } else {
                    status.text('Not Published').removeClass('text-success').addClass('text-danger');
                }
            },
            error: function () {
                alert('Failed to update publish status.');
                checkbox.prop('checked', !isChecked); // revert toggle
            }
        });
    });

    // 🗑️ Delete modal: fill blog id and title dynamically
    var confirmModal = document.getElementById('confirmModal');
    if (confirmModal) {
        confirmModal.addEventListener('show.bs.modal', function (event) {
            var button = event.relatedTarget;
            var blogId = button.getAttribute('data-blog-id');
            var blogTitle = button.getAttribute('data-blog-title');

            document.getElementById('deleteBlogId').value = blogId;
            document.getElementById('blogTitle').textContent = blogTitle;
        });
    }
});
