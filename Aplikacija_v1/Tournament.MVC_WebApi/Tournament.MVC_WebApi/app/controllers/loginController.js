﻿//Define log in controller
angular.module('TournamentModule').controller('loginController', ['$scope', '$http', '$stateParams', '$window', '$state','$location','AuthenticationService', 'md5', loginController]);

function loginController($scope, $http, $stateParams, $window, $state, $location, AuthenticationService, md5) {

    $scope.loginData = {
        UserName: undefined,
        Password: undefined
    }

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

        AuthenticationService.Login(userToLogin.UserName, userToLogin.Password, function (result) {
            if (result === true) {
                $window.alert("You are logged.");
                //$state.transitionTo('home', null, { reload: true, inherit: true, notify: true });
                $state.go('home');
            } else if (result == 404) {
                $window.alert("Username not found.");
            } else if (result == 400) {
                $window.alert("Password is incorrect.");
            } else {
                $window.alert("Can't log in.");
            }
        });
    };
}