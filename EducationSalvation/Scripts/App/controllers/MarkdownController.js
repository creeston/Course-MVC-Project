var EducationApp;
(function (EducationApp) {
    var Controllers;
    (function (Controllers) {
        var IndexController = (function () {
            function IndexController($scope, $http) {
                this.scope = $scope;
                this.http = $http;
            }
            IndexController.prototype.getThumbnails = function () {
                var _this = this;
                this.http({ method: 'GET', url: 'GetPublicationThumbnails' })
                    .success(function (data) { _this.publicationThumbnails = data; });
            };
            return IndexController;
        })();
        Controllers.IndexController = IndexController;
        var TemplateController = (function () {
            function TemplateController($scope, wizMarkdownSvc) {
                var _this = this;
                this.images = new Array();
                this.videos = new Array();
                this.tags = new Array();
                this.extraTags = new Array();
                this.previewMode = false;
                var template = this;
                this.scope = $scope;
                this.dropzoneConfig = {
                    'options': {
                        'url': 'SaveUploadedFile',
                        'maxFiles': 1,
                        'addRemoveLinks': 'true'
                    },
                    'eventHandlers': {
                        'success': function (file, response) {
                            template.images[response.Index] = response.Url;
                            template.scope.$apply();
                        },
                        'removedfile': function (file) {
                        }
                    }
                };
                this.scope.$watch(function () { return _this.tags; }, function (newValue, oldValue) {
                    if (newValue[newValue.length - 1] == "")
                        template.helpfulTags = new Array();
                    else
                        template.helpfulTags = template.extraTags.filter(function (x) { return x.indexOf(newValue[newValue.length - 1]) == 0; });
                });
            }
            TemplateController.prototype.showPreview = function () {
                this.previewMode = !this.previewMode;
            };
            TemplateController.prototype.setTag = function (helpfulTag) {
                var tags = angular.copy(this.tags);
                tags[tags.length - 1] = helpfulTag;
                this.tags = tags;
            };
            return TemplateController;
        })();
        Controllers.TemplateController = TemplateController;
        var MarkdownController = (function () {
            function MarkdownController($scope, $http, wizMarkdownSvc) {
                this.http = $http;
                this.scope = $scope;
                this.GetTemplates();
                this.GetTags($scope);
            }
            MarkdownController.prototype.GetTemplates = function () {
                var _this = this;
                this.http({ method: 'GET', url: 'GetTemplates' })
                    .success(function (data) { _this.templates = data; });
            };
            MarkdownController.prototype.GetTags = function (scope) {
                this.http({ method: 'GET', url: 'GetTags' })
                    .success(function (data) {
                    var templateScope = scope.$parent['template'];
                    templateScope['extraTags'] = data;
                });
            };
            return MarkdownController;
        })();
        Controllers.MarkdownController = MarkdownController;
        var PublicateController = (function () {
            function PublicateController($scope, $http) {
                this.http = $http;
                this.scope = $scope;
            }
            PublicateController.prototype.Publish = function () {
                var templateScope = this.scope.$parent['template'];
                var markedScope = this.scope['$$prevSibling']['marked'];
                var template = markedScope['template']['Name'];
                var title = templateScope['title'];
                var description = templateScope['description'];
                var tags = templateScope['tags'].join(' ');
                if (title == "" || description == "" || tags == "") {
                    return;
                }
                var images = templateScope['images'].join(' ');
                var videos = templateScope['videos'].join(' ');
                var markdown = templateScope['markdown'];
                var date = moment().format("YYYY-MM-DD HH:mm:ss").toString();
                //var Data: EducationApp.Interfaces.PublicationUploadingModel = {
                var Data = {
                    Template: template,
                    Title: title,
                    Description: description,
                    Markdown: markdown,
                    Tags: tags,
                    Images: images,
                    Videos: videos,
                    Date: date
                };
                this.http.post("Publish", Data).success(function () { return alert("!"); });
            };
            return PublicateController;
        })();
        Controllers.PublicateController = PublicateController;
        var PostController = (function () {
            function PostController($scope, $http, wizMarkdownSvc) {
                this.rating = 3;
                this.range = 3;
                this.isReadonly = false;
                this.isUserAlreadyGraduateIt = false;
                this.scope = $scope;
                this.http = $http;
            }
            PostController.prototype.PublicationInit = function (index) {
                this.getPublication(index);
            };
            PostController.prototype.getPublication = function (index) {
                var scope = this;
                this.http({
                    method: 'POST',
                    url: '/Publication/GetPublication',
                    data: { Index: index }
                }).success(function (data) { return scope.publication = data; });
            };
            PostController.prototype.sendGrade = function () {
                var scope = this;
                this.http.post('/Publication/SendPublicationGrade', {
                    Value: scope.range,
                    PublicationId: scope.publication.Id
                }).success(function () { return scope.getPublicationGrade(); });
            };
            PostController.prototype.getPublicationGrade = function () {
                var scope = this;
                this.http({
                    method: 'POST',
                    url: '/Publication/GetPublicationGrade',
                    data: { Index: scope.publication.Id }
                }).success(function (data) {
                    scope.publicationGrade = data['Grade'];
                    scope.isUserAlreadyGraduateIt = data['Result'];
                });
            };
            PostController.prototype.likeComment = function (commentId) {
                var scope = this;
                this.http({
                    method: 'POST',
                    url: '/Publication/LikeComment',
                    data: { CommentId: commentId }
                }).success(function () { return scope.getComments(); });
            };
            PostController.prototype.sendComment = function () {
                var scope = this;
                var date = moment().format("YYYY-MM-DD HH:mm:ss").toString();
                this.http.post('/Publication/SendComment', {
                    Content: scope.commentContent,
                    Date: date,
                    PublicationId: scope.publication.Id
                }).success(function () { scope.getComments(); scope.commentContent = ""; });
            };
            PostController.prototype.getComments = function () {
                var scope = this;
                this.http.post('/Publication/GetComments', { publicationId: scope.publication.Id })
                    .success(function (data) { return scope.publication.Comments = data; });
            };
            return PostController;
        })();
        Controllers.PostController = PostController;
    })(Controllers = EducationApp.Controllers || (EducationApp.Controllers = {}));
})(EducationApp || (EducationApp = {}));
//# sourceMappingURL=MarkdownController.js.map