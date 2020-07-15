if (!marelibusoft) {
    var marelibusoft = {};
}

if (!marelibusoft.admin) {
    marelibusoft.admin = {};
}

marelibusoft.admin.categoryIndexer = 0;

marelibusoft.admin.getCategories = function () {
    marelibusoft.admin.categoryIndexer++;
    var apiurl = "/api/AdminCategories";
    $.ajax(apiurl, {
        type: "GET",
        contentType: "application/json",
        dataType: 'json',
        timeout: 60000,
        success: function (result) {
            var data = result;
            marelibusoft.admin.renderCatgories(data);
        },
        error: function () {
            alert("fehler");
        }
    });
};

marelibusoft.admin.getCategorySubs = function (sender) {
    var cat = sender;
    var apiurl = "/api/AdminCategorySubs/Category/" + cat;
    $.ajax(apiurl, {
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            var data = result;
            marelibusoft.admin.renderSubs(data);
        }, error: function () {
            alert("Fehler");
        }
    });
};

marelibusoft.admin.getCategoryDetails = function (sender) {
    var sub = sender;
    var apiurl = "/api/AdminCategoryDetails/Sub/" + sub;
    $.ajax(apiurl, {
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            var data = result;
            marelibusoft.admin.renderDetails(data);
        }, error: function (result) {
            alert(result);
        }
    });
};

marelibusoft.admin.renderCatgories = function (response) {
    var idx = marelibusoft.admin.categoryIndexer;
    var table = $("#tableBody");
    var categoryselect = $("#categories_" + idx);
    var html = "";

    if (categoryselect.length === 0) {
        html += "<div id='row_" + idx + "' class='row' >";
        html += "<div id='div_categories_" + idx + "' class='col-sm-3'>";
        html += "<select id='categories_" + idx + "' class='form-control' onchange='marelibusoft.admin.getCategorySubs(this.value)'>";
        html += marelibusoft.admin.renderOptions(response);
        html += "</select></div>";
        html += "<div id='subs_" + idx + "' class='col-sm-3'></div>";
        html += "<div id='details_" + idx + "' class='col-sm-3' ></div>";
        html += "<div id='deleterow_" + idx + "' class='col-sm-3'>";
        html += "<button class='btn btn-danger' onclick='marelibusoft.admin.deleteRow(\"row_" + idx + "\")'>Zeile L&ouml;schen</button>";
        html += "</div></div>";
        table.append(html);
    } else {
        categoryselect.empty();
        html += marelibusoft.admin.renderOptions(response);
        categoryselect.append();

        var selectsubs = $("#categorysubs_" + idx);
        if (selectsubs.length > 0) {
            selectsubs.empty();
            table.remove("#categorysubs_" + idx);
        }

        var detailsselect = $("#categorydetails_" + idx);
        if (detailsselect.length > 0) {
            detailsselect.empty();
            table.remove("#categorydetails_" + idx);
        }
    }
};

marelibusoft.admin.renderSubs = function (response) {
    var idx = marelibusoft.admin.categoryIndexer;
    var row = $("#row_" + idx);
    var selectsubs = $("#categorysubs_" + idx);
    var html = "";

    if (selectsubs.length === 0) {
        html += "<select id='categorysubs_" + idx + "' class='form-control' onchange='marelibusoft.admin.getCategoryDetails(this.value)'>";
        html += marelibusoft.admin.renderOptions(response);
        html += "</select>";
        $("#subs_" + idx).append(html);
    } else {
        selectsubs.empty();
        html += marelibusoft.admin.renderOptions(response);
        selectsubs.append(html);

        var detailsselect = $("#categorydetails_" + idx);
        if (detailsselect.length > 0) {
            detailsselect.empty();
            row.remove("#categorydetails_" + idx);
        }
    }
};

marelibusoft.admin.renderDetails = function (response) {
    var idx = marelibusoft.admin.categoryIndexer;
    var row = $("#row_" + idx);
    var html = "";
    var detailsselect = $("#categorydetails_" + idx);

    if (detailsselect.length === 0) {
        html += "<select id='categorydetails_" + idx + "' class='form-control'>";
        html += marelibusoft.admin.renderOptions(response);
        html += "</select>";
        $("#details_" + idx).append(html);
    } else {
        detailsselect.empty();
        html += marelibusoft.admin.renderOptions(response);
        detailsselect.append(html);
    }
};

marelibusoft.admin.renderOptions = function (response) {
    var html = "<option value=0>nicht Zugewiesen</option>";
    if (response) {
        for (var i = 0; i < response.length; i++) {
            html += "<option value='" + response[i].key + "'>" + response[i].value + "</option>";
        }
    }
    return html;
};

marelibusoft.admin.cleareTable = function () {
    var tableBody = $("#tableBody");
    tableBody.empty();
    $("#messages").empty();
    marelibusoft.admin.categoryIndexer = 0;
};

marelibusoft.admin.deleteRow = function (rowId) {
    $("#" + rowId).empty();
    var tbody = $("#tablebody");
    tbody.remove("#" + rowId);

    if (tbody.children("#tablebody").length === 0) {
        marelibusoft.admin.categoryIndexer = 0;
    }

};

marelibusoft.admin.submitTable = function () {
    var productselect = $("#productID");
    var productID = productselect.val();
    var rows = marelibusoft.admin.categoryIndexer + 1;

    for (var i = 1; i < rows; i++) {
        var categoryselect = $("#categories_" + i);
        var selectsubs = $("#categorysubs_" + i);
        var detailsselect = $("#categorydetails_" + i);
        var categoryID = 0;
        var categorySubID = 0;
        var categoryDetailID = 0;
        if (categoryselect.length > 0) {
            categoryID = categoryselect.val();
        }
        if (selectsubs.length > 0) {
            categorySubID = selectsubs.val();
        }
        if (detailsselect.length > 0) {
            categoryDetailID = detailsselect.val();
        }
        if (productID > 0 && categoryID > 0) {
            marelibusoft.admin.postAssignment(productID, categoryID, categorySubID, categoryDetailID);
        }
    }
};

marelibusoft.admin.postAssignment = function (productID, categoryID, categorySubID, categoryDetialID) {
    var body = JSON.stringify({
        productID: productID,
        categoryID: categoryID,
        categorySubID: categorySubID,
        categoryDetailID: categoryDetialID
    });

    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    var apiurl = "/api/AdminCategoryAssignments";

    $.ajax(apiurl, {
        type: "POST",
        headers: { 'X-XSRF-TOKEN': xxsrf },
        contentType: "application/json",
        dataType: "json",
        data: body,
        timeout: 60000,
        success: function () {
            var html = "<div class=\"alert alert-success alert-dismissable fade in\">" +
                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a>" +
                "<strong>Erledigt!</strong> Artikel Kategorien Zuordnung angelegt</div>";

            $("#messages").empty().append(html).fadeIn(3000);
        }, error: function () {
            var html = "<div class=\"alert alert-danger alert-dismissable fade in\">" +
                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a>" +
                "<strong>Fehler!</strong> Fehler, bei der Anlage.</div>";
            $("#messages").empty().append(html).fadeIn(3000).fadeOut(1500);
        }
    });
};

marelibusoft.admin.calculateSecondBasePrice = function () {
    var pPrice = parseFloat($("#basisPrice").val().replace(",", "."));
    var pwith = parseFloat($("#productwith").val().replace(",", "."));
    var qmeter = 1 * pwith;
    var secondPrice = pPrice / qmeter;
    secondPrice = secondPrice.toFixed(2);
    secondPrice = secondPrice.replace(".", ",");
    $("#sbPrice").val(secondPrice);
    $("#myModal").modal("hide");
};

marelibusoft.admin.sendShippingMail = function (orderid) {
    console.log("marelibusoft.admin.sendShippingMail -> orderid:" + orderid);

    var xxsrf = $("input[name='__RequestVerificationToken']").val();
};

$(".modal-wide").on("show.bs.modal", function () {
    var height = $(window).height() - 200;
    $(this).find(".modal-body").css("max-height", height);
});

marelibusoft.admin.getVariants = function () {
    var url = "/api/ProductVariants";
    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    $.ajax(url, {
        type: "GET",
        contentType: "application/json",
        headers: { 'X-XSRF-TOKEN': xxsrf },
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            var data = result;
            return (data);
        }, error: function () {
            alert("Fehler");
        }

    });
};

marelibusoft.admin.getVariantOptions = function () {
    var url = "/api/VariantOptionTemplates/variant/";
    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    var id = $("#selectVariant").find(":selected")[0].id;
    var variantName = $("#selectVariant").find(":selected").text();
    var optionName = $("#selectVariant").find(":selected").attr("data-optionname");
    $("#variantNameAdd").val(variantName);
    $("#variantOptionNameAdd").val(optionName);
    console.log("selected variant:" + id);
    url += id;
    $.ajax(url, {
        type: "GET",
        contentType: "application/json",
        headers: { 'X-XSRF-TOKEN': xxsrf },
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            var data = result;
            var table = "tblOpt";
            if (data) {
                for (var i = 0; i < data.length; i++) {
                    marelibusoft.admin.AddVariantOption(table, data[i].option);
                }
            }
        }, error: function () {
            alert("Fehler");
        }

    });
};


marelibusoft.admin.getVariantCombiOptions = function () {
    var url = "/api/VariantOptionTemplates/variant/";
    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    var id = $("#selectVariantCombi").find(":selected")[0].id;
    var variantName = $("#selectVariantCombi").find(":selected").text();
    var optionName = $("#selectVariantCombi").find(":selected").attr("data-optionname");
    $("#combiVariantNameAdd").val(variantName);
    $("#combiVariantOptionNameAdd").val(optionName);
    console.log("selected variant:" + id);
    url += id;
    $.ajax(url, {
        type: "GET",
        contentType: "application/json",
        headers: { 'X-XSRF-TOKEN': xxsrf },
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            var data = result;
            var table = "tblCombi";
            if (data) {
                for (var i = 0; i < data.length; i++) {
                    marelibusoft.admin.AddVariantOption(table, data[i].option);
                }
            }
        }, error: function () {
            alert("Fehler");
        }

    });
};

marelibusoft.admin.cleareModalVariant = function () {
    marelibusoft.admin.variantOptionCount = 0;
    marelibusoft.admin.variantCombiCount = 0;
    $("#variantNameAdd").val("");
    $("#variantOptionNameAdd").val("");
    $("#combiVariantNameAdd").val("");
    $("#combiVariantOptionNameAdd").val("");
    $("#tblOpt").empty();
    $("tblCombi").empty();
};

marelibusoft.admin.variantOptionCount = 0;
marelibusoft.admin.variantCombiCount = 0;

marelibusoft.admin.AddVariantOption = function (table, value) {

    var rowid = 0;
    if (table === "tblOpt") {
        marelibusoft.admin.variantOptionCount++;
        rowid = marelibusoft.admin.variantOptionCount;
    } else {
        if (table === "tblCombi") {
            marelibusoft.admin.variantCombiCount++;
            rowid = marelibusoft.admin.variantCombiCount;
        }
    }
    if (!value) value = "";
    var tabelbody = $("#" + table);
    var htmlrow = "<div id='" + table + "_row_" + rowid + "' class='row'>" +
        "<div class='col-xs-9'>" +
        "<input id='" + table + "_optionId_" + rowid + "' class='form-control' type='text' value='" + value + "' />" +
        "</div >" +
        "<div class='col-xs-3'>" +
        "<a class='btn btn-danger' onclick='marelibusoft.admin.delteVariantRow(\"" + table + "\", " + rowid + ")'><i class='fa fa-trash'></i></a>" +
        "</div ></div>";


    tabelbody.append(htmlrow);
    $("#" + table + "_optionId_" + rowid).focus();
};

marelibusoft.admin.delteVariantRow = function (table, rowid) {
    var id = "#" + table + "_row_" + rowid;
    $(id).empty();
    var tbody = $("#" + table);
    tbody.remove(id);
};

marelibusoft.admin.saveVariantOnSubmit = function () {
    event.preventDefault();
    $(".marelibu-varinat-rows").each(function (i, el) {
        var varinatid = $(this).data("variant-id");
        console.log(varinatid);
        marelibusoft.admin.saveVariant(varinatid);
    });

};

marelibusoft.admin.saveVariant = function (varinatId) {
    var id = varinatId;

    $("#variantTableBodyId_" + id + " > tr").each(function (i, el) {
        var optId = $(this).data("option-id");
        marelibusoft.admin.saveOption(id, optId);
    });
};

marelibusoft.admin.createVariants = function () {
    var productID = $("#ProductID").val();
    var variantName = $("#variantNameAdd").val();
    var variantOption = $("#variantOptionNameAdd").val();
    var combiVariantName = $("#combiVariantNameAdd").val();
    var combiVariantOption = $("#combiVariantOptionNameAdd").val();
    var copyTemplateControl = $('#copyTemplate');
    var copyTemplate = false;
    var isAbsolutelyNecessaryControl = $("#isAbsolutelyNecessary");
    var isAbsolutelyNecessary = false;
    var tbody = $("#tblOpt");
    var tcombi = $("#tblCombi");
    var optionInputs = tbody.find("input");
    var combiInputs = tcombi.find("input");
    var counter = marelibusoft.admin.variantOptionCount;
    var combiCounter = marelibusoft.admin.variantCombiCount;
    var variantOptions = [];
    var templateOptions = [];
    var templateOptCombi = [];

    if (isAbsolutelyNecessaryControl.prop("checked")) isAbsolutelyNecessary = true;

    if (copyTemplateControl.prop("checked")) copyTemplate = true;

    if (combiInputs.length > 0) {
        $.each(optionInputs, function (i, option) {
            $.each(combiInputs, function (j, combi) {
                var productVariantOption = {};
                productVariantOption.option = option.value;
                productVariantOption.quantity = 15.0;
                productVariantOption.price = 0.0;
                productVariantOption.combi = combi.value;
                variantOptions.push(productVariantOption);
                if (copyTemplate) {
                    var variantOptionTemplate = {};
                    variantOptionTemplate.option = option.value;
                    templateOptions.push(variantOptionTemplate);
                }
            });
        });
    } else {
        $.each(optionInputs, function (i, option) {
            var productVariantOption = {};
            productVariantOption.option = option.value;
            productVariantOption.quantity = 15.0;
            productVariantOption.price = 0.0;
            productVariantOption.combi = "";
            variantOptions.push(productVariantOption);
            if (copyTemplate) {
                var variantOptionTemplate = {};
                variantOptionTemplate.option = option.value;
                templateOptions.push(variantOptionTemplate);
            }

        });
    }

    var productVariant = {};
    productVariant.Name = variantName;
    productVariant.OptionName = variantOption;
    productVariant.Options = variantOptions;
    productVariant.IsAbsolutelyNecessary = isAbsolutelyNecessary;
    productVariant.ProductId = productID;

    if (copyTemplate) {
        var variantTemplate = {};
        variantTemplate.Name = variantName;
        variantTemplate.OptionName = variantOption;
        variantTemplate.VariantOptionTemplates = templateOptions;
        marelibusoft.admin.postVariantTemplate(variantTemplate);
    }
    marelibusoft.admin.postVariant(productVariant);

    $("#variantModal").modal("hide");
    location.reload();
};

marelibusoft.admin.putVariantIsAbsolutelyNecessary = function (variantId) {
    var control = $("#isAbsolutelyNecessary_" + variantId);
    var isAbsolutelyNecessary = false;
    if (control.prop("checked")) isAbsolutelyNecessary = true;

    var url = "/api/ProductVariants/isNecessary/" + variantId;
    var xxsrf = $("input[name='__RequestVerificationToken']").val();

    $.ajax(url, {
        headers: { 'X-XSRF-TOKEN': xxsrf },
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify({ "isActive": isAbsolutelyNecessary }),
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            var data = result;
            if ($(".new-product").length > 0) {
                location.reload();
            }
        }, error: function () {
            alert("Fehler");
        }
    });
};

marelibusoft.admin.postVariant = function (variant) {
    var url = "/api/ProductVariants";
    var xxsrf = $("input[name='__RequestVerificationToken']").val();

    var variantObj = variant;

    $.ajax(url, {
        headers: { 'X-XSRF-TOKEN': xxsrf },
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(variantObj),
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            var data = result;
            console.log(data);
            if ($(".new-product").length > 0) {
                location.reload();
            }
        }, error: function () {
            alert("Fehler");
        }
    });
};

marelibusoft.admin.postVariantTemplate = function (variantTemplate) {
    var url = "/api/VariantTemplates";
    var xxsrf = $("input[name='__RequestVerificationToken']").val();

    var temp = variantTemplate;

    $.ajax(url, {
        headers: { 'X-XSRF-TOKEN': xxsrf },
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(temp),
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            console.log("Variante angelegt");
        }, error: function () {
            alert("Fehler");
        }
    });
};

marelibusoft.admin.deleteProductVariant = function (variantId) {
    var url = "/api/ProductVariants/" + variantId;
    var xxsrf = $("input[name='__RequestVerificationToken']").val();

    $.ajax(url, {
        headers: { 'X-XSRF-TOKEN': xxsrf },
        type: "DELETE",
        contentType: "application/json",
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            location.reload();
        }, error: function () {
            alert("Fehler");
        }
    });
};

marelibusoft.admin.addOption = function (variantId) {
    var option = {
        Opiton: "",
        Qunatity: 1.0,
        Price: 0.0,
        ProductVariantID: variantId
    };

    var url = "/api/ProductVariantOptions";
    var xxsrf = $("input[name='__RequestVerificationToken']").val();

    $.ajax(url, {
        headers: { 'X-XSRF-TOKEN': xxsrf },
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(option),
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            location.reload();
        }, error: function () {
            alert("Fehler");
        }
    });
};

marelibusoft.admin.saveOption = function (variantId, optionId) {
    var optionValue = $("#option_" + optionId).val();
    var quantityValue = marelibusoft.common.str2Float($("#quatity_" + optionId).val());
    var priceValue = marelibusoft.common.str2Float($("#price_" + optionId).val());
    var isNotShownValue = false;
    if ($("#isNotShown_" + optionId).prop("checked")) {
        isNotShownValue = true;
    }
    var url = "/api/ProductVariantOptions/" + optionId;
    var xxsrf = $("input[name='__RequestVerificationToken']").val();

    $.ajax(url, {
        headers: { 'X-XSRF-TOKEN': xxsrf },
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify({
            "id": optionId,
            "option": optionValue,
            "quantity": quantityValue,
            "price": priceValue,
            "isNotShown": isNotShownValue,
            "productVariantID": variantId,
        }),
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            console.log("geändert angelegt");
        }, error: function () {
            alert("Fehler");
        }
    });

};

marelibusoft.admin.deleteOption = function (variantId, optionId) {
    var url = "/api/ProductVariantOptions/" + optionId;
    var xxsrf = $("input[name='__RequestVerificationToken']").val();

    $.ajax(url, {
        headers: { 'X-XSRF-TOKEN': xxsrf },
        type: "DELETE",
        contentType: "application/json",
        data: JSON.stringify({
            "id": optionId,
            "productVariantID": variantId
        }),
        dataType: "json",
        timeout: 60000,
        success: function (result) {
            console.log("gelöscht");
            location.reload();
        }, error: function () {
            alert("Fehler");
        }
    });
};

marelibusoft.admin.addCombiVariant = function () {
    var container = $("containerCombi");
    var html = "<a class='btn btn-success' onclick='marelibusoft.admin.AddVariantOption('tableBodyVariantCombi')'>" +
        "<i class='fa fa - plus - circle'></i> Option hinzuf& uuml; gen</a >" +
        "<div id = 'tableCombi class='table'>" +
        "<div class='row'>" +
        "<div class='col-xs-3'>" +
        "<label class='control-label'>Wert</label>" +
        "</div>" +
        "<div class='col-xs-3'>" +
        "<label class='control-label'>Menge</label>" +
        "</div>" +
        "<div class='col-xs-3'>" +
        "<label class='control-label'>Preis</label>" +
        "</div>" +
        "<div class='col-xs-'>" +
        "<label class='control-label'>Aktionen</label>" +
        "</div>" +
        "</div >" +
        "<div id='tableBodyVariantCombi'></div></div >";
    container.append(html);
};