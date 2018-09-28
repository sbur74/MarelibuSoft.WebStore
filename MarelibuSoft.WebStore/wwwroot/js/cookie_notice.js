/**
 * 	
 * Pure javascript cookie notice 2018 by Michael Hudault, https://hudault.de
 * css for styling in /cookie_notice/cookie_notice.css
 * 
 * Without-any-warranty-do-whatever-you-want licence
 *
 */

window.onload = function (e) {
	var csplit = document.cookie.split(";");
	var cookie_accepted = false;
	for (var i = 0; i < csplit.length; i++) {
		var cstr = csplit[i].replace(" ", "");
		var esplit = cstr.split("=");
		if (esplit[0] === "COOKIES_MARELIBU_ACCEPTED") {
			cookie_accepted = esplit[1];
		}
	}

	if (cookie_accepted) {
		document.querySelector("#cookieConsent").classList.add("hidden");
	}
	else {
		this.console.log("remove class hidden");
		document.querySelector("#cookieConsent").classList.remove("hidden");
		// Bind click-accept-event to accept-button
		document.getElementById('cookie_notice_button_accept').addEventListener('click', function () {
			document.querySelector("#cookieConsent").classList.add("hidden");
			accept_cookies();
		});

	}

}

function accept_cookies() {

	// Set cookie
	var d = new Date();
	d.setTime(d.getTime() + ((30 * 24 * 60 * 60 * 1000) + 30));
	var expires = "expires=" + d.toGMTString();

	document.cookie = "COOKIES_MARELIBU_ACCEPTED=true;" + expires + ";path=/";

	// Hide notice
	document.getElementById('cookieConsent').style.display = "none";
	return false;

}

