//Define log in controller
angular.module('TournamentModule').controller('loginController', ['$scope', '$http', '$stateParams', '$window', '$state','$location','AuthenticationService', 'md5', loginController]);

function loginController($scope, $http, $stateParams, $window, $state, $location, AuthenticationService, md5) {

    $scope.loginData = {
        UserName: undefined,
        Password: undefined
    }

    
    $scope.$on('LOAD', function () {
        $scope.loading = true;
    })
    $scope.$on('UNLOAD', function () {
        $scope.loading = false;
    })

    initController();

    function initController() {
        // reset login status
        AuthenticationService.CheckIsStoraged(); 
    };


    $scope.login = function () {
        
        var userToLogin = {
            UserName: $scope.loginData.UserName,
            Password: undefined
        }
        userToLogin.Password = md5.createHash($scope.loginData.Password || '');

        $scope.$emit('LOAD');

        AuthenticationService.Login(userToLogin.UserName, userToLogin.Password, function (result) {

            if (result === true) {
                $scope.$emit('UNLOAD');
                $window.alert("You are logged.");
                $state.go('home');
                location.reload(true);
            } else if (result == 404) {
                $scope.$emit('UNLOAD');
                $window.alert("Username not found.");
            } else if (result == 400) {
                $scope.$emit('UNLOAD');
                $window.alert("Password is incorrect.");
            } else {
                $scope.$emit('UNLOAD');
                $window.alert("Can't log in.");
            }
        });
    };
}