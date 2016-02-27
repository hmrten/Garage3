
var mainApp = angular.module('main', []);

mainApp.controller('mainController', function ($scope, $http) {

	function getSlots() {
		$http.get(AppData.rootPath + 'Garage/Slots').then(function (resp) {
			$scope.slots = resp.data;
		});
	}

	$scope.message = 'hello from Angular!';

	getSlots();

	$scope.vehicleInfo = null;
	$scope.showInfo = function (s) {
		if (s == null) {
			$scope.vehicleInfo = null;
		} else {
			if (s.v_id != null) {
				$http.get(AppData.rootPath + 'Vehicle/List/' + s.v_id).then(function (resp) {
					$scope.vehicleInfo = resp.data;
				});
			}
		}
	};

	$scope.showRegForm = false;
	$scope.showFullReg = false;
	$scope.showUnpark = false;

	$scope.useSlot = function (slot) {
		$scope.parkMessage = '';
		$scope.showRegForm = false;
		$scope.showFullReg = false;

		if (slot.v_id == null) {
			$scope.slotId = slot.id;
			$scope.showRegForm = true;
			$scope.showUnpark = false;
		} else {
			$scope.showUnpark = true;
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