/***************************************** 
* RecordList function section
*****************************************/
$(function () {
    $("#RecordListDisplayStatus").change(function () {
        $("#RecordListCurrentPage").val(1);
        var page = parseInt($("#RecordListCurrentPage").val());
        GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
    });

    $("#RecordListBtnRefresh").click(function () {
        $("#RecordListCurrentPage").val(1);
        var page = parseInt($("#RecordListCurrentPage").val());
        GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
    });

    $("#RecordListBtnFirstPage").click(function () {
        $("#RecordListCurrentPage").val(1);
        var page = parseInt($("#RecordListCurrentPage").val());
        GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
    });

    $("#RecordListBtnPrevPage").click(function () {
        var page = parseInt($("#RecordListCurrentPage").val());
        if (page > 1) {
            page -= 1;
            $("#RecordListCurrentPage").val(page);
            GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
        }
    });

    $("#RecordListBtnGotoPage").click(function () {
        var page = parseInt($("#RecordListCurrentPage").val());
        var total_page = parseInt($("#RecordListTotalPage").html());
        if (page < 1) {
            page = 1;
        } else if (page > total_page) {
            page = total_page;
        };
        $("#RecordListCurrentPage").val(page);
        GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
    });

    $("#RecordListBtnNextPage").click(function () {
        var page = parseInt($("#RecordListCurrentPage").val());
        var total_page = parseInt($("#RecordListTotalPage").html());
        if (page < total_page) {
            page += 1;
            $("#RecordListCurrentPage").val(page);
            GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
        }
    });

    $("#RecordListBtnLastPage").click(function () {
        var page = parseInt($("#RecordListCurrentPage").val());
        var total_page = parseInt($("#RecordListTotalPage").html());
        page = total_page;
        $("#RecordListCurrentPage").val(page);
        GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
    });

})

function GetRecordList(agent_id, status_id, page) {
    var url = doc_root + "Record/AjaxJsonList/" + agent_id;
    var data_obj = {
        'status': status_id || 1,
        'page': page || 1
    };
    log.debug('Get Record List(' + url + '), data: ' + var_dump(data_obj));
    $.ajax({
        url: url,
        method: HTTP_POST,
        data: data_obj,
        dataType: DATA_JSON,
        success: function (records, status, jqXHR) {
            log.debug('Get Record List return: ' + var_dump(records));
            var total_page = parseInt(records["total_page"]);
            var curr_page = page;
            $("#RecordListTotalPage").html(total_page);
            ShowRecordList(records["list"]);

            if (curr_page <= 1) {
                curr_page = 1;
                $("#RecordListCurrentPage").val(curr_page);
                $("#RecordListBtnFirstPage").attr("disabled", true);
                $("#RecordListBtnPrevPage").attr("disabled", true);
            } else {
                $("#RecordListBtnFirstPage").attr("disabled", false);
                $("#RecordListBtnPrevPage").attr("disabled", false);
            }
            if (curr_page >= total_page) {
                curr_page = total_page;
                $("#RecordListCurrentPage").val(curr_page);
                $("#RecordListBtnNextPage").attr("disabled", true);
                $("#RecordListBtnLastPage").attr("disabled", true);
            } else {
                $("#RecordListBtnNextPage").attr("disabled", false);
                $("#RecordListBtnLastPage").attr("disabled", false);
            }
        },
        error: function (jqXHR, status, msg) {
            alert('Error in Get records list: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
            log.error('Error in Get records list: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
        }
    })
}

function ShowRecordList(records) {
    $("#RecordTable").empty().append(
        $('<tr/>')
            .append($('<th/>').html("編號"))
            .append($('<th/>').html("類型"))
            .append($('<th/>').html("客戶"))
            .append($('<th/>').html("客服人員"))
            .append($('<th/>').html("處理狀態"))
            .append($('<th/>').html("小結"))
            .append($('<th/>').html("進件時間"))
            .append($('<th/>').html("結案時間"))
            //.append($('<th/>').html("顯示案件"))
            );
    for (var key in records) {
        var new_tr = $('<tr/>')
            .append($('<td/>').html(records[key]['RID']))
            .append($('<td/>').html(records[key]['TypeName']))
            .append($('<td/>').html(records[key]['CustomerName']))
            .append($('<td/>').html(records[key]['AgentName']))
            .append($('<td/>').html(records[key]['StatusName']))
            .append($('<td/>').html(records[key]['ServiceItemName']))
            .append($('<td/>').html(ISODateStringToString(records[key]['IncomingDT'])))
            .append($('<td/>').html(ISODateStringToString(records[key]['FinishDT']))
                .append($('<input class="record_key" type=hidden value="' + key + '"/>'))
            );

        // 郵件案件
        if (records[key]['TypeID'] == 3) {
            // 新回信
            if (records[key]['MsgUnRead'] == "1" && records[key]['StatusID'] == "2") {
                new_tr.attr('style', 'font-weight:bold;vertical-align:text-bottom;');
                new_tr.click(function () {
                    var k = $(this).children('td').children('input.record_key').val();
                    curr_email_customer_id = records[k]['CustomerID'];
                    curr_email_record_id = records[k]['RID'];
                    //SetEmailStatusEnable(true, true, true, true, true, true, true);
                    // !NOTE 2 is TabEmailRecord
                    $("#Tabs").tabs("option", "active", 2);
                });
            } else {
                new_tr.click(function () {
                    var k = $(this).children('td').children('input.record_key').val();
                    curr_email_customer_id = records[k]['CustomerID'];
                    curr_email_record_id = records[k]['RID'];
                    //SetEmailStatusEnable(true, true, true, true, true, true, true);
                    // !NOTE 2 is TabEmailRecord
                    $("#Tabs").tabs("option", "active", 2);
                });
            }
        }
        else {
            new_tr.click(function () {
                var k = $(this).children('td').children('input.record_key').val();
                curr_telphone_customer_id = records[k]['CustomerID'];
                curr_telphone_record_id = records[k]['RID'];
                //!NOTE 1 is TabTelphoneRecord
                $("#Tabs").tabs("option", "active", 1);
            });
        }
        $("#RecordTable").append(new_tr);
    }
}