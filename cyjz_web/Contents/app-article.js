
// 学术查询
function ArticleSearch() {
    myApp.showIndicator();
    var data = myApp.formToJSON('#formArticleSearch');
    $$.post("/article/SubmitSearch", data, function (ret) {
        myApp.hideIndicator();
        view2.router.loadContent(ret);
    });
}

// 添加新学术文章的submit事件处理，因为有input = file，所以需要form ajax-post
myApp.onPageInit('article-new', function (page) {

    $$('#formNewArticle').on('submitted', function (e) {
        var data = e.detail.data;

        if (data != "OK") {
            myApp.alert(data, '提交新的学术文章失败');
            return;
        }

        myApp.alert('新的学术文章已经提交，请等待管理员审核后发布。', '提交新的学术文章成功', function () {
            view2.router.loadPage("/article/index?new");
        });
    });

    $$('#formNewArticle').on('submitError', function (e) {
        myApp.alert('提交错误，请重新提交', '提交新的学术文章');
    });
});

// 提交文章评论
function SubmitArticleComments(id) {
    myApp.showIndicator();
    var data = $$("#txtArticleComments").val();
    online = window.navigator.onLine; //获取浏览器联网状态
    if (online) {
        $$.post("/article/submitarticlecomments", { id: id, content: data }, function (ret) {
            myApp.hideIndicator();
            if (ret != "OK") {
                myApp.alert(ret, '提交文章评论失败');
                return;
            }

            myApp.alert('新评论已提交，管理员可能会审核您发布的内容。', '提交新的文章评论成功', function () {
                getCurrentView().router.refreshPage(); // 提价评论成功，刷新页面
            });
        });
    }
    else {
        myApp.hideIndicator();
        myApp.alert("提交错误，请检查网络连接");   //联网异常
    }
}

// 删除文章评论
function DeleteArticleComments(id, comments_id) {
    myApp.confirm('是否确定要删除该条文章评论？', function () {
        $$.post("/article/DeleteArticleComments", { id: id, comments_id: comments_id }, function (ret) {
            if (ret != "OK") {
                myApp.alert(ret, '删除文章评论内容失败');
                return;
            }

            getCurrentView().router.refreshPage(); // 删除评论成功，刷新页面
        });
    });
}

// 下拉刷新
var ptrArticle = $$('#ptrArticle');
ptrArticle.on('refresh', function (e) {
    setTimeout(function () {
        if ($$("#tab21").hasClass('active'))
            $$.get("/article/articlelist", function (ret) {
                ptrArticle.find('#tab21').html(ret);
                myApp.pullToRefreshDone();
            });

        if ($$("#tab22").hasClass('active'))
            $$.get("/news/index", function (ret) {
                ptrArticle.find('#tab22').html(ret);
                myApp.pullToRefreshDone();
            });

        if ($$("#tab23").hasClass('active')) {
            myApp.pullToRefreshDone();
        }

        if ($$("#tab24").hasClass('active')) {
            $$.get("/cds/index", function (ret) {
                ptrArticle.find('#tab24').html(ret);
                myApp.pullToRefreshDone();
            });
        }
    }, 1000);
});