//Define tournaments controller
angular.module('TournamentModule').controller('addTournamentController', ['$scope', '$http', '$stateParams', '$window', '$state','AuthenticationService', addTournamentController]);

function addTournamentController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    var Place = undefined;
    var StartDate = undefined;
    var EndDate = undefined;

    initController();

    function initController() {
        // reset login status
        AuthenticationService.CheckIsStoraged();
    };

    $scope.addtournamentdata = {
        AspNetUserId: undefined,
        StarTime: undefined,
        EndTime: undefined,
        Type: undefined,
        Name: undefined
    }
    //initial and handle objects and events on google map
    angular.extend($scope, {
        map: {
            center: {
                latitude: 45.5511111,
                longitude: 18.6938889
            },
            zoom: 11,
            markers: [],
            markersEvents: {
                click: function (marker, eventName, model) {
                    $scope.map.window.model = model;
                    $scope.map.window.show = true;
                    $scope.address = Place;
                }
            },
            events: {
                click: function (map, eventName, originalEventArgs) {
                    var e = originalEventArgs[0];
                    var geocoder = new google.maps.Geocoder();
                    var lat = e.latLng.lat(), lon = e.latLng.lng();
                    var latlng = new google.maps.LatLng(e.latLng.lat(), e.latLng.lng());
                    //get place/address grom geocoder
                    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {
                            if (results[1]) {
                                //console.log(results[1].formatted_address); // details address
                                Place = results[1].formatted_address;
                            } else {
                                console.log('Location not found');
                                address = 'Address is unknown';
                            }
                        } else {
                            console.log('Geocoder failed due to: ' + status);
                        }
                    });
                    //set id and longitude and latitude for marker
                    var marker = {
                        id: Date.now(),
                        coords: {
                            latitude: lat,
                            longitude: lon
                        }
                    };
                    while ($scope.map.markers.length > 0) {
                        $scope.map.markers.pop();
                    }
                    $scope.map.markers.push(marker);
                    $scope.$apply();
                }
            },
            window: {
                marker: {},
                show: false,
                closeClick: function () {
                    this.show = false;
                },
                options: {}
            }
        }
    });

    //set watchers on date pickers
    //for server, date need to be in YYYY-MM-DD format
    $scope.$watch('StartDate', function (value) {
        try {
            StartDate = new Date(value);
            $scope.addtournamentdata.StartTime = StartDate.toISOString().slice(0, 10).replace(/-/g, "-");
        } catch (e) { }
        //haven't been used in ng-error
        if (!StartDate) {
            $scope.error = "This is not a valid date";
        } else {
            $scope.error = false;
        }
    });

    $scope.$watch('EndDate', function (value) {
        try {
            EndDate = new Date(value);
            $scope.addtournamentdata.EndTime = EndDate.toISOString().slice(0, 10).replace(/-/g, "-");;
        } catch (e) { }
        //haven't been used in ng-error
        if (!EndDate) {
            $scope.error = "This is not a valid date";
        } else {
            $scope.error = false;
        }
    });

    $scope.addtournament = function () {
        if ($scope.addtournamentdata.Name == undefined) {
            $window.alert("Please add tournament name.");
        }
        else if ($scope.addtournamentdata.Type == undefined) {
            $window.alert("Please select tournament type.");
        }
        //else if($scope.map.markers[0] == undefined) {
        //    $window.alert("Please mark tournament position.");
        //}
        else if ($scope.addtournamentdata.StartTime == undefined) {
            $window.alert("Please select start date.");
        }
        else if ($scope.addtournamentdata.EndTime == undefined) {
            $window.alert("Please select end date.");
        }
        //else if ($scope.addtournamentdata.AspNetUserId == undefined) {
        //    $window.alert("To add tournament you need to log in first.");
        //}
        else {
            
            console.log(AuthenticationService.GetUsername());
            //TODO: post method
            console.log($scope.addtournamentdata.Name);
            console.log($scope.addtournamentdata.Type);
            console.log($scope.addtournamentdata.StartTime);
            console.log($scope.addtournamentdata.EndTime);
            //console.log($scope.map.markers[0].coords.latitude);
            //console.log($scope.map.markers[0].coords.longitude);
            //console.log(Place);
            //$scope.map.markers[0].coords.latitude = "45.00000"
            //$scope.map.markers[0].coords.longitude = "45.000000"
            Place = "Osijek,Croatia";
            Description = "None";
            var tournament = {
                UserName: AuthenticationService.GetUsername(),
                StarTime: $scope.addtournamentdata.StartTime,
                EndTime: $scope.addtournamentdata.EndTime,
                Type: $scope.addtournamentdata.Type,
                Name: $scope.addtournamentdata.Name
            };
            var location = {
                Place: Place,
                Description: Description,
                Longitude: "45.00000",
                Latitude: "45.00000"
            }

            $http.post('/api/tournament/add', tournament, location)
                    .then(function (response) {
                        console.log(response);
                        $window.alert("Sad cu dodat turnir.");
                    }, function (response) {
                        $window.alert("Can't add tournament.");
                    });
        }
    }

}