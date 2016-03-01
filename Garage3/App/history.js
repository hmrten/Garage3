(function () {
	var app = angular.module('history', ['base']);

	app.controller('historyCtrl', function ($scope, $http, garageService) {
		function init() {
		    $scope.sortID = "id";
		    $scope.date_in = "Date In ►";
		    $scope.date_out = "Date Out ►";
		    $scope.slot_id = "Slot ID ▲";
		    $scope.duration = "Duration ►";

		    $scope.filterPrice = 20;

		    garageService.getParkings(false, null, function (parkings) { $scope.parkings = parkings; });
		    garageService.getTypes(function (types) { $scope.vehicleTypes = types; });
		}

		init();

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
		        $scope.sortID = "-duration";
		        $scope.duration = "Duration ▼";
		    }
		    else {
		        $scope.sortID = "duration";
		        $scope.duration = "Duration ▲";
		    }
		}

		$scope.price = function (p) {
		    if (angular.isNumber($scope.filterPrice) && ($scope.filterPrice != null || $scope.filterPrice != 0)) {
	                return (p.duration * $scope.filterPrice)
		    }
		    return 0;
	    }

		$scope.typeFilterOn = function (p) {
		    if ($scope.typeId == -1 || $scope.typeId == null) { return true; }
		    return p.vehicle.type.id == $scope.typeId;
		}

	});
}());