angular.module('TournamentModule').factory('AuthenticationService', Service);

function Service($http, $localStorage) {

    var service = {};

    service.Login = Login;
    service.Logout = Logout;

    return service;

    function Login(username, password) {
        var unvalidateUser = {
            username: username,
            password: password
        }
        $http.post('/api/authenticate', unvalidateUser)
            .success(function (response) {
                // login successful if there's a token in the response
                if (response.token) {
                    // store username and token in local storage to keep user logged in between page refreshes
                    $localStorage.currentUser = { username: username, token: response.token };

                    // add jwt token to auth header for all requests made by the $http service
                    $http.defaults.headers.common.Authorization = 'Bearer ' + response.token;
                }
            });
    }

    function Logout() {
        // remove user from local storage and clear http auth header
        delete $localStorage.currentUser;
        $http.defaults.headers.common.Authorization = '';
    }
}
