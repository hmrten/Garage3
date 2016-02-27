(function () {
	var app = angular.module('manage', []);

	app.controller('manageViewModel', function ($scope, $http) {
		// must be called everytime you wanna update the grid of slots
		function refreshSlots() {
			$http.get(AppData.rootPath + 'Garage/Slots').then(function (resp) {
				$scope.slots = resp.data;
			});
		}

		function getParkings() {
			$http.get(AppData.rootPath + 'Garage/Parkings').then(function (resp) {
				var parkings = [];
				var data = resp.data;
				for (var i = 0; i < data.length; ++i) {
					if (data[i].date_out == null)
						parkings.push(data[i]);
				}
				$scope.parkings = parkings;
			});
		}

		$scope.searchText = '';

		$scope.parkingFilter = function (p) {
			var s = $scope.searchText.toLowerCase();
			var r = p.vehicle.reg.toLowerCase();
			var o = p.vehicle.owner.name.toLowerCase();
			return (s == '' || s == r || s == o);
		};

		$scope.message = 'hello from Angular!';

		refreshSlots();
		getParkings();

		// TODO: this is not nice, have to juggle 3 'visibility' flags
		$scope.showRegForm = false;
		$scope.showFullReg = false;
		$scope.parking = null;

		// newtonsoft.JSON serializer has a weird format for dates
		// so lets fix it up so JS can understand it.
		function parseMSDate(s) {
			if (!s) return null;
			return new Date(parseInt(s.substr(6)));
		};

		// When user clicks on one of the parking slot boxes
		// this serves both as parking and unparking
		// maybe should split up?
		$scope.useSlot = function (slot) {
			// reset everything to start with
			$scope.parkMessage = '';
			$scope.showRegForm = false;
			$scope.showFullReg = false;

			// if user clicked on a parking slot that was empty
			// it means they probably want to park a vehicle there
			if (slot.p_id == null) {
				$scope.slotId = slot.id;
				// TODO: better handling of toggling visibility
				$scope.showRegForm = true;
				$scope.parking = null;
			} else {
				// if the user clicked on a parking slot that was occupied
				// then we should show the part for unparking
				// but first we need to get more data about the vehicle in that parking slot
				$http.get(AppData.rootPath + 'Garage/Parkings/' + slot.p_id).then(function (resp) {
					// it returns an array of size 1
					$scope.parking = resp.data[0];
					// fixup dates
					$scope.parking.date_in = parseMSDate($scope.parking.date_in);
					$scope.parking.date_out = parseMSDate($scope.parking.date_out);

					// calculate how long the vehicle has been parked for
					// and display in days : hours : minutes
					// TODO: make it adaptable? only display 'days' if days > 0, etc?
					var now = new Date();
					var secs = Math.floor((now.getTime() - $scope.parking.date_in.getTime()) / 1000);
					var days = Math.floor(secs / (3600 * 24));
					secs -= (days * 3600 * 24);
					var hours = Math.floor(secs / 3600);
					secs -= (hours * 3600);
					var mins = Math.floor(secs / 60);
					$scope.parking.date_dur = days + ' days, ' + hours + ' hours, ' + mins + ' mins';
				});
			}
		};

		// function for submitting POST to park a vehicle
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
					// TODO: make this better
					$scope.showRegForm = false;
					$scope.showFullReg = false;
					refreshSlots();
				},
				function (reason) { // failure
					// it can fail for two reasons:
					//   1: vehicle was already parked, in which case we reject the attempt.
					//   2: the vehicle with that reg nr was not found, so we should ask
					//      user to register a new one.
					// 400 == already parked
					// 404 == no vehicle

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
			// we don't really care about handling errors here
			$http.put(AppData.rootPath + 'Garage/Unpark', data).then(function (resp) {
				$scope.parkMessage + resp.statusText;
				// TODO: better visibility handling
				$scope.parking = null;
				// parking slots have changed, so we need to refresh the view
				refreshSlots();
			});
		};
	});
}());