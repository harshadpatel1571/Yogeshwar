const list = [];
const quantities = [];
const dataAccessories = [];
let time = 0;

const html = "<li class=\"list-group-item\" id=\"replace_id\">\n" +
    "                            <div class=\"d-flex align-items-center\">\n" +
    "                                <div class=\"flex-shrink-0\">\n" +
    "                                    <input type=\"hidden\" value=\"AccessoriesValue\" name=\"AccessoriesQuantity.AccessoriesId\" />\n" +
    "                                </div>\n" +
    "                                <div class=\"flex-grow-1 ms-2\">\n" +
    "                                    replace_name\n" +
    "                                </div>\n" +
    "                                <div class=\"flex-grow-2 ms-2\" style='position:relative;right:40%;'>\n" +
    "                                    <input class=\"form-control\" value='quantity_value' name='AccessoriesQuantity.Quantity' required type=\"number\" />\n" +
    "                                </div>\n" +
    "                            </div>\n" +
    "                        </li>";

$(document).ready(function () {
    const url = window.location.pathname.split('/');

    if (isEdit(url)) {
        $.ajax({
            url: '/Product/BindQuantity/' + url[url.length - 1],
            type: 'POST',
            success: function (response) {
                for (let i = 0; i < response.length; i++) {
                    quantities.push({ key: response[i].key, value: response[i].value });
                }

                displayMenu(true);
            }
        });
    } else {
        displayMenu(false);
    }
})

function isEdit(url) {
    return ($.isNumeric(url[url.length - 1]));
}

function displayMenu(edit) {
    const selectedValue = $('#Accessories option:selected');

    // If nothing is selected then hide the element and clear the list.
    if (selectedValue.length === 0) {
        $('#PAdiv').hide();

        removeElementAndClearList();

        return;
    } else {
        // If something is selected then show element.
        $('#PAdiv').show();
    }

    // If it is fist time, then add element to list and html.
    if (time === 0) {

        const allValue = $('#Accessories option');

        allValue.each(function (x, y) {
            dataAccessories.push({ key: y.value, name: y.text })
        });

        selectedValue.each(function (x, y) {
            list.push(y.text);
            $('#PAList').append(generateHtml(edit, y.text))
        })

        time++;

        return;
    }

    selectedValue.each(function (x, y) {
        // If not contains then add to html.
        if (!list.includes(y.text)) {
            $('#PAList').append(generateHtml(edit, y.text))
        } else {
            // If list contains element then remove it.
            const index = list.indexOf(y.text);
            if (index > -1) {
                list.splice(index, 1);
            }
        }
    })

    removeElementAndClearList();

    // Add new coming value to list.
    selectedValue.each(function (x, y) {
        list.push(y.text);
    })
}

function removeElementAndClearList() {
    // If any element is remaining, then remove.    
    for (let i = 0; i < list.length; i++) {
        $('#' + list[i].split(' ').join('')).remove();
    }

    // Empty list if there is any element.
    if (list.length > 0)
        list.splice(0, list.length);
}

function generateHtml(edit, text) {
    let newHtml = html.replace('replace_name', text)
        .replace('AccessoriesValue', dataAccessories.find(x => x.name === text).key);

    if (edit) {
        const obj = quantities.find(x => x.key === text);

        if (obj != undefined || obj != null) {
            newHtml = newHtml.replace('quantity_value', obj.value)
        } else {
            newHtml = newHtml.replace('quantity_value', '')
        }
    } else {
        newHtml = newHtml.replace('quantity_value', '')
    }

    return newHtml.replace('replace_id', text.split(' ').join(''));
}

$("#ImageFiles").change(function () {
    displayImage(this);
});


$("#VideoFile").change(function () {
    displayVideo(this);
});


let lastLength = 0

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
    if (event.altKey && event.shiftKey && event.code === "KeyC") {
        alert('Alt + Shift + C pressed!');
        event.preventDefault();
    }
});