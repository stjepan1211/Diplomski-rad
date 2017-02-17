angular.module('TournamentModule').factory('PromiseUtilsService', Service);

function Service($q) {
    return {
        getPromiseHttpResult: function (httpPromise) {
            var deferred = $q.defer();
            httpPromise.then(function (data) {
                deferred.resolve(data);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        }
    }
}