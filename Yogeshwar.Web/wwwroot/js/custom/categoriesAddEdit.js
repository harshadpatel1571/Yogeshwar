function changeImage(input) {
    if (input.files && input.files[0]) {
        const imageDiv = $('#ImageDiv');
        imageDiv.empty();
        
        const imageHtml = "<img id=\"imageContainerNew\" src='-' class=\"rounded-squre avatar-xl img-thumbnail shadow\" alt=\"accessories-image\">\n" +
            "                                <div class=\"avatar-xs p-0 rounded-circle profile-photo-edit\">\n" +
            "                                    <label for=\"profile-img-file-input\" class=\"profile-photo-edit avatar-xs\" onclick='deleteImage()'>\n" +
            "                                        <span class=\"avatar-title rounded-circle bg-light text-body shadow\">\n" +
            "                                            <i class=\"ri-delete-bin-line\"></i>\n" +
            "                                        </span>\n" +
            "                                    </label>\n" +
            "                                </div>";

        imageDiv.append(imageHtml);

        const reader = new FileReader();

        reader.onload = function (e) {
            $('#imageContainerNew').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#ImageFile").change(function () {
    changeImage(this);
    $('#ImageDiv').show();
});

function deleteImage() {
    const id = $('#Id').val();
    const file = $('#ImageFile');

    if (id == 0 || file.val()) {
        file.val('')
        $('#ImageDiv').empty();
        return
    }

    const imageName = $('#Image').val();

    if (imageName == undefined || imageName == '' || imageName == null) {
        file.val('')
        $('#ImageDiv').empty();
        return;
    }

    Swal.fire({
        title: "Are you sure want to delete image from system?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: !0,
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        confirmButtonText: "Yes, delete it!",
        buttonsStyling: !1,
        showCloseButton: !0,
    }).then(function (t) {
        if (!t.isConfirmed) return;
        $.ajax({
            type: "POST",
            url: "/Categories/DeleteImage/" + id,
            success: function () {
                t.value && Swal.fire({
                    title: "Deleted!",
                    text: "Your image has been deleted.",
                    icon: "success",
                    confirmButtonClass: "btn btn-primary w-xs mt-2",
                    buttonsStyling: !1
                }).then(function () {
                    $('#ImageDiv').empty();
                    $('#Image').val('');
                });
            }
        });
    });
}