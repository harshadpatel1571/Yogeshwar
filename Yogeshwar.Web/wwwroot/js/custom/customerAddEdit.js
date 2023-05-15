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
        data: {customerAddresses: customerAddresses},
        success: function (html) {
            $('#addressDiv').html(html);
            closePopup();
        },
        error: function (error) {
            console.log(error)
        }
    });
}

function closePopup() {
    $('#addressModal').modal('hide');

    $('#fullAddress').val('')
    $('#city').val('')
    $('#district').val('')
    $('#state').val('')
    $('#pincode').val('')
    $('#phoneNo').val('')

    $('#ValidationFullAddress').text('')
    $('#ValidationCity').text('')
    $('#ValidationDistrict').text('')
    $('#ValidationState').text('')
    $('#ValidationPincode').text('')
    $('#ValidationPhoneNo').text('')

    $('#updateAddressDiv').hide();
    $('#addAddressDiv').show();
}

function validateAddress() {
    let isOk = true;

    const address = $('#fullAddress').val().trim();
    if (address.length < 10 || address.length > 250) {
        $('#ValidationFullAddress').text('Address must be 10 to 250 character long.')
        isOk = false;
    }

    const city = $('#city').val().trim();
    if (city.length < 3 || city.length > 25) {
        $('#ValidationCity').text('City must be 3 to 50 character long.')
        isOk = false;
    }

    const district = $('#district').val().trim();
    if (city.length < 3 || city.length > 25) {
        $('#ValidationDistrict').text('District must be 3 to 50 character long.')
        isOk = false;
    }

    const state = $('#state').val().trim();
    if (city.length < 3 || city.length > 25) {
        $('#ValidationState').text('State must be 3 to 50 character long.')
        isOk = false;
    }

    const pinCode = $('#pincode').val().trim();
    if (city.length < 3 || city.length > 25) {
        $('#ValidationPincode').text('Pincode must be 3 to 50 character long.')
        isOk = false;
    }

    const phoneNo = $('#phoneNo').val().trim();
    if (phoneNo.length < 10 || phoneNo.length > 13) {
        $('#ValidationPhoneNo').text('Phone no must be 10 to 13 character long.')
        isOk = false;
    }

    if (!isOk) {
        return {isOk};
    }

    const customerId = $('#Id').val();

    const customerAddress = {
        id: 0, customerId: customerId ? parseInt(customerId) : 0, city, district, state, address, pinCode, phoneNo,
    };

    return {
        isOk, customerAddress
    }
}

function addAddress() {
    const status = validateAddress();

    if (!status.isOk) {
        return;
    }

    customerAddresses.push(status.customerAddress);

    populateAddress();
}

function removeAddress(num) {
    customerAddresses.splice(num, 1);

    populateAddress();
}

function openEditPopup(num) {
    const data = customerAddresses[num];

    if (!data) return;

    $('#updateAddressDiv').show();
    $('#addAddressDiv').hide();

    $('#fullAddress').val(data.address)
    $('#city').val(data.city)
    $('#district').val(data.district)
    $('#state').val(data.state)
    $('#pincode').val(data.pinCode)
    $('#phoneNo').val(data.phoneNo)

    $('#addressModal').modal('show');

    $('#updateListId').val(num)
}

function updateAddress() {
    const updateId = parseInt($('#updateListId').val());

    const data = customerAddresses[updateId];

    if (!data) return;

    data.address = $('#fullAddress').val();
    data.city = $('#city').val();
    data.district = $('#district').val();
    data.state = $('#state').val();
    data.pinCode = $('#pincode').val();
    data.phoneNo = $('#phoneNo').val();

    populateAddress();
}