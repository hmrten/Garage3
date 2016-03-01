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
            
				    var DateRightNow = parkings[i].date_out;
				    if (DateRightNow==null){DateRightNow=new Date();}

				    parkings[i].duration = Math.ceil((DateRightNow - parkings[i].date_in) / 1000 / 60 / 60);
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
		$scope.sekprice = "Price ►";

	
		
		$scope.OrderByDIn = function (p) {
		    $scope.duration = "Duration ►";
		    $scope.date_out = "Date Out ►";
		    $scope.slot_id = "Slot ID ►";
		    $scope.sekprice = "Price ►";
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
		    $scope.sekprice = "Price ►";
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
		    $scope.sekprice = "Price ►";
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
		    $scope.sekprice = "Price ►";
		    if ($scope.duration == "Duration ▲") {
		        $scope.sortID = "-duration";
		        $scope.duration = "Duration ▼";
		    }
		    else {
		        $scope.sortID = "duration";
		        $scope.duration = "Duration ▲";
		    }
		}
		$scope.OrderByPrice = function (p) {
		    $scope.slot_id = "Slot ID ►";
		    $scope.date_in = "Date In ►";
		    $scope.date_out = "Date Out ►";
		    $scope.duration = "Duration ►";
		    if ($scope.sekprice == "Price ▲") {
		        $scope.sortID = "-price";
		        $scope.sekprice = "Price ▼";
		    }
		    else {
		        $scope.sortID = "price";
		        $scope.sekprice = "Price ▲";
		    }
		}
		$scope.filterPrice = 20;

	    $scope.price =function(p){
	        if (angular.isNumber($scope.filterPrice) && ($scope.filterPrice != null || $scope.filterPrice != 0)) {
	            p.price = p.duration * $scope.filterPrice;
	            return (p.duration * $scope.filterPrice);
	        }
	        
	        else {
	            p.price = 0;
	            return 0;
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