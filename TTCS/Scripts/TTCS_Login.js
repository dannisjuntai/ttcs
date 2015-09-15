/***************************************** 
* Login/logout function Section
*****************************************/
var g_agent_id;
var g_agent_login;
var g_agent_group = [];
var g_agent_group_list = [];
var g_ext_no;

function login() {
    // Login id is CTILoginID, i.e. 'james'
    var agent_id = $("#AgentId").val();
    var ext_no = $("#ExtNo").val();
    feed_login_info(agent_id, ext_no);
    GetRecordList(g_agent_id, 1);
}

function logout() {
    g_agent_id = null;
    g_ext_no = null;
    $.removeCookie('agent_id');
    $.removeCookie('ext_no');
    $("#Tabs").hide();
    $("#Unlogin").show();
}

function InitServiceItemPicker(agent_id) {
    $("#ServiceItemPickerTree").dynatree({
        title: "Dynatree", // Tree's name (only used for debug outpu)
        checkbox: true,
        selectMode: 2,
        minExpandLevel: 1, // 1: root node is not collapsible
        initAjax: {
            url: doc_root + "ServiceItem/JsonServiceTree/" + agent_id,
        },
    });
}

function feed_login_info(agent_login, ext_no) {
    //agent_login is CTILoginID
    var url = doc_root + "Agent/GetAgentIDByCTI/" + agent_login; 
    if (agent_login !== null && ext_no !== null) {
        log.debug('Get AgentID by CTI info(' + url + ')');
        $.ajax({
            url: url,
            dataType: DATA_JSON,
            success: function (data, status, jqXHR) {
                log.debug('Get AgentID by CTI, return: ' + var_dump(data));
                g_agent_id = data.agent_id;
                g_agent_group = data.agent_group;
                g_agent_login = agent_login;
                g_ext_no = ext_no;

                g_agent_group_list = [];
                data.agent_group.forEach(function (g) { g_agent_group_list.push(g.GroupID) });

                $.cookie('agent_id', data.agent_id);
                $.cookie('agent_login', agent_login);
                $.cookie('agent_group', data.agent_group);
                $.cookie('ext_no', ext_no);
                $("#Unlogin").hide();
                $("#Tabs").show();
                $("#RecordListCurrentPage").val(1);
                InitServiceItemPicker(data.agent_id);

                var page = parseInt($("#RecordListCurrentPage").val());
                GetRecordList(g_agent_id, $("#RecordListDisplayStatus").val(), page);
                SetGroupOption("#TelphoneRecord", g_agent_group);
                SetGroupOption("#EmailRecord", g_agent_group);

                var ajaxAppender = new log4javascript.AjaxAppender(doc_root + "Log/Add/" + g_agent_id);
                ajaxAppender.setThreshold(log4javascript.Level.DEBUG);
                var jsonLayout = new log4javascript.JsonLayout(false, true);
                //ajaxAppender.setLayout(jsonLayout);
                //ajaxAppender.addHeader("Content-Type", "application/json");
                log.addAppender(ajaxAppender);
            },
            error: function (jqXHR, status, msg) {
                alert('Error in Login: ' + jqXHR.statusText + '(' + jqXHR.statusCode() + ')');
                log.error('Error in Login: ' + jqXHR.statusText + '(' + jqXHR.statusCode() + ')');
            }
        })
    } else {
        alert("Feed Login info failure");
        log.error("Feed Login info failure");
    }
}