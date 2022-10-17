function changeImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imageContainer').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#File").change(function () {
    changeImage(this);
    $('#ImageDiv').show();
});

function deleteImage() {
    const id = $('#Id').val();

    if (id == 0) {
        $('#File').val('')
        $('#ImageDiv').hide();
        return
    };

    const imageName = $('#Image').val();

    if (imageName == undefined || imageName == '' || imageName == null) {
        $('#File').val('')
        $('#ImageDiv').hide();
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
            url: "/Accessories/DeleteImage/" + id,
            success: function () {
                t.value && Swal.fire({
                    title: "Deleted!",
                    text: "Your record has been deleted.",
                    icon: "success",
                    confirmButtonClass: "btn btn-primary w-xs mt-2",
                    buttonsStyling: !1
                }).then(function () {
                    $('#ImageDiv').hide();
                    $('#Image').val('');
                });
            }
        });
    });
}