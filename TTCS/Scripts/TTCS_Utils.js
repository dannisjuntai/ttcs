/***************************************** 
* Utilities function Section
*****************************************/
function MsJsonDatetimeToString(dt) {
    return JsDateToString(TimestampToDate(dt.substr(6)));
}

function MsJsonDatetimeToJsDate(dt) {
    return TimestampToDate(dt.substr(6));
}

function TimestampToJsDate(ts) {
    return new Date(parseInt(ts));
}

function ISODateStringToString(iso_string) {
    return (iso_string === null || iso_string.length < 1 )?"N/A":JsDateToString(ISODateStringToJsDate(iso_string));
}

function ISODateStringToJsDate(iso_string) {
    return new Date(iso_string);
}

function JsDateToString(dt) {
    return dt.getFullYear() + "-" +
        StrPadLeft((dt.getMonth() + 1).toString(), 2) + "-" +
        StrPadLeft(dt.getDate().toString(), 2) + " " +
        StrPadLeft(dt.getHours().toString(), 2) + ":" +
        StrPadLeft(dt.getMinutes().toString(), 2) + ":" +
        StrPadLeft(dt.getSeconds().toString(), 2);
}

/*
Ugly code come from Internet
*/
function StrPadLeft(str, length, ch) {
    ch = ch || "0";
    if (str.length >= length)
        return str;
    else
        return StrPadLeft("0" + str, length);
}

function var_dump(obj) {
    var str = "";
    if (typeof (obj) !== "object") {
        return obj.toString();
    }
    for (var key in obj) {
        str += key + ": " + obj[key] + "\n"
    }
    return str;
}

function FilterLastSlash(UrlStr)
{
    if (UrlStr[UrlStr.length - 1] == '/')
        return UrlStr.substr(0, UrlStr.length - 1);
    else
        return UrlStr;
}

function HtmlEncode(value) {
    return $('<div/>').text(value).html();
}

function HtmlDecode(value) {
    return $('<div/>').html(value).text();
}

function nl2br(str) {
    return str.replace(/([^>])\n/g, '$1<br/>\n');
}

// usage: document.write(var_dump(ANY-JS-VAR,'html'));
function var_dump(data, addwhitespace, safety, level) {
    var rtrn = '';
    var dt, it, spaces = '';

    if (!level) {
        level = 1;
    }

    for (var i = 0; i < level; i++) {
        spaces += '   ';
    } //end for i<level

    if (typeof (data) != 'object') {
        dt = data;

        if (typeof (data) == 'string') {
            if (addwhitespace == 'html') {
                dt = dt.replace(/&/g, '&amp;');
                dt = dt.replace(/>/g, '&gt;');
                dt = dt.replace(/</g, '&lt;');
            } //end if addwhitespace == html
            dt = dt.replace(/\"/g, '\"');
            dt = '"' + dt + '"';
        } //end if typeof == string

        if (typeof (data) == 'function' && addwhitespace) {
            dt = new String(dt).replace(/\n/g, "\n" + spaces);
            if (addwhitespace == 'html') {
                dt = dt.replace(/&/g, '&amp;');
                dt = dt.replace(/>/g, '&gt;');
                dt = dt.replace(/</g, '&lt;');
            } //end if addwhitespace == html
        } //end if typeof == function

        if (typeof (data) == 'undefined') {
            dt = 'undefined';
        } //end if typeof == undefined

        if (addwhitespace == 'html') {
            if (typeof (dt) != 'string') {
                dt = new String(dt);
            } //end typeof != string
            dt = dt.replace(/ /g, "&nbsp;").replace(/\n/g, "<br>");
        } //end if addwhitespace == html
        return dt;
    } //end if typeof != object && != array

    for (var x in data) {
        if (safety && (level > safety)) {
            dt = '*RECURSION*';
        } else {
            try {
                dt = var_dump(data[x], addwhitespace, safety, level + 1);
            } catch (e) {
                continue;
            }
        } //end if-else level > safety
        it = var_dump(x, addwhitespace, safety, level + 1);
        rtrn += it + ':' + dt + ',';
        if (addwhitespace) {
            rtrn += '\n' + spaces;
        } //end if addwhitespace
    } //end for...in

    if (addwhitespace) {
        rtrn = '{\n' + spaces + rtrn.substr(0, rtrn.length - (2 + (level * 3))) + '\n' + spaces.substr(0, spaces.length - 3) + '}';
    } else {
        rtrn = '{' + rtrn.substr(0, rtrn.length - 1) + '}';
    } //end if-else addwhitespace

    if (addwhitespace == 'html') {
        rtrn = rtrn.replace(/ /g, "&nbsp;").replace(/\n/g, "<br>");
    } //end if addwhitespace == html

    return rtrn;
} //end function var_dump
