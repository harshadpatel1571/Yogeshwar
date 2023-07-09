function changeImage(input) {
    if (input.files && input.files[0]) {
        const imageDiv = $('#ImageDiv');
        imageDiv.empty();

        const imageHtml = "<img id=\"imageContainerNew\" src='-' class=\"rounded-squre avatar-xl img-thumbnail shadow\" alt=\"accessories-image\">\n" + "                                <div class=\"avatar-xs p-0 rounded-circle profile-photo-edit\">\n" + "                                    <label for=\"profile-img-file-input\" class=\"profile-photo-edit avatar-xs\" onclick='deleteImage()'>\n" + "                                        <span class=\"avatar-title rounded-circle bg-light text-body shadow\">\n" + "                                            <i class=\"ri-delete-bin-line\"></i>\n" + "                                        </span>\n" + "                                    </label>\n" + "                                </div>";

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
    debugger;
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
            type: "POST", url: "/Customer/DeleteImage/" + id, success: function () {
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

$('#PinCode').keypress(function (e) {
    return $(this).val().length < 6;
});

$('#PhoneNo').keypress(function (e) {
    return $(this).val().length < 10;
});

let customerAddresses = [];

$(document).ready(function () {
    const jsonString = $('#jsonDataHolder').val();

    if (jsonString) customerAddresses = JSON.parse(jsonString)
});

function populateAddress() {
    $.ajax({
        type: "POST",
        url: "/PartialView/CustomerAddressView/",
        data: { customerAddresses: customerAddresses },
        success: function (html) {
            $('#addressDiv').html(html);
            $('#addressModal').modal('hide');
        },
        error: function (error) {
            console.log(error)
        }
    });
}

$('#addressModal').on('hidden.bs.modal', function () {
    $('#addressModal').modal('hide');

    $('#customerAddressForm #Address').val('');
    $('#customerAddressForm #City').val('');
    $('#customerAddressForm #District').val('');
    $('#customerAddressForm #State').val('');
    $('#customerAddressForm #PinCode').val('');
    $('#customerAddressForm #PhoneNo').val('');

    $('#customerAddressForm #Address-error').text('');
    $('#customerAddressForm #City-error').text('');
    $('#customerAddressForm #District-error').text('');
    $('#customerAddressForm #State-error').text('');
    $('#customerAddressForm #PinCode-error').text('');
    $('#customerAddressForm #PhoneNo-error').text('');


    $('#updateAddressDiv').hide();
    $('#addAddressDiv').show();
})


function addAddress() {
    debugger;
    if (!$('#customerAddressForm').valid()) {
        return;
    }

    const formData = $("#customerAddressForm").serialize();
    const modelObj = JSON.parse('{"' + decodeURI(formData.replace(/&/g, "\",\"").replace(/=/g, "\":\"")) + '"}');

    customerAddresses.push(toCamelObj(modelObj));

    populateAddress();
}

function removeAddress(num) {
    customerAddresses.splice(num, 1);

    populateAddress();
}

function openEditPopup(num) {
    debugger;
    const data = customerAddresses[num];

    if (!data) return;

    $('#updateAddressDiv').show();
    $('#addAddressDiv').hide();

    $('#customerAddressForm #Address').val(data.address);
    $('#customerAddressForm #City').val(data.city);
    $('#customerAddressForm #District').val(data.district);
    $('#customerAddressForm #State').val(data.state);
    $('#customerAddressForm #PinCode').val(data.pinCode);
    $('#customerAddressForm #PhoneNo').val(data.phoneNo);

    $('#addressModal').modal('show');

    $('#customerAddressForm #updateListId').val(num);
}

function openAddPopup() {
    $('#updateAddressDiv').hide();
    $('#addAddressDiv').show();
    $('#addressModal').modal('show');
}

function updateAddress() {
    const updateId = parseInt($('#customerAddressForm #updateListId').val());

    const data = customerAddresses[updateId];

    if (!data) return;

    data.address = $('#customerAddressForm #Address').val();
    data.city = $('#customerAddressForm #City').val();
    data.district = $('#customerAddressForm #District').val();
    data.state = $('#customerAddressForm #State').val();
    data.pinCode = $('#customerAddressForm #PinCode').val();
    data.phoneNo = $('#customerAddressForm #PhoneNo').val();

    populateAddress();
}