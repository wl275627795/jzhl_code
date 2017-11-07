
// 提交新闻评论
function SubmitNewsComments(id) {
    var data = $$("#txtNewsComments").val();
    $$.post("/news/submitnewscomments", { id: id, content: data }, function (ret) {
        if (ret != "OK") {
            myApp.alert(ret, '提交新闻评论失败');
            return;
        }

        myApp.alert('新评论已提交，管理员可能会审核您发布的内容。', '提交新的新闻评论成功', function () {
            getCurrentView().router.refreshPage(); // 提价评论成功，刷新页面
        });
    });
}

// 删除新闻评论
function DeleteNewsComments(id, comments_id) {
    myApp.confirm('是否确定要删除该条新闻评论？', function () {
        $$.post("/news/DeleteNewsComments", { id: id, comments_id: comments_id }, function (ret) {
            if (ret != "OK") {
                myApp.alert(ret, '删除新闻评论内容失败');
                return;
            }

            getCurrentView().router.refreshPage(); // 删除评论成功，刷新页面
        });
    });
}

// 显示新闻照片浏览
function ShowNewsImages(data) {
    var newsImagesBrowser = myApp.photoBrowser({
        theme: 'dark',
        backLinkText: '返回',
        zoom: 400,
        photos: data
    });
    newsImagesBrowser.open();
}