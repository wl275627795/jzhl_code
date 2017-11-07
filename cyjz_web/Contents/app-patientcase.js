
// 教学视频查询
function PatientCaseSearch() {
    myApp.showIndicator();
    var data = myApp.formToJSON('#formPatientCaseSearch');
    $$.post("/patientcase/SubmitSearch", data, function (ret) {
        myApp.hideIndicator();
        view3.router.loadContent(ret);
    });
}

// 提交会诊讨论意见
function SubmitConclusion(id) {
    myApp.showIndicator();
    var data = $$("#txtPatientCaseConclusion").val();
    online = window.navigator.onLine; //获取浏览器联网状态
    // 联网正常
    if (online)
    {
        $$.post("/patientcase/submitConclusion", { id: id, content: data }, function (ret) {
            myApp.hideIndicator();
            if (ret != "OK") {
                myApp.alert(ret, '提交会诊讨论意见失败');
                return;
            }

            myApp.alert('新会诊讨论意见已提交，管理员可能会审核您发布的内容。', '提交新的会诊讨论意见成功', function () {
                getCurrentView().router.refreshPage(); // 提价评论成功，刷新页面
            });
        });
    }
    else {
        myApp.hideIndicator();
        myApp.alert("提交错误，请检查网络连接");   //联网异常
    }
}

// 删除会诊讨论意见
function DeleteConclusion(id) {
    myApp.confirm('是否确定要删除该条会诊讨论意见？', function () {
        $$.post("/patientcase/DeleteConclusion", { id: id }, function (ret) {
            if (ret != "OK") {
                myApp.alert(ret, '删除会诊讨论意见内容失败');
                return;
            }

            getCurrentView().router.refreshPage(); // 删除评论成功，刷新页面
        });
    });
}

// 显示会诊病例照片浏览
function ShowPatientCaseImages(data) {
    var caseImagesBrowser = myApp.photoBrowser({
        theme: 'dark',
        backLinkText: '返回',
        zoom: 400,
        photos: data
    });
    caseImagesBrowser.open();
}

// 添加新的附件照片的submit事件处理，因为有input=file，所以需要form ajax-post
myApp.onPageInit('patientcase-image-new', function (page) {
    $$('#formNewPatientCaseImage').on('submitted', function (e) {
        var data = e.detail.data;

        if (data != "OK") {
            myApp.alert(data, ' 提交新附件图片失败 ');
            return;
        }

        var id = $$("input[name='id']").val();
        view3.router.loadPage("/patientcase/edit?tab=3&id=" + id);
    });
});

// 保存附件照片的描述
function SavePatientCaseImageEdit(case_id, id) {
    var descrip = $$("#txtPatientCaseImageDescrip").val();
    $$.post("/patientcase/SubmitImageEdit", { id: id, descrip: descrip }, function (ret) {
        view3.router.loadPage("/patientcase/edit?tab=3&id=" + case_id);
    });
}

// 删除附件照片
function DeletePatientCaseImage(case_id, id) {
    myApp.confirm('是否确定要删除该附件图片？', function () {
        $$.post("/patientcase/DeleteImage?id=" + id, function (ret) {
            view3.router.loadPage("/patientcase/edit?tab=3&id=" + case_id);
        });
    });
}

// 保存会诊病例（存草稿or提交）
function SavePatientCase(case_id, mode) {
    var data = myApp.formToJSON('#formPatientCaseEdit');
    $$.post("/patientcase/submitpatientcase?mode=" + mode, data, function (ret) {
        if (mode == 1) {
            myApp.alert('已保存新会诊病例的草稿。', '新会诊病例', function () {
                view3.router.loadPage("/patientcase/index");
            });
        }
        else if (mode == 2) {
            myApp.alert('新会诊病例已提交，请等待管理员审核后发布。', '新会诊病例', function () {
                view3.router.loadPage("/patientcase/index");
            });
        }
        else if (mode == 3) {
            myApp.alert('会诊病例内容已更新。', '编辑会诊病例', function () {
                view3.router.loadPage("/patientcase/detail?id=" + case_id);
            });
        }
    });
}

// 下拉刷新
var ptrPatientCase = $$('#ptrPatientCase');
ptrPatientCase.on('refresh', function (e) {
    setTimeout(function () {
        $$.get("/patientcase/index", function (ret) {
            ptrArticle.find('#tab24').html(ret);
            myApp.pullToRefreshDone();
        });
    }, 1000);
});