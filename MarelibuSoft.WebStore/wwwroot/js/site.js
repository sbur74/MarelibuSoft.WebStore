if (!marelibusoft) {
	var marelibusoft = {};
}

if (!marelibusoft.admin) {
	marelibusoft.admin = {};
}

marelibusoft.admin.checkDescrition = function (descripiton) {
	if (descripiton.length > 100) {
		var shorter = descripiton.substr(0, 100) + "...";
		return shorter;
	} else {
		return descripiton;
	}
}

function getSubCatagoriesByCatagoryId() {
	var id = $("#listCatagories").val();
	var listSubCatagories = $("#listSubCatagories")
	var geturi = "/api/CatagorySubs/catagory/" + id;
	var httpRequest = new XMLHttpRequest();
	httpRequest.open("GET", geturi, true);
	httpRequest.onload = function () {
		var jdata = JSON.parse(httpRequest.responseText);
		renderOptions(listSubCatagories, jdata);
	};
	httpRequest.send();
}

function getDetailCatagories() {
	var id = $("#listSubCatagories").val();
	var list = $("#listDetailCatagories");
	var geturi = "/api/CatagoryDetails/subcatagory/" + id;
	var request = new XMLHttpRequest();
	request.open("GET", geturi, true);
	request.onload = function () {
		var jdata = JSON.parse(request.responseText);
		renderOptions(list, jdata);
	};
	request.send();
}

function renderOptions(obj, data) {
	obj.empty();
	if (data.length > 0) {
		for (var i = 0; i < data.length; i++) {
			obj.append("<option value='" + data[i].key + "'>" + data[i].value + "</option>");
		}
	} else {
		obj.append("<option value='0'>keine Ergebnisse</option>");
	}

}

function onCreateProductLoad() {
	getSubCatagoriesByCatagoryId();
	getDetailCatagories();
}


window.onscroll = function () { scrollFunction() };

function scrollFunction() {
	if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
		document.getElementById("myBtn").style.display = "block";
	} else {
		document.getElementById("myBtn").style.display = "none";
	}
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
	document.body.scrollTop = 0;
	document.documentElement.scrollTop = 0;
}