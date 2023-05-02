function displayMenu() {
    const selectedValue = $('#AccessoryIds option:selected');

    if (selectedValue.length < 1) {
        $('#PAdiv').hide();
        $('#PAList').html('');
    }
    else {
        const ids = [];

        selectedValue.each(function (x, y) {
            ids.push(y.value)
        });

        $('#PAList').html(generateHtml(ids));
        $('#PAdiv').show();
    }
}

function generateHtml(ids) {
    let newHtml;

    $.ajax({
        async: false,
        type: "POST",
        url: "/partialview/accessoryquantityview/",
        data: { accessoryIds: ids },
        success: function (dataHtml) {
            newHtml = dataHtml;
        }
    });

    return newHtml;
}

$("#ImageFiles").change(function () {
    displayImage(this);
});


$("#VideoFile").change(function () {
    displayVideo(this);
});


function displayImage(input) {

    for (let i = 0; i < lastLength; i++) {
        $('#newImageDiv_' + i.toString()).remove();
    }

    if (input.files && input.files.length > 0) {
        for (let i = 0; i < input.files.length; i++) {
            const image = " <div id='newImageDiv_id' class=\"profile-user position-relative d-inline-block mx-auto mt-3\">\n" +
                "                            <img src='-' id=\"image_id\" class=\"rounded-squre avatar-xl img-thumbnail shadow\" alt=\"accessories-image\">\n" +
                "                        </div>"

            $('#ImageApped').append(image.replace('image_id', 'newImage' + i.toString())
                .replace('newImageDiv_id', 'newImageDiv_' + i.toString()))

            const reader = new FileReader();

            reader.onload = function (e) {
                $('#newImage' + i.toString()).attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[i]);
        }

        lastLength = input.files.length;
    }
}

function displayVideo(input) {
    if (input.files && input.files[0]) {
        const videoDiv = $('#videoDiv');
        videoDiv.empty();

        const videoHtml = "<video width=\"320\" height=\"240\" id='videoId' controls>\n" +
            "                                        <source src='-' >\n" +
            "                                    </video>\n" +
            "                                    <div class=\"avatar-xs p-0 rounded-circle profile-photo-edit\">\n" +
            "                                        <label for=\"profile-img-file-input\" class=\"profile-photo-edit avatar-xs\" onclick=\"deleteVideo(0)\">\n" +
            "                                            <span class=\"avatar-title rounded-circle bg-light text-body shadow\">\n" +
            "                                                <i class=\"ri-delete-bin-line\"></i>\n" +
            "                                            </span>\n" +
            "                                        </label>\n" +
            "                                    </div>";

        videoDiv.append(videoHtml);

        const video = $('#videoId');
        const fileUrl = URL.createObjectURL(input.files[0]);
        video.attr('src', fileUrl);
    }
}

function isNumberKey(evt, element) {
    const charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8))
        return false;
    else {
        const len = $(element).val().length;
        const index = $(element).val().indexOf('.');
        if (index > 0 && charCode === 46) {
            return false;
        }
        if (index > 0) {
            const number = (len + 1) - index;
            if (number > 3) {
                return false;
            }
        }
    }
    return true;
}

function deleteVideo(id) {
    if (id == 0) {
        const videoDiv = $('#videoDiv');
        videoDiv.empty();
        return;
    }

    Swal.fire({
        title: "Are you sure want to delete video from system?",
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
            url: "/Product/DeleteVideo/" + id,
            success: function () {
                t.value && Swal.fire({
                    title: "Deleted!",
                    text: "Your video has been deleted.",
                    icon: "success",
                    confirmButtonClass: "btn btn-primary w-xs mt-2",
                    buttonsStyling: !1
                }).then(function () {
                    $('#videoDiv').empty();
                });
            }
        });
    });
}

function deleteImage(id) {
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
            url: "/Product/DeleteImage/" + id,
            success: function () {
                t.value && Swal.fire({
                    title: "Deleted!",
                    text: "Your image has been deleted.",
                    icon: "success",
                    confirmButtonClass: "btn btn-primary w-xs mt-2",
                    buttonsStyling: !1
                }).then(function () {
                    $('#existingDiv' + id.toString()).remove();
                });
            }
        });
    });
}

document.addEventListener("keydown", function (event) {
    if (event.shiftKey && event.code === "KeyA") {
        event.preventDefault();
        openPopupForAccessories()
    }

    else if (event.shiftKey && event.code === "KeyC") {
        event.preventDefault();
        openPopupForCategories()
    }
});