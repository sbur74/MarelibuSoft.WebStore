function addQuantity(sender, minQuantity, basePrice) {
	var id = sender;
	var input = $("#" + id + "_quantity");
	var quntityString = input.val();
	quntityString = quntityString.replace(",", "."); //todo: localization
	var quantity = parseFloat(quntityString);
	var minstr = minQuantity.replace(",", ".");
	var minfl = parseFloat(minstr);
	if (minfl < 1.0) {
		quantity += 0.1;
	} else {
		quantity++;
	}

	quantity = quantity.toFixed(2);
	quantity = parseFloat(quantity);
	quantity = quantity.toLocaleString();

	input.val(quantity);
	quantityPrice(sender, basePrice);
}

function subQuantity(sender, minQuantity, basePrice) {
	var id = sender;
	var input = $("#" + id + "_quantity");
	var quntityString = input.val();
	quntityString = quntityString.replace(",", "."); //todo: localization
	var quantity = parseFloat(quntityString);
	var chkq = quantity;
	var minq = parseFloat(minQuantity.replace(",","."));

	if (minq < 1.0) {
		chkq -= 0.1;
	}
	else {
    chkq--;
	}

	if (chkq >= minq) {
		quantity =  chkq;
	} 

	quantity = quantity.toFixed(2);
	quantity = parseFloat(quantity);
	quantity = quantity.toLocaleString();
	
	input.val(quantity);
	quantityPrice(sender, basePrice);
}

function quantityPrice(itemId, basePrice) {
	var qntStr = $("#" + itemId + "_quantity").val();
	var labelPrice = $("#" + itemId + "_quantityPrice");
	qntStr = qntStr.replace(",", ".");
	basePrice = basePrice.replace(",", ".");
	var quantity = parseFloat(qntStr);
	var prBasePrice = parseFloat(basePrice);
	var qntPrice = prBasePrice * quantity;

	var priceStr = qntPrice.toFixed(2);
	qntPrice = parseFloat(priceStr);
	priceStr = qntPrice.toLocaleString();
	priceStr += " &euro;";
	labelPrice.html(priceStr);

}

function postCartLine(itemId, itemUnit, basePrice, path) {
	var id = itemId;
	var unit = itemUnit;
	var input = $("#" + id + "_quantity");
	var quantityString = input.val();
	input.val(0.0);
	quantityString = quantityString.replace(",", ".");
	var bpricestr = basePrice.replace(",", ".");
	var fBasePrice = parseFloat(bpricestr);
	var fquantity = parseFloat(quantityString);
	var apiurl = "/api/ShoppingCartLines";
	console.log("postCartLine -> quantity: " + fquantity);

	var cartID = $("#cartid").html();
	var xxsrf = $("input[name='__RequestVerificationToken']").val();

	$.ajax(apiurl, {
		type: 'POST',
		headers: { 'X-XSRF-TOKEN': xxsrf },
		data: JSON.stringify({
			shoppingCartID: cartID, // localStorage.getItem("CARTID"),
			quantity: fquantity,
			productID: id,
			unitID: itemUnit,
			sellBasePrice: fBasePrice
		}),
		success: function () {
			//TODO: add message system and refresh of shoppingcart view component
			$("#cartDisplay").load("/AjaxContent/ShoppingCart");
		},
		error: function () {
			//TODO: show message
			alert("Anlage hat nicht geklappt! :(");
		},
		contentType: "application/json",
		dataType: 'json'
	});
}  

function updateCartline(itemId, pos, unit, product, sellPrice) {
	var id = itemId;
	var cartId = $("#cartid").html(); //localStorage.getItem("CARTID");
	var apiurl = "/api/ShoppingCartLines/" + id;
	var quantityString = $("#" + itemId + "_quantity").val();
	quantityString = quantityString.replace(",", ".");
	var strprice = sellPrice.replace(",", ".");
	var fprice = parseFloat(strprice);
	var fquantity = parseFloat(quantityString);
	var xxsrf = $("input[name='__RequestVerificationToken']").val();
	$.ajax(apiurl, {
		type: 'PUT',
		headers: { 'X-XSRF-TOKEN': xxsrf },
		data: JSON.stringify({
			id: itemId,
			position: pos,
			unitID: unit,
			productID: product,
			quantity: fquantity,
			shoppingCartID: cartId,
			sellBasePrice: fprice
		}),
		success: function () {
			$("#cartDisplay").load("/AjaxContent/ShoppingCart");
			location.reload();
		}, error: function () {
			//TODO: show message
			alert("irgendetwas ist schief gelaufen! :(");
		},
		contentType: "application/json",
		dataType: 'json'
	});
}

function deleteCartLine(itemId) {
	var id = itemId;
	var cartId = $("#cartid").html();
	var apiurl = "/api/ShoppingCartLines/" + id;
	var xxsrf = $("input[name='__RequestVerificationToken']").val();
	$.ajax(apiurl, {
		type: 'DELETE',
		headers: { 'X-XSRF-TOKEN': xxsrf },
		data: JSON.stringify({
			shoppingCartID: cartId,
			id: id
		}), success: function () {
			$("#cartDisplay").load("/AjaxContent/ShoppingCart");
			location.reload();
		},error: function () {
			//TODO: show message
			alert("löschen ist böse gewesen! :(");
		},
		contentType: "application/json",
		dataType: 'json'
	});
}

function creatCart() {
	var id = $("#cartid").html();

	if (!id) {
		var url = "/api/ShoppingCarts";
		$.ajax(url, {
			type: 'POST',
			data: JSON.stringify({
				shoppingCartID: ''
			}), success: function (response) {
				var json = JSON.parse(response);
				var id = $("#cartid").html(json.shoppingCartID);
				$("#cartCount").html(0);
			}, error: function () {
				alert("error on creat cart!");
			},
			contentType: "application/json",
			dataType :"json"
		});
	}
}