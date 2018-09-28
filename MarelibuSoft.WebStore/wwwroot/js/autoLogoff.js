function wireUpWindowUnloadEvents() {
	/*
	* List of events which are triggered onbeforeunload on IE
	* check http://msdn.microsoft.com/en-us/library/ms536907(VS.85).aspx
	*/

	// Attach the event keypress to exclude the F5 refresh
	$(document).on('keypress', function (e) {
		if (e.keyCode == 116) {
			validNavigation = true;
		}
	});

	// Attach the event click for all links in the page
	$(document).on("click", "a", function () {
		validNavigation = true;
	});

	// Attach the event submit for all forms in the page
	$(document).on("submit", "form", function () {
		validNavigation = true;
	});

	// Attach the event click for all inputs in the page
	$(document).bind("click", "input[type=submit]", function () {
		validNavigation = true;
	});

	$(document).bind("click", "button[type=submit]", function () {
		validNavigation = true;
	});

}



function windowCloseEvent() {
	window.onbeforeunload = function () {
		if (!validNavigation) {
			callServerForBrowserCloseEvent();
		}
	}
}



function callServerForBrowserCloseEvent() {
	//…...Do you operation here
}