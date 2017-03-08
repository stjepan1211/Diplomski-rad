//Define sign up controller
angular.module('TournamentModule').controller('signupController', ['$scope', '$http', '$stateParams', '$window', '$state', '$location', 'md5', signupController]);

function signupController($scope, $http, $stateParams, $window, $state, $location, md5) {

    $scope.$on('LOAD', function () {
        $scope.loading = true;
    })
    $scope.$on('UNLOAD', function () {
        $scope.loading = false;
    })

    var takenUsernames = {
        UserName: undefined
    };
    var takenEmails = {
        Email: undefined
    };
    $scope.confirmedPassword = undefined;
    //initialize sign up data
    $scope.signupData = {
        Address: undefined,
        Place: undefined,
        Age: undefined,
        Name: undefined,
        LastName: undefined,
        PhoneNumber: undefined,
        Email: undefined,
        UserName: undefined,
        PasswordHash: undefined,
    }

    $scope.getUsers = function() {
        $http.get('/api/aspnetuser/getusernames').then(function (response) {
            takenUsernames = response.data;
        }, function (response) {
            console.log("Couldn't get response");
        })
    }
    $scope.getEmails = function () {
        $http.get('/api/aspnetuser/getemails').then(function (response) {
            takenEmails = response.data;
        }, function (response) {
            console.log("Couldn't get response");
        })
    }
    $scope.ispis = function () {
        console.log(takenUsernames);
        console.log(takenUsernames.length);
        for (var i = 0; i < takenUsernames.length; i++) {
            if(takenUsernames[i].UserName == "sbaricevic")
            console.log(takenUsernames[i].UserName)
        }
    }

    $scope.addUser = function () {
        if ($scope.signupData.Address != undefined && $scope.signupData.Place != undefined && $scope.signupData.Age != undefined
            && $scope.signupData.Name != undefined && $scope.signupData.LastName != undefined && $scope.signupData.PhoneNumber != undefined
            && $scope.signupData.Email != undefined && $scope.signupData.UserName != undefined && $scope.signupData.PasswordHash != undefined)

            if ($scope.signupData.PasswordHash != $scope.confirmedPassword) {
                $window.alert("Confirmed password doesn't match first password.");
            }
            else if ($scope.signupData.PasswordHash.length < 6) {
                $window.alert("Password is too short.");
            }
            else
            {
                $scope.$emit('LOAD');

                var user = {
                    Address: $scope.signupData.Address,
                    Place: $scope.signupData.Place,
                    Age: $scope.signupData.Age,
                    Name: $scope.signupData.Name,
                    LastName: $scope.signupData.LastName,
                    PhoneNumber: $scope.signupData.PhoneNumber,
                    Email: $scope.signupData.Email,
                    UserName: $scope.signupData.UserName,
                    PasswordHash: undefined
                };

                for (var i = 0; i < takenUsernames.length; i++) {
                    if (takenUsernames[i].UserName == $scope.signupData.UserName) {
                        return $window.alert("Username is already taken.");
                    }
                }
                for (var i = 0; i < takenEmails.length; i++) {
                    if (takenEmails[i].Email == $scope.signupData.Email) {
                        return $window.alert("Email is already taken.");
                    }
                }

                user.PasswordHash = md5.createHash($scope.signupData.PasswordHash || '');

                $http.post('/api/aspnetuser/add', user)
                    .then(function (response) {
                        $scope.$emit('UNLOAD');
                        $window.alert("You are registered.");
                        $location.path('/login');
                    }, function (response) {
                        $window.alert("Can't register user.");
                    });
            }
    }
}

