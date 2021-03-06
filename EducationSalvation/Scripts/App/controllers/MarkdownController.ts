﻿
module EducationApp.Controllers {
    declare var moment: any;

    export class IndexController {
        scope: ng.IScope;
        http: ng.IHttpService;
        publicationThumbnails: any;
        constructor($scope: ng.IScope, $http: ng.IHttpService) {
            this.scope = $scope;
            this.http = $http;
        }

        public getThumbnails() {
            this.http({ method: 'GET', url: '/Home/GetPublicationThumbnails' })
                .success((data) => { this.publicationThumbnails = data });
        }
    }

    export class TemplateController {
        images: Array<string> = new Array<string>();
        videos: Array<string> = new Array<string>();
        tags: Array<string> = new Array<string>();
        extraTags: any = new Array<string>();
        helpfulTags: any;
        dropzoneConfig: any;
        scope: ng.IScope;
        markdown: string;
        previewMode: boolean = false;
        constructor($scope: ng.IScope, wizMarkdownSvc) {
            var template = this;
            this.scope = $scope;
            this.dropzoneConfig = {
                'options': { // passed into the Dropzone constructor
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
            this.scope.$watch(() => this.tags,
                (newValue: Array<string>, oldValue: Array<string>) => {
                    if (newValue[newValue.length - 1] == "")
                        template.helpfulTags = new Array<string>();
                    else
                        template.helpfulTags = template.extraTags.filter((x) => x.indexOf(newValue[newValue.length - 1]) == 0)
                });
        }

        showPreview() {
            this.previewMode = !this.previewMode;
        }

        setTag(helpfulTag: string) {
            var tags = angular.copy(this.tags);
            tags[tags.length - 1] = helpfulTag;
            this.tags = tags;
        }

        
        
    }

    export class MarkdownController {
        http: ng.IHttpService;
        scope: ng.IScope;
        templates: any;

        constructor($scope: ng.IScope, $http, wizMarkdownSvc) {
            this.http = $http;
            this.scope = $scope;
            this.GetTemplates();
            this.GetTags($scope);
        }

        public GetTemplates() {
            this.http({ method: 'GET', url: 'GetTemplates' })
                .success((data) => { this.templates = data });
        }
        public GetTags(scope: ng.IScope) {
            this.http({ method: 'GET', url: 'GetTags' })
                .success((data) => {
                    var templateScope = scope.$parent['template'];
                    templateScope['extraTags'] = data;
                });
        }

    }

    export class PublicateController {
        http: ng.IHttpService;
        scope: ng.IScope;


        constructor($scope: ng.IScope, $http) {
            this.http = $http;
            this.scope = $scope;
        }

        public Publish() {
            var templateScope = this.scope.$parent['template'];
            var markedScope = this.scope['$$prevSibling']['marked'];
            var template = markedScope['template']['Name'];
            var title = templateScope['title'];
            var description = templateScope['description'];
            var tags = templateScope['tags'].join(' ')
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
            this.http.post("Publish", Data).success(() => alert("!"));

        }

    }

    export class PostController {
        scope: ng.IScope;
        http: ng.IHttpService;
		connection: HubConnection;
        commentsHub: HubProxy;
        publication: any;
        commentContent: string;
		anotherRange:any = 5;
		range:any = 3;
		isReadonly:boolean = false;
        constructor($scope: ng.IScope, $http: ng.IHttpService, wizMarkdownSvc) {
            this.scope = $scope;
            this.http = $http;
			this.connection = $.hubConnection();
			this.commentsHub = this.connection.createHubProxy("commentsHub");
			this.commentsHub.on("updateCommentSection", () => this.getComments());
			this.commentsHub.on("display", (message) => alert(message));
			this.connection.start();
        }

		public PublicationInit(index: number){
			this.getPublication(index);
		}

        public getPublication(index: number) {
            var scope = this;
            this.http({
                method: 'POST',
                url: '/Publication/GetPublication',
                data: { Index: index }
            }).success((data) => { scope.publication = data; scope.range = data['Stars']; });
        }

		public sendGrade() {
			var scope = this;
			this.http.post('/Publication/SendPublicationGrade', {
                Value: scope.range,
                PublicationId: scope.publication.Id
            }).success(() => scope.getPublicationGrade());
		}

		public getPublicationGrade(){
			var scope = this;
            this.http({
                method: 'POST',
                url: '/Publication/GetPublicationGrade',
                data: { Index: scope.publication.Id }
            })
			//.success((data) => { 
			//	scope.publicationGrade = data['Grade']; 
			//	scope.isUserAlreadyGraduateIt = data['Result'];
			//});
			.success((data) => { 
				scope.publication.Stars = data; 
				scope.publication.IsUserAlreadyGraduateIt = true;
			});
		}

		public likeComment(commentId:number) {
			var scope = this;
			this.http({
                method: 'POST',
                url: '/Publication/LikeComment',
                data: { CommentId: commentId }
            }).success(() => scope.getComments());
		}

		public sendComment() {
            var scope = this;
            var date = moment().format("YYYY-MM-DD HH:mm:ss").toString();
            this.http.post('/Publication/SendComment', {
                Content: scope.commentContent,
                Date: date,
                PublicationId: scope.publication.Id
			}).success(() => { scope.commentsHub.invoke("SendComment"); scope.commentContent = ""; });
            //}).success(() => { scope.getComments(); scope.commentContent = ""; });

        }

		public getComments() {
			var scope = this;
            this.http.post('/Publication/GetComments', { publicationId: scope.publication.Id })
			.success((data) => scope.publication.Comments = data);
		}
    }

	export class UserInfoController {
		scope:ng.IScope;
		http: ng.IHttpService;
		info:any;
		uniqueCheckingLabel:any;
		constructor($scope: ng.IScope, $http: ng.IHttpService) {
			this.scope = $scope; this.http = $http;
			this.uniqueCheckingLabel = "Check";
		}

		public getUserInfo(userInfo:any) {
			this.info = JSON.parse(userInfo);
		}

		public checkUniqueness() {
			this.uniqueCheckingLabel = "Checking...";
			if (this.isNicknameUnique())
				this.uniqueCheckingLabel = "Name is unique!";
			else this.uniqueCheckingLabel = "Name is already taken";
		}

		public isNicknameUnique () {
			var result = true;
			this.uniqueCheckingLabel = "checking...";
			for (var i = 0; i < this.info.Nicknames.length; i++)
				if (this.info.Nickname == this.info.Nicknames[i])
					result = false;
			return result;
		}

		public sendUserInfo() {
			var scope = this;
			if (scope.Validation())
				this.http.post('/Manage/CreateAdditionalUserInfo', { model: scope.info });
		}

		public Validation() {
			var result = true;
			if (!this.isNicknameUnique())
				result = false;
			return result;
		}
	}
}