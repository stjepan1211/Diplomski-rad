//Define tournaments controller
angular.module('TournamentModule').controller('addTournamentController', ['$scope', '$http', '$stateParams', '$window', '$state','AuthenticationService','uiGmapGoogleMapApi', addTournamentController]);

function addTournamentController($scope, $http, $stateParams, $window, $state, AuthenticationService, uiGmapGoogleMapApi) {

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
        Name: undefined,
        NumberOfTeams: undefined
    }

    //uiGmapGoogleMapApi.then(function (maps) {
    //    // Configuration needed to display the road-map with traffic
    //    // Displaying Ile-de-france (Paris neighbourhood)
    //    maps.visualRefresh = true;
    //    $scope.map = {
    //        center: {
    //            latitude: -23.598763,
    //            longitude: -46.676547
    //        },
    //        zoom: 13,
    //        options: {
    //            mapTypeId: google.maps.MapTypeId.ROADMAP, // This is an example of a variable that cannot be placed outside of uiGmapGooogleMapApi without forcing of calling ( like ugly people ) the google.map helper outside of the function
    //            streetViewControl: false,
    //            mapTypeControl: false,
    //            scaleControl: false,
    //            rotateControl: false,
    //            zoomControl: false
    //        },
    //        showTraficLayer: true
    //    };
    //    console.log($scope.map);
    //    $scope.isOffline = false;
    //})
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
        } catch (e) { }
        //haven't been used in ng-error
        if (!EndDate) {
            $scope.error = "This is not a valid date";
        } else {
            $scope.error = false;
        }
    });

    $scope.addtournament = function () {

        //StartDate.setHours(0, 0, 0);
        $scope.addtournamentdata.StartTime = StartDate.toISOString().slice(0, 10).replace(/-/g, "-");
        //EndDate.setHours(0, 0, 0);
        $scope.addtournamentdata.EndTime = EndDate.toISOString().slice(0, 10).replace(/-/g, "-");

        if ($scope.addtournamentdata.Name == undefined) {
            $window.alert("Please add tournament name.");
        }
        else if ($scope.addtournamentdata.Type == undefined) {
            $window.alert("Please select tournament type.");
        }
        else if($scope.map.markers[0] == undefined) {
            $window.alert("Please mark tournament position.");
        }
        else if ($scope.addtournamentdata.StartTime == undefined) {
            $window.alert("Please select start date.");
        }
        else if ($scope.addtournamentdata.EndTime == undefined) {
            $window.alert("Please select end date.");
        }
        else if ($scope.addtournamentdata.NumberOfTeams == undefined) {
            $window.alert("Please add number of teams. It can be between 2 and 32.");
        }
        //else if ($scope.addtournamentdata.AspNetUserId == undefined) {
        //    $window.alert("To add tournament you need to log in first.");
        //}
        else {
            //console.log($scope.addtournamentdata.Name);
            //console.log($scope.addtournamentdata.Type);
            //console.log($scope.addtournamentdata.StartTime);
            //console.log($scope.addtournamentdata.EndTime);
            //console.log($scope.map.markers[0].coords.latitude);
            //console.log($scope.map.markers[0].coords.longitude);
            //console.log(Place);

            //$scope.map.markers[0].coords.latitude = "45.00000"
            //$scope.map.markers[0].coords.longitude = "45.000000"
            //Place = "Osijek,Croatia";
            var Description = "None";
            var tournament = {
                AspNetUserId: AuthenticationService.GetId(),
                UserName: AuthenticationService.GetUsername(),
                StartTime: $scope.addtournamentdata.StartTime,
                EndTime: $scope.addtournamentdata.EndTime,
                Type: $scope.addtournamentdata.Type,
                Name: $scope.addtournamentdata.Name,
                NumberOfTeams: $scope.addtournamentdata.NumberOfTeams
            };
            var location = {
                Place: Place,
                Description: Description,
                Longitude: $scope.map.markers[0].coords.longitude,
                Latitude: $scope.map.markers[0].coords.latitude
            }

            //post two different objects
            $http({
                method: 'POST',
                url: '/api/result/add',
                data: {
                    tournament: tournament,
                    location: location
                }
            }).then(function (response) {
                        console.log(response);
                        $window.alert("Tournament added successfully.");
                        $state.go('mytournaments');
                    }, function (response) {
                        $window.alert("Error: " + response.data.Message);
                    });
        }
    }

}