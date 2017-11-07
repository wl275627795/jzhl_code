
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


function DeleteUser(user_id)
   {
        $$.post("/Admin/UserDeleteExcute", { id: user_id })
    }

