//Define log in controller
angular.module('TournamentModule').controller('loginController', ['$scope', '$http', '$stateParams', '$window', '$state','AuthenticationService', 'md5', loginController]);

function loginController($scope, $http, $stateParams, $window, $state, AuthenticationService, md5) {

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
            console.log("Error: " + response.error);
        });
    }

    initController();

    function initController() {
        // reset login status
        AuthenticationService.Logout();
    };

    $scope.login = function () {
        var userToLogin = {
            UserName: $scope.loginData.UserName,
            PasswordHash: undefined
        }
        userToLogin.PasswordHash = md5.createHash($scope.loginData.Password || '');
        $scope.loginData.loading = true;
        //ovdje jos provjeriti je li password ispravan, ili sve gore u checkIsRegistered
        //ali onda treba i preko backenda provjeriti pass
        //AuthenticationService.Login($scope.loginData.UserName, $scope.loginData.Password);
        $http.post('/api/aspnetuser/logintoken', userToLogin)
            .then(function (response) {
                if (response)
                $window.alert("You are logged.");
                console.log(response.error)
                
            });
    };

    $scope.ispis = function () {
    }
}