let mainHtml;
let quantityDivHtml;
let deliveredDateDivHtml;
let orderStatusDivHtml;
let productImageHtml;
let accessoriesHtml;
let accessoriesImageHtml;

let times = 1;
let productCount = 1;

const accessoriesKeyValuePair = [];

$(document).ready(function () {
    quantityDivHtml = $('#quantityDiv_0').html();
    productImageHtml = $('#image_product_0')[0].outerHTML;

    deliveredDateDivHtml = $('#deliveredDateDiv_0').html();
    orderStatusDivHtml = $('#orderStatusDiv_0').html();

    const accessoryImage = $('#accessories_0_accessoriesId_Image');
    accessoriesImageHtml = accessoryImage[0].outerHTML;
    accessoryImage.remove();

    accessoriesHtml = $('#accessoriesStock_0_0')[0].outerHTML;

    removeAssociatedElement(0);
    hideAccessoriesStockLabel(0);
    hideAccessoriesStock(0);

    mainHtml = $('#productDiv_0')[0].outerHTML;

    makeDropDownSearchable(0);

    updateTotalAmount();
});

function minusQuantity(obj) {
    const id = parseInt(obj.id.substring(12, obj.id.length));

    const quantityElement = $('#quantity_' + id);

    const quantity = parseInt(quantityElement.val());

    if (quantity === 1) {
        return;
    }

    quantityElement.val(quantity - 1);
}

function plusQuantity(obj) {
    const id = parseInt(obj.id.substring(11, obj.id.length));

    const quantityElement = $('#quantity_' + id);

    const quantity = parseInt(quantityElement.val());

    if (quantity >= 100) {
        return;
    }

    quantityElement.val(quantity + 1);
}

function removeModule(obj) {
    const id = parseInt(obj.id.substring(13, obj.id.length));

    if (productCount <= 1) {
        return;
    }

    productCount--;
    $('#productDiv_' + id).remove();

    updateTotalAmount();
}

function makeDropDownSearchable(number) {
    $("#selectAccessories_" + number).select2();
}

function hideAccessoriesStock(id) {
    $('#accessoriesStockLI_' + id).hide();
}

function cleanPrice(id) {
    $('#productPrice_' + id).text('');
}

function hideAccessoriesStockLabel(id) {
    $('#accessoriesStockLabel_' + id).hide();
}

function showAccessoriesStock(id) {
    $('#accessoriesStockLI_' + id).show();
}

function showAccessoriesStockLabel(id) {
    $('#accessoriesStockLabel_' + id).show();
}

function updatePrice(id, amount) {
    $('#productPrice_' + id).text(amount);
}

function updateTotalAmount() {
    let amount = 0;

    for (let i = 0; i < times; i++) {
        const priceStr = $('#productPrice_' + i).text().split(' ')[0];
        const val = parseInt(priceStr);

        if (!(val === undefined || val === null || isNaN(val))) {
            amount += val;
        }
    }

    $('#totalAmount').text(amount + " RS");
}

function reBindProduct(obj) {
    const val = $('#' + obj.id).val();

    const id = parseInt(obj.id.substring(18, obj.id.length));

    if (val === undefined || val == null || val === '') {
        hideAccessoriesStockLabel(id);
        hideAccessoriesStock(id);
        removeAssociatedElement(id);

        cleanPrice(id);

        updateTotalAmount();
    } else {
        removeAssociatedElement(id);

        $.ajax({
            type: "POST",
            url: "/Order/GetAccessoriesDetail?productId=" + $('#selectAccessories_' + id).val(),
            success: function (data) {
                showAccessoriesStockLabel(id);
                showAccessoriesStock(id);

                displayProductImage(id, data.image);
                displayQuantityDiv(id);

                displayDeliveredDate(id);
                displayOrderStatus(id);

                data.accessories.forEach(function (value) {
                    displayAccessories(id, value);
                });

                updatePrice(id, data.amount + " RS")

                updateTotalAmount();
            },
            error: function () {
                hideAccessoriesStockLabel(id);
                hideAccessoriesStock(id);
                removeAssociatedElement(id);

                cleanPrice(id);

                updateTotalAmount();
            }
        });
    }
}

function removeAssociatedElement(id) {
    $('#image_product_' + id).remove();
    $('#quantityDiv_' + id).empty();
    $('#accessoriesStockAppender_' + id).empty();
    $('#deliveredDateDiv_' + id).empty();
    $('#orderStatusDiv_' + id).empty();
}

function displayProductImage(id, source) {
    if (source !== undefined)
        $('#imageAppender_' + id).append(productImageHtml.replace('image_product_0', 'image_product_' + id)
            .replace('Product_image_source', source));
}

function displayQuantityDiv(id) {
    $('#quantityDiv_' + id).append(quantityDivHtml
        .replace('minusButton_0', 'minusButton_' + id)
        .replace('quantity_0', 'quantity_' + id)
        .replace('plusButton_0', 'plusButton_' + id))
}

function displayDeliveredDate(id) {
    $('#deliveredDateDiv_' + id).append(deliveredDateDivHtml.replace('deliverDate_0', 'deliverDate_' + id));
}

function displayOrderStatus(id) {
    $('#orderStatusDiv_' + id).append(orderStatusDivHtml.replace('orderStatus_0', 'orderStatus_' + id));
}

function displayAccessories(id, obj) {

    accessoriesKeyValuePair.push({ id: id, accessoryId: obj.id, value: 'accessories_' + id + '_' + obj.id + '_Chk' });

    const newHtml = accessoriesHtml
        .replace('accessoriesStock_0_0', 'accessoriesStock_' + id + '_' + obj.id)
        .replace('accessoriesValueAppender_0_0', 'accessoriesValueAppender_' + id + '_' + obj.id)
        .replaceAll('accessories_0_accessoriesId_Chk', 'accessories_' + id + '_' + obj.id + '_Chk')
        .replace('accessories_0_accessoriesId_ImageAppender', 'accessories_' + id + '_' + obj.id + '_ImageAppender')
        .replace('accessories_0_accessoriesId_Label', 'accessories_' + id + '_' + obj.id + '_Label')
        .replace('Accessories_Name', obj.name);

    $('#accessoriesStockAppender_' + id).append(newHtml);

    if (obj.image !== undefined) {
        $('#accessories_' + id + '_' + obj.id + '_ImageAppender').append(accessoriesImageHtml
            .replace('Image_source', obj.image)
            .replace('accessories_0_accessoriesId_Image', 'accessories_' + id + '_' + obj.id + '_Image'))
    }
}

$('#addButton').click(function () {
    const newHtml = mainHtml.replace('productDiv_0', 'productDiv_' + times)
        .replace('quantityAppend_0', 'quantityAppend_' + times)
        .replace('selectAccessories_0', 'selectAccessories_' + times)
        .replace('imageAppender_0', 'imageAppender_' + times)
        .replace('deliveredDateDiv_0', 'deliveredDateDiv_' + times)
        .replace('orderStatusDiv_0', 'orderStatusDiv_' + times)
        .replace('quantityDiv_0', 'quantityDiv_' + times)
        .replace('accessoriesStockLabel_0', 'accessoriesStockLabel_' + times)
        .replace('accessoriesStockLI_0', 'accessoriesStockLI_' + times)
        .replace('accessoriesStockAppender_0', 'accessoriesStockAppender_' + times)
        .replace('productPrice_0', 'productPrice_' + times)
        .replace('removeButton_0', 'removeButton_' + times);

    $('#orderDetail').append(newHtml);

    makeDropDownSearchable(times);

    times++;
    productCount++;
});

function submit() {
    const customer = $('#customer').val();

    const orderDate = $('#orderDate');
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    const splitedValues = orderDate.val().split('-');
    orderDateFormatted = splitedValues[0] + '-' + (months.indexOf(splitedValues[1]) + 1) + '-' + splitedValues[2];

    const discount = $('#discount').val();

    const status = $('#status').val();

    const orderDetails = [];

    for (let i = 0; i < times; i++) {
        let product = $('#selectAccessories_' + i);

        if (product.length < 1) {
            continue;
        }

        product = $('#selectAccessories_' + i).val();

        const deliveredDate = $('#deliverDate_' + i).val();

        const orderStatus = $('#orderStatus_' + i).val();

        const quantity = $('#quantity_' + i).val();

        const accessories = [];

        for (let x = 0; x < accessoriesKeyValuePair.length; x++) {
            if (accessoriesKeyValuePair[x].id === i) {
                const isChecked = $('#' + accessoriesKeyValuePair[x].value).prop('checked');
                accessories.push({ isSelected: isChecked, accessoryId: accessoriesKeyValuePair[x].accessoryId })
            }
        }

        orderDetails.push({ productId: parseInt(product), orderStatus: parseInt(orderStatus), quantity: parseInt(quantity), deliveredDate, accessories });
    }

    const obj = { customerId: parseInt(customer), discount: parseInt(discount), orderDate: orderDateFormatted, status: parseInt(status) , orderDetails };

    create(obj);
}

function create(obj) {
    //$.ajax({
    //    type: "POST",
    //    url: "/Order/AddEdit",
    //    data: obj,
    //    success: function () {
    //        window.location.href = window.location.hostname;
    //    }
    //});

    console.log(obj);
}