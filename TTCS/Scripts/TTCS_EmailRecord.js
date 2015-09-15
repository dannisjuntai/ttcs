/***************************************** 
* TelphoneRecordInfo function Section
*****************************************/
var curr_email_customer_id = null;
var curr_email_record_id = null;
var curr_email_record = {
    // 案件共同資訊
    RID: "",
    CustomerID: "",
    TypeID: "",
    AgentID: "",
    StatusID: "",
    Comment: "",
    ServiceGroupID: "",
    ServiceItemID: "",
    IncomingDT: "",
    FinishDT: "",
    GroupID: "",
    CloseApproachID: "",
    CloseApproachList: [],

    /* Additional Data*/
    CustomerName: "",
    TypeName: "",
    StatusName: "",
    ServiceItemName: "",
    ServiceItemList: [],

    // email record data
    EmailRecordErrMsg: "",      // 錯誤訊息
    MsgUnRead: "0",             // 郵件是否被讀取過, 檢視是否有新回信
    MsgSubjectModifiedBy: "",   // 郵件主旨是否被修改過, 僅能被更改一次
    EOrder: "0",                // 郵件順序, 配合郵件主旨僅能在新案件時修改, 若已有回信, 則無法修改
    MsgFrom: "",                // 郵件來源
    MsgSubject: "",             // 郵件主旨
    ExpireOn: "",               // 逾期時間
    AssignCount: "0",           // 轉派次數
    MsgReceivedOn: "",          // 郵件寄送時間
    CurrAgent: "",              // 目前客服人員ID
    EmailRecordID: "",          // 欲顯示之郵件ID, 要顯示最新一筆郵件
    MsgBody: "",                // 郵件內容
    MsgAttachments: "",         // 郵件附件
    ProcessStatus: "",          // 郵件處理狀況
    SendTo: "",                 // 郵件寄送目的地
    IsDraft: "",                // 是否已有草稿
    Cc: "",                     // 郵件副本
    Bcc: "",                    // 郵件密件副本
    MailSig: "1",               // 是否夾帶簽名檔
};

//20140303, Clyde, move set customer function handler to here
//Make sure those handler only be binded once
$(function () {
    $('#EmailRecordCloseApproach').change(function () { curr_email_record.CloseApproachID = $('#EmailRecordCloseApproach option:selected').val(); });

    $.cleditor.defaultOptions.width = 720;
    $("#EmailRecordMsgBody").cleditor();//{ width: 720, height: 250 });
    
    DisableEmailRecordAllInputs();

    $('#EmailRecord div.CustomerInfo input.CustomerAdd').click(function () {
        CustomerViewStatusAdd('#EmailRecord');
        var nStartPos = curr_email_record['MsgFrom'].lastIndexOf('<');
        var nEndPos = curr_email_record['MsgFrom'].lastIndexOf('>');
        if(nStartPos < 0){
            nStartPos = -1;
        };
        if(nEndPos < 0){
            nEndPos = curr_email_record['MsgFrom'].length+1;
        };
        $('#EmailRecord div.CustomerInfo input.Customer_Contact1_Email1').val(curr_email_record['MsgFrom'].substr(nStartPos+1, nEndPos-nStartPos-1));
    });
    $('#EmailRecord div.CustomerInfo input.CustomerEdit').click(function () {
        CustomerViewStatusEdit('#EmailRecord');
    });
    $('#EmailRecord div.CustomerInfo input.CustomerConfirm').click(function () {
        //Send to server
        CustomerModelUpdateFromView('#EmailRecord');
        if (curr_customer["CName"] === null ||
                curr_customer["CName"] === undefined ||
                curr_customer["CName"] === "") {
            alert("請輸入中文名稱");
            return;
        }
        if (curr_customer_id === null) {
            CustomerModelAdd('#EmailRecord', function () {
                curr_email_customer_id = curr_customer_id;
                CustomerViewStatusShow('#EmailRecord', function () {
                    if (curr_email_record !== null) {
                        curr_email_record.CustomerID = curr_customer_id;
                        $("#EmailRecord div.RecordInfo input.RecordSave").click();
                    }
                });
            });
        }
        else {
            UploadCustomerInfo('#EmailRecord', function () {
                curr_email_customer_id = curr_customer_id;
                CustomerViewStatusShow('#EmailRecord', function () {
                    if (curr_email_record !== null) {
                        curr_email_record.CustomerID = curr_customer_id;
                        $("#EmailRecord div.RecordInfo input.RecordSave").click();
                    }
                });
            });
        }
    });

    $('#EmailRecord input#EmailRecordSearchButton').click(function () {
        CustomerSearch('#EmailRecord', function () {
            curr_email_customer_id = curr_customer_id;
            CustomerViewStatusShow('#EmailRecord', function () {
                if (curr_email_record !== null) {
                    curr_email_record.CustomerID = curr_customer_id;
                    $("#EmailRecord div.RecordInfo input.RecordSave").click();
                }
            });
        });
    });

    // Button Events
    // 1.儲存
    $("#EmailRecord div.RecordInfo input.RecordSave").click(function () {
        DisableEmailRecordAllInputs();
        
        SaveForEmailRecord(true);

        InitEmailRecordAllInputs();
    });

    // 2.結案
    $("#EmailRecord div.RecordInfo input.RecordFinish").click(function () {
        if (curr_email_record.ServiceItemList == null || curr_email_record.ServiceItemList.length < 1) {
            alert("請選擇小結!!");
            return;
        }
        DisableEmailRecordAllInputs();
        curr_email_record.StatusID = 3;
        SaveForEmailRecord(true);

        InitEmailRecordAllInputs();
        $("#Tabs").tabs({ active: 0 });
    });

    // 3. 回覆
    $("#EmailRecord div.RecordInfo input.RecordReply").click(function () {
        EnableEmailRecordAllInputs();
        $('#forward').attr("disabled", true);                                       // 轉寄名單
        $('#EmailRecord div.RecordInfo input.Contacts').attr("disabled", true);     // 轉寄清單
        $('#EmailRecord div.RecordInfo input.RecordReply').attr("disabled", true);      // 回覆

        var editor = $("#EmailRecordMsgBody").cleditor()[0];
        if (editor.$area.val().indexOf('<div id="ttcsoriginal">') < 0) {            
            var msghead = '<b>From:</b>&nbsp;' + curr_email_record.MsgFrom.replace("<", "&lt; ").replace(">", "&gt; ") +
                          '<br><b>Sent:</b>&nbsp;' + curr_email_record.MsgReceivedOn +
                          '<br><b>Subject:</b>&nbsp;' + curr_email_record.MsgSubject;
            editor.$area.html('<br><div id="ttcsoriginal"><hr id="seperate" size="0.5px" color="#339999"></hr>' + msghead + editor.$area.html() + "</div>");
            editor.updateFrame();
            editor.updateTextArea();
        }

        editor.focus();
    });

    // 4. 轉寄
    $("#EmailRecord div.RecordInfo input.RecordForward").click(function () {
        EnableEmailRecordAllInputs();
        $('#EmailRecord div.RecordInfo input.RecordForward').attr("disabled", true);    // 轉寄
        $('#EmailRecordMsgFrom').attr("disabled", true);                            // 客戶信箱

        var editor = $("#EmailRecordMsgBody").cleditor()[0];
        
        if (editor.$area.val().indexOf('<div id="ttcsoriginal">') < 0) {
            var msghead = '<b>From:</b>&nbsp;' + curr_email_record.MsgFrom.replace("<", "&lt; ").replace(">", "&gt; ") +
                          '<br><b>Sent:</b>&nbsp;' + curr_email_record.MsgReceivedOn +
                          '<br><b>Subject:</b>&nbsp;' + curr_email_record.MsgSubject;
            editor.$area.html('<br><div id="ttcsoriginal"><hr id="seperate" size="0.5px" color="#339999"></hr>' + msghead + editor.$area.html() + "</div>");
            editor.updateFrame();
            editor.updateTextArea();
        }
        editor.focus();

    });

    // 5. 退回
    $("#EmailRecord div.RecordInfo input.RecodReject").click(function () {
        log.debug('Send to Record/_PartialOpenPopupReject');
        $.ajax({
            url: doc_root + "Record/_PartialOpenPopupReject",
            type: 'Get',
            data: "",
            datatype: "html",
            cache: false,
            success: function (result) {
                $("#dialog-reject").dialog("open");
                $("#dialog-reject").empty().append(result);
                $('#EmailRecodRejectConfirm').click(function () {
                    RejectForEmailRecord();
                    $("#dialog-reject").dialog("close");
                    $("#Tabs").tabs({ active: 0 });
                });
            },
            error: function () {
                alert("something seems wrong");
            }
        });
    });

    // 6. 插入回覆範本
    $("#EmailRecord div.RecordInfo input.ReplyCan").click(function () {
        var data_obj = "agentId=" + escape(curr_email_record.AgentID);
        log.debug('Send to Record/_PartialOpenPopupReply' + var_dump(data_obj));
        $.ajax({
            url: doc_root + "Record/_PartialOpenPopupReply",
            type: 'Get',
            data: data_obj,
            datatype: "html",
            cache: false,
            success: function (result) {
                $("#dialog-modal-reply").dialog("open");
                $("#dialog-modal-reply").empty().append(result);
                $('#saveReplyCan').click(function () {                    
                    var selectedIndex = $("input[name=replycan]:checked").index();
                    //var replycan = $("input[name=replycan]:checked").next('input:hidden').val();
                    var tmpid = "#" + selectedIndex.toString();
                    var replycan = $(tmpid).val();
                    var editor = $("#EmailRecordMsgBody").cleditor()[0];
                    $('#EmailRecordMsgBody').val(replycan + editor.$area.val()).blur();
                    //editor.execCommand("paste", replycan, null, null);
                    //editor.execCommand("inserthtml", replycan, null, null);
                    //editor.updateFrame();
                    $("#dialog-modal-reply").dialog("close");
                });
            },
            error: function () {
                alert("something seems wrong");
            }
        });
    });

    // 7. 轉寄清單
    $("#EmailRecord div.RecordInfo input.Contacts").click(function () {
        log.debug('Send to Record/_PartialOpenPopupContacts');
        $.ajax({
            url: doc_root + "Record/_PartialOpenPopupContacts",
            type: 'Get',
            data: "",
            datatype: "html",
            cache: false,
            success: function (result) {
                $("#dialog-modal-contacts").dialog("open");
                $("#dialog-modal-contacts").empty().append(result);
                $('#savecontacts').click(function () {
                    var contacts = $("input[name=contacts]:checked").val();
                    $("#forward").val(contacts);
                    $("#dialog-modal-contacts").dialog("close");
                });
            },
            error: function () {
                alert("something seems wrong");
            }
        });
    });

    // 8. 相關案件
    $("#EmailRecord div.RecordInfo input.RecordBCase").click(function () {
        var data_obj = "id=" + escape(curr_email_record_id);
        log.debug('Send to Record/_PartialOpenPopupForwards, ' + var_dump(data_obj));
        $.ajax({
            url: doc_root + "Record/_PartialOpenPopupForwards",
            type: 'Get',
            data: data_obj,
            datatype: "html",
            cache: false,
            success: function (result) {
                $("#dialog-modal-forward").dialog("open");
                $("#dialog-modal-forward").empty().append(result);
            },
            error: function () {
                alert("something seems wrong");
            }
        });
    });

    // 9. 寄送
    $("#EmailRecord div.RecordInfo input.RecordSend").click(function () {
        if (curr_email_record.MsgReceivedOn.length < 1) {
            var SendTo = $('#EmailRecordMsgFrom').val();
            if (SendTo.length < 1) {
                alert("請輸入客戶信箱資訊");
                return;
            }
            var cfmmsg = "確定寄送給 '" + SendTo + "' ?";

            var c = confirm(cfmmsg);

            if (c == false)
                return;
        } else {
            var SendTo = $('#forward').val();
            var att = $('#EmailRecord div.RecordInfo input.RecordForward').attr("disabled");
            if (att == true || att == "disabled") {
                if (SendTo.length < 1) {
                    alert("請輸入轉寄名單資訊");
                    return;
                }
            } else {
                SendTo = "";
            };


            var cfmmsg = "";
            if (SendTo.length > 0) {
                cfmmsg = "確定轉寄給 '" + SendTo + "' ?"
            } else {
                cfmmsg = "確定回覆給寄件者 '" + $('#EmailRecordMsgFrom').val() + "' ?"
            }

            var c = confirm(cfmmsg);

            if (c == false)
                return;
        }
        DisableEmailRecordAllInputs();
        $('#EmailRecord div.RecordInfo input.RecordSend').attr("disabled", true);
        var editor = $("#EmailRecordMsgBody").cleditor()[0];
        //curr_email_record.MsgBody = editor.$area.html();
        curr_email_record.MsgBody = editor.$area.val();
        curr_email_record.SendTo = SendTo;
        curr_email_record.Cc = $('#EmailCC').val();
        curr_email_record.Bcc = $('#EmailBCC').val();
        curr_email_record.MsgSubject = $('#EmailRecordMsgSubject').val();
        if ($('input[name="mailsignature"]').is(":checked")) {
            curr_email_record.MailSig = "1";
        } else {
            curr_email_record.MailSig = "0";
        }
        curr_email_record.ProcessStatus = $('#EmailRecordProcessStatus option:selected').val();

        ReplyForEmailRecord();
        
    });

    // 10.上傳檔案
    $('#fileupload').fileupload({
            dataType: 'json',
            url: doc_root + "Record/_UploadFiles/" + escape(curr_email_record.EmailRecordID),
        //maxFileSize: 10000000,
        //maxChunkSize:10000000,
            done: function (e, data) {                
                if (data.result.NewFileName != undefined) {
                    var arrAttachments = data.result.NewFileName.split('|');
                    $('#uploadfiles').empty('');
                    for (var n = 0; n < arrAttachments.length; n++) {
                        var arrAttachment = arrAttachments[n];
                        var arrAttachmentInfo = arrAttachment.split(':');
                        $('#uploadfiles')
                            .append($('<a href="' + doc_root + 'Record/DownloadFile/' + arrAttachmentInfo[0] + '">' + arrAttachmentInfo[1] + '</a>'))
                            .append($('<input type=button value="刪除" />').click(function () { DeleteFile(arrAttachmentInfo[0]); })).append($('<br />'));
                    }
                    //$('#uploadfiles').empty('').html(appendAttachments);
                    // update c_newfileName      
                    // results in: data.result.UserFileName   
                } else {
                    alert(data.result);
                }
            },
            add: function (e, data) {
                curr_email_record.IsDraft = "1";
                data.url = doc_root + "Record/_UploadFiles/" + escape(curr_email_record.EmailRecordID);
                // strats upload after adding file
                data.submit();
            },
            fail: function (e, data) {
                log.debug('Send to Record/AjaxGetMaxRequestLen');
                $.ajax({
                    url: doc_root + "Record/AjaxGetMaxRequestLen",
                    type: 'Get',
                    data: "",
                    datatype: "json",
                    cache: false,
                    success: function (record, status, jqXHR) {
                        if (record.result * 1000 < data.total) {
                            alert("Upload File Failed, Max Size:" + record.result + " KB!");
                        }
                    },
                    error: function (jqXHR, status, msg) {
                        alert("Upload File Failed");
                    }
                });
            },
    });
    
    // 11. 建立新郵件案件
    $("#btnCreateEmailCase").click(function () {
        var data_obj = "agentId=" + escape($.cookie('agent_id'));
        log.debug('Send to Record/AjaxEmailRecordCreate, ' + var_dump(data_obj));
        $.ajax({
            url: doc_root + "Record/AjaxEmailRecordCreate",
            type: 'Get',
            data: data_obj,
            datatype: "json",
            cache: false,
            success: function (record, status, jqXHR) {
                if (record['result'] != null && record['result'].length > 0) {
                    alert(record['result']);
                } else {
                    if (record['EmailRecordErrMsg'].length > 0) {
                        alert("Get EmailRecord Failed[" + record['EmailRecordErrMsg'] + "]");
                    } else {
                        UpdateEmailRecordModel(record);
                        curr_email_record_id = record['RID'];
                        ShowEmailRecordInfo('#EmailRecord', eval(record));
                        $("#Tabs").tabs({ active: 2 });
                    }
                }
            },
            error: function (jqXHR, status, msg) {
                alert('Error' + jqXHR + ", Msg=" + msg + "[" + curr_email_record_id + "]");
            }
        });
    });
});

function DeleteFile(filename) {
    var data_obj = "id=" + escape(filename);
    log.debug('Send to Record/DeleteFile, data: ' + var_dump(data_obj));
    curr_email_record.IsDraft = "1";
    $.ajax({
        url: doc_root + "Record/DeleteFile",
        type: 'Get',
        data: data_obj,
        datatype: "html",
        cache: false,
        success: function (result) {
            $('#uploadfiles').empty('');
            if (result['NewFileName'].length > 0) {
                var appendAttachments = "";
                var arrAttachments = result['NewFileName'].split('|');

                for (var n = 0; n < arrAttachments.length; n++) {
                    var arrAttachment = arrAttachments[n];
                    var arrAttachmentInfo = arrAttachment.split(':');
                    $('#uploadfiles').append($('<a href="' + doc_root + 'Record/DownloadFile/' + arrAttachmentInfo[0] + '">' + arrAttachmentInfo[1] + '</a>'))
                            .append($('<input type=button value="刪除" />').click(function () { DeleteFile(arrAttachmentInfo[0]); })).append($('<br />'));
                }
                if (appendAttachments.length > 0)
                    appendAttachments = appendAttachments.substr(1, appendAttachments.length - 1);

                //$('#uploadfiles').empty('').html(appendAttachments);
            }
        },
        error: function () {
            alert("something seems wrong");
        }
    });
};

function GetEmailRecordInfo() {
    log.debug('Send to Record/JsonInfo/' + curr_email_record_id);
    $.ajax({
        url: doc_root + "Record/JsonInfo/" + curr_email_record_id,
        dataType: 'json',
        cache: false,
        success: function (record, status, jqXHR) {
            if (record['EmailRecordErrMsg'].length > 0) {
                alert("Get EmailRecord Failed[" + record['EmailRecordErrMsg'] + "]");
            } else {
                UpdateEmailRecordModel(record);
                curr_email_record_id = record['RID'];
                ShowEmailRecordInfo('#EmailRecord', eval(record));
            }
        },
        error: function (jqXHR, status, msg) {
            alert('Error' + jqXHR + ", Msg=" + msg + "[" + curr_email_record_id + "]");
        }
    });
};

function UpdateEmailRecordModel(record) {
    curr_email_record = record;
};

function RejectForEmailRecord() {
    var data_obj = "eid=" + curr_email_record.EmailRecordID + "&Reason=" + $("input[name=RejectReason]:checked").val();
    log.debug('Send to Record/AjaxRejectForEmailRecord, data:' + var_dump(data_obj));
    $.ajax({
        type: 'POST',
        url: doc_root + "Record/AjaxRejectForEmailRecord",
        data: data_obj,
        dataType: 'json',
        success: function (response, status, jqXHR) {
            if (response['result'] != 0) {
                alert('Reject Email record failure');
            }
        },
        error: function (jqXHR, status, msg) {
            alert('Error' + jqXHR);
        }
    })
}

function ReplyForEmailRecord() {
    var dataToSend = JSON.stringify(
        {
            'RID': curr_email_record.RID,
            'EID': curr_email_record.EmailRecordID,
            'SendTo': curr_email_record.SendTo,
            'Cc': curr_email_record.Cc,
            'Bcc': curr_email_record.Bcc,
            'MailSig': curr_email_record.MailSig,            
            'ProcessStatus': curr_email_record.ProcessStatus,
            'MsgReplyBody': curr_email_record.MsgBody,
            'MsgSubject': curr_email_record.MsgSubject
        });
    log.debug('Send to Record/AjaxReplyEmailRecord, data: ' + var_dump(dataToSend));
    $.ajax({
        type: 'POST',
        url: doc_root + "Record/AjaxReplyEmailRecord",
        data: dataToSend,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (response, status, jqXHR) {
            if (response['result'] != "0" && response['result'].length > 0) {
                alert('寄送郵件失敗!![' + response['result']+']');
            } else {
                alert('寄送郵件成功...');
                $("#Tabs").tabs({ active: 0 });
            }
            InitEmailRecordAllInputs();
        },
        error: function (jqXHR, status, msg) {
            alert('Error' + status + ":" + msg + ":" + jqXHR);
            InitEmailRecordAllInputs();
        }
    })
}

function SaveForEmailRecord(byclick, before_send) {
    
    var editor = $("#EmailRecordMsgBody").cleditor()[0];
    var SendTo = $('#forward').val();
    var MailSig = "0";
    if ($('input[name="mailsignature"]').is(":checked")) {
        MailSig = "1";
    } else {
        MailSig = "0";
    }
    var Cc = $('#EmailCC').val();
    var Bcc = $('#EmailBCC').val();
    var MsgSubject = $('#EmailRecordMsgSubject').val();
    if (curr_email_record.IsDraft != "1") {
        if (curr_email_record.MsgBody != editor.$area.val() ||
            curr_email_record.SendTo != SendTo || curr_email_record.Cc != Cc || curr_email_record.Bcc != Bcc || curr_email_record.MsgSubject != MsgSubject) {

            curr_email_record.MsgBody = editor.$area.val();
            curr_email_record.SendTo = SendTo;
            curr_email_record.Cc = Cc;
            curr_email_record.Bcc = Bcc;
            curr_email_record.MsgSubject = MsgSubject;
        } else {
            curr_email_record.SendTo = "-1";
        };
    } else {
        curr_email_record.MsgBody = editor.$area.val();
        if (SendTo.length < 1) {
            SendTo = $('#EmailRecordMsgFrom').val();
        }
        curr_email_record.SendTo = SendTo;
        curr_email_record.Cc = Cc;
        curr_email_record.Bcc = Bcc;
        curr_email_record.MsgSubject = MsgSubject;
    };

    curr_email_record.Comment = $("#EmailRecordComment").val();
    curr_email_record.ProcessStatus = $('#EmailRecordProcessStatus option:selected').val();

    var dataToSend = {
        'record': GenerateULEmailRecord(),
        'eid': curr_email_record.EmailRecordID,
        'SendTo': curr_email_record.SendTo,
        'Cc': curr_email_record.Cc,
        'Bcc': curr_email_record.Bcc,
        'MailSig': curr_email_record.MailSig,
        'MsgReplyBody': curr_email_record.MsgBody,
        'ProcessStatus': curr_email_record.ProcessStatus,
        'MsgSubject': curr_email_record.MsgSubject,
        'serviceItemList': curr_email_record.ServiceItemList,
    };
    log.debug('Send to Record/AjaxUpdateEmailRecord, data: ' + var_dump(dataToSend));
    $.ajax(
        {
            type: 'POST',
            url: doc_root + "Record/AjaxUpdateEmailRecord",
            data: JSON.stringify(dataToSend),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (response, status, jqXHR) {
                if (byclick == true) {
                    if (response['result'] != "0") {                    
                        alert('儲存失敗[' + response['errMsg'] + ']');
                    } else {
                        alert('儲存成功');
                    }
                }
                if (before_send !== undefined) {
                    before_send();
                }
                curr_email_record.SendTo = SendTo;
            },
            error: function (jqXHR, status, msg) {
                alert('Error' + jqXHR);
            }
        }
    )
}

function GenerateULEmailRecord() {
    return {
        RID: curr_email_record.RID,
        CustomerID: curr_email_record.CustomerID,
        TypeID: curr_email_record.TypeID,
        AgentID: curr_email_record.AgentID,
        StatusID: curr_email_record.StatusID,
        Comment: curr_email_record.Comment,
        ServiceGroupID: curr_email_record.ServiceGroupID,
        ServiceItemID: curr_email_record.ServiceItemID,
        IncomingDT: curr_email_record.IncomingDT,
        FinishDT: curr_email_record.FinishDT,
        GroupID: curr_email_record.GroupID,
        CloseApproachID: curr_email_record.CloseApproachID,
    };
}

function DisableEmailRecordAllInputs() {
    $('#EmailRecord div.RecordInfo input.RecordSave').attr("disabled", true);       // 儲存    
    $('#EmailRecord div.RecordInfo input.RecordFinish').attr("disabled", true);     // 結案
    $('#EmailRecord div.RecordInfo input.RecordBCase').attr("disabled", true);      // 相關案件
    $('#EmailRecord div.RecordInfo input.RecodReject').attr("disabled", true);      // 退回
    $('#EmailRecord div.RecordInfo input.RecordReply').attr("disabled", true);      // 回覆
    $('#EmailRecord div.RecordInfo input.RecordForward').attr("disabled", true);    // 轉寄
    $('#EmailRecordServiceItemPicker').attr("disabled", true);                      // 小結
    
    $('#EmailRecordProcessStatus').attr("disabled", true);                      // 處理狀況

    $('#EmailRecordMsgFrom').attr("disabled", true);                            // 客戶信箱
    $('#forward').attr("disabled", true);                                       // 轉寄名單
    $('#EmailRecord div.RecordInfo input.Contacts').attr("disabled", true);     // 轉寄清單
    $('#EmailRecord div.RecordInfo input.RecordSend').attr("disabled", true);   // 寄送
    $('#EmailRecord div.RecordInfo input.ReplyCan').attr("disabled", true);     // 插入範本
    $('#EmailCC').attr("disabled", true);                                       // 副本名單
    $('#EmailBCC').attr("disabled", true);                                      // 密件副本
    $('#EmailRecordMsgSubject').attr("disabled", true);                         // 郵件主旨
    $('#EmailRecordMsgSubjectWarn').text('');                                   // 郵件主旨警語
    var editor = $("#EmailRecordMsgBody").cleditor()[0];                        // 郵件內容
    editor.disable(true);

    $('#mailsignature').attr("disabled", true);                                 // 夾帶簽名檔
    $('#uploadattach').attr("disabled", true);                                  // 上傳附檔

    $('#EmailRecordCloseApproach').attr("disabled", true);                      // 處理方式
    $('#EmailRecordComment').attr("disabled", true);                            // 案件內容描述
};

function EnableEmailRecordAllInputs() {
    $('#EmailRecord div.RecordInfo input.RecordSave').attr("disabled", false);       // 儲存    
    $('#EmailRecord div.RecordInfo input.RecordFinish').attr("disabled", false);     // 結案
    $('#EmailRecord div.RecordInfo input.RecordBCase').attr("disabled", false);      // 相關案件
    $('#EmailRecord div.RecordInfo input.RecodReject').attr("disabled", false);      // 退回
    $('#EmailRecord div.RecordInfo input.RecordReply').attr("disabled", false);      // 回覆
    $('#EmailRecord div.RecordInfo input.RecordForward').attr("disabled", false);    // 轉寄
    $('#EmailRecordServiceItemPicker').attr("disabled", false);                      // 小結

    $('#EmailRecordProcessStatus').attr("disabled", false);                      // 處理狀況

    $('#EmailRecordMsgFrom').attr("disabled", false);                            // 客戶信箱
    $('#forward').attr("disabled", false);                                       // 轉寄名單
    $('#EmailRecord div.RecordInfo input.Contacts').attr("disabled", false);     // 轉寄清單
    $('#EmailRecord div.RecordInfo input.RecordSend').attr("disabled", false);   // 寄送
    $('#EmailRecord div.RecordInfo input.ReplyCan').attr("disabled", false);     // 插入範本
    $('#EmailCC').attr("disabled", false);                                       // 副本名單
    $('#EmailBCC').attr("disabled", false);                                      // 密件副本
    $('#EmailRecordMsgSubject').attr("disabled", false);                         // 郵件主旨
    $('#EmailRecordMsgSubjectWarn').text('');                                    // 郵件主旨警語
    var editor = $("#EmailRecordMsgBody").cleditor()[0];                         // 郵件內容
    editor.disable(false);

    $('#mailsignature').attr("disabled", false);                                 // 夾帶簽名檔
    $('#uploadattach').attr("disabled", false);                                  // 上傳附檔

    $('#EmailRecordCloseApproach').attr("disabled", false);                      // 處理方式
    $('#EmailRecordComment').attr("disabled", false);                            // 案件內容描述
};

function InitEmailRecordAllInputs() {

    // 初始化
    if (curr_email_record_id == null) {
        DisableEmailRecordAllInputs();
        return;
    };

    // 已結案
    if (curr_email_record.StatusID == "3") {
        DisableEmailRecordAllInputs();
        $('#EmailRecord div.RecordInfo input.RecordBCase').attr("disabled", false);      // 相關案件
        return;
    };

    EnableEmailRecordAllInputs();
    $('#forward').attr("disabled", true);                                       // 轉寄名單
    $('#EmailRecord div.RecordInfo input.Contacts').attr("disabled", true);     // 轉寄清單

    if (curr_email_record.MsgReceivedOn.length > 0 || curr_email_record.EOrder != "0") {

        $('#EmailRecordProcessStatus').attr("disabled", true);                      // 處理狀況

        $('#EmailRecordMsgFrom').attr("disabled", true);                            // 客戶信箱        
        $('#EmailRecord div.RecordInfo input.RecordSend').attr("disabled", true);   // 寄送
        $('#EmailRecord div.RecordInfo input.ReplyCan').attr("disabled", true);     // 插入範本
        $('#EmailCC').attr("disabled", true);                                       // 副本名單
        $('#EmailBCC').attr("disabled", true);                                      // 密件副本
        $('#EmailRecordMsgSubject').attr("disabled", true);                         // 郵件主旨
        $('#EmailRecordMsgSubjectWarn').text('');                                   // 郵件主旨警語
        var editor = $("#EmailRecordMsgBody").cleditor()[0];                        // 郵件內容
        editor.disable(true);

        $('#mailsignature').attr("disabled", true);                                 // 夾帶簽名檔
        $('#uploadattach').attr("disabled", true);                                  // 上傳附檔

        $('#EmailRecordComment').attr("disabled", true);                            // 案件內容描述
    } else {
        $('#EmailRecord div.RecordInfo input.RecordReply').attr("disabled", true);      // 回覆
        $('#EmailRecord div.RecordInfo input.RecordForward').attr("disabled", true);    // 轉寄
        $('#EmailRecord div.RecordInfo input.RecodReject').attr("disabled", true);      // 退回
    };

    if (curr_email_record.EOrder == "0") {
        if (curr_email_record.MsgSubjectModifiedBy.length > 1) {
            $('#EmailRecordMsgSubjectWarn').text('(此主旨已被' + curr_email_record.MsgSubjectModifiedBy + '修改過)');
        };
    }else {
        if (curr_email_record.MsgSubjectModifiedBy.length > 1) {
            $('#EmailRecordMsgSubjectWarn').text('(此主旨已修改過:修改人員' + curr_email_record.MsgSubjectModifiedBy + ')');
        } else {
            $('#EmailRecordMsgSubjectWarn').text('(新案件才可修改主旨, 一次為限)');
        };
    };
};

function SetEmailStatusEnable(save_btn, reject_btn, reply_btn, forward_btn, finish_btn, bcase_btn, serviceitem_btn) {

    $('#EmailRecord div.RecordInfo input.RecordSave').attr("disabled", !save_btn);          // 儲存    
    $('#EmailRecord div.RecordInfo input.RecordFinish').attr("disabled", !finish_btn);      // 結案
    $('#EmailRecord div.RecordInfo input.RecordBCase').attr("disabled", !bcase_btn);        // 相關案件
    $('#EmailRecord div.RecordInfo input.RecodReject').attr("disabled", !reject_btn);       // 退回
    $('#EmailRecord div.RecordInfo input.RecordReply').attr("disabled", !reply_btn);        // 回覆
    $('#EmailRecord div.RecordInfo input.RecordForward').attr("disabled", !forward_btn);    // 轉寄
    $('#EmailRecordServiceItemPicker').attr("disabled", !serviceitem_btn);                  // 小結
};
  
function ShowEmailRecordInfo(parent_id, record) {

    InitEmailRecordAllInputs();

    // 案件基本資料
    $(parent_id + " div.RecordInfo label.RecordID").empty('').html(record['RID']);
    $(parent_id + " div.RecordInfo label.RecordType").empty('').html(record['TypeName']);
    $(parent_id + " div.RecordInfo label.RecordStatus").empty('').html(record['StatusName']);
    $(parent_id + " div.RecordInfo label.RecordExpireOn").empty('').html(ISODateStringToString(record['ExpireOn']));
    $(parent_id + " div.RecordInfo label.RecordIncomingDT").empty('').html(ISODateStringToString(record['IncomingDT']));
    $(parent_id + " div.RecordInfo label.RecordAssignCount").empty('').html(record['AssignCount']);
    $('#EmailRecordProcessStatus').val(record['ProcessStatus']);

    // 郵件內容
    if (record['IsDraft'] == "1") {
        $('#EmailRecordDraft').text("(草稿)");
    } else {
        $('#EmailRecordDraft').text("");
    }
    $(parent_id + " div.RecordInfo input#EmailRecordMsgFrom").val(record['MsgFrom']);
    $(parent_id + " div.RecordInfo input#forward").val(record['SendTo']);
    $(parent_id + " div.RecordInfo input#EmailCC").val(record['Cc']);
    $(parent_id + " div.RecordInfo input#EmailBCC").val(record['Bcc']);
    $(parent_id + " div.RecordInfo input#EmailRecordMsgSubject").val(record['MsgSubject']);
    $(parent_id + " div.RecordInfo label.RecordMsgReceivedOn").empty('').html(ISODateStringToString(record['MsgReceivedOn']));
        
    $('#EmailRecordMsgBody').val(record["MsgBody"]).blur();
    var editor = $("#EmailRecordMsgBody").cleditor()[0];    
    editor.updateFrame();
    editor.updateTextArea();
    curr_email_record.MsgBody = editor.$area.val();

    var appendAttachments = ""; appendAttachmentsSaved = "";
    $('#uploadfiles').empty('');
    if (record["MsgAttachments"] != null && record["MsgAttachments"].length > 0) {
        var attchs = record["MsgAttachments"];
        if (attchs.indexOf('|') == 0) {
            attchs = attchs.substr(1, attchs.length - 1);
        };
        var arrAttachments = attchs.split('|');        
        for (var n = 0; n < arrAttachments.length; n++) {
            var arrAttachment = arrAttachments[n].split(':');

            if (record['IsDraft'] == "1") {
                $('#uploadfiles').append($('<a href="' + doc_root + 'Record/DownloadFile/' + arrAttachment[0] + '">' + arrAttachment[1] + '</a>'))
                        .append($('<input type=button value="刪除" />').click(function () { DeleteFile(arrAttachment[0]); })).append($('<br />'));
            } else {
                appendAttachments += '<br /><a href="' + doc_root + 'Record/Download/' + arrAttachment[0] + '">' + arrAttachment[1] + '</a>';
            }
        }
    }
    $(parent_id + " div.RecordInfo label.RecordMsgAttachments").empty('');
    if (appendAttachments.length > 0) {
        //appendAttachments = appendAttachments.substr(1, appendAttachments.length - 1);
        $(parent_id + " div.RecordInfo label.RecordMsgAttachments").empty().append(appendAttachments);
    }

    if (record['MailSig'] == "1") {
        $('#mailsignature').attr('checked', true);
    } else {
        $('#mailsignature').attr('checked', false);
    }

    // 案件內容編修
    $(parent_id + " div.RecordInfo textarea.RecordComment").empty().html(record['Comment']);
    $(parent_id + " div.RecordInfo label.RecordServiceItem").empty().html(record['ServiceItemName']);

    $(parent_id + " div.RecordInfo select.RecordCloseApproach").empty();
    $(parent_id + " div.RecordInfo select.RecordCloseApproach").append($('<option></option>').val("").text("--請選擇--"));
    $.each(record['CloseApproachList'], function (i, item) {
        if (curr_email_record.CloseApproachID != null && item.ApproachID == curr_email_record.CloseApproachID) {
            $(parent_id + " div.RecordInfo select.RecordCloseApproach").append($('<option selected></option>').val(item.ApproachID).text(item.ApproachName));
        } else {
            $(parent_id + " div.RecordInfo select.RecordCloseApproach").append($('<option></option>').val(item.ApproachID).text(item.ApproachName));
        }
    });
};