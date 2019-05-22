if (!marelibusoft) {
    var marelibusoft = {};
}

if (!marelibusoft.admin) {
    marelibusoft.admin = {};
}

if (!marelibusoft.admin.sellaction) {
    marelibusoft.admin.sellaction = {};
}

marelibusoft.admin.sellaction.addItem = function (itemid) {
    var sellactionid = $("#sellactionid").val();
    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    var apiurl = "/api/SellActionItems"
    var body = JSON.stringify({
        sellActionID : sellactionid,
        fkProductID : itemid
    });

    $.ajax(apiurl, {
        type: "POST",
        headers: { 'X-XSRF-TOKEN': xxsrf },
        contentType: "application/json",
        dataType: "json",
        data: body,
        timeout: 60000,
        success: function (data) {
            var html = "<div class=\"alert alert-success alert-dismissable fade in\">" +
                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a>" +
                "<strong>Erledigt!</strong></div>";
            $("#messages").empty().append(html).fadeIn(3000);
            marelibusoft.admin.sellaction.reloadselected();
        }, error: function () {
            var html = "<div class=\"alert alert-danger alert-dismissable fade in\">" +
                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a>" +
                "<strong>Fehler!</strong> Fehler, bei der Anlage.</div>";
            $("#messages").empty().append(html).fadeIn(3000).fadeOut(1500);
        }
    });
}

marelibusoft.admin.sellaction.dropItem = function (itemid) {
    var apiurl = "/api/SellActionItems/" + itemid;
    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    $.ajax(apiurl, {
        type: 'DELETE',
        headers: { 'X-XSRF-TOKEN': xxsrf },
        data: JSON.stringify({
            id: itemid
        }), success: function (data) {
            var html = "<div class=\"alert alert-success alert-dismissable fade in\">" +
                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a>" +
                "<strong>Erledigt!</strong></div>";
            $("#messages").empty().append(html).fadeIn(3000);
            marelibusoft.admin.sellaction.reloadselected();
        }, error: function (data) {
            var html = "<div class=\"alert alert-danger alert-dismissable fade in\">" +
                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a>" +
                "<strong>Fehler!</strong> Fehler, beim Löschen.</div>";
            $("#messages").empty().append(html).fadeIn(3000).fadeOut(1500);
        },
        contentType: "application/json",
        dataType: 'json',
        timeout: 60000
    });
}

marelibusoft.admin.sellaction.reloadselected = function () {
    var xxsrf = $("input[name='__RequestVerificationToken']").val();
    var sellactionid = $("#sellactionid").val();
    var apiurl = "/api/SellActionItems/ActionId/" + sellactionid;
    $.ajax(apiurl, {
        type: "GET",
        headers: { 'X-XSRF-TOKEN': xxsrf },
        success: function (data) {
            if (data) {
                $("#actionItems").empty();
                var items = '';
                for (var i = 0; i < data.length; i++) {
                    items += '<li class="list-group-item"><div class="row" >' +
                        '<div class="col-xs-1" >' +
                        '<img class="img-responsive" src="/images/store/' + data[i]['img'] + '" style="max-width: 30px; max-height: 30px;" />' +
                        '</div>' +
                        '<div class="col-xs-9">' +
                        '<p>' + data[i]['no'] + ' - ' + data[i]['name'] + '</p>' +
                        '</div>' +
                        '<div class="col-xs-1">' +
                        '<button class="btn btn-danger" onclick="marelibusoft.admin.sellaction.dropItem(' + data[i]['sellActionItemID'] + ')">' +
                        '<i class="fa fa-trash" title="löschen"></i>' +
                        '</button>' +
                        '</div>' +
                        '</div >' +
                        '</li >';
                }
                $("#actionItems").append(items);
            }
        }, error: function (data) {
            var html = "<div class=\"alert alert-danger alert-dismissable fade in\">" +
                "<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a>" +
                "<strong>Fehler!</strong> Fehler, beim Reload!</div>";
            $("#messages").empty().append(html).fadeIn(3000).fadeOut(1500);
        },
        contentType: "application/json",
        dataType: 'json',
        timeout: 60000
    });
}