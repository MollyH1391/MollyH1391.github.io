
$(document).ready(function () {

    $('#comment_table').DataTable({
        language: {
            "sProcessing": "處理中...",
            "sLengthMenu": "顯示 _MENU_ 項結果",
            "sZeroRecords": "沒有符合的項目",
            "sInfo": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
            "sInfoEmpty": "顯示第 0 至 0 項結果，共 0 項",
            "sInfoFiltered": "(由 _MAX_ 項結果過濾)",
            "sInfoPostFix": "",
            "sSearch": "搜尋:",
            "sEmptyTable": "表中資料為空",
            "sLoadingRecords": "載入中...",
            "sInfoThousands": ",",
            "oPaginate": {
                "sFirst": "首頁",
                "sPrevious": "上頁",
                "sNext": "下頁",
                "sLast": "末頁"
            }
        },
        "order": [[4, "desc"]],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    if (data.length > 47) {
                        //return data.substring(0, 45) + "<input type='button' onclick='more()' value='...顯示更多'></input>"
                        return data.substr(0, 47) + '<a href="javascript:void(0)">...顯示更多</a>'
                    }
                    else {
                        return data;
                    }
                },
                "targets": 3,
                "orderable": false,
            },
        ]
    });
})


function changeShow(obj) {
    var full = $(obj).attr("fullName");
    if (full.length > 47) {
        var detail = $(obj).attr("fullName");

        var isFull = $(obj).attr('isFull');
        if (isFull == 'true') {
            $(obj).html(detail.substr(0, 47) + '<a href="javascript:void(0)">...顯示更多</a>');
            $(obj).attr('isFull', false);
        }
        else {
            $(obj).html(detail + '<a href="javascript:void(0)">收起</a>');
            $(obj).attr('isFull', true);
        }
    }

}


            //function CommentFormat(value, row) {
            //    if (value.length > 50) {
            //        return value.substring(0, 50) + "..."  
            //    }
            //    else {
            //        return value;
            //    }
            //}
            //function format(value, row) {
            //    return "<p style='color:red'>" + value + "</p>";
            //}


