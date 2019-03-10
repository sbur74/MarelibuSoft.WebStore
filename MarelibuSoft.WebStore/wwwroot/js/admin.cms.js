if (!marelibusoft) {
	var marelibusoft = {};
}

if (!marelibusoft.admin) {
	marelibusoft.admin = {};
}

if (!marelibusoft.admin.cms) {
	marelibusoft.admin.cms = {};
}

marelibusoft.admin.cms.editContent = "";

marelibusoft.admin.cms.renderContentEditor = function () {
	var pageenum = $("#pageenum").val();
	var pagelayoutenum = $("#pagelayoutenum").val();
	var mycmscontent = $("#mycmscontent");

	var content = $("cms-page-content");
	var html = "";


	switch (pageenum) {
		case "0":
			html += "<label class='control-label'>Oben</label><textarea id='top' class='summernote' rows='5'></textarea>";
			html += "<label class='control-label'>links</label><textarea id='left' class='summernote' rows='5'></textarea>";
			html += "<label class='control-label'>rechts</label><textarea id='rigth' class='summernote' rows='5'></textarea>";
			break;

		default:
			html += "<label class='control-label'>Oben</label><textarea id='site' class='summernote' rows='15'></textarea>";
				break;
	}

	content.append(html);
}
