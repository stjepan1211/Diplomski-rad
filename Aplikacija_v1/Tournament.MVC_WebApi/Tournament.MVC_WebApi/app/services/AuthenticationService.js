﻿angular.module('TournamentModule').factory('AuthenticationService', Service);

function Service($http, $localStorage, localStorageService) {

    var service = {};

    service.Login = Login;
    service.Logout = Logout;
    service.CheckIsStoraged = CheckIsStoraged;
    service.Check = Check;
    return service;

    function Login(username, password, callback) {
        var obj = {
            UserName: username,
            Password: password
        };
        $http.post('/api/aspnetuserlogin/logintoken', obj)
        .then(function successCallback(response) {
            if (response.data.Token) {
                // store username and token in local storage
                $localStorage.currentUser = {
                    UserName: response.UserName,
                    Token: response.Token
                };
                // add jwt token to auth header for all requests made by the $http service
                console.log(response.data.Token.TokenString);
                $http.defaults.headers.common.Authorization = 'Bearer ' + response.data.Token.TokenString;
                // execute callback with true to indicate successful login
                callback(true);
            } else {
                // execute callback with false to indicate failed login
                callback(false);
            } 
        }, function errorCallback(response) {
            if (response.status == 404) {
                // execute callback with 404 to indicate that username is not found
                callback(404);
            }
            else if (response.status == 400) {
                // execute callback with 404 to indicate that password is incorrect
                callback(400);
            }
            else
                callback(false);
        });
    }

    function Logout() {
        // remove user from local storage and clear http auth header
        delete $localStorage.currentUser;
        $http.defaults.headers.common.Authorization = '';
    }

    function CheckIsStoraged() {
        var dateTime = new Date();
        var miliseconds = dateTime.getTime();

        //localStorageService.get(currentUser);
        //if ($localStorage.getItem("currentUser") != null) {
            
        //    if ($localStorage.currentUser.Token.ExpirationTime < miliseconds) {
        //        Logout();
        //    }
        //}
        if ($http.defaults.headers.common.Authorization == '')
            console.log("nije registriran");
        else
            console.log("registriran je");

    }
    function Check() {
        if (localStorageService.isSupported) {
            return localStorageService.keys();
        }
    }
}
