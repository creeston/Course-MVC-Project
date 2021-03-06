var EducationApp;
(function (EducationApp) {
    var Directives;
    (function (Directives) {
        var dropZoneContainer = (function () {
            function dropZoneContainer() {
                this.compile = function (templateElement, templateAttributes, transclude) {
                    var index = templateAttributes['index'];
                    var width = templateAttributes['width'] || 200;
                    width = width > 780 ? 780 : width;
                    var height = templateAttributes['height'] || 200;
                    var html = "";
                    html = html.concat('<div style="width:' + width + 'px; height:' + height + 'px">');
                    html = html.concat('<form ng-hide="template.images[' + index + ']" class="dropzone" dropzone="template.dropzoneConfig" id="' + index + '" >');
                    html = html.concat('<div class="dz-default dz-message"></div>');
                    html = html.concat('</form>');
                    html = html.concat('<div ng-dblclick="template.deleteImage(' + index + ')">');
                    html = html.concat('<img src="{{template.images[' + index + ']}}" class="img-rounded" />');
                    html = html.concat('</div>');
                    html = html.concat('</div>');
                    templateElement.html(html);
                };
                //link: IDirectiveLinkFn | IDirectivePrePost;
                this.restrict = 'E';
            }
            return dropZoneContainer;
        })();
        Directives.dropZoneContainer = dropZoneContainer;
        var markDownContainer = (function () {
            function markDownContainer() {
                this.compile = function (templateElement, templateAttributes, transclude) {
                    var index = templateAttributes['index'];
                    var html = '<wiz-markdown-editor ng-hide="template.mdFields[' + index + ']" content="mdText' + index + '"> \
                        <wiz-toolbar-button command="bold">Bold</wiz-toolbar-button> \
                        <wiz-toolbar-button command="italic">Italic</wiz-toolbar-button> \
                        <button ng-click="template.saveMarkdown(' + index + ')">Save</button> \
                        </wiz-markdown-editor> \
                        <wiz-markdown ng-dblclick="template.saveMarkdown(' + index + ')" ng-show="template.mdFields[' + index + ']" content="mdText' + index + '"></wiz-markdown>';
                    templateElement.html(html);
                };
                this.restrict = 'E';
            }
            return markDownContainer;
        })();
        Directives.markDownContainer = markDownContainer;
        var headerContainer = (function () {
            function headerContainer() {
                this.compile = function (templateElement, templateAttributes, transclude) {
                    var html = '<div id="templateHeader"> \
                          <h1 id="title">{{marked.title}}</h1> \
                          <h2 id="description">{{marked.description}}</h2> \
                          <h3 id="tags">{{marked.tags}}</h3>\
                        </div>';
                    templateElement.html(html);
                };
                this.restrict = 'E';
            }
            return headerContainer;
        })();
        Directives.headerContainer = headerContainer;
    })(Directives = EducationApp.Directives || (EducationApp.Directives = {}));
})(EducationApp || (EducationApp = {}));
//# sourceMappingURL=directives.js.map