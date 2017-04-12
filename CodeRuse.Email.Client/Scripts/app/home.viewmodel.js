function HomeViewModel(app, dataModel) {
    var self = this;

    $.extend(self, {
        myHometown: ko.observable(''),
        toAddresses: ko.observable(''),
        ccAddresses: ko.observable(''),
        emailSubject: ko.observable(''),
        sendEmail: function () {
            var toAddresses = this.toAddresses();
            var ccAddresses = this.ccAddresses();
            var emailSubject = this.emailSubject();
            var emailBody = CKEDITOR.instances['email-editor'].getData();

            $.ajax({
                url: app.dataModel.email.send,
                type: 'post',
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                data: {
                    ToAddresses: toAddresses,
                    CcAddresses: ccAddresses,
                    Subject: emailSubject,
                    Body: emailBody
                },
                dataType: 'json', 
                success: function (data) {
                    console.log(data);
                }
            });
        }
    });

    Sammy(function () {
        this.get('#home', function () {
            // Make a call to the protected Web API by passing in a Bearer Authorization Header
            $.ajax({
                method: 'get',
                url: app.dataModel.userInfoUrl,
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                success: function (data) {
                    self.myHometown('Your Hometown is : ' + data.hometown);
                }
            });
        });
        this.get('/', function () { this.app.runRoute('get', '#home'); });
    });

    CKEDITOR.replace('email-editor', {
        language: 'en',
        removePlugins: 'resize',
        image_previewText: 'Sample text',
        resize_enabled: false,
        toolbar: [
            { name: 'clipboard', items: ['Undo', 'Redo'] },
            { name: 'styles', items: ['Styles', 'Format'] },
            { name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', '-', 'RemoveFormat'] },
            { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
            { name: 'links', items: ['Link', 'Unlink'] },
            { name: 'insert', items: ['Image', 'Table'] },
            { name: 'tools', items: ['Maximize'] },
            { name: 'editing', items: ['Scayt'] }
        ],
        customConfig: '',
        height: 250,
        format_tags: 'p;h1;h2;h3;pre',
        removeDialogTabs: 'image:advanced;link:advanced',
        stylesSet: [
            /* Inline Styles */
            { name: 'Marker', element: 'span', attributes: { 'class': 'marker' } },
            { name: 'Cited Work', element: 'cite' },
            { name: 'Inline Quotation', element: 'q' },
            /* Object Styles */
            {
                name: 'Special Container',
                element: 'div',
                styles: {
                    padding: '5px 10px',
                    background: '#eee',
                    border: '1px solid #ccc'
                }
            },
            {
                name: 'Compact table',
                element: 'table',
                attributes: {
                    cellpadding: '5',
                    cellspacing: '0',
                    border: '1',
                    bordercolor: '#ccc'
                },
                styles: {
                    'border-collapse': 'collapse'
                }
            },
            { name: 'Borderless Table', element: 'table', styles: { 'border-style': 'hidden', 'background-color': '#E6E6FA' } },
            { name: 'Square Bulleted List', element: 'ul', styles: { 'list-style-type': 'square' } },
            /* Widget Styles */
            // We use this one to style the brownie picture.
            { name: 'Illustration', type: 'widget', widget: 'image', attributes: { 'class': 'image-illustration' } },
            // Media embed
            { name: '240p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-240p' } },
            { name: '360p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-360p' } },
            { name: '480p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-480p' } },
            { name: '720p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-720p' } },
            { name: '1080p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-1080p' } }
        ],
        on: {
            instanceReady: function (evt) {
                //this.setData($(".tab-content[tab-id='" + tabID + "'] .temp").val());
            }
        }
    });

    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
