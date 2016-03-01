(function () {
    var app = angular.module('base', ['ngRoute']);

    app.factory('garageService', function ($http) {
        var g = {};

        g.getParkings = function (skipNulls, id, fn) {
            var url = garage.rootPath + 'Garage/Parkings';
            if (id)
                url += '/' + id;
            $http.get(url).then(function (resp) {
                var parkings = [];
                var data = resp.data;
                if (skipNulls) {
                    for (var i = 0; i < data.length; ++i) {
                        if (data[i].date_out == null)
                            parkings.push(data[i]);
                    }
                } else {
                    parkings = data;
                    for (var i = 0; i < parkings.length; ++i) {
                        parkings[i].date_in = garage.parseMSDate(parkings[i].date_in); //fixing datetime format for js
                        parkings[i].date_out = garage.parseMSDate(parkings[i].date_out);
                        var d = parkings[i].date_out;
                        if (d == null) { d = new Date(); }
                        parkings[i].duration = Math.ceil((d - parkings[i].date_in) / 1000 / 60 / 60);
                    }
                }
                fn(parkings);
            });
        };

        g.getTypes = function (fn) {
            $http.get(garage.rootPath + 'Vehicle/Types').then(function (resp) {
                fn(resp.data);
            });
        };

        g.getSlots = function (fn) {
            $http.get(garage.rootPath + 'Garage/Slots').then(function (resp) {
                fn(resp.data);
            });
        };

        g.getDurationString = function (d2, d1) {
            if (!d2) d2 = new Date();
            var secs = Math.floor((d2 - d1.getTime()) / 1000);
            var days = Math.floor(secs / (3600 * 24));
            secs -= (days * 3600 * 24);
            var hours = Math.floor(secs / 3600);
            secs -= (hours * 3600);
            var mins = Math.floor(secs / 60);
            return days + ' days, ' + hours + ' hours, ' + mins + ' mins';
        };

        g.park = function (data, onSuccess, onError) {
            $http.post(garage.rootPath + 'Garage/Park', data).then(function (resp) {
                onSuccess(resp.data);
            },
            function (reason) {
                onError(reason);
            });
        };

        g.unpark = function (data, fn) {
            $http.put(garage.rootPath + 'Garage/Unpark', data).then(function (resp) {
                fn(resp.statusText);
            });
        };

        return g;
    });
}());