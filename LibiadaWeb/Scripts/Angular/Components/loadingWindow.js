﻿function loadingWindow() {
	"use strict";

	function LoadingWindowController() {
		var ctrl = this;

		ctrl.loadingModalWindow = $("#loadingDialog");

		ctrl.$onInit = function () {
		}

		ctrl.$onChanges = function (changes) {
			if (changes.loading) {
				if (ctrl.loading) {
					ctrl.loadingModalWindow.modal("show");
				}
				else {
					ctrl.loadingModalWindow.modal("hide");
				}
			}
		}

	}

	angular.module("libiada").component("loadingWindow", {
        templateUrl: window.location.origin + "/Partial/_LoadingWindow",
		controller: [LoadingWindowController],
		bindings: {
			loading: "<",
			loadingScreenHeader: "<"
		}
	});
}

loadingWindow();
