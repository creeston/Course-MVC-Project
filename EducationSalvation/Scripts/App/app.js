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
var appModule = angular.module("educationModule", ['ngSanitize', 'wiz.markdown', 'dropzone', 'youtube-embed', 'ngMessages', 'ui.bootstrap']);
appModule.controller("MarkdownController", ["$scope", '$http', 'wizMarkdownSvc', function ($scope, $http, wizMarkdownSvc) {
        return new EducationApp.Controllers.MarkdownController($scope, $http, wizMarkdownSvc);
    }]);
appModule.controller("TemplateController", ["$scope", 'wizMarkdownSvc', function ($scope, wizMarkdownSvc) {
        return new EducationApp.Controllers.TemplateController($scope, wizMarkdownSvc);
    }]);
appModule.controller("PublicateController", ["$scope", '$http', function ($scope, $http) {
        return new EducationApp.Controllers.PublicateController($scope, $http);
    }]);
appModule.controller("PostController", ["$scope", '$http', 'wizMarkdownSvc', function ($scope, $http, wizMarkdownSvc) {
        return new EducationApp.Controllers.PostController($scope, $http, wizMarkdownSvc);
    }]);
appModule.controller("IndexController", ['$scope', '$http', function ($scope, $http) {
        return new EducationApp.Controllers.IndexController($scope, $http);
    }]);
appModule.filter('trusted', ['$sce', function ($sce) {
        return function (url) { return $sce.trustAsResourceUrl(url); };
    }]);
appModule.directive('templateDropzone', function () { return new EducationApp.Directives.dropZoneContainer(); });
appModule.directive('templateMarkdown', function () { return new EducationApp.Directives.markDownContainer(); });
appModule.directive('templateHeader', function () { return new EducationApp.Directives.headerContainer(); });
//# sourceMappingURL=app.js.map