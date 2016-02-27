
var mainApp = angular.module('main', []);

mainApp.controller('mainController', function ($scope, $http) {

	function refreshSlots() {
		$http.get(AppData.rootPath + 'Garage/Slots').then(function (resp) {
			$scope.slots = resp.data;
		});
	}

	$scope.message = 'hello from Angular!';

	refreshSlots();

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

		if (slot.p_id == null) {
			$scope.slotId = slot.id;
			$scope.showRegForm = true;
			$scope.parking = null;
		} else {
			$http.get(AppData.rootPath + 'Garage/Parkings/' + slot.p_id).then(function (resp) {
				$scope.parking = resp.data[0];
				$scope.parking.date_in = parseMSDate($scope.parking.date_in);
				$scope.parking.date_out = parseMSDate($scope.parking.date_out);
				var now = new Date();
				var secs = Math.floor((now.getTime() - $scope.parking.date_in.getTime()) / 1000);
				var days = Math.floor(secs / (3600 * 24));
				secs -= (days * 3600*24);
				var hours = Math.floor(secs / 3600);
				secs -= (hours * 3600);
				var mins = Math.floor(secs / 60);
				$scope.parking.date_dur = days + ' days, ' + hours + ' hours, ' + mins + ' mins';
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
				refreshSlots();
			},
			function (reason) { // failure
				// vehicle was already parked
				if (reason.status == 400) {
					$scope.parkMessage = reason.statusText;
				} else if (reason.status == 404) {
					// no vehicle with that reg, so ask to register a new one
					$http.get(AppData.rootPath + 'Vehicle/Types').then(function (resp) {
						$scope.vehicleTypes = resp.data;
						$scope.showFullReg = true;
						$scope.parkMessage = 'Please register a new vehicle';
					});
				}
			}
		);
	}

	$scope.unpark = function () {
		var data = {
			id: $scope.parking.id
		};
		$http.put(AppData.rootPath + 'Garage/Unpark', data).then(function (resp) {
			$scope.parkMessage + resp.statusText;
			$scope.parking = null;
			refreshSlots();
		});
	};
});