//Define log in controller
angular.module('TournamentModule').controller('loginController', ['$scope', '$http', '$stateParams', '$window', '$state','AuthenticationService', loginController]);

function loginController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    $scope.loginData = {
        UserName: undefined,
        Password: undefined
    }

    $scope.checkIsRegistered = function () {
        $http.get('api/aspnetuser/getbyusername', {
            params: { username: $scope.loginData.UserName }
        })
        .then(function (response) {
            if (response.data == null) {
                $window.alert("You need to sign up first.");
            }
        }, function (response) {
            Console.log("No response getbyusername.")
        });
    }

    initController();

    function initController() {
        // reset login status
        AuthenticationService.Logout();
    };

    $scope.login = function () {
        $scope.loginData.loading = true;
        //ovdje jos provjeriti je li password ispravan, ili sve gore u checkIsRegistered
        //ali onda treba i preko backenda provjeriti pass
        AuthenticationService.Login($scope.loginData.UserName, $scope.loginData.Password);
        $location.path('/');
    };

    $scope.ispis = function () {
    }
}