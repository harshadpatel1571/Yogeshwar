function showImage(input) {
    if (input.files && input.files[0]) {
        const imageDiv = $('#AccesoryImageDiv');
        imageDiv.empty();

        const imageHtml = "<img id=\"imageAccessoryContainerNew\" src='-' class=\"rounded-squre avatar-xl img-thumbnail shadow\" alt=\"accessories-image\">";

        imageDiv.html(imageHtml);

        const reader = new FileReader();

        reader.onload = function (e) {
            $('#imageAccessoryContainerNew').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);

        $('#AccesoryImageDiv').show();
    }
    else {
        $('#AccesoryImageDiv').empty();
    }
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
            const dropDown = $('#Accessories');
            if (dropDown.length > 0) {
                dropDown.append(new Option(obj.name, obj.id));
                dataAccessories.push({ key: obj.id, name: obj.name })
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
        url: "/partialview/accessoriesaddpopupview/",
        success: function (html) {
            $("#accessoryModalBody").html(html);
            ckEditorInit("#accessoryModalBody");
            $('#showAccessoryModal').modal('show');
        }
    });
}