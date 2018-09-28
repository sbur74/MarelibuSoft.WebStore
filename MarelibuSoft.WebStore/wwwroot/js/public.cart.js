if (!marelibusoft) {
	var marelibusoft = {};
}

if (!marelibusoft.cart) {
	marelibusoft.cart = {};
}

marelibusoft.cart.changedItems = [];

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
	var minq = parseFloat(minQuantity.replace(",", "."));
	var fstartQuantity = 0.0;

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
	priceStr += " &euro; <b>*</b>";
	labelPrice.html(priceStr);
}

function postCartLine(itemId, itemUnit, basePrice) {
	var id = itemId;
	var unit = itemUnit;
	var input = $("#" + id + "_quantity");
	var quantityString = input.val();
	quantityString = quantityString.replace(",", ".");
	var bpricestr = basePrice.replace(",", ".");
	var fBasePrice = parseFloat(bpricestr);
	var fquantity = parseFloat(quantityString);
	var apiurl = "/api/ShoppingCartLines";
	console.log("postCartLine -> quantity: " + fquantity);

	var cartID = $("#cartid").html();
	var xxsrf = $("input[name='__RequestVerificationToken']").val();

	if (id && checkIsQuantityAvailable(id, id)) {
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
			success: function (data) {
				//TODO: add message system and refresh of shoppingcart view component
				console.log(data);
				getAvailableQuantity(data);
				$("#cartDisplay").load("/AjaxContent/ShoppingCart");
				marelibusoft.common.showModalAlert("alert", "Erfolgreich angelegt!", "success");
			},
			error: function (data) {
				$("#cartDisplay").load("/AjaxContent/ShoppingCart");
				marelibusoft.common.showModalAlert("alert", "Bei der Anlage ist ein Fehler aufgetreten!", "danger");
				setTimeout(
					location.reload(), 800);
			},
			contentType: "application/json",
			//dataType: 'json',
			timeout: 60000
		});
	} else {
		setTimeout(
			location.reload(), 800);
	}

	input.val(0.0);
}  

function updateCartline(itemId, pos, unit, product, sellPrice, minQuantity) {
	var id = itemId;
	var cartId = $("#cartid").html(); //localStorage.getItem("CARTID");
	var apiurl = "/api/ShoppingCartLines/" + id;
	var quantityString = $("#" + itemId + "_quantity").val();
	var minq = parseFloat(minQuantity.replace(",", "."));
	$("#" + id + "_minus").addClass('disabled');
	$("#" + id + "_plus").addClass('disabled');
	document.getElementById( id +"_loader").style.display = "block";
	
	quantityString = quantityString.replace(",", ".");

	var strprice = sellPrice.replace(",", ".");
	var fprice = parseFloat(strprice);
	var fquantity = parseFloat(quantityString);
	if (fquantity < minq) {
		marelibusoft.common.showModalAlert("alertMessage", "Die Mimdestabnahme " + minQuantity + " " + unit +" darf nicht unterschriten werden!","warning");
		return false;
	}
	var xxsrf = $("input[name='__RequestVerificationToken']").val();
	if (id && checkIsQuantityAvailable(id, product)) {
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
			success: function (data) {
				console.log(data);
				//getAvailableQuantity(data);
				$("#cartDisplay").load("/AjaxContent/ShoppingCart");
				updateTotal();
				location.reload();
			}, error: function (data) {
				$("#cartDisplay").load("/AjaxContent/ShoppingCart");

				marelibusoft.common.showModalAlert("alert", "Bei der Anlage ist ein Fehler aufgetreten!", "danger");

				setTimeout(
					location.reload(), 800);
			},
			contentType: "application/json",
			//dataType : "json",
			timeout: 60000
		});
	} else {
		setTimeout(
			location.reload(), 800);
	}
}


function getAvailableQuantity(data) {
	var postfix = "_availableQuantity";
	var id = data.productid;
	var xxsrf = $("input[name='__RequestVerificationToken']").val();
	var url = "/api/Products/" + id;
	if (id) {
		$.ajax(url, {
			type: 'GET',
			headers: { 'X-XSRF-TOKEN': xxsrf },
			success: function (data) {
				console.log(data);
				var avilavbleQunatityElement = $("#" + id + postfix);
				var avquantity = parseFloat(data.availableQuantity);
				avquantity = avquantity.toFixed(2);
				avquantity = avquantity.replace(".", ",");
				avilavbleQunatityElement.html(avquantity);

				if (data.avquantity < data.minimumPurchaseQuantity) {
					$("#" + id + "_btnCart").addClass('disabled');
				}

				$("#cartDisplay").load("/AjaxContent/ShoppingCart");
			},
			error: function (data) {
				console.log(data);
				marelibusoft.common.showModalAlert("alert", "Verfügbaremege konnte nicht abgerufen werden!", "warning");
				$("#cartDisplay").load("/AjaxContent/ShoppingCart");
				setTimeout(
					location.reload(), 800);
			},
			contentType: "application/json",
			dataType: 'json',
			timeout: 60000
		});
	}
}


function deleteCartLine(itemId) {
	var id = itemId;
	var cartId = $("#cartid").html();
	var apiurl = "/api/ShoppingCartLines/" + id;
	var xxsrf = $("input[name='__RequestVerificationToken']").val();
	document.getElementById(id + "_loader").style.display = "block";
	$.ajax(apiurl, {
		type: 'DELETE',
		headers: { 'X-XSRF-TOKEN': xxsrf },
		data: JSON.stringify({
			shoppingCartID: cartId,
			id: id
		}), success: function (data) {
			console.log("on delete cart line: "+data)
			$("#cartDisplay").load("/AjaxContent/ShoppingCart");
			location.reload();
		},error: function (data) {
			//TODO: show message
			$("#cartDisplay").load("/AjaxContent/ShoppingCart");
			marelibusoft.common.showModalAlert("alert", "Beim löschen ist ein fehler aufgetreten!", "danger");
			location.reload();
		},
		contentType: "application/json",
		dataType: 'json',
		timeout: 60000
	});
}

function creatCart() {
	var id = $("#cartid").html();
	var xxsrf = $("input[name='__RequestVerificationToken']").val();
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
				$("#cartDisplay").load("/AjaxContent/ShoppingCart");
				alert("error on creat cart!");
			},
			contentType: "application/json",
			dataType: "json",
			timeout: 60000
		});
	}
}

function onShipPriceChange() {
	var totalstr = $("#total").html();
	totalstr = totalstr.replace(",", ".");
	var countryPriceStr = $("#countries").val();
	countryPriceStr = countryPriceStr.replace(",", ".");
	var shipPriceStr = $("#shipPrice").html();
	shipPriceStr = shipPriceStr.replace(",", ".");
	var total = parseFloat(totalstr);
	var shipPrice = parseFloat(shipPriceStr);
	var countryPrice = parseFloat(countryPriceStr);

	total = total - shipPrice;
	total = total + countryPrice;

	total = total.toFixed(2);

	totalstr = total.toLocaleString();
	totalstr = totalstr.replace(".", ",");
	shipPrice = shipPrice.toFixed(2);
	shipPriceStr = shipPrice.toLocaleString();
	shipPriceStr = shipPriceStr.replace(".", ",");
	countryPrice = countryPrice.toFixed(2);
	countryPriceStr = countryPrice.toLocaleString();
	countryPriceStr = countryPriceStr.replace(".", ",");
	$("#shipPrice").html(countryPriceStr);
	$("#total").html(totalstr);
	
}

function updateTotal() {
	var totalstr = $("#total").html();
	var etotal = 0.0;
	var shipPriceStr = $("#shipPrice").html();
	shipPriceStr = shipPriceStr.replace(",", ".");
	var shipPrice = parseFloat(shipPriceStr);

	$(".listprice").each(function (index) {
		console.log(index + ": " + $(this).html());
		var listpricestr = $(this).html();
		listpricestr = listpricestr.replace(",", ".");
		var listprice = parseFloat(listpricestr);

		etotal += listprice;
	});

	etotal += shipPrice;
	etotal = etotal.toFixed(2);
	totalstr = etotal.toLocaleString();
	totalstr = totalstr.replace(".", ",");
	$("#total").html(totalstr);
}

/// id kann vom artikel oder von der warenkorbzeile kommen (update)
function checkIsQuantityAvailable(id, productId) {
	var availableQuantityObj = $("#" + productId + "_availableQuantity");
	var quantityObj = $("#" + id + "_quantity");
	var quantityStr = quantityObj.val();
	var availableQuantityStr = availableQuantityObj.html();

	var startQuantity = $("#" + id + "_startQuantity").html();

	quantityStr = quantityStr.replace(",", ".");
	availableQuantityStr = availableQuantityStr.replace(",", ".");

	var quant = parseFloat(quantityStr);
	var avaiQuant = parseFloat(availableQuantityStr);
	if (startQuantity) {
		var staq = parseFloat(startQuantity.replace(",", "."));
		quant -= staq;
	}

	if (quant > avaiQuant) {
		//$("div.failure").html("Die die eingegebene Megene &uuml;bersteigt die Verf&uuml;gbaremenge!").fadeIn(300).delay(1500).fadeOut(400);
		//alert("Die die eingegebene Megene &uuml;bersteigt die Verf&uuml;gbaremenge!");
		marelibusoft.common.showModalAlert("alert", "Die die eingegebene Megene &uuml;bersteigt die verf&uuml;gbare Menge!", "danger");
		return false;
	}
	return true;
}
