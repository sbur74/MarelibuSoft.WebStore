if (!marelibusoft) {
    var marelibusoft = {};
}

if (!marelibusoft.shop) {
    marelibusoft.shop = {};
}

marelibusoft.shop.lastPriceChangeVariantId = 0;

marelibusoft.shop.checkOption = function (variantId, productId) {
    var selectControl = $("#variantSelect_" + variantId);
    var quantity = marelibusoft.common.str2Float(selectControl.find(":selected").attr("data-option-quantity"));
    var price = marelibusoft.common.str2Float(selectControl.find(":selected").attr("data-option-price"));

    if (quantity > 0.0) {
        $("#variantQuantity_" + variantId).html(quantity);
    }

    if (marelibusoft.shop.lastPriceChangeVariantId == variantId) {
        $("#optionPrice").empty();
        $("#optionPrice").remove();
        $("#" + productId + "_quantityPrice").html($("#grundPreis").html());
    }

    if (price > 0.0) {
        marelibusoft.shop.lastPriceChangeVariantId = variantId;
        var startpreis = marelibusoft.common.str2Float($("#grundPreis").html());
        $("#optionPrice").empty();
        $("#optionPrice").remove();
        $("#optionPriceTableBody").append(
            "<tr id='optionPrice'><td> + </td><td>" + marelibusoft.common.float2LocalCurrencyString(price) + "</td></tr>");
        var gesamtPreis = startpreis + price;
        var gvalue = marelibusoft.common.float2LocalCurrencyString(gesamtPreis);
        $("#" + productId + "_quantityPrice").html(gvalue);
    }
};

marelibusoft.shop.sendCartLine = function (itemId, itemUnit, basePrice) {
    var cartID = $("#cartid").html();
    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    var id = itemId;
    var fBasePrice = marelibusoft.common.str2Float(basePrice);
    var sellactitid = $("#" + id + "_sellActionItemId").val();
    var fquantity = marelibusoft.common.str2Float($("#" + id + "_quantity").val());
    var apiurl = "/api/ShoppingCartLines/variant";

    var textInput = $("#textOptionInput");

    var cartLine = {
        shoppingCartID: cartID,
        quantity: fquantity,
        productID: id,
        unitID: itemUnit,
        sellBasePrice: fBasePrice,
        sellActionItemId: sellactitid,
        shoppingCartLineTextOptions: [],
        variantValues: []
    };

    if (textInput.length > 0) {
        if (!marelibusoft.shop.checkTextVariant()) {
            return;
        }
        var str = textInput.val();
        txtOption = { "text": str };
        cartLine.shoppingCartLineTextOptions.push(txtOption);
        cartLine["textVariant"] = str;
    }

    var variantoptions = $("#variants");
    if (variantoptions) {
        var selecets = variantoptions.find("select");
        for (var i = 0; i < selecets.length; i++) {
            var elementId = selecets[i].getAttribute("id");
            var selecetbox = $("#" + elementId);
            var productVariant = selecetbox.attr("data-variant-id");
            var price = selecetbox.find(":selected").attr("data-option-price");
            var productVariantOption = selecetbox.find(":selected").attr("data-option-id");
            var value = selecetbox.find(":selected").val();
            if (productVariantOption) {
                var variant = {
                    "productVariant": productVariant,
                    "productVariantOption": productVariantOption,
                    "price": price,
                    "value": value,
                    "quantity": fquantity
                };
                cartLine.variantValues.push(variant);
            }
        }
    }

    var available = false;
    for (var i = 0; i < cartLine.variantValues.length; i++) {
        var varvalue = cartLine.variantValues[i];
        available = marelibusoft.shop.checkAvailableQuantity(
            varvalue.productVariantOption,
            varvalue.quantity
        );
    }

    if (available) {
        $.ajax(apiurl, {
            type: 'POST',
            headers: { 'X-XSRF-TOKEN': xxsrf },
            data: JSON.stringify(cartLine),
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
    }   
};

marelibusoft.shop.checkAvailableQuantity = function (optionId, qunatity) {
    var availableQuantity = $("#optionId_" + optionId).data("option-quantity");
    availableQuantity = marelibusoft.common.str2Float(availableQuantity);
    var unit = $("#description").data("base-unit");
    if (qunatity > availableQuantity) {
        marelibusoft.common.showModalAlert(optionId, "Menge nicht verfügbar! Maximal " + availableQuantity + " " + unit + " verfügbar!", "danger");
        return false;
    }
    return true;
};

marelibusoft.shop.checkTextVariant = function () {
    var txtinputelement = $("#textOptionInput");
    if (txtinputelement.length > 0) {
        var intputtext = txtinputelement.val();
        if (intputtext.length == 0) {
            marelibusoft.shop.showTextInputError();
            return false;
        }
        var split = intputtext.split(" ");
        if (split.length > 1) {
            for (var i = 0; i < split.length; i++) {
                if (split[i].length > 0) {
                    marelibusoft.shop.hideTextInputError();
                    return true;
                }
            }
            marelibusoft.shop.showTextInputError();
            return false;
        }
        marelibusoft.shop.hideTextInputError();
        return true;
    }
};

marelibusoft.shop.showTextInputError = function () {
    marelibusoft.common.showModalAlert("textOptionInput", "Sie müssen einen Text eingeben!", "danger");
    $("#textVariantValidate").append("Sie müssen einen Text eingeben!");
    $("#textOptionInput").addClass("danger");
};

marelibusoft.shop.hideTextInputError = function () {
    $("#textVariantValidate").hide();
    $("#textOptionInput").removeClass("danger");
}; 

marelibusoft.shop.countText = function () {
    var text = $("#textOptionInput").val();

    if (text.length > 16) {
        marelibusoft.common.showModalAlert("myalert", "Sie können nicht mehr als 16 Zeichen verwenden!", "warning");
        $("#textOptionInput").val(text.substr(0, text.length - 1));
        return;
    }
};


function sendDeleteCart() {
    var cartID = $("#cartid").html();
    var sessionID = $("#sessionid").html();
    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    var apiurl = "api/ShoppingCarts/" + cartID;

    $.ajax(apiurl,
        {
            type: "DELETE",
            headers: xxsrf,
            data: JSON.stringify({
                iD: cartID,
                sessionId: sessionID
            }),
            success: function (data) {
                $("#cartDisplay").load("/AjaxContent/ShoppingCart");
            },
            async: false,
            contentType: "application/json",
            dataType: "json",
            timeout: 60000
        }
    );
}