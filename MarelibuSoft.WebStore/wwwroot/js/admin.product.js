if (!marelibusoft) {
	var marelibusoft = {};
}

if (!marelibusoft.admin) {
	marelibusoft.admin = {};
}

if (!marelibusoft.admin.product) {
	marelibusoft.admin.product = {};
}

marelibusoft.admin.product.sendActive = function (senderId) {
	var checked = false;
	var controlChecked = $("#product" + senderId);
	var url = "/api/Products/" + senderId;
	if (controlChecked.prop("checked")) {
		checked = true;
	}

	$.ajax(url, {
		type: "PUT",
		data: JSON.stringify({ IsActive: checked }),
		contentType: "application/json",
		dataType: "json",
		timeout: 60000,
		success: function (data) {
			console.log(data);
		},
		error: function (data) {
			console.log(data);
		}
	});
}