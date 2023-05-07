//function onSignIn(googleUser) {
//    var profile = googleUser.getBasicProfile();
//    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
//    console.log('Name: ' + profile.getName());
//    console.log('Image URL: ' + profile.getImageUrl());
//    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
//}

//登入程序
//var startApp = function(){
//    gapi.load('auth2', function () {
//        auth2 = gapi.auth2.init({
//            client_id: '884318299815-157j2rm0ikit076bm2gl0jclff57p44s.apps.googleusercontent.com',
//            cookiepolicy: 'single_host_origin'
//        })
//        attachSignin(document.getElementById('google-login'))
//    })
//}

//function attachSignin(element) {
//    auth2.attachClickHandler(element, {},
//        //登入成功
//        function (googleUser) {
//            var profile = googleUser.getBasicProfile(),
//            $target = $('#GOOGLE_STATUS_1'),
//            html = ''
//            html += "ID: " + profile.getId() + "<br/>"
//            html += "會員暱稱： " + profile.getName() + "<br/>"
//            html += "會員頭像：" + profile.getImageUrl() + "<br/>"
//            html += "會員 email：" + profile.getEmail() + "<br/>"
//            $target.html(html)
//        },
//        //登入失敗
//        function (error) {
//            $("#GOOGLE_STATUS_1").html("")
//            alert(JSON.stringify(error, undefined, 2))
//        }
//    )
//}

//startApp()

////點擊登入
//$("#GOOGLE_login").click(function () {
//    // 進行登入程序
//    startApp();
//});

//// 點擊登出
////$("#GOOGLE_logout").click(function () {
////    var auth2 = gapi.auth2.getAuthInstance();
////    auth2.signOut().then(function () {
////        // 登出後的動作
////        $("#GOOGLE_STATUS_1").html("");
////    });
////});

//var googleUser = {};
//var startApp = function () {
//    gapi.load('auth2', function () {
//        // Retrieve the singleton for the GoogleAuth library and set up the client.
//        auth2 = gapi.auth2.init({
//            client_id: '160668882695-moti92oqdi35hvnstq4coe9rq7h6eb14.apps.googleusercontent.com',
//            cookiepolicy: 'single_host_origin',
//            // Request scopes in addition to 'profile' and 'email'
//            //scope: 'additional_scope'
//        });
//        attachSignin(document.getElementById('customBtn'));
//    });
//};

//function attachSignin(element) {
//    console.log(element.id);
//    auth2.attachClickHandler(element, {},
//        function (googleUser) {
//            document.getElementById('name').innerText = "Signed in: " +
//                googleUser.getBasicProfile().getName();
//        }, function (error) {
//            alert(JSON.stringify(error, undefined, 2));
//        });
//}

function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
}
