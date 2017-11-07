
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