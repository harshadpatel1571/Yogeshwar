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

$("#AccesoryFile").change(function () {
    changeImage(this);
    $('#AccesoryImageDiv').show();
});

function deleteImage() {
    const id = $('#AccesoryId').val();
    const file = $('#AccesoryFile');

    if (id == 0 || file.val()) {
        file.val('')
        $('#AccesoryImageDiv').empty();
        return
    }

    //const imageName = $('#AccesoryImage').val();

    //if (imageName == undefined || imageName == '' || imageName == null) {
    //    file.val('')
    //    $('#AccesoryImageDiv').empty();
    //    return;
    //}

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

function saveAccessories() {

    var formData = new FormData();

    const file = $('#AccesoryFile')[0].files;

    if (file.length > 0) {
        formData.append("File", file[0])
    }

    formData.append("Name", $('#AccesoryName').val())
    formData.append("Quantity", $('#AccesoryQuantity').val())
    formData.append("Description", document.getElementsByClassName('ck ck-content ck-editor__editable ck-rounded-corners ck-editor__editable_inline ck-blurred')[1].innerHTML)

    $.ajax({
        type: "POST",
        url: "/Accessories/AddEditPopup/",
        contentType: false,
        data: formData,
        cache: false,
        processData: false,
        success: function (obj) {
            const dropDown = $('.select2-results');
            if (dropDown.length > 0) {
                dropDown.append(`<li class="select2-results__option select2-results__option--selectable select2-results__option--selected" id="select2-Accessories-result-q6av-${obj.id}" role="option" data-select2-id="select2-data-select2-Accessories-result-q6av-${obj.id}" aria-selected="false">${obj.name}</li>`)
            }

            showToaster("success", "Created", "Accessory has been Created.");

            $('#showAccessoryModal').modal('hide');
        },
        error: function (obj) {
            if (obj.responseJSON) {
                for (let i = 0; i < obj.responseJSON.length; i++) {
                    $('#ValidationAccesory' + obj.responseJSON[i].key).text(obj.responseJSON[i].message)
                }
            }
        }
    });
}

function openPopupForAccessories() {
    $.ajax({
        type: "GET",
        url: "/accessories/foo/",
        success: function (html) {
            $("#accessoryModalBody").html(html);
            ckEditorInit("#accessoryModalBody");
            $('#showAccessoryModal').modal('show');
        }
    });
}