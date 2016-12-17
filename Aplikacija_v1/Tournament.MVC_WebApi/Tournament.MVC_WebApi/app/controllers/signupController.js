//Define sign up controller
angular.module('TournamentModule').controller('signupController', ['$scope', '$http', '$stateParams', '$window', '$state', signupController]);

function signupController($scope, $http, $stateParams, $window, $state) {

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
    $scope.confirmedPassword = undefined;

    $scope.addUser = function () {
        if ($scope.signupData.Address != undefined && $scope.signupData.Place != undefined && $scope.signupData.Age != undefined
            && $scope.signupData.Name != undefined && $scope.signupData.LastName != undefined && $scope.signupData.PhoneNumber != undefined
            && $scope.signupData.Email != undefined && $scope.signupData.UserName != undefined && $scope.signupData.PasswordHash != undefined) {

            if ($scope.signupData.PasswordHash != $scope.confirmedPassword) {
                $window.alert("Confirmed password doesn't match first password.");
            }
            else
            {
                var user = {
                    Address: $scope.signupData.Address,
                    Place: $scope.signupData.Place,
                    Age: $scope.signupData.Age,
                    Name: $scope.signupData.Name,
                    LastName: $scope.signupData.LastName,
                    PhoneNumber: $scope.signupData.PhoneNumber,
                    Email: $scope.signupData.Email,
                    UserName: $scope.signupData.UserName,
                    PasswordHash: $scope.signupData.PasswordHash
                };

                $http.post('/api/aspnetuser/add', user).success(function (data) {
                    $scope.response = data;
                    console.log(data);
                    $window.alert("Success");
                })
                .error(function (data) {
                    $scope.error = "An error has occured while trying to add user";
                    $window.alert("Error! " + data.Message);
                });
            }
           
        }
    }

    $scope.ispis = function () {
        $window.alert("ispis");
        console.log($scope.signupData.Address);
        console.log($scope.signupData.Place);
        console.log($scope.signupData.Age);
        console.log($scope.signupData.Name);
        console.log($scope.signupData.LastName);
        console.log($scope.signupData.PhoneNumber);
        console.log($scope.signupData.Email);
        console.log($scope.signupData.UserName);
        console.log($scope.signupData.PasswordHash);
        console.log($scope.signupData.Name);
        console.log($scope.confirmedPassword);
    }

}