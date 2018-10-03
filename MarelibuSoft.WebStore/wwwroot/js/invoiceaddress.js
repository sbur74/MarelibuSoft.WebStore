if (!marelibusoft) {
	var marelibusoft = {};
}

if (!marelibusoft.invioceaddress) {
	marelibusoft.invioceaddress = {};
}

//marelibusoft.invioceaddress.getInvioceCountries = function() {
//	var xxsrf = $("input[name='__RequestVerificationToken']").val();
//	var url = "/api/Countries";

//	$.ajax(url, {
//		type: 'GET',
//		headers: { 'X-XSRF-TOKEN': xxsrf },
//		success: function (data) {
//			console.log(data);
//			marelibusoft.invioceaddress.renderOptions(data);
//		},
//		error: function (data) {
//			console.log("Error getting countries: ", data);
//		},
//		contentType: "application/json",
//		dataType:"json"
//	});
//}

//marelibusoft.invioceaddress.renderOptions = function (data) {
//	var arraydata = data;
//	var element = $("#selectCountry");
//	//{id: 1, code: "DE", name: "Deutschland", isAllowedShipping: true}
//	if (arraydata) {
//		for (var i = 0; i < arraydata.length; i++) {
//			var html = "<option id='" + arraydata[i].id
//				+ "' value ='" + arraydata[i].id
//				+ "' data-allowed-for-shipping='" + arraydata[i].isAllowedShipping + "'>";
//			if (arraydata[i].isAllowedShipping) {
//				html += "<b>" + arraydata[i].name + "</a></option>";
//			} else {
//				html += arraydata[i].name + "</option>";
//			}
//			element.append(html);
//		}
//	}
//}

marelibusoft.invioceaddress.onSelectionChange = function() {
	var selected = $("#selectCountry option:selected");

	if (selected.attr("data-allowed-for-shipping") === "False") {
		marelibusoft.common.showModalInfo("Lieferadresse", "Du m&ouml;chtest dich aus einem Land au&szlig;erhalb unseres Liefergebietes anmelden. Eine Lieferung ist aktuell nur innerhalb Deutschlands m&ouml;glich.<br /> Wenn du eine Lieferadresse innerhalb unseres Liefergebietes hast, logge dich bitte nach Abschluss der Registrierung in dein Kundenkonto ein.<br /> Dort kannst du die Adresse eingeben, an die deine Bestellung gesendet werden soll.<br /> Ohne Adresse innerhalb unseres Liefergebietes ist ein Kauf bei uns leider nicht m&ouml;glich.");
	}
}