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
}

marelibusoft.admin.getCategorySubs = function (sender) {
	var cat = sender;
	var apiurl = "/api/AdminCategorySubs/Category/" + cat;
	$.ajax(apiurl, {
		type : "GET",
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
}

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
}

marelibusoft.admin.renderCatgories = function (response) {
	var idx = marelibusoft.admin.categoryIndexer;
	var table = $("#tableBody");
	var categoryselect = $("#categories_" + idx);
	var html = "";

	if (categoryselect.length === 0) {
		html += "<div id='row_" + idx + "' class='row' >"
			+ "<div id='div_categories_" + idx + "' class='col-sm-3'>"
			+ "<select id='categories_" + idx + "' class='form-control' onchange='marelibusoft.admin.getCategorySubs(this.value)'>";
		html += marelibusoft.admin.renderOptions(response);
		html += "</select></div>" +
			"<div id='subs_" + idx + "' class='col-sm-3'></div>" +
			"<div id='details_" + idx + "' class='col-sm-3' ></div>" +
			"<div id='deleterow_" + idx + "' class='col-sm-3'>" +
			"<button class='btn btn-danger' onclick='marelibusoft.admin.deleteRow(\"row_" + idx + "\")'>Zeile L&ouml;schen</button>" +
			"</div></div>";
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
}

marelibusoft.admin.renderSubs = function (response) {
	var idx = marelibusoft.admin.categoryIndexer;
	var row = $("#row_" + idx);
	var selectsubs = $("#categorysubs_" + idx);
	var html = "";

	if (selectsubs.length === 0) {
		html += "<select id='categorysubs_" + idx + "' class='form-control' onchange='marelibusoft.admin.getCategoryDetails(this.value)'>";
		html += marelibusoft.admin.renderOptions(response);
		html += "</select>";
		$("#subs_"+idx).append(html);
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
}

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
}

marelibusoft.admin.renderOptions = function (response) {
	var html = "<option value=0>nicht Zugewiesen</option>";
	if (response) {
		for (var i = 0; i < response.length; i++) {
			html += "<option value='" + response[i].key + "'>" + response[i].value + "</option>";
		}
	}
	return html;
}

marelibusoft.admin.cleareTable = function () {
	var tableBody = $("#tableBody");
	tableBody.empty();
	$("#messages").empty();
	marelibusoft.admin.categoryIndexer = 0;
}

marelibusoft.admin.deleteRow = function (rowId) {
	$("#" + rowId).empty()
	var tbody = $("#tablebody");
	tbody.remove("#" + rowId);

	if (tbody.children("#tablebody").length === 0) {
		marelibusoft.admin.categoryIndexer = 0;
	}

}

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
}

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
	})
}

marelibusoft.admin.calculateSecondBasePrice = function () {
	var pPrice = parseFloat($("#basisPrice").val().replace(",", "."));
	var pwith = parseFloat($("#productwith").val().replace(",", "."));
	var qmeter = 1 * pwith;
	var secondPrice = pPrice / qmeter;
	secondPrice = secondPrice.toFixed(2);
	secondPrice = secondPrice.replace(".", ",");
	$("#sbPrice").val(secondPrice);
	$("#myModal").modal("hide");
}

marelibusoft.admin.sendShippingMail = function (orderid) {
	console.log("marelibusoft.admin.sendShippingMail -> orderid:" + orderid);

	var xxsrf = $("input[name='__RequestVerificationToken']").val();
}