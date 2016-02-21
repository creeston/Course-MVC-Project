/// <reference path="../typings/signalr/signalr.d.ts"/>

declare var Dropzone: any;

angular.module('dropzone', []).directive('dropzone', function () {
    return function (scope, element, attrs) {
        var config, dropzone;

        //config = scope.dropzone.dropzoneConfig;
        config = scope.template.dropzoneConfig;
        config.options.headers = { 'width': element[0].offsetWidth, 'height': element[0].offsetHeight, 'index': element[0].id };

        // create a Dropzone for the element with the given options
        dropzone = new Dropzone(element[0], config.options);

        // bind the given event handlers
        angular.forEach(config.eventHandlers, function (handler, event) {
            dropzone.on(event, handler);
        });
    };
});


var appModule = angular.module("educationModule", 
	['ngSanitize', 'wiz.markdown', 'dropzone', 'youtube-embed', 'ngMessages', 'ui.bootstrap']);

appModule.controller("MarkdownController", ["$scope", '$http', 'wizMarkdownSvc', ($scope, $http, wizMarkdownSvc) =>
    new EducationApp.Controllers.MarkdownController($scope, $http, wizMarkdownSvc)]);
appModule.controller("TemplateController", ["$scope", 'wizMarkdownSvc', ($scope, wizMarkdownSvc) =>
    new EducationApp.Controllers.TemplateController($scope, wizMarkdownSvc)]);
appModule.controller("PublicateController", ["$scope", '$http', ($scope, $http) =>
    new EducationApp.Controllers.PublicateController($scope, $http)]);
appModule.controller("PostController", ["$scope", '$http', 'wizMarkdownSvc',($scope, $http, wizMarkdownSvc) =>
    new EducationApp.Controllers.PostController($scope, $http, wizMarkdownSvc)]);
appModule.controller("IndexController", ['$scope', '$http', ($scope, $http) =>
    new EducationApp.Controllers.IndexController($scope, $http)]);
appModule.controller("UserInfoController", ['$scope', '$http', ($scope, $http) =>
    new EducationApp.Controllers.UserInfoController($scope, $http)]);

appModule.filter('trusted', ['$sce', function ($sce) {
    return (url) => { return $sce.trustAsResourceUrl(url); };
}]);

appModule.directive('templateDropzone', () => new EducationApp.Directives.dropZoneContainer());
appModule.directive('templateMarkdown', () => new EducationApp.Directives.markDownContainer());
appModule.directive('templateHeader', () => new EducationApp.Directives.headerContainer());