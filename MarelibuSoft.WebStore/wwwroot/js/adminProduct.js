if (!marelibusoft.admin) {
	marelibusoft.admin = {};
}

marelibusoft.admin.getCategories = function () {

}

marelibusoft.admin.getSubCatagories = function (id) {
	$.ajax(
		{
			url: "/api/CatagorySubs/catagory/" + id,
			accepts: "text/json",
			type: "get"
		});
}


