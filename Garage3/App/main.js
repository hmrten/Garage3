
var mainApp = angular.module('main', []);

mainApp.controller('mainController', function ($scope, $http) {

	function getSlots() {
		$http.get(AppData.rootPath + 'Garage/Slots').then(function (resp) {
			$scope.slots = resp.data;
		});
	}

	$scope.message = 'hello from Angular!';

	getSlots();


	$scope.showRegForm = false;
	$scope.showFullReg = false;
	$scope.parking = null;

	function parseMSDate(s) {
		if (!s) return null;
		return new Date(parseInt(s.substr(6)));
	};

	$scope.useSlot = function (slot) {
		$scope.parkMessage = '';
		$scope.showRegForm = false;
		$scope.showFullReg = false;

		if (slot.v_id == null) {
			$scope.slotId = slot.id;
			$scope.showRegForm = true;
			$scope.parking = null;
		} else {
			$http.get(AppData.rootPath + 'Garage/ParkingBySlot/' + slot.id).then(function (resp) {
				$scope.parking = resp.data;
				$scope.parking.date_in = parseMSDate($scope.parking.date_in);
				$scope.parking.date_out = parseMSDate($scope.parking.date_out);
			});
		}
	};

	$scope.park = function () {
		var data = {
			slotId: $scope.slotId,
			regNr: $scope.regNr,
			typeId: $scope.typeId,
			ownerName: $scope.ownerName
		};
		$http.post(AppData.rootPath + 'Garage/Park', data).then(
			function (resp) { // success
				$scope.parkMessage = 'succesfully parked in slot: ' + $scope.slotId;
				$scope.showRegForm = false;
				$scope.showFullReg = false;
				getSlots();
			},
			function (resp) { // failure
				// might be because vehicle reg was not found
				// so lets try and register a new vehicle
				$http.get(AppData.rootPath + 'Vehicle/Types').then(function (resp) {
					$scope.vehicleTypes = resp.data;
					$scope.showFullReg = true;
					$scope.parkMessage = 'could not park in slot: ' + $scope.slotId;
				});
			}
		);
	}
});