//const area = document.querySelector('#testArea')
//const returnUrl = document.querySelector('form').getAttribute('asp-route-returnUrl')

//載入和初始化FB SDK
window.fbAsyncInit = function () {
    FB.init({
        appId: '7574574305918196',
        cookie: true,
        xfbml: true,
        version: 'v13.0'
    });
    
}
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

//FB登入按鈕呼叫
function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}

function statusChangeCallback(response) {
    if (response.status === 'connected') {
        debugger
        //已登入
        getUserData()
        
    } else {
        loginFB()
    }
}

function loginFB() {
    FB.login(function (response) {
        getUserData()
        //fallback_redirect_uri
    }, { scope: 'public_profile, email'})
}

function getUserData() {
    FB.api('/me', 'GET', { "fields": "id, name, email" }, function (response) {
        debugger
        toastr['info'](`${response.name},歡迎回來~`)
        //area.classList.remove("d-none")

        let userData = {
            Name: response.name,
            Email: response.email
            //ReturnUrl: returnUrl
        } 

        fetch('/Member/FacebookLogin', {
            
            headers: {
                "Accept": "application/json, text/plain",
                "Content-Type": "application/json;charset=UTF-8"
            },
            method: "Post",
            body: JSON.stringify(userData)
        })
            .then(function (res) {
                if (res.ok) {
                    console.log(res)
                    window.location.href = '/Home/Index'
                }
            })
        
    });
}

//function loginFB() {
//    FB.login(function (response) {
//        if (response.status === 'connected') {
//            //getUserData()

//        }
//    }, { scope: 'public_profile,email' })

//}

function logoutFB() {
    FB.logout(function (response) {
        toastr['warning']('logout')
        Area.classList.add("d-none")
    });
}


//response結構範例
//{
//    status: 'connected',
//        authResponse: {
//        accessToken: '{access-token}',
//            expiresIn: '{unix-timestamp}',
//                reauthorize_required_in: '{seconds-until-token-expires}',
//                    signedRequest: '{signed-parameter}',
//                        userID: '{user-id}'
//    }
//}

//不知道要幹嘛
//(function (d, s, id) {
//    var js, fjs = d.getElementsByTagName(s)[0];
//    if (d.getElementById(id)) { return; }
//    js = d.createElement(s); js.id = id;
//    js.src = "https://connect.facebook.net/en_US/sdk.js";
//    fjs.parentNode.insertBefore(js, fjs);
//}(document, 'script', 'facebook-jssdk'));
