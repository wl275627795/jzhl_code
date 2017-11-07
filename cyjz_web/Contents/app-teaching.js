
// 教学视频查询
function TeachingSearch() {
    myApp.showIndicator();
    var data = myApp.formToJSON('#formTeachingSearch');
    $$.post("/teaching/SubmitSearch", data, function (ret) {
        myApp.hideIndicator();
        mainView.router.loadContent(ret);
    });
}

// 提交教学直播/视频评论
function SubmitTeachingComments(id, isLiv) {
    myApp.showIndicator();
    var data = "";
    if (isLiv == 1)
        data = $$("#txtTeachingComments").val();
    else
        data = $$("#txtTeachingVodComments").val();

    $$.post("/teaching/submitteachingcomments", { id: id, isLiv: isLiv, content: data }, function (ret) {
        myApp.hideIndicator();
        if (ret != "OK") {
            myApp.alert(ret, '提交评论失败');
            return;
        }

        myApp.alert('新评论已提交，管理员可能会审核您发布的内容。', '提交新评论成功', function () {
           // getCurrentView().router.refreshPage(); // 提价评论成功，刷新页面
        });
    });
}

function PageRefresh()
{
    getCurrentView().router.refreshPage();
}

// 删除评论
function DeleteTeachingComments(comments_id) {
    myApp.confirm('是否确定要删除该条评论？', function () {
        $$.post("/teaching/DeleteTeachingComments", { comments_id: comments_id }, function (ret) {
            if (ret != "OK") {
                myApp.alert(ret, '删除评论失败');
                return;
            }

            getCurrentView().router.refreshPage(); // 删除评论成功，刷新页面
        });
    });
}

// 提交直播报名申请
function SubmitTeachingApply(liv_id, user_id) {
    $$.post("/teaching/SubmitTeachingApply", { liv_id: liv_id, user_id: user_id }, function (ret) {
        if (ret != "OK") {
            if (ret == "KO") {
                myApp.alert(ret, '该直播报名已满，感谢关注');
                return;
            }
            myApp.alert(ret, '直播报名申请失败');
            return;
        }

        myApp.alert('您已经成功报名参加该教学视频直播，请留意直播开始时间。', '直播报名申请成功', function () {
            getCurrentView().router.refreshPage(); // 刷新页面
        });
    });
}

// 删除直播报名
function DeleteTeachingApply(id) {
    myApp.confirm('是否确定要取消该直播报名？', function () {
        $$.post("/teaching/DeleteTeachingApply", { apply_id: id }, function (ret) {
            if (ret != "OK") {
                myApp.alert(ret, '取消该直播报名失败');
                return;
            }

            getCurrentView().router.refreshPage(); // 删除评论成功，刷新页面
        });
    });
}

// 下拉刷新
var ptrTeaching = $$('#ptrTeaching');
ptrTeaching.on('refresh', function (e) {
    setTimeout(function () {
        var tab_index = 1;
        if ($$("#tab2").hasClass('active'))
            tab_index = 2;
        if ($$("#tab3").hasClass('active'))
            tab_index = 3;
        if ($$("#tab4").hasClass('active'))
            tab_index = 4;
        $$.get("/teaching/teachinglist?tab=" + tab_index, function (ret) {
            ptrTeaching.find('#teaching_list').html(ret);
            myApp.pullToRefreshDone();
        });
    }, 1000);

});