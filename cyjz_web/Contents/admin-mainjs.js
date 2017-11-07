
function showDialog(id) {
    var dialog = $(id).data('dialog');
    dialog.open();
}

function closeDialog(id) {
    var dialog = $(id).data('dialog');
    dialog.close();
}

$(function () {
    $(".tag2").hover(function () {
        $(this).append("<span id='btn_tag_remove' class='place-right mif-cancel' style='position:absolute; right:8px; top:3px;'></span>");
    }, function () {
        $("#btn_tag_remove").remove();
    });
});


function DeleteUser(user_id) {
    $$.post("/Admin/UserDeleteExcute", { id: user_id });
}

function UserSearch() {
 
    //var d = {};
    //var t = $('form').serializeArray();
    //$.each(t, function () {
    //    d[this.name] = this.value;
    //});
    //var r=JSON.stringify(d);
    $.ajax({
        url: "/admin/UserListtable",
        type: "Post",
        data: { name: $("#uname").val(), mobile: $("#mobnumber").val(), hosp: $("#uorg").val(), dept: $("#udept").val(), status: $("#apprstatus").val(), range: $("#regdate").val(), page: $("#H_page").val() },
        success: function (data) {
            $("#Selector").html(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("加载失败：" + errorThrown);
        }
    });

   // $.post("/admin/UserListtable", { user_name: $("#uname").val(), mobile: $("#mobnumber").val(), hosp: $("#uorg").val(), dept: $("#udept").val(), status: $("#apprstatus").val(), range: $("#regdate").val(), page: $("#H_page").val() });
}

function EmptySearch() {
    $("#uname").val("");
    $("#mobnumber").val("");
    $("#uorg").val("");
    $("#udept").val("");
    $("#apprstatus").val("全部");
    $("#regdate").val("全部");
}


function getCookie(cookieName) {
    var cookieValue = "";
    if (document.cookie && document.cookie != '') {
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            if (cookie.substring(0, cookieName.length + 1).trim() == cookieName.trim() + "=") {
                cookieValue = cookie.substring(cookieName.length + 1, cookie.length);
                break;
            }
        }
    }
    return cookieValue;
}

function Region() {
    $.ajax({
        url: "/admin/OutLogin",
        data: {},
        success: function (data) {
            if (data == "") {
                alert("退出失败！");
            } else {
                window.location.href = data;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("加载失败：" + errorThrown);
        }
    });
}
//会诊
function CaseSearch() {
    $.ajax({
        url: "/admin/CaseListTable",
        type: "Post",
        data: { Title: $("#Case_Title").val(), Proposer: $("#Case_Proposer").val(), status: $("#Case_stutes").val(), DateT: $("#Case_Datetime").val(), page: $("#H_page").val(), Sex: $("#Case_sex").val() },
        success: function (data) {
            $("#Case_Selector").html(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("加载失败：" + errorThrown);
        }
    });
}
String.prototype.ToString = function (format) {

    var dateTime = new Date(parseInt(this.substring(6, this.length - 2)));
    format = format.replace("yyyy", dateTime.getFullYear());
    format = format.replace("yy", dateTime.getFullYear().toString().substr(2));
    format = format.replace("MM", dateTime.getMonth() + 1);
    format = format.replace("dd", dateTime.getDate());
    format = format.replace("hh", dateTime.getHours());
    format = format.replace("mm", dateTime.getMinutes());
    format = format.replace("ss", dateTime.getSeconds());
    format = format.replace("ms", dateTime.getMilliseconds());
    return format;
};