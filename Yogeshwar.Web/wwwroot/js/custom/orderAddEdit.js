let mainHtml;
let quantityDivHtml;
let productImageHtml;
let accessoriesHtml;
let accessoriesImageHtml;
let times = 1;

$(document).ready(function () {
    quantityDivHtml = $('#quantityDiv_0')[0].outerHTML;
    productImageHtml = $('#image_product_0')[0].outerHTML;

    const accessoryImage = $('#accessories_0_accessoriesId_Image');
    accessoriesImageHtml = accessoryImage[0].outerHTML;
    accessoryImage.remove();

    accessoriesHtml = $('#accessoriesStock_0_0')[0].outerHTML;

    removeAssociatedElement(0);
    hideAccessoriesStockLabel(0);
    hideAccessoriesStock(0);
    cleanPrice(0)

    mainHtml = $('#productDiv_0')[0].outerHTML;

    makeDropDownSearchable(0);

    cleanTotalAmount();
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

    if (id === 0) {
        return;
    }

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

function cleanTotalAmount() {
    $('#totalAmount').text('');
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
        showAccessoriesStockLabel(id);
        showAccessoriesStock(id);

        const arr = [];

        arr.push({accessoriesId: times + 200, name: 'Harshad', image: '/images/users/avatar-1.jpg'});
        arr.push({accessoriesId: times + 300, name: 'Vaibhav'});
        arr.push({accessoriesId: times + 400, name: 'Sahil'});
        arr.push({accessoriesId: times + 100, name: 'Soham', image: '/images/users/avatar-1.jpg'});

        const data = {Image: '/images/users/avatar-1.jpg', accessories: arr, amount: 500}

        displayProductImage(id, data.Image);
        displayQuantityDiv(id);

        data.accessories.forEach(function (value) {
            displayAccessories(id, value);
        });

        updatePrice(id, data.amount + " RS")

        updateTotalAmount();
    }
}

function removeAssociatedElement(id) {
    $('#image_product_' + id).remove();
    $('#quantityDiv_' + id).remove();
    $('#accessoriesStockAppender_' + id).empty();
}

function displayProductImage(id, source) {
    $('#imageAppender_' + id).append(productImageHtml.replace('image_product_0', 'image_product_' + id)
        .replace('Product_image_source', source));
}

function displayQuantityDiv(id) {
    $('#quantityAppend_' + id).append(quantityDivHtml
        .replace('quantityDiv_0', 'quantityDiv_' + id)
        .replace('minusButton_0', 'minusButton_' + id)
        .replace('quantity_0', 'quantity_' + id)
        .replace('plusButton_0', 'plusButton_' + id))
}

function displayAccessories(id, obj) {
    const newHtml = accessoriesHtml
        .replace('accessoriesValueAppender_0_0', 'accessoriesValueAppender_' + id + '_' + obj.accessoriesId)
        .replaceAll('accessories_0_accessoriesId_Chk', 'accessories_' + id + '_' + obj.accessoriesId + '_Chk')
        .replace('accessories_0_accessoriesId_ImageAppender', 'accessories_' + id + '_' + obj.accessoriesId + '_ImageAppender')
        .replace('accessories_0_accessoriesId_Label', 'accessories_' + id + '_' + obj.accessoriesId + '_Label')
        .replace('Accessories_Name', obj.name);

    $('#accessoriesStockAppender_' + id).append(newHtml);

    if (obj.image !== undefined) {
        $('#accessories_' + id + '_' + obj.accessoriesId + '_ImageAppender').append(accessoriesImageHtml
            .replace('Image_source', obj.image)
            .replace('accessories_0_accessoriesId_Image', 'accessories_' + id + '_' + obj.accessoriesId + '_Image'))
    }
}

$('#addButton').click(function () {
    const newHtml = mainHtml.replace('productDiv_0', 'productDiv_' + times)
        .replace('quantityAppend_0', 'quantityAppend_' + times)
        .replace('selectAccessories_0', 'selectAccessories_' + times)
        .replace('imageAppender_0', 'imageAppender_' + times)
        .replace('accessoriesStockLabel_0', 'accessoriesStockLabel_' + times)
        .replace('accessoriesStockLI_0', 'accessoriesStockLI_' + times)
        .replace('accessoriesStockAppender_0', 'accessoriesStockAppender_' + times)
        .replace('productPrice_0', 'productPrice_' + times)
        .replace('removeButton_0', 'removeButton_' + times);

    $('#orderDetail').append(newHtml);

    makeDropDownSearchable(times);

    times++;
});