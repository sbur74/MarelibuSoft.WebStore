if (!marelibusoft) {
    var marelibusoft = {};
}
if (!marelibusoft.order) {
    marelibusoft.order = {};
}


marelibusoft.order.changeShipToAddress = function (id, value) {

};


marelibusoft.order.initPaymendOnload = function () {
    $("#payment2").attr("checked");
    marelibusoft.order.onSubmitPayPal();
};

marelibusoft.order.shopFileChecked = function () {
    var agbCheck = $("#agbCheck");
    var paymentId = $("#paymentId").val();

    if (agbCheck.prop("checked") === true && paymentId > 0) {
        $("#submitBuy").removeClass('disabled').removeClass('btn-default').addClass('btn-success');
    } else {
        $("#submitBuy").removeClass('btn-success').addClass('btn-default').addClass('disabled');
    }
};

marelibusoft.order.onSubmitUeberweisung = function () {
    $("#paymentId").val(1);
    //$("#payment2").removeAttr("checked");
    //$("#payment3").removeAttr("checked");
    marelibusoft.order.shopFileChecked();
};

marelibusoft.order.onSubmitPayPal = function () {
    $("#paymentId").val(2);
    //$("#payment1").removeAttr("checked");
    //$("#payment3").removeAttr("checked");
    marelibusoft.order.shopFileChecked();
};

marelibusoft.order.onSubmitBill = function () {
    $("#paymentId").val(3);
    //$("#payment2").removeAttr("checked");
    //$("#payment1").removeAttr("checked");
    marelibusoft.order.shopFileChecked();
};