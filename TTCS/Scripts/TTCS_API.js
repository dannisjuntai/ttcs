function TelphoneIncoming(IOType, TelphoneNumber, IndexID, QueueName, CallInfo, GroupID) {
    log.info("Telphone incoming, paramters: " + var_dump(arguments));
    upload_telphone_info(function () {
        upload_email_info(function () {
            var data_obj = { qs: IndexID };
            log.debug("Search Exist PhoneRecord, data: " + var_dump(data_obj));
            $.ajax({
                url: doc_root + "PhoneRecord/AjaxQueryByIndex/",
                type: 'GET',
                dataType: 'json',
                data : data_obj,
                success: function (query_ret, status, jqXHR) {
                    log.debug("Search exist PhoneRecord, return:" + var_dump(query_ret));
                    if (query_ret['result'] < 0) {
                        alert('result:' + query_ret['result'] + '\n' + 'msg:' + query_ret['msg']);
                        log.warn('Search PR failure:' + var_dump(query_ret));
                        return;
                    }

                    if (query_ret['RID']) {
                        curr_telphone_record_id = query_ret['RID'] || null;
                        curr_telphone_customer_id = query_ret['CustomerID'] || null;
                        curr_customer_id = curr_telphone_customer_id;
                        if ($("#Tabs").tabs("option", "active") == 1) {
                            SetTelphoneRecordTab();
                        } else {
                            $("#Tabs").tabs("option", "active", 1);
                        }
                        log.warn('Search existed PR, ret: ' + var_dump(query_ret));
                        return;
                        /*************************/
                        /*  Recursive Endpoint1  */
                        /*************************/
                    }

                    // If no exist IndexID, do search customer and create Record
                    var incoming_search_obj ={
                        number: TelphoneNumber,
                        agent_groups: g_agent_group_list
                    };
                    log.debug("Search exist customers, data: " + var_dump(incoming_search_obj));
                    $.ajax({
                        url: doc_root + "Customer/AjaxIncomingSearch/",
                        type: 'POST',
                        data: JSON.stringify(incoming_search_obj),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (search_ret, status, jqXHR) {
                            log.debug('Search exist customers, return: '+ var_dump(search_ret))
                            if (search_ret['result'] !== 0) {
                                alert('result:' + search_ret['result'] + '\n' + 'msg:' + search_ret['err'] + '\n obj:' + var_dump(incoming_search_obj));
                                log.warn('Search Customer failure: ' + var_dump(search_ret));
                                return;
                            }

                            var customers = search_ret['ret'];
                            if (customers.length === 0) {
                                curr_telphone_customer_id = null;
                                curr_customer_id = null;
                            }
                            else if (customers.length === 1) {
                                var customer = search_ret['ret'][0];
                                curr_telphone_customer_id = (customer !== "" && customer["ID"] !== "undefined") ? customer["ID"] : null;
                                curr_customer_id = curr_telphone_customer_id;
                            }
                            else {
                                ShowCustomerSearchResult("#TelphoneRecord", customers,
                                    function () {
                                        log.info("Before_select Callback of ShowCustomerSearchResult() in API");
                                        curr_telphone_customer_id = curr_customer_id;
                                        CustomerViewStatusShow('#TelphoneRecord', function () {
                                            log.info("Callback of CustomerViewStatusShow() in TelphoneIncoming()");
                                            if (curr_telphone_record !== null) {
                                                curr_telphone_record.CustomerID = curr_customer_id;
                                                $("#TelphoneRecord div.RecordInfo input.RecordSave").click();
                                            }
                                        });
                                    },
                                    function () {
                                        log.debug("Before_close Callback of ShowCustomerSearchResult() in API");
                                        curr_telphone_customer_id = null;
                                        curr_customer_id = null;
                                        curr_telphone_record['CustomerID'] = null;
                                        CustomerViewStatusShow("#TelphoneRecord", function () {
                                            log.debug("Callback of CustomerViewStatusShow in #CustomerResultList tr.click()");
                                            GetRecordByCustomer("#TelphoneRecord", null, 10, function () {
                                                $("#TelphoneRecord div.RecordInfo input.RecordSave").click();
                                            });
                                        });
                                    }
                                );
                            }
                            
                            var new_record = {
                                RID: "",
                                CustomerID: curr_customer_id,
                                TypeID: IOType,
                                AgentID: g_agent_id,
                                StatusID: 2,
                                Comment: "",
                                ServiceGroupID: "",
                                ServiceItemID: "",
                                GroupID: GroupID,
                                IncomingDT: JsDateToString(new Date),
                                FinishDT: "",
                            };
                            log.debug("Create New Record(Record/AjaxCreate/), data: " + var_dump(new_record));
                            // 2. Create record with/without customer
                            $.ajax({
                                url: doc_root + "Record/AjaxCreate/",
                                type: 'POST',
                                data: new_record,
                                dataType: 'json',
                                success: function (ret_record, status, jqXHR) {
                                    log.debug("Create New Record, return: " + var_dump(ret_record));
                                    if (ret_record['result'] === 0) {
                                        // 3. Create PhoneRecord
                                        phone_record_data = {
                                            RID: ret_record['RID'],
                                            PhoneNum: TelphoneNumber || "",
                                            QueueName: QueueName || "",
                                            IndexID: IndexID || "",
                                        };
                                        log.debug("Create new phone record, send: " + var_dump(phone_record_data));
                                        $.ajax({
                                            url: doc_root + "PhoneRecord/AjaxCreate/",
                                            type: 'POST',
                                            data: phone_record_data,
                                            dataType: 'json',
                                            success: function (ret_phoneRecord, status, jqXHR) {
                                                // 3. set current_telphonerecord
                                                //curr_customer_id = records[k]['CustomerID'];
                                                //alert('phone result: ' + ret_phoneRecord['result']);
                                                //alert('phone info: ' + ret_phoneRecord['info']);
                                                log.debug('Create phone record, ret: ' + var_dump(ret_phoneRecord));
                                                if (ret_phoneRecord['result'] === 0) {
                                                    curr_telphone_record_id = ret_record['RID'];
                                                    if ($("#Tabs").tabs("option", "active") == 1) {
                                                        SetTelphoneRecordTab();
                                                    } else {
                                                        $("#Tabs").tabs("option", "active", 1);
                                                    }
                                                } else {
                                                    alert('Create phone record error: ' + var_dump(ret_phoneRecord));
                                                }
                                                /*************************/
                                                /*  Recursive Endpoint2  */
                                                /*************************/
                                            },
                                            error: function (jqXHR, status, msg) {
                                                //TODO  Need to handle this exception with delete record which is related
                                                alert('Error in Create PhoneRecord data: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                                                log.warn('Error in Create PhoneRecord data: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                                            }
                                        });
                                    } else {
                                        alert('Create record error: ' + var_dump(ret_record));
                                        log.debug('Create record error: ' + var_dump(ret_record));
                                    }
                                },
                                error: function (jqXHR, status, msg) {
                                    alert('Error in Create Record data: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                                    log.debug('Error in Create Record data: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                                }
                            });
                        },
                        error: function (jqXHR, status, msg) {
                            alert('Error in fetch Customer by Telphone number: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                            log.warn('Error in fetch Customer by Telphone number: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                        }
                    });
                },
                error: function (jqXHR, status, msg) {
                    alert('Error in fetch phone record by Index: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                    log.warn('Error in fetch phone record by Index: ' + jqXHR.state() + '(' + jqXHR.statusText + ')');
                }
            })
        });
    });
};


var upload_telphone_info = function (before_upload) {
    if (curr_telphone_record_id !== null) {
        if (curr_telphone_customer_id !== null) {
            curr_customer_id = curr_telphone_customer_id;
            CustomerModelUpdateFromView("#TelphoneRecord");
            UploadCustomerInfo("#TelphoneRecord", function () {
                //Modify record data
                log.debug("Callback Upload Customer Info in upload_telphone_info()");
                if (curr_telphone_record.StatusID == 1) {
                    curr_telphone_record.StatusID = 2;
                }
                curr_telphone_record.Comment = $("#TelphoneRecord div.RecordInfo textarea").val();
                UploadTelphoneRecordInfo(before_upload);
                SetTelphoneButtonEnable(false, !$('#TelphoneRecord div.RecordInfo input.RecordFinish').attr("disabled"));
            }, true);
        } else {
            if (before_upload !== undefined) {
                before_upload();
            }
        }
    } else {
        if (before_upload !== undefined) {
            before_upload();
        }
    }
};


// if email record is editing, save email record
var upload_email_info = function (before_upload) {
    if (curr_email_record_id !== null) {
        if (curr_email_customer_id !== null) {
            curr_customer_id = curr_email_customer_id;
            CustomerModelUpdateFromView("#EmailRecord");
            UploadCustomerInfo("#EmailRecord", function () {
                log.debug("Callback of UploadCustomerInfo() in upload_email_info()");
                SaveForEmailRecord(false, before_upload);
                InitEmailRecordAllInputs();
            }, true);
        } else {
            if (before_upload !== undefined) {
                before_upload();
            }
        }
    } else {
        if (before_upload !== undefined) {
            before_upload();
        }
    }
}