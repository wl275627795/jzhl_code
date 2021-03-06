var myApp = new Framework7({
    modalTitle: "黄金急救",
    modalButtonOk: "确定",
    modalButtonCancel: "取消",
    smartSelectBackText: "返回"
});

var $$ = Dom7;

var mainView = myApp.addView('.view-main', {
});

var view2 = myApp.addView('.view-2');
var view3 = myApp.addView('.view-3');
var view4 = myApp.addView('.view-4');
var view5 = myApp.addView('.view-5');

myApp.onPageInit('article-detail', function (page) {
    $$('.btn-addfav').on('click', function () {
        myApp.alert('已添加至收藏列表');
    });
});

myApp.onPageInit('guide-detail', function (page) {
    $$('.btn-addfav').on('click', function () {
        myApp.alert('已添加至收藏列表');
    });
});

myApp.onPageInit('guide-detail', function (page) {
    $$('.btn-download').on('click', function () {
        myApp.alert('已将教学视频保存至您的相册');
    });

    $$('.btn-askhelp').on('click', function () {
        myApp.showTab('#view-3');
    });
});

myApp.onPageInit('account', function (page) {
    $$('.confirm-signout').on('click', function () {
        myApp.confirm('是否退出登录？', function () {
            mainView.router.loadPage("/account/signin");
        });
    });
});

function ScanGuideQR() {
    myApp.alert('即将访问急救指南《气管异物》', '扫描急救指南二维码', function () {
        mainView.router.loadPage("/didi/guidedetail?title=气管异物");
    });
}

function ScanDeptQR() {
    myApp.alert('成功扫描分诊单二维码，已获取您的本次挂号信息。');
}

myApp.onPageInit('consultation-detail', function (page) {
    $$('.link-photos').on('click', function () {
        var myPhotoBrowser = myApp.photoBrowser({
            theme: 'dark',
            backLinkText: '返回',
            zoom: 400,
            photos: [
                '/Contents/image/ekg.jpg',
                '/Contents/image/dicom1.jpg',
                '/Contents/image/ultrasound.jpg'
            ]
        });
        myPhotoBrowser.open();
    });
});


function LoadMap() {
    // 百度地图API功能
    var map = new BMap.Map("ambulance_map");    // 创建Map实例
    map.centerAndZoom(new BMap.Point(116.404, 39.915), 12);
    var driving = new BMap.DrivingRoute(map, { renderOptions: { map: map, panel: "r-result", autoViewport: true } });
    driving.search("海淀公园", "魏公村");
}
