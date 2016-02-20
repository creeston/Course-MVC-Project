var EducationApp;
(function (EducationApp) {
    var Controllers;
    (function (Controllers) {
        var testController = (function () {
            function testController($scope) {
                this.scope = $scope;
                this.data = ["One, Two, Three"];
            }
            return testController;
        })();
        Controllers.testController = testController;
    })(Controllers = EducationApp.Controllers || (EducationApp.Controllers = {}));
})(EducationApp || (EducationApp = {}));
//# sourceMappingURL=testController.js.map