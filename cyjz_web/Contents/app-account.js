

// 尝试自动登录，从localStorage中获取保存在本地的用户名和密码
if (localStorage.username != null) {
    var data = {
        mobile: localStorage.username,
        password: localStorage.password
    };

    $$.post("/account/signin", data, function (ret) {
        if (ret == "")
            return;

        getCurrentView().router.refreshPage();
        view2.router.refreshPage();
        view3.router.refreshPage();
        view4.router.refreshPage();
        view5.router.refreshPage();
    });
}

// 用户登录
function Signin() {
    myApp.showIndicator();

    var data = myApp.formToJSON('#formSignin');
    $$.post("/account/signin", data, function (ret) {
        if (ret == "") {
            myApp.hideIndicator();
            myApp.alert('请检查用户名和密码是否正确。', '登录失败');
            return;
        }

        localStorage.username = $$("#txtSigninMobile").val();
        localStorage.password = $$("#txtSigninPassword").val();

        getCurrentView().router.refreshPage();
        view2.router.refreshPage();
        view3.router.refreshPage();
        view4.router.refreshPage();
        view5.router.refreshPage();

        setTimeout(function () {
            myApp.hideIndicator();
            myApp.closeModal();
        }, 1000);
    });
}

// 用户退出登录
function Signout() {
    myApp.confirm('是否退出登录？', function () {

        localStorage.username = null;
        localStorage.password = null;

        $$.post("/account/signout", function (ret) {
            view2.router.refreshPage();
            view3.router.refreshPage();
            view4.router.refreshPage();
            view5.router.loadPage("/account/index?" + ret);
        });
    });
}

// 显示登录弹出框
function AskSignin() {
    myApp.loginScreen();
}

// 显示提示登录对话框
function ArticleDetailNeedSignin() {
    myApp.confirm('需要登录后才可以使用发表、分享、下载、收藏等功能，是否现在登录或新注册？', function () {
        myApp.loginScreen();
    });
}
function TeachingDetailNeedSignin() {
    myApp.confirm('需要登录后才可以使用分享、报名等功能，是否现在登录或新注册？', function () {
        myApp.loginScreen();
    });
}


// 显示新用户注册界面
function Register() {
    myApp.closeModal();
    getCurrentView().router.loadPage("/account/register");
}

// 提交新用户注册申请
function SubmitRegister()
{ 
        online = window.navigator.onLine; //获取浏览器联网状态
        // 联网正常
        if (online)
        {
            myApp.showIndicator();
            var data = myApp.formToJSON('#formRegister');
            $$.post("/account/submitregister", data, function (ret) {
                if (ret != "OK") {
                    myApp.hideIndicator();
                    myApp.alert(ret, '新用户注册失败');
                    return;
                }

                view3.router.refreshPage();
                view4.router.refreshPage();
                view5.router.loadPage("/account/index?" + ret);
            });

            setTimeout(function () {
                myApp.hideIndicator();
            }
                , 500);
        }
        else
        {
            myApp.alert("提交错误，请检查网络连接");   //联网异常
        }

}

// 显示忘记密码界面
function ForgotPassword() {
    myApp.closeModal();
    getCurrentView().router.loadPage("/account/forgotpassword");
}

// 显示重设密码界面
function ResetPassword() {
    //myApp.closeModal();
    myApp.alert('验证码已发送到您的邮箱，请注意查收');
    getCurrentView().router.loadPage("/account/resetpassword");
}

// 提交新密码
function SubmitPassword() {
    var data = myApp.formToJSON('#formChangePassword');
    $$.post("/account/submitpassword", data, function (ret) {
        if (ret != "OK") {
            myApp.alert(ret, '修改登录密码失败');
            return;
        }

        myApp.alert('修改登录密码成功', function () {
            view5.router.back();
        });
    });
}

// 更新用户个人信息
function SaveMyProfile() {
    myApp.showIndicator();

    var data = myApp.formToJSON('#formMyProfile');
    $$.post("/account/UpdateProfile", data, function (ret) {
        if (ret != "OK") {
            myApp.hideIndicator();
            myApp.alert(ret, '更新个人信息失败');
            return;
        }

        myApp.hideIndicator();
        myApp.alert('更新个人信息成功', function () {
            view5.router.loadPage("/account/index?" + ret);
        });
    });
}

// 添加到个人收藏
function AddToCollection(userid, type, contentid) {
    var data = {
        'userid': userid,
        'type': type,
        'contentid': contentid,
    };
    $$.post("/account/AddToCollection", data, function (ret) {
        getCurrentView().router.refreshPage();
    });
}

// 从个人收藏中移除
function RemoveCollection(userid, type, contentid) {
    var data = {
        'userid': userid,
        'type': type,
        'contentid': contentid,
    };
    $$.post("/account/RemoveCollection", data, function (ret) {
        getCurrentView().router.refreshPage();
    });
}

// 每次都刷新“我的账户”页面
function ReloadMyAccount() {
    view5.router.refreshPage();
}


// 将消息message标记为已读
function MessageIsRead(id) {
    $$.post("/account/MessageIsRead?id=" + id, function (ret) {
        getCurrentView().router.refreshPage(); // 直接刷新页面
    });
}

// 将消息message标记为已删除
function MessageIsDeleted(id, back) {
    myApp.confirm('是否确认删除该消息？', function () {
        $$.post("/account/MessageIsDeleted?id=" + id, function (ret) {
            if (back == true) // 来自于消息详情页面的删除操作
                getCurrentView().router.loadPage("account/mymessages");
            else
                getCurrentView().router.refreshPage(); // 直接刷新页面
        });
    });
}

// 将指定用户的全部未读消息标记为已读
function MessageAllRead() {
    myApp.confirm('是否确认将全部未读消息标记为已读？', function () {
        $$.post("/account/MessageAllRead", function (ret) {
            getCurrentView().router.refreshPage(); // 直接刷新页面
        });
    });
}

// 用户选择了一个头像
function SelectAvatar(img) {
    $$("#hidSelectedAvatar").val(img);
    $$(".img_avatar").removeClass("border-blue border-white");
    $$(".img_avatar").addClass("border-white");
    $$("#img_avatar_" + img).removeClass("border-white");
    $$("#img_avatar_" + img).addClass("border-blue");
}

// 用户提交了新头像
function SubmitAvatar() {
    var img = $$("#hidSelectedAvatar").val();
    $$.post("/account/submitavatar", { img: img }, function (ret) {
        view5.router.back({ url: "/account/myprofile?" + ret });
    });
}

// 扫描结果测试
function qrcodeCallback(result) {
    myApp.alert(result);
}