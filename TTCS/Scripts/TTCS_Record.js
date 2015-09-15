function QueryRecord() {
    var url = doc_root + "Record/Query/";
    var data_obj = {
        Name: $('#QueryRecordQueryName').val(),
        Tel: $('#QueryRecordQueryTel').val(),
        Email: $('#QueryRecordQueryEmail').val(),
        ID2: $('#QueryRecordQueryID2').val(),
        RID: $('#QueryRecordQueryRID').val(),
        Subject: $('#QueryRecordQuerySubject').val(),
        Type: $('#QueryRecordQueryType').val(),
        Status: $('#QueryRecordQueryStatus').val(),
        IncomingDTStart: $('#QueryRecordQueryIncomingDTStart').val(),
        IncomingDTEnd: $('#QueryRecordQueryIncomingDTEnd').val(),
        Approach: $('#QueryRecordQueryApproach').val(),
        Agent: $('#QueryRecordQueryAgent').val(),
    };
    log.debug('Qurey Record(' + url + '), data: ' + var_dump(data_obj));
    $.ajax({
        url: url,
        dataType: DATA_JSON,
        type: HTTP_POST,
        data: data_obj,
        success: function (records, status, jqXHR) {
            log.debug("Query Record return: " + var_dump(records));
            if (Object.keys(records).length === 0) {
                alert("No Search Result");
                log.info('No Record in this query');
            }
            else if (Object.keys(records).length === 1) {
                ShowRecordInfo("#QueryRecordContent", records[0]);
            }
            else {
                ShowRecordQueryResult(records);
            }
        },
        error: function (jqXHR, statud, msg) {
            alert('Error in Query records: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
            log.error('Error in Query records: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
        }
    })
}


function ShowRecordDetailList(parent_id, record_list) {
    $(parent_id + ' div.RecordInfo table.RecordListTable').empty()
        .append($('<caption/>').html('客戶歷史案件'))
        .append(
            $('<tr/>')
                .append($('<th/>').html("編號"))
                .append($('<th/>').html("類型"))
                .append($('<th/>').html("處理狀態"))
                .append($('<th/>').html("小結"))
                //.append($('<th/>').html("案件內容描述"))
                .append($('<th/>').html("客服人員"))
                .append($('<th/>').html("進件時間"))
                .append($('<th/>').html("結案時間"))
                .append($('<th/>').html("進線電話/郵件"))
                .append($('<th/>', { style: "visibility: hidden" }).html()));
    for (var key in record_list) {
        var record = record_list[key];
        $(parent_id + ' div.RecordInfo table.RecordListTable').append;
        var temp_row = $('<tr/>')
            .append($('<td/>').html(record['RID']))
            .append($('<td/>').html(record['TypeName']))
            .append($('<td/>').html(record['StatusName']))
            .append($('<td/>').html(record['ServiceItemName']))
            //.append($('<td/>').append($('<span/>').html('游標停留觀看').attr('title', record['Comment'])))
            .append($('<td/>').html(record['AgentID']))
            .append($('<td/>').html(ISODateStringToString(record['IncomingDT'])))
            .append($('<td/>').html(ISODateStringToString(record['FinishDT'])));
            
        if (record['TypeID'] == 1 || record['TypeID'] == 2) {
            temp_row.append($('<td/>').html(record['PhoneNum']));
        }
        else if (record['TypeID'] == 3) {
            temp_row.append($('<td/>').html(record['MsgFrom'].replace("<", "&lt;").replace(">", "&gt")));
            //TODO
        }
        temp_row.append($('<td/>', { style: "visibility: hidden" }).html(key))
            .click(function () {
            $("#HistoryRecordDialog").dialog({
                closeOnEscape: true,
                modal: true,
                resizable: false,
                width: 800,
            });
            var k = $("td:last", this).html();
            //ShowRecordInfo("#QueryRecordContent, records[key]);
            ShowRecordInfo("#HistoryRecordContent", record_list[k]);

        });
        $(parent_id + ' div.RecordInfo table.RecordListTable').append(temp_row);
    };
}

function ShowRecordQueryResult(records) {
    $("#RecordQueryResult").dialog({
        closeOnEscape: true,
        modal: true,
        resizable: false,
        width: 800,
    });

    $("#RecordQueryResultList").empty().append(
        $('<tr/>')
            .append($('<th/>').html("編號"))
            .append($('<th/>').html("客戶"))
            .append($('<th/>').html("客服人員"))
            .append($('<th/>').html("類型"))
            .append($('<th/>').html("狀態"))
            .append($('<th/>').html("進件時間"))
            .append($('<th/>').html("結案時間"))
            .append($('<th/>', { style: "visibility: hidden" }).html()));

    for (var key in records) {
        $("#RecordQueryResultList").append(
            $('<tr/>')
                .append($('<td/>').html(records[key]['RID']))
                .append($('<td/>').html(records[key]['CustomerName']))
                .append($('<td/>').html(records[key]['AgentName']))
                .append($('<td/>').html(records[key]['TypeName']))
                .append($('<td/>').html(records[key]['StatusName']))
                .append($('<td/>').html(ISODateStringToString(records[key]['IncomingDT'])))
                .append($('<td/>').html(ISODateStringToString(records[key]['FinishDT'])))
                .append($('<td/>', { style: "visibility: hidden" }).html(key))
                .click(function () {
                    $("#RecordQueryResult").dialog("destroy");
                    var key = $("td:last", this).html();
                    ShowRecordInfo("#QueryRecordContent", records[key]);
                    $("#RecordQueryResult").hide();
                })
        );
    }
}

function ShowRecordInfo(parent_id, record) {
    $(parent_id + " .RecordID").empty().html(record['RID']);
    $(parent_id + " .RecordType").empty().html(record['TypeName']);
    $(parent_id + " .RecordStatus").empty().html(record['StatusName']);
    $(parent_id + " .RecordCustomer").empty().html(record['CustomerName']);
    $(parent_id + " .RecordAgent").empty().html(record['AgentName']);
    $(parent_id + " .RecordIncomingDT").empty().html(ISODateStringToString(record['IncomingDT']));
    $(parent_id + " .RecordFinishDT").empty().html(ISODateStringToString(record['FinishDT']));
    $(parent_id + " .RecordServiceItem").empty().html(record['ServiceItemName']);
    $(parent_id + " .RecordComment").empty().html(nl2br(HtmlEncode(record['Comment'])));

    $(parent_id + ' .RecordDetail').empty();
    if (record['TypeID'] == 1 || record['TypeID'] == 2) {
        $(parent_id + ' .RecordDetail')
            .append("連絡電話：").append($("<label>").html(record['PhoneNum'])).append($("<br>"))
            .append("群組資訊：").append($("<label>").html(record['QueueName'])).append($("<br>"))
            .append("通話索引：").append($("<label>").html(record['IndexID'])).append($("<br>"))
    }
    else if (record['TypeID'] == 3) {
        var appendAttachments = "";
        if (record["MsgAttachments"] != null && record["MsgAttachments"].length > 0)
        {
            var arrAttachments = record["MsgAttachments"].split('|');
            for (var n = 0; n < arrAttachments.length; n++)
            {
                var arrAttachment = arrAttachments[n].split(':');
                appendAttachments += ',<a href="Record/Download/' + arrAttachment[0] + '">' + arrAttachment[1] + '</a>';
            }
        }    
        if (appendAttachments.length > 0)
            appendAttachments = appendAttachments.substr(1, appendAttachments.length - 1);

        $(parent_id + ' .RecordDetail')
            .append("寄 件 者：").append($("<label>").html(record['MsgFrom'])).append($("<br>"))
            .append("主    旨：").append($("<label>").html(record['MsgSubject'])).append($("<br>"))
            .append("進件時間：").append($("<label>").html(record['MsgReceivedOn'])).append($("<br>"))
            .append("內    容：").append($("<label>").html(record['MsgReceivedOn'])).append($("<br>"))
            .append($("<div>").html(record['MsgBody'])).append($("<br>"))
            .append("附    件：").append($("<label>").html(appendAttachments)).append($("<br>"))
    }
}