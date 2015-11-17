zenergyApp.factory('authInterceptorService', ['$q', '$location', 'tokenService', function ($q, $location, tokenService) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {

        //TODO : vérifier les id sur api/users/{:id}
        config.headers = config.headers || {};

        if (tokenService.tokenExists()) {
            config.headers.Authorization = 'Bearer ' + tokenService.getToken();
        }

        return config;
    }

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);

zenergyApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});
