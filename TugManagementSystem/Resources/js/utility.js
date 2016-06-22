﻿function Util() { }

//验证时间，形如01:03
Util.prototype.isTime = function (columnLabel, inputStrTime) {

    var ret = inputStrTime.match(/^(\d{2})(:)?(\d{2})$/);
    if (ret == null) {alert(columnLabel + ':不是正确的时间格式(hh:mm)'); return false;}
    if (ret[1]>=24 || ret[3]>=60)
    {
        alert(columnLabel + ":输入的时间无效");
        return false
    }
    return true;
}

//验证日期，形如2001-03-01
Util.prototype.isDate = function (columnLabel, inputStrDate) {
    var r = inputStrDate.match(/^(\d{4})(-|\/)(\d{2})\2(\d{2})$/);
    if (r == null) { alert(columnLabel + ':不是正确的日期格式(yyyy-mm-dd)'); return false; }
    var d = new Date(r[1], r[3] - 1, r[4]);
    if (false == (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4])) {
        alert(columnLabel + ":输入的日期无效");
        return false
    }
    return true;
}

//验证日期时间，形如 (2008-07-22 13:04:06)
Util.prototype.isDateTime = function (columnLabel, inputStrDateTime) {
    var reg = /^(\d{4})(-|\/)(\d{2})\2(\d{2}) (\d{2}):(\d{2}):(\d{2})$/;
    var r = inputStrDateTime.match(reg);
    if (r == null) { alert(columnLabel + ':不是正确的日期时间格式(yyyy-mm-dd hh:mm:ss)'); return false; }
    var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
    if (false == (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7])) {
        alert(columnLabel + ":输入的日期时间无效");
        return false;
    }
    return true;
}

//验证字符串
Util.prototype.isValidString = function (columnLabel, inputString) { }

//验证Email
Util.prototype.isEmail = function (columnLabel, inputStrEmail) {
    var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
    if (reg.test(inputStrEmail)) {
        return true;
    } else {
        alert(columnLabel + ":输入的电子邮件无效");
        return false;
    }
}

//验证货币金额
Util.prototype.isValidCurrency = function (columnLabel, inputStrCurrency) {
    var reg = /^\d+\.?\d{0,2}$/;
    if (reg.test(inputStrCurrency)) {
        return true;
    }
    else {
        alert(columnLabel + ":输入的金额无效(最多两位小数)");
        return false;
    }
}

//验证小数 
Util.prototype.isValidFloat = function (columnLabel, inputStrFloat) { 
    var reg = /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/;
    if (reg.test(inputStrFloat)) {
        return true;
    }
    else {
        alert(columnLabel + ":输入的小数无效");
        return false;
    }
}

//验证整数
Util.prototype.isValidinteger = function (columnLabel, inputStrInteger) {
    var reg = /^-?\d+$/;
    if (reg.test(inputStrInteger)) { return true; }
    else {
        alert(columnLabel + ":输入的整数无效");
        return false;
    }
}

//验证自然数0，1，2，3...
Util.prototype.isValidNaturalNumber = function (columnLabel, inputStrNaturalNumber) {
    
    var r = inputStrNaturalNumber.match("^\\d+$");
    if (r == null)
    {  
        alert(columnLabel + ":请输入大于或等于0的整数");
        return false;
    }   
    else   
    {  
        return true;
    }
}