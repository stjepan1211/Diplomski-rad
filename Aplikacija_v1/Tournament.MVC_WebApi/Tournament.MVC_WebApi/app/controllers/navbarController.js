//Define navbar controller
angular.module('TournamentModule').controller('navbarController', ['$scope', '$http', '$stateParams', '$window', '$state', 'AuthenticationService', navbarController]);

function navbarController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    initController();
  
    function initController() {
        AuthenticationService.CheckIsStoraged();
        if (AuthenticationService.Check()) {
            //if user is logged hide log in element
            $scope.isLogged = false;
        }
        else {
            //if user is not logged show log in element
            $scope.isLogged = true;
        }
    }

    $scope.Check = function() {
        $window.alert("usus");
        $scope.isLogged = false;
        AuthenticationService.CheckIsStoraged();
        if (AuthenticationService.Check()) {
            //if user is logged hide log in element
            $scope.isLogged = false;
        }
        else {
            //if user is not logged show log in element
            $scope.isLogged = true;
        }
    }

    $scope.LogOut = function () {
        if ($window.confirm('Are you sure you want to log out?')) {
            AuthenticationService.Logout();
            $state.transitionTo('home', null, {'reload': true });
        };
    }

    //$state.get('home').onEnter = $window.alert("ovo je on enter");
}