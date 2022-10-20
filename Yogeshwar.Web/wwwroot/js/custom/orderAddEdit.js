let mainHtml;
let quantityDivHtml
let times = 1;
const addOrNot = [];

$(document).ready(function () {
	mainHtml = $('#productDiv_0')[0].outerHTML;
	quantityDivHtml = $('#quantityDiv_0')[0].outerHTML;
	makeDropDownSerachable(0);
});

function minusQuantity(obj) {
	const id = parseInt(obj.id.substring(12, obj.id.length));

	const quantity = parseInt($('#quantity_' + id).val());

	if (quantity == 1) {
		return;
	}

	$('#quantity_' + id).val(quantity - 1);
}

function plusQuantity(obj) {
	const id = parseInt(obj.id.substring(11, obj.id.length));

	const quantity = parseInt($('#quantity_' + id).val());

	if (quantity >= 100) {
		return;
	}

	$('#quantity_' + id).val(quantity + 1);
}

function removeModule(obj) {
	const id = parseInt(obj.id.substring(13, obj.id.length));

	if (id == 0) {
		return;
	}

	$('#productDiv_' + id).remove();
}

$('#addButton').click(function () {
	const newHtml = mainHtml.replace('productDiv_0', 'productDiv_' + times)
		.replace('quantityAppend_0', 'quantityAppend_' + times)
		.replace('quantityDiv_0', 'quantityDiv_' + times)
		.replace('minusButton_0', 'minusButton_' + times)
		.replace('quantity_0', 'quantity_' + times)
		.replace('plusButton_0', 'plusButton_' + times)
		.replace('selectAccessories_0', 'selectAccessories_' + times)
		.replace('image_0', 'image_' + times)
		.replace('quantity_0', 'quantity_' + times)
		.replace('accessioriesStock_0', 'accessioriesStock_' + times)
		.replace('removeButton_0', 'removeButton_' + times);

	$('#orderDetail').append(newHtml);

	makeDropDownSerachable(times);

	times++;
});

function makeDropDownSerachable(number) {
	$("#selectAccessories_" + number).select2();
}

function reBindProduct(obj) {
	const val = $('#' + obj.id).val();

	var id = parseInt(obj.id.substring(18, obj.id.length));

	if (val == undefined || val == null || val == '') {
		$('#image_' + id).hide();
		removeQuantityDiv(id);
		$('#accessioriesStock_' + id).hide();
	}
	else {
		$('#image_' + id).show();
		addQauntityDivIfRemoved(id);
		$('#accessioriesStock_' + id).show();
	}
}

function removeQuantityDiv(id) {
	$('#quantityDiv_' + id).remove();
	addOrNot.push(id);
}

function addQauntityDivIfRemoved(id) {
	if (!addOrNot.includes(id)) {
		return;
	}

	$('#quantityAppend_' + id).append(quantityDivHtml
		.replace('quantityDiv_0', 'quantityDiv_' + id)
		.replace('minusButton_0', 'minusButton_' + id)
		.replace('quantity_0', 'quantity_' + id)
		.replace('plusButton_0', 'plusButton_' + id))

	const index = addOrNot.indexOf(id);
	addOrNot.splice(index, 1);
}
