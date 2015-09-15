/***************************************** 
    * Init function Section
    *****************************************/
var doc_root;
var log;
var popUpAppender;
var activeTabIndex;
$(function () {
    log = log4javascript.getLogger();
    //popUpAppender = new log4javascript.PopUpAppender();
    //popUpAppender.setFocusPopUp(true);
    //popUpAppender.setNewestMessageAtTop(true);
    //var layout = new log4javascript.PatternLayout("[%-5p] %c: %m");
    //popUpAppender.setLayout(layout);
    //log.addAppender(popUpAppender);
    //popUpAppender.hide();
    log4javascript.setEnabled(true);

    doc_root = document.URL + ((document.URL[document.URL.length - 1] == "/") ? "" : "/");
    $("#Tabs").tabs({
        beforeActivate: function (event, ui) {            
            //console.log(ui.newTab[0].id);
            if (ui.newTab[0].id === "TabRecordBrowser") {
                $("#RecordListCurrentPage").val(1);
                var page = parseInt($("#RecordListCurrentPage").val());
                GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
            } else if (ui.newTab[0].id === "TabTelphoneRecord") {   
                SetTelphoneRecordTab();
            } else if (ui.newTab[0].id === "TabEmailRecord") {
                //Modify by Kevin 20140211
                curr_customer_id = curr_email_customer_id;
                SetCustomerBlock('#EmailRecord');

                if (curr_email_record_id !== null) {
                    GetEmailRecordInfo();
                } else {
                    // If no record which should be show, Lock all customer buttons.
                    SetCustomerButtonEnable("#EmailRecord", false, false, false, false);
                }
                //curr_telphone_record_id = null;
            }
            //----------------------
            if (activeTabIndex == "TabEmailRecord" && ui.newTab[0].id != "TabEmailRecord") {
                if (curr_email_record != null && curr_email_record_id != null && curr_email_record.StatusID != 3) {
                    SaveForEmailRecord(false);
                }
            };
            activeTabIndex = ui.newTab[0].id;
        },
    }).hide();

    // Hover states on the static widgets
    $("#dialog-link, #icons li").hover(
        function () {
            $(this).addClass("ui-state-hover");
        },
        function () {
            $(this).removeClass("ui-state-hover");
        }
    );

    $("#Unlogin").show();
    
    //Check Cookie with login info
    var agent_id = $.cookie('agent_id');
    var agent_login = $.cookie('agent_login');
    var ext_no = $.cookie('ext_no');

    if (agent_login !== null && ext_no !== null) {
        feed_login_info(agent_login, ext_no);
    }

    $("#dialog-reject").dialog({
        autoOpen: false,
        width: 300,
        height: 250,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });

    $("#dialog-modal-reply").dialog({
        autoOpen: false,
        width: 600,
        height: 250,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });

    $("#dialog-modal-contacts").dialog({
        autoOpen: false,
        width: 600,
        height: 250,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });

    $("#dialog-modal-forward").dialog({
        autoOpen: false,
        width: 1000,
        height: 400,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });
    //-----------------------------------------

    

    $('#ServiceItemSubmit').click(function () {
        var item_list = [];
        var title_array = [];
        var selected = $("#ServiceItemPickerTree").dynatree('getSelectedNodes');
        for (var idx in selected) {
            var service = {
                GroupID: selected[idx].data.group_id,
                ItemID: selected[idx].data.item_id,
            }
            item_list.push(service);
            title_array.push(selected[idx].data.title);
        }

        if ($("#Tabs").tabs("option", "active") === 1) {
            curr_telphone_record.ServiceItemList = item_list;
            SetTelphoneButtonEnable(true, !$('#TelphoneRecord div.RecordInfo input.RecordFinish').attr("disabled"));
        } else if ($("#Tabs").tabs("option", "active") === 2) {
            curr_email_record.ServiceItemList = item_list;
        }
        $("div.RecordInfo label.RecordServiceItem").html(title_array.join('，'));

        $("#ServiceItemPicker").dialog("destroy");
        $("#ServiceItemPicker").hide();
    }); 

    $("input.RecordServiceItemPicker").click(function () {
        $("#ServiceItemPicker").dialog({
            closeOnEscape: true,
            modal: true,
            resizable: false,
            width: 800,
        });
        var list;
        if ($("#Tabs").tabs("option", "active") === 1) {
            list = curr_telphone_record.ServiceItemList;
        } else if ($("#Tabs").tabs("option", "active") === 2) {
            list = curr_email_record.ServiceItemList;
        }
        $("#ServiceItemPickerTree").dynatree("getTree").reload();
        $("#ServiceItemPickerTree").dynatree("getTree").visit(function (node) {
            //node.select(false);
            for (var idx in list) {
                if (list[idx].GroupID === node.data.group_id &&
                    list[idx].ItemID === node.data.item_id) {
                    node.toggleSelect();
                }
            }
        });
    });

    $("#HistoryRecordDialog").hide();

    $("#TESTINCOME").click(function () {
        TelphoneIncoming(1, '43215678', '123456.789', 'queueueueueue', "call_info_call_info", "Enterprise");
    });

    $('#OpenLogWindow').click(function () {
        log4javascript.setEnabled(true);
        popUpAppender.show();
    })
});
