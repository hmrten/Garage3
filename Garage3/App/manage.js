(function () {
    var app = angular.module('manage', ['base']);

	app.controller('manageCtrl', function ($scope, $http, garageService) {
		// must be called everytime you wanna update the grid of slots
	    function refreshSlots() {
            garageService.getSlots(function(slots) { $scope.slots = slots; });
		}

		function getParkings() {
		    garageService.getParkings(true, null, function(parkings) {
		        $scope.parkings = parkings;
		    });
		}

		function init() {
		    // TODO: this is not nice, have to juggle 3 'visibility' flags
		    $scope.showRegForm = false;
		    $scope.showFullReg = false;
		    $scope.parking = null;
		    $scope.searchText = '';

		    refreshSlots();
		    garageService.getParkings(true, null, function (parkings) { $scope.parkings = parkings; });
		};

		init();

		// When user clicks on one of the parking slot boxes
		// this serves both as parking and unparking
		// maybe should split up?
		$scope.useSlot = function (slot) {
			// reset everything to start with
			$scope.alertMessage = '';
			$scope.showRegForm = false;
			$scope.showFullReg = false;

			// if user clicked on a parking slot that was empty
			// it means they probably want to park a vehicle there
			if (slot.p_id == null) {
				$scope.slotId = slot.id;
				// TODO: better handling of toggling visibility
				$scope.showRegForm = true;
				$scope.parking = null;
				$scope.headerText = 'Park';
			} else {
				// if the user clicked on a parking slot that was occupied
				// then we should show the part for unparking
			    // but first we need to get more data about the vehicle in that parking slot
			    garageService.getParkings(false, slot.p_id, function (parkings) {
					// it returns an array of size 1
			        $scope.parking = parkings[0];
					$scope.parking.date_dur = garageService.getDurationString(null, $scope.parking.date_in);
					$scope.headerText = 'Unpark';
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
			garageService.park(data, function (data) {
			    $scope.alertMessage = 'succesfully parked in slot: ' + $scope.slotId;
			    $scope.alertType = 'success';
			    // TODO: make this better
			    $scope.showRegForm = false;
			    $scope.showFullReg = false;
			    refreshSlots();
			},
            function (reason) {
                // it can fail for two reasons:
                //   1: vehicle was already parked, in which case we reject the attempt.
                //   2: the vehicle with that reg nr was not found, so we should ask
                //      user to register a new one.
                // 400 == already parked
                // 404 == no vehicle

                if (reason.status == 400) {
                    $scope.alertMessage = reason.statusText;
                    $scope.alertType = 'danger';
                } else if (reason.status == 404) {
                    // no vehicle with that reg, so ask to register a new one
                    garageService.getTypes(function (types) {
                        $scope.vehicleTypes = types;
                        $scope.showFullReg = true;
                        $scope.alertMessage = 'Please register a new vehicle';
                        $scope.alertType = 'warning';
                    });
                }
            });
		}

		$scope.unpark = function () {
			var data = {
				id: $scope.parking.id
			};
			garageService.unpark(data, function(msg) {
				$scope.alertMessage = msg;
				$scope.alertType = 'success';
				$scope.parking = null;
				// parking slots have changed, so we need to refresh the view
				refreshSlots();
			});
		};
	});
}());