// 应用程序初始化
var myApp = new Framework7({
    cache: false,
    modalTitle: "急诊互联",
    modalButtonOk: "确定",
    modalButtonCancel: "取消",
    //pushState: true,
    material: true, // 启用material design主题
});

var $$ = Dom7;

// 添加视图
var mainView = myApp.addView('.view-main', {
});

var view2 = myApp.addView('.view-2');
var view3 = myApp.addView('.view-3');
var view4 = myApp.addView('.view-4');
var view5 = myApp.addView('.view-5');


var getCurrentView = function () {
    return myApp.getCurrentView();
}


// 通知（Notification）测试  // for test