﻿function showImage(input) {
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

    if ($('#AccesoryFile').length > 0) {
        const files = $('#AccesoryFile').prop('files');
        formData.append("ImageFile", files[0])
    }

    formData.append("Name", $('#AccesoryName').val())
    formData.append("Quantity", $('#AccesoryQuantity').val())
    formData.append("MeasurementType", $('#AccesoryMeasurementType').val())
    formData.append("Price", $('#AccesoryPrice').val())
    formData.append("Description", document.getElementsByClassName('ck ck-content ck-editor__editable ck-rounded-corners ck-editor__editable_inline ck-blurred')[1].innerHTML)

    $.ajax({
        type: "POST",
        url: "/Accessories/AddEditPopup/",
        contentType: false,
        data: formData,
        cache: false,
        processData: false,
        success: function (obj) {
            const dropDown = $('#AccessoryIds');
            if (dropDown.length > 0) {
                dropDown.append(new Option(obj.name, obj.id));
            }

            showToaster("success", "Created", "Accessory has been created.");

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

// ============ Code for Categories AddEdit Popup ==================
function showCategoryImage(input) {
    if (input.files && input.files[0]) {
        const imageDiv = $('#CategoryImageDiv');
        imageDiv.empty();

        const imageHtml = "<img id=\"imageCategoryContainerNew\" src='-' class=\"rounded-squre avatar-xl img-thumbnail shadow\" alt=\"category-image\">";

        imageDiv.html(imageHtml);

        const reader = new FileReader();

        reader.onload = function (e) {
            $('#imageCategoryContainerNew').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);

        $('#CategoryImageDiv').show();
    }
    else {
        $('#CategoryImageDiv').empty();
    }
}

function saveCategories() {
    var formData = new FormData();

    if ($('#CategoryFile').length > 0) {
        const files = $('#CategoryFile').prop('files');
        formData.append("ImageFile", files[0])
    }

    formData.append("Name", $('#CategoryName').val())
    formData.append("HsnNo", $('#CategoryHsnNo').val())

    $.ajax({
        type: "POST",
        url: "/Categories/AddEditPopup/",
        contentType: false,
        data: formData,
        cache: false,
        processData: false,
        success: function (obj) {
            const dropDown = $('#CategoryIds');
            if (dropDown.length > 0) {
                dropDown.append(new Option(obj.name, obj.id));
            }

            showToaster("success", "Created", "Category has been created.");

            $('#showCategoryModal').modal('hide');
        },
        error: function (obj) {
            if (obj.responseJSON) {
                for (let i = 0; i < obj.responseJSON.length; i++) {
                    $('#ValidationCategory' + obj.responseJSON[i].key).text(obj.responseJSON[i].message)
                }
            }
        }
    });
}

function openPopupForCategories() {
    $.ajax({
        type: "GET",
        url: "/partialview/categoryaddpopupview/",
        success: function (html) {
            $("#categoryModalBody").html(html);
            $('#showCategoryModal').modal('show');
        }
    });
}