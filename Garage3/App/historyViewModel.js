(function () {
	var app = angular.module('history', []);

	app.controller('historyViewModel', function ($scope) {
		$scope.msg = 'parking history from angular';
	});
}());