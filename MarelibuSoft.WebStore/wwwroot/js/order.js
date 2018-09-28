if (!marelibusoft) {
	var marelibusoft = {};
}
if (!marelibusoft.order) {
	marelibusoft.order = {};
}

marelibusoft.order.shopFileChecked = function () {
	var agbCheck = $("#agbCheck");


	if (agbCheck.prop("checked") === true) {
		$("#buyBottons").removeClass("hidden").show();
	} else {
		$("#buyBottons").hide();
	}
}
marelibusoft.order.onSubmitUeberweisung = function () {
	$("#paymentId").val(1);
}

marelibusoft.order.onSubmitPayPal = function () {
	$("#paymentId").val(2);
	$("form").submit();
}

marelibusoft.order.onSubmitBill = function () {
	$("#paymentId").val(3);
}