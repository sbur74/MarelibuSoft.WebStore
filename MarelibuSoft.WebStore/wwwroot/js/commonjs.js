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