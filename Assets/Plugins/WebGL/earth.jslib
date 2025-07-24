mergeInto(LibraryManager.library, {
	showAlert: function (message) {
		alert(UTF8ToString(message));
	},

	callMethod: function () {
		console.log("Unity call method");
	}
});