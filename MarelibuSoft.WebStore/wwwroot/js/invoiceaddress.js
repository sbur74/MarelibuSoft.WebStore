if (!marelibusoft) {
    var marelibusoft = {};
}

if (!marelibusoft.invioceaddress) {
    marelibusoft.invioceaddress = {};
}

if (!marelibusoft.shipToAddress) {
    marelibusoft.shipToAddress = {};
}

marelibusoft.invioceaddress.onSelectionChange = function () {
    var selected = $("#selectCountry option:selected");

    if (selected.attr("data-allowed-for-shipping") === "False") {
        marelibusoft.common.showModalInfo("Lieferadresse", "Du m&ouml;chtest dich aus einem Land au&szlig;erhalb unseres Liefergebietes anmelden. Eine Lieferung ist aktuell nur innerhalb Deutschlands m&ouml;glich.<br /> Wenn du eine Lieferadresse innerhalb unseres Liefergebietes hast, logge dich bitte nach Abschluss der Registrierung in dein Kundenkonto ein.<br /> Dort kannst du die Adresse eingeben, an die deine Bestellung gesendet werden soll.<br /> Ohne Adresse innerhalb unseres Liefergebietes ist ein Kauf bei uns leider nicht m&ouml;glich.");
    }
};

marelibusoft.shipToAddress.useAddress = function (id) {
    var url = "/api/ShippingAddresses/" + id;

    $.ajax(url, { contentType: "application/json", complete: function () { location.reload(); }, dataType: "json" });
};