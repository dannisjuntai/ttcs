/***************************************** 
* Customer function section
*****************************************/
var customer_edit_flag = false;
var curr_customer_id = null;
var curr_customer = {
    ID: "",
    CName: "",
    EName: "",
    Category: "",
    ID2: "",
    Region: "",
    City: "",
    Address: "",
    GroupID: "",
    Comment: "",
    Marketer: "",
    AgentID1: "",
    AgentID2: "",
    Contact1_Name: "",
    Contact1_Title: "",
    Contact1_Tel1: "",
    Contact1_Tel2: "",
    Contact1_Email1: "",
    Contact1_Email2: "",
    Contact1_DoSend: "",
    Contact2_Name: "",
    Contact2_Title: "",
    Contact2_Tel1: "",
    Contact2_Tel2: "",
    Contact2_Email1: "",
    Contact2_Email2: "",
    Contact2_DoSend: "",
    Contact3_Name: "",
    Contact3_Title: "",
    Contact3_Tel1: "",
    Contact3_Tel2: "",
    Contact3_Email1: "",
    Contact3_Email2: "",
    Contact3_DoSend: "",

    CategoryName: "",
};


function SetGroupOption(parent_id, groups) {
    var s = $(parent_id + ' .Customer_Group').empty();
    for (var idx in groups) {
        s.append($('<option>').attr('value', groups[idx].GroupID).html(groups[idx].GroupName)).addClass('');
    }
    s.val(groups[0].GroupID).attr("disabled", true);
}

function SetCustomerBlock(parent_id) {
    if (curr_customer_id === null) {
        CustomerViewStatusEmpty(parent_id);
    }
    else {
        CustomerViewStatusShow(parent_id);
    }
}

function CustomerViewStatusEmpty(parent_id) {
    CustomerViewClearData(parent_id);
    ShowRecordDetailList(parent_id, null)
    CustomerViewEnableEdit(parent_id, false);
    SetCustomerButtonEnable(parent_id, true, false, false, true);
}

function CustomerViewStatusShow(parent_id, before_send) {
    GetCustomerInfo(parent_id, before_send);
    CustomerViewEnableEdit(parent_id, false);
    SetCustomerButtonEnable(parent_id, true, true, false, true);
}

function CustomerViewStatusAdd(parent_id) {
    curr_customer_id = null;
    curr_telphone_customer_id = null;
    CustomerViewClearData(parent_id);
    CustomerViewEnableEdit(parent_id, true);
    SetCustomerButtonEnable(parent_id, false, false, true, false);
}

function CustomerViewStatusEdit(parent_id) {
    CustomerViewEnableEdit(parent_id, true);
    SetCustomerButtonEnable(parent_id, false, false, true, false);
}

function CustomerSearch(parent_id, before_send) {
    // Modify by Kevin 20140211
    //var search_text = $('#TelphoneRecordSearchText').val();
    var url = doc_root + "Customer/AjaxSearch/";
    var search_text = $(parent_id + ' .CustomerSearchText').val();
    if (search_text !== "") {
        var data_obj = {
            search_text: search_text,
            search_type: $(parent_id + ' input[name=search_type]:checked').val(),
            agent_groups: g_agent_group_list,
        }
        log.debug('Search Customer info(' + url + '), data:' + var_dump(data_obj));
        $.ajax({
            type: HTTP_POST,
            data: JSON.stringify(data_obj),
            url: url,
            contentType: MINE_JSON,
            success: function (customers, status, jqXHR) {
                log.debug('Search Customer info, return:' + var_dump(customers));
                if (Object.keys(customers).length === 0) {
                    alert("No Search Result");
                    log.info("Search Customer No Result: " + var_dump(customers));
                }
                else if (Object.keys(customers).length === 1) {
                    CustomerViewEnableEdit(parent_id, false);
                    UpdateCustomerModel(customers[0]);
                    //CustomerViewShowInfo(parent_id, customers[0]);
			            
                    CustomerViewStatusShow(parent_id, function () {
                        log.debug("Callback of CustomerViewStatusShow() in CustomerSearch()");
                        GetRecordByCustomer(parent_id, customers[0]['ID'], 10, before_send);
                    });
			            
                }
                else {
                    ShowCustomerSearchResult(parent_id, customers, before_send);
                }
            },
            error: function (jqXHR, statud, msg) {
                alert('Error in fetch Customer by Telphone number: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                log.error('Error in fetch Customer by Telphone number: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
            }
        })
    }
}

function ShowCustomerSearchResult(parent_id, customers, before_select, before_close) {
    log.debug("ShowCustomerSearchResult()");
    $("#CusomterSearchResult").dialog({
        closeOnEscape: true,
        modal: true,
        resizable: false,
        width: 800,
        close: function (event, ui) {
            if (before_close) {
                before_close();
            }
        }
    });

    $("#CustomerResultList").empty().append(
        $('<tr/>')
            .append($('<th/>').html("編號"))
            .append($('<th/>').html("中文"))
            .append($('<th/>').html("英文"))
            .append($('<th/>').html("等級"))
            .append($('<th/>').html("區域"))
            .append($('<th/>').html("城市"))
            .append($('<th/>').html("住址"))
            .append($('<th/>', { style: "visibility: hidden" }).html()));

    for (var key in customers) {
        $("#CustomerResultList").append(
            $('<tr/>')
                .append($('<td/>').html(customers[key]['ID2']))
                .append($('<td/>').html(customers[key]['CName']))
                .append($('<td/>').html(customers[key]['EName']))
                .append($('<td/>').html(customers[key]['CategoryName']))
                .append($('<td/>').html(customers[key]['Region']))
                .append($('<td/>').html(customers[key]['City']))
                .append($('<td/>').html(customers[key]['Address']))
                .append($('<td/>', { style: "visibility: hidden" }).html(key))
                .click(function () {
                    $("#CusomterSearchResult").dialog("destroy");
                    var key = $("td:last", this).html();
                    var my_customer = customers[key];


                    if ($("#Tabs").tabs("option", "active") === 1) {
                        curr_telphone_customer_id = null;
                        if ($('#TelphoneRecordRID').html() === "") {
                            log.warn("Record is not ready");
                            return;
                        }
                        curr_customer_id = my_customer["ID"];
                        curr_telphone_customer_id = curr_customer_id;
                        curr_telphone_record['CustomerID'] = curr_telphone_customer_id;
                    }
                    else {
                        curr_email_customer_id = null;
                        if ($('#EmailRecordRID').html() === "") {
                            log.warn("Record is not ready");
                            return;
                        }
                        curr_customer_id = my_customer["ID"];
                        curr_email_customer_id = curr_customer_id;
                        curr_email_record['CustomerID'] = curr_telphone_customer_id;
                    }

                    CustomerViewEnableEdit(parent_id, false);
                    UpdateCustomerModel(my_customer);
                    //CustomerViewShowInfo(parent_id, my_customer);  //unneccessary
                    CustomerViewStatusShow(parent_id, function () {
                        log.debug("Callback of CustomerViewStatusShow in #CustomerResultList tr.click()");
                        GetRecordByCustomer(parent_id, my_customer['ID'], 10, before_select);
                    });
                    
                    $("#CusomterSearchResult").hide();
                })
        )
    }
}

function SetCustomerButtonEnable(parent_id, add_btn, edit_btn, confirm_btn, search_bar) {
    log.debug("In SetCustomerButtonEnable()");
    $(parent_id + ' div.CustomerInfo input.CustomerAdd').attr("disabled", !add_btn);
    $(parent_id + ' div.CustomerInfo input.CustomerEdit').attr("disabled", !edit_btn);
    $(parent_id + ' div.CustomerInfo input.CustomerConfirm').attr("disabled", !confirm_btn);
    $(parent_id + ' div.CustomerInfo div.CustomerSearchBar input').attr("disabled", !search_bar);
}

function GetCustomerInfo(parent_id, before_send) {
    if (curr_customer_id === null) {
        CustomerModelClear();
        $('#TelphoneRecord input:gt(8):lt(28)').val("");
        if (before_send) {
            before_send();
        }
        return;
    }
    var url = doc_root + 'Customer/JsonInfo/' + curr_customer_id;
    log.debug('Get Customer info(' + url + ')');
    $.ajax({
        url: url,
        success: function (customer, status, jqXHR) {
            log.debug('Get Customer info, return:' + var_dump(customer));
            CustomerViewEnableEdit(parent_id, false);
            UpdateCustomerModel(customer);
            CustomerViewShowInfo(parent_id, customer);
            GetRecordByCustomer(parent_id, customer['ID'], 10, before_send);
        },
        error: function (jqXHR, status, msg) {
            alert('Error in GetCustomerInfo: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
            log.error('Error in GetCustomerInfo: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
        }
    })
}

function UpdateCustomerModel(customer) {
    curr_customer = customer;
    curr_customer_id = customer['ID'];
}

function CustomerModelAdd(parent_id, before_add) {
    log.debug('Send to Customer/AjaxNewGuid/');
    var url = doc_root + "Customer/AjaxNewGuid/";
    log.debug('Get new GUID for Customer(' + url + ')');
    $.ajax(
        {
            type: HTTP_GET,
            url: url,
            dataType: DATA_JSON,
            success: function (ret, status, jqXHR) {
                log.debug('Get new GUID for Customer, return: ' + var_dump(ret));
                if (ret['result'] != 0) {
                    log.error("Get new GUID for Customer error, message: " + ret.message);
                    alert("Get new GUID for Customer error, message: " + ret.message);
                    return;
                }
                curr_customer.ID = ret.guid;
                curr_customer_id = ret.guid;
                CreateCustomerInfo(parent_id, before_add);
            },
            error: function (jqXHR, status, msg) {
                alert('Error in CustomerModelAdd: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
                log.debug('Error in CustomerModelAdd: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
            }
        }
    )
}


function CreateCustomerInfo(parent_id, before_send) {
    var url = doc_root + "Customer/AjaxCreate/";
    var data_obj = {
        customer: GenerateULCustomer(),
        agent_groups: g_agent_group_list
    };
    log.debug('Create Customer info(' + url + '), data:' + var_dump(data_obj));
    $.ajax(
        {
            type: HTTP_POST,
            url: url,
            data: JSON.stringify(data_obj),
            contentType: MINE_JSON,
            dataType: DATA_JSON,
            success: function (response, status, jqXHR) {
                log.debug('Create Customer info, return:' + var_dump(response));
                if (response['result'] == -2) {
                    alert("客戶電話或Email已經存在!");
                    log.error('Telphone or Email is existed: ' + response["msg"]);
                    curr_customer_id = null;
                    curr_customer.ID = null;
                    return;
                }
                else if (response['result'] != 0) {
                    alert('Create CustomerInfo failure: ' + response["msg"]);
                    log.error('Create CustomerInfo failure: ' + response["msg"]);
                    curr_customer_id = null;
                    curr_customer.ID = null;
                    return;
                }
                before_send();
            },
            error: function (jqXHR, status, msg) {
                alert('Error in CreateCustomerInfo: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
                log.error('Error in CreateCustomerInfo: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
            }
        }
    )
}

function UploadCustomerInfo(parent_id, before_send, ignore_fail) {
    ignore_fail = ignore_fail || false;
    var url = doc_root + "Customer/AjaxUpdate/";
    var data_obj = {
        customer: GenerateULCustomer(),
        agent_groups: g_agent_group_list
    };
    log.debug('Upload Customer info' + url + ', data:' + var_dump(data_obj));
    $.ajax(
        {
            type: HTTP_POST,
            url: url,
            data: JSON.stringify(data_obj),
            contentType: MINE_JSON,
            dataType: DATA_JSON,
            success: function (response, status, jqXHR) {
                log.debug('Upload Customer info, return:' + var_dump(response));
                if (response['result'] == -2) {
                    alert('客戶電話或郵件重複: ' + response['msg']);
                    log.error('Telphone or Email is existed: ' + response["msg"]);
                    if (!ignore_fail) {
                        return;
                    }
                }
                else if (response['result'] != 0) {
                    alert('Update Customer failure: ' + response['msg']);
                    log.error('Update Customer failure: ' + response["msg"]);
                    if (!ignore_fail) {
                        return;
                    }
                }
                if (before_send !== undefined) {
                    before_send();
                }
            },
            error: function (jqXHR, status, msg) {
                alert('Error in UploadCustomerInfo: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
                log.info('Error in UploadCustomerInfo: ' + jqXHR.statusText + '(' + jqXHR.state() + ')');
            }
        }
    )
}

function CustomerViewClearData(parent_id) {
    $(parent_id + ' div.CustomerInfo span.data_pair input:not(".CustomerSearchBar input")').val("");
}

function CustomerViewEnableEdit(parent_id, enable) {
    customer_edit_flag = enable;
    //$(parent_id + ' div.CustomerInfo span.data_pair input:not(".CustomerSearchBar input")').attr('disabled', !enable);
    $(parent_id + ' div.CustomerInfo span.data_pair .CustomerFiled').attr('disabled', !enable);
    if (g_agent_group.length == 1) {
        $(parent_id + ' .Customer_Group').attr('disabled', true);
    }
}


function GenerateULCustomer() {
    return {
        ID: curr_customer.ID,
        CName: curr_customer.CName,
        EName: curr_customer.EName,
        Category: curr_customer.Category,
        ID2: curr_customer.ID2,
        Region: curr_customer.Region,
        City: curr_customer.City,
        Address: curr_customer.Address,
        GroupID: curr_customer.GroupID,
        Marketer: curr_customer.Marketer,
        AgentID1: curr_customer.AgentID1,
        AgentID2: curr_customer.AgentID2,
        Contact1_Name: curr_customer.Contact1_Name,
        Contact1_Title: curr_customer.Contact1_Title,
        Contact1_Tel1: curr_customer.Contact1_Tel1,
        Contact1_Tel2: curr_customer.Contact1_Tel2,
        Contact1_Email1: curr_customer.Contact1_Email1,
        Contact1_Email2: curr_customer.Contact1_Email2,
        Contact2_Name: curr_customer.Contact2_Name,
        Contact2_Title: curr_customer.Contact2_Title,
        Contact2_Tel1: curr_customer.Contact2_Tel1,
        Contact2_Tel2: curr_customer.Contact2_Tel2,
        Contact2_Email1: curr_customer.Contact2_Email1,
        Contact2_Email2: curr_customer.Contact2_Email2,
        Contact3_Name: curr_customer.Contact3_Name,
        Contact3_Title: curr_customer.Contact3_Title,
        Contact3_Tel1: curr_customer.Contact3_Tel1,
        Contact3_Tel2: curr_customer.Contact3_Tel2,
        Contact3_Email1: curr_customer.Contact3_Email1,
        Contact3_Email2: curr_customer.Contact3_Email2,
        Modifier: g_agent_login,
    };
}

function CustomerViewShowInfo(parent_id, customer) {
    $(parent_id + ' div.CustomerInfo input.Customer_CName').val(customer['CName']);
    $(parent_id + ' div.CustomerInfo input.Customer_EName').val(customer['EName']);
    $(parent_id + ' div.CustomerInfo .Customer_Category').val(customer['Category']);
    $(parent_id + ' div.CustomerInfo input.Customer_ID2').val(customer['ID2']);
    $(parent_id + ' div.CustomerInfo input.Customer_Region').val(customer['Region']);
    $(parent_id + ' div.CustomerInfo input.Customer_City').val(customer['City']);
    $(parent_id + ' div.CustomerInfo input.Customer_Address').val(customer['Address']);
    $(parent_id + ' div.CustomerInfo .Customer_Group').val(customer['GroupID']);
    $(parent_id + ' div.CustomerInfo input.Customer_Marketer').val(customer['Marketer']);
    $(parent_id + ' div.CustomerInfo input.Customer_AgentID1').val(customer['AgentID1']);
    $(parent_id + ' div.CustomerInfo input.Customer_AgentID2').val(customer['AgentID2']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Name').val(customer['Contact1_Name']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Title').val(customer['Contact1_Title']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Tel1').val(customer['Contact1_Tel1']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Tel2').val(customer['Contact1_Tel2']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Email1').val(customer['Contact1_Email1']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Email2').val(customer['Contact1_Email2']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Name').val(customer['Contact2_Name']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Title').val(customer['Contact2_Title']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Tel1').val(customer['Contact2_Tel1']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Tel2').val(customer['Contact2_Tel2']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Email1').val(customer['Contact2_Email1']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Email2').val(customer['Contact2_Email2']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Name').val(customer['Contact3_Name']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Title').val(customer['Contact3_Title']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Tel1').val(customer['Contact3_Tel1']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Tel2').val(customer['Contact3_Tel2']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Email1').val(customer['Contact3_Email1']);
    $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Email2').val(customer['Contact3_Email2']);
}

function CustomerModelUpdateFromView(parent_id) {
    curr_customer['ID'] = curr_customer_id;
    curr_customer['CName'] = $(parent_id + ' div.CustomerInfo input.Customer_CName').val();
    curr_customer['EName'] = $(parent_id + ' div.CustomerInfo input.Customer_EName').val();
    curr_customer['Category'] = $(parent_id + ' div.CustomerInfo .Customer_Category').val();
    curr_customer['ID2'] = $(parent_id + ' div.CustomerInfo input.Customer_ID2').val();
    curr_customer['Region'] = $(parent_id + ' div.CustomerInfo input.Customer_Region').val();
    curr_customer['City'] = $(parent_id + ' div.CustomerInfo input.Customer_City').val();
    curr_customer['Address'] = $(parent_id + ' div.CustomerInfo input.Customer_Address').val();
    curr_customer['GroupID'] = $(parent_id + ' div.CustomerInfo .Customer_Group').val();
    curr_customer['Marketer'] = $(parent_id + ' div.CustomerInfo input.Customer_Marketer').val();
    curr_customer['AgentID1'] = $(parent_id + ' div.CustomerInfo input.Customer_AgentID1').val();
    curr_customer['AgentID2'] = $(parent_id + ' div.CustomerInfo input.Customer_AgentID2').val();
    curr_customer['Contact1_Name'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Name').val();
    curr_customer['Contact1_Title'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Title').val();
    curr_customer['Contact1_Tel1'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Tel1').val();
    curr_customer['Contact1_Tel2'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Tel2').val();
    curr_customer['Contact1_Email1'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Email1').val();
    curr_customer['Contact1_Email2'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact1_Email2').val();
    curr_customer['Contact2_Name'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Name').val();
    curr_customer['Contact2_Title'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Title').val();
    curr_customer['Contact2_Tel1'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Tel1').val();
    curr_customer['Contact2_Tel2'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Tel2').val();
    curr_customer['Contact2_Email1'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Email1').val();
    curr_customer['Contact2_Email2'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact2_Email2').val();
    curr_customer['Contact3_Name'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Name').val();
    curr_customer['Contact3_Title'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Title').val();
    curr_customer['Contact3_Tel1'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Tel1').val();
    curr_customer['Contact3_Tel2'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Tel2').val();
    curr_customer['Contact3_Email1'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Email1').val();
    curr_customer['Contact3_Email2'] = $(parent_id + ' div.CustomerInfo input.Customer_Contact3_Email2').val();
}

function CustomerModelClear() {
    curr_customer['ID'] = null;
    curr_customer['CName'] = null;
    curr_customer['EName'] = null;
    curr_customer['Category'] = null;
    curr_customer['ID2'] = null;
    curr_customer['Region'] = null;
    curr_customer['City'] = null;
    curr_customer['Address'] = null;
    curr_customer['GroupID'] = null;
    curr_customer['Marketer'] = null;
    curr_customer['AgentID1'] = null;
    curr_customer['AgentID2'] = null;
    curr_customer['Contact1_Name'] = null;
    curr_customer['Contact1_Title'] = null;
    curr_customer['Contact1_Tel1'] = null;
    curr_customer['Contact1_Tel2'] = null;
    curr_customer['Contact1_Email1'] = null;
    curr_customer['Contact1_Email2'] = null;
    curr_customer['Contact2_Name'] = null;
    curr_customer['Contact2_Title'] = null;
    curr_customer['Contact2_Tel1'] = null;
    curr_customer['Contact2_Tel2'] = null;
    curr_customer['Contact2_Email1'] = null;
    curr_customer['Contact2_Email2'] = null;
    curr_customer['Contact3_Name'] = null;
    curr_customer['Contact3_Title'] = null;
    curr_customer['Contact3_Tel1'] = null;
    curr_customer['Contact3_Tel2'] = null;
    curr_customer['Contact3_Email1'] = null;
    curr_customer['Contact3_Email2'] = null;
}