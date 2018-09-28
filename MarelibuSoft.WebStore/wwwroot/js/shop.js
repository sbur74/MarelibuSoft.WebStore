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