/***************************************** 
* TelphoneRecordInfo function Section
*****************************************/
var curr_telphone_customer_id = null;
var curr_telphone_record_id = null;
var curr_telphone_record = {
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
    AgentName: "",
    TypeName: "",
    StatusName: "",
    ServiceItemName: "",
    ServiceItemList: []
};

//20140303, Clyde, move set customer function handler to here
//Make sure those handler only be binded once
$(function () {
    $('#TelphoneRecordCloseApproach').change(function () { curr_telphone_record.CloseApproachID = $('#TelphoneRecordCloseApproach option:selected').val(); });

    $('#TelphoneRecord div.CustomerInfo input.CustomerAdd').click(function () {
        CustomerViewStatusAdd('#TelphoneRecord');
        $('#TelphoneRecord div.CustomerInfo input.Customer_Contact1_Tel1').val(curr_telphone_record['PhoneNum']);
    });
    $('#TelphoneRecord div.CustomerInfo input.CustomerEdit').click(function () {
        CustomerViewStatusEdit('#TelphoneRecord');
    });
    $('#TelphoneRecord div.CustomerInfo input.CustomerConfirm').click(function () {
        //Send to server
        CustomerModelUpdateFromView('#TelphoneRecord');
        if (curr_customer["CName"] === null ||
                curr_customer["CName"] === undefined ||
                curr_customer["CName"] === "") {
            alert("請輸入中文名稱");
            return;
        }
        if (curr_customer_id === null) {
            CustomerModelAdd('#TelphoneRecord', function () {
                log.debug("Callback of CustomerModelAdd() in CustomerConfirm.click()");
                curr_telphone_customer_id = curr_customer_id;
                CustomerViewStatusShow('#TelphoneRecord', function () {
                    log.debug("Callback of CustomerViewStatusShow() in CustomerModelAdd()");
                    if (curr_telphone_record !== null) {
                        curr_telphone_record.CustomerID = curr_customer_id;
                        $("#TelphoneRecord div.RecordInfo input.RecordSave").click();
                    }
                });
                
            });
        } else {
            UploadCustomerInfo('#TelphoneRecord', function () {
                log.debug("Callback of UploadCustomerInfo() in CustomerConfirm.click()");
                curr_telphone_customer_id = curr_customer_id;
                CustomerViewStatusShow('#TelphoneRecord', function () {
                    log.debug("Callback of CustomerViewStatusShow() in UploadCustomerInfo()");
                    if (curr_telphone_record !== null) {
                        curr_telphone_record.CustomerID = curr_customer_id;
                        $("#TelphoneRecord div.RecordInfo input.RecordSave").click();
                    }
                });
            });
        }
    });

    $('#TelphoneRecord input#TelphoneRecordSearchButton').click(function () {
        CustomerSearch('#TelphoneRecord', function () {
            log.debug("Callback of CustomerSearch() in TelphoneRecordSearchButton.click()");
            curr_telphone_customer_id = curr_customer_id;
            CustomerViewStatusShow('#TelphoneRecord', function () {
                log.debug("Callback of CustomerViewStatusShow() in CustomerViewStatusShow()");
                if (curr_telphone_record !== null) {
                    curr_telphone_record.CustomerID = curr_customer_id;
                    $("#TelphoneRecord div.RecordInfo input.RecordSave").click();
                }
            });

        });
    });

    $("#TelphoneRecord div.RecordInfo input.RecordSave").click(function () {
        curr_telphone_record.Comment = $("#TelphoneRecord div.RecordInfo textarea").val();
        UploadTelphoneRecordInfo();
        SetTelphoneButtonEnable(false, !$('#TelphoneRecord div.RecordInfo input.RecordFinish').attr("disabled"));
    });

    $("#TelphoneRecord div.RecordInfo input.RecordFinish").click(function () {
        curr_telphone_record.Comment = $("#TelphoneRecord div.RecordInfo textarea").val();
        if (curr_telphone_record.ServiceItemList == null || curr_telphone_record.ServiceItemList.length < 1)/* && (curr_telphone_record.ServiceGroupID === null ||
            curr_telphone_record.ServiceItemID === null))*/ {
            alert("請選擇小結");
        }
        else {
            curr_telphone_record.StatusID = 3;  //已結案，注意需與資料庫內容一致
            curr_telphone_record.FinishDT = (new Date()).toISOString();
            UploadTelphoneRecordInfo(function () { $("#Tabs").tabs("option", "active", 0); });
            SetTelphoneButtonEnable(false, false);   
        }
    });

    $("#TelphoneRecord textarea.RecordComment").change(function () {
        SetTelphoneButtonEnable(true, !$('#TelphoneRecord div.RecordInfo input.RecordFinish').attr("disabled"));
    });
});

function SetTelphoneRecordTab() {
    curr_customer_id = curr_telphone_customer_id;
    
    if (curr_telphone_record_id !== null) {
        GetTelphoneRecordInfo(function () {
            SetCustomerBlock('#TelphoneRecord');
        });
    } else {
        // If no record which should be show, Lock all customer buttons.
        SetCustomerButtonEnable("#TelphoneRecord", false, false, false, false);
    }
}

function GetTelphoneRecordInfo(before_send) {
    var url = doc_root + "Record/JsonInfo/" + curr_telphone_record_id;
    log.debug('Get Telphone record info(' + url + ')');
    $.ajax({
        url : url,
        dataType: DATA_JSON,
        success: function (record, status, jqXHR) {
            log.debug("Get Telphone Record Info, return: " + var_dump(record));
            UpdateTelphoneRecordModel(record);
            curr_telphone_record_id = record['RID'];
            ShowTelphoneRecordInfo('#TelphoneRecord', record);
            if (before_send !== undefined) {
                before_send();
            }
        },
        error: function (jqXHR, status, msg) {
            alert('Network error when ajax to ' + url + ', ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
        }}
    )
}

function UpdateTelphoneRecordModel(record) {
    curr_telphone_record = record;
}

function UploadTelphoneRecordInfo(before_upload) {   
    var url = doc_root + "Record/AjaxUpdate/";
    var data_obj = {
        records: GenerateULTelphoneRecord(),
        serviceItemList: curr_telphone_record.ServiceItemList,
    }
    log.debug('Upload telphone record info(' + url + ') , data: ' + var_dump(data_obj));
    $.ajax(
        {
            type: HTTP_POST,
            url: url,
            data: JSON.stringify(data_obj),
            dataType: DATA_JSON,
            contentType: MINE_JSON,
            success: function (response, status, jqXHR) {
                log.debug("Upload Telphone Record Info, return: " + var_dump(response));
                if (response['result'] != 0) {
                    log.error('Upload Telphone Record Info error:' + var_dump(response));
                    alert(var_dump(data_obj) + var_dump(response));
                };  
                if (before_upload !== undefined) {
                    before_upload();
                }
            },
            error: function (jqXHR, status, msg) {
                alert('Error in upload Telphone Record: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
            }
        }
    )
}

function GenerateULTelphoneRecord() {
    return {
        RID: curr_telphone_record.RID,
        CustomerID: curr_telphone_record.CustomerID,
        TypeID: curr_telphone_record.TypeID,
        AgentID: g_agent_id,
        StatusID: curr_telphone_record.StatusID,
        Comment: curr_telphone_record.Comment,
        ServiceGroupID: curr_telphone_record.ServiceGroupID,
        ServiceItemID: curr_telphone_record.ServiceItemID,
        IncomingDT: curr_telphone_record.IncomingDT,
        FinishDT: curr_telphone_record.FinishDT,
        GroupID: curr_telphone_record.GroupID,  
        CloseApproachID: curr_email_record.CloseApproachID,
    };
}

function SetTelphoneButtonEnable(save_btn, finish_btn) {
    $('#TelphoneRecord div.RecordInfo input.RecordSave').attr("disabled", !save_btn);
    $('#TelphoneRecord div.RecordInfo input.RecordFinish').attr("disabled", !finish_btn);
}

function ShowTelphoneRecordInfo(parent_id, record) {
    $(parent_id + " div.RecordInfo label.RecordID").empty('').html(record['RID']);
    $(parent_id + " div.RecordInfo label.RecordType").empty('').html(record['TypeName']);
    $(parent_id + " div.RecordInfo label.RecordStatus").empty('').html(record['StatusName']);
    $(parent_id + " div.RecordInfo label.RecordPhoneNum").empty('').html(record['PhoneNum']);
    $(parent_id + " div.RecordInfo label.RecordIncomingDT").empty('').html(ISODateStringToString(record['IncomingDT']));
    $(parent_id + " div.RecordInfo textarea").empty().html(record['Comment']);
    $(parent_id + " div.RecordInfo label.RecordServiceItem").empty().html(record['ServiceItemName']);

    if (record['StatusID'] === 3) {
        SetTelphoneButtonEnable(false, false);
        $("input.RecordServiceItemPicker").attr("disabled", true);
    } else {
        $("input.RecordServiceItemPicker").attr("disabled", false);
        SetTelphoneButtonEnable(false, true);
    }

    $(parent_id + " div.RecordInfo select.RecordCloseApproach").empty();
    $(parent_id + " div.RecordInfo select.RecordCloseApproach").append($('<option></option>').val("").text("--請選擇--"));
    $.each(record['CloseApproachList'], function (i, item) {
        if (curr_telphone_record.CloseApproachID != null && item.ApproachID == curr_email_record.CloseApproachID) {
            $(parent_id + " div.RecordInfo select.RecordCloseApproach").append($('<option selected></option>').val(item.ApproachID).text(item.ApproachName));
        } else {
            $(parent_id + " div.RecordInfo select.RecordCloseApproach").append($('<option></option>').val(item.ApproachID).text(item.ApproachName));
        }});
}

function GetRecordByCustomer(parent_id, customer_id, limited, before_send) {
    var url = doc_root + "Record/AjaxInfoList/" + customer_id + '/' + limited;
    if (customer_id === null) {
        ShowRecordDetailList(parent_id, null);
        if (before_send) {
            before_send();
        }
        return;
    }
    log.debug('Get Record by Customer(' + url + ')');
    $.ajax({
        url: url,
        dataType: DATA_JSON,
        success: function (record_list, status, jqXHR) {
            log.debug('Get Record by Customer, return:' + var_dump(record_list));
            ShowRecordDetailList(parent_id, record_list)
            if (before_send != undefined) {
                before_send();
            }
        },
        error: function (jqXHR, status, msg) {
            alert('Error' + jqXHR);
        }
    })
}
