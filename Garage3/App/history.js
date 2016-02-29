(function () {
	var app = angular.module('history', ['base']);

	app.controller('historyCtrl', function ($scope, $http) {

		$scope.parkings = [];

		function getParkings() {
			$http.get(garage.rootPath + 'Garage/Parkings').then(function (resp) {
				var parkings = resp.data;
				for (var i = 0; i < parkings.length; ++i) {
					parkings[i].date_in = garage.parseMSDate(parkings[i].date_in); //fixing datetime format for js
					parkings[i].date_out = garage.parseMSDate(parkings[i].date_out);
            
					parkings[i].duration = parkings[i].date_out - parkings[i].date_in;
				}
				$scope.parkings = parkings;
			});
		};


		getParkings();

		$scope.msg = 'Parking History! View data only!';

		$scope.filterReg = function (p) {
		    var s1 = $scope.filterResults;
		    var s2 = p.vehicle.reg;
		    if (s1 == null || s1== "") {
		        return true;
		    }
		    s1 = s1.toLowerCase();
		    s2 = s2.toLowerCase();
		    if (s2.indexOf(s1) != -1) {
		        return true;
		    }
		    else {
		        return false;
		    }
		}
		$scope.filterOwner = function (p) {
		    var s1 = $scope.filterResultsOwn;
		    var s2 = p.vehicle.owner.name;
		    if (s1 == null || s1 == "") {
		        return true;
		    }
		    s1 = s1.toLowerCase();
		    s2 = s2.toLowerCase();
		    if (s2.indexOf(s1) != -1) {
		        return true;
		    }
		    else {
		        return false;
		    }
		}

		$scope.sortID = "id";
	
		$scope.date_in = "Date In ►";
		$scope.date_out = "Date Out ►";
		$scope.slot_id = "Slot ID ▲";
		$scope.duration = "Duration ►";

	
		
		$scope.OrderByDIn = function (p) {
		    $scope.duration = "Duration ►";
		    $scope.date_out = "Date Out ►";
		    $scope.slot_id = "Slot ID ►";
		    if ($scope.date_in == "Date In ▲") {
		        $scope.sortID = "-date_in";
		        $scope.date_in = "Date In ▼";
		    }
		    else {
		        $scope.sortID = "date_in";
		        $scope.date_in = "Date In ▲";
		    }
		}
		$scope.OrderByDOut = function (p) {
		    $scope.duration = "Duration ►";
		    $scope.date_in = "Date In ►";
		    $scope.slot_id = "Slot ID ►";
		    if ($scope.date_out == "Date Out ▲") {
		        $scope.sortID = "-date_out";
		        $scope.date_out = "Date Out ▼";
		    }
		    else {
		        $scope.sortID = "date_out";
		        $scope.date_out = "Date Out ▲";
		    }
		}
		$scope.OrderBySID = function (p) {
		    $scope.duration = "Duration ►";
		    $scope.date_in = "Date In ►";
		    $scope.date_out = "Date Out ►";
		    if ($scope.slot_id == "Slot ID ▲") {
		        $scope.sortID = "-slot_id";
		        $scope.slot_id = "Slot ID ▼";
		    }
		    else {
		        $scope.sortID = "slot_id";
		        $scope.slot_id = "Slot ID ▲";
		    }
		}
		$scope.OrderByDur = function (p) {
		    $scope.slot_id = "Slot ID ►";
		    $scope.date_in = "Date In ►";
		    $scope.date_out = "Date Out ►";
		    if ($scope.duration == "Duration ▲") {
		        $scope.sortID = "-slot_id";
		        $scope.duration = "Duration ▼";
		    }
		    else {
		        $scope.sortID = "slot_id";
		        $scope.duration = "Duration ▲";
		    }
		}

		$http.get(garage.rootPath + 'Vehicle/Types').then(function (resp) {
		    $scope.vehicleTypes = resp.data;
		});

		$scope.typeFilterOn = function (p) {
		    if ($scope.typeId == -1 || $scope.typeId == null) { return true; }
		    return p.vehicle.type.id == $scope.typeId;
		}

	});
}());