(function () {
	var app = angular.module('history', ['base']);

	app.controller('historyCtrl', function ($scope, $http) {

		$scope.parkings = [];

		function getParkings() {
			$http.get(garage.rootPath + 'Garage/Parkings').then(function (resp) {
				var parkings = resp.data;
				for (var i = 0; i < parkings.length; ++i) {
					parkings[i].date_in = garage.parseMSDate(parkings[i].date_in);
					parkings[i].date_out = garage.parseMSDate(parkings[i].date_out);
				}
				$scope.parkings = parkings;
			});
		};

		getParkings();

		$scope.msg = 'parking history from angular';
	});
}());