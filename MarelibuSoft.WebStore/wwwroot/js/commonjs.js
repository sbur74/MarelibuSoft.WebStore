if (!marelibusoft) {
	var marelibusoft = {};
}

if (!marelibusoft.common) {
	marelibusoft.common = {};
}
marelibusoft.common.showModalAlert = function (elementId, message, type) {
	var alertMessage = $("#myalert");
	switch (type) {
		case "success":
			alertMessage.append("<div class='media - body'><div class ='alert alert-success'>" + message + "</div></div>");
			//alertMessage.addClass("success");
			break;
		case "danger":
			alertMessage.append("<div class='media - body'><div class ='alert alert-danger'>" + message + "</div></div>");
			//alertMessage.addClass("failure");
			break;
		case "warning":
			alertMessage.append("<div class='media - body'><div class ='alert alert-warning'>" + message + "</div></div>");
			//alertMessage.addClass("warning");
			break;
		
		default:
			alertMessage.append("<div class='media - body'><div class ='alert alert-info'>" + message + "</div></div>");
			break;
	}
	alertMessage.show("fade", null,3000, function () {
		setTimeout(function () {
			$("#myalert").hide("fade", null, 1500, null).empty();
		}, 1500);
	});
}

marelibusoft.common.showModalInfo = function (titel, message) {
	$("#myModalTitle").html(titel);
	$("#myModalBody").append(message);
	$("#myModal").modal("show");
}

marelibusoft.common.countChar = function (object) {
	var textLength = object.val();

	if (textLength.Length >= 155) {
		marelibusoft.common.showModalAlert("myalert", "Du darfst nur 155 Zeichen verwenden!", "wanring");
	}
}

marelibusoft.common.str2Float = function (str) {
    if(!str) return 0.0;
    var value = str.replace(",", ".");
    return parseFloat(value);
}

marelibusoft.common.float2LocalCurrencyString = function (fvalue) {
    if (!isNaN(fvalue)) {
        return fvalue.toLocaleString('de-DE', { style: 'currency', currency: 'EUR' });
    }
    return "";
}


marelibusoft.common.float2LocalString = function (fvalue) {
    return fvalue.toLocaleString('de-De');
}