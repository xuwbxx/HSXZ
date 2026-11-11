/**
* Check form
*/
/// <reference path="jquery-1.4.1-vsdoc.js" />



function check(vform) {

    var flag = true;
    try {
        form = document.getElementById(vform);
        for (var i = form.elements.length - 1; i > 0; i--) {

            if (form.elements[i].getAttribute("ValidateRequired") != null || form.elements[i].getAttribute("ValidateMode") != null) {
                try {
                    var checkResult = checkElement(form.elements[i]);
                    var name = form.elements[i].getAttribute("name");
                    if (checkResult) {
                        document.getElementById("rfv" + name).style.display = "none";
                        //document.getElementById(name).className = "msgHide";
                        form.elements[i].className = "msgHide";

                    }
                    else {
                        document.getElementById("rfv" + name).style.display = "inline";
                        //document.getElementById(name).className = "msgDisplay";
                        form.elements[i].className = "msgDisplay";
                        //if (!form.elements[i].disabled)
                        // form.elements[i].focus();
                        flag = false;
                    }
                }
                catch (e) {

                }
            }
        }
    }
    catch (e) {
        alert(e.message);
    }

    if (!flag)
        alert("数据验证未通过,请检查后重新提交！");

    return flag;
}

function checkInnerCtl(ctl) {
    var flag = true;
    try {
        form = document.getElementById(ctl);
        for (var i = form.all.length - 1; i > 0; i--) {
            if (form.all[i].getAttribute("ValidateRequired") != null || form.all[i].getAttribute("ValidateMode") != null) {
                try {
                    var checkResult = checkElement(form.all[i]);
                    var name = form.all[i].getAttribute("name");
                    if (checkResult) {
                        document.getElementById("rfv" + name).style.display = "none";
                        form.all[i].className = "msgHide";

                    }
                    else {
                        document.getElementById("rfv" + name).style.display = "inline";
                        form.all[i].className = "msgDisplay";
                        flag = false;
                    }
                }
                catch (e) {

                }
            }
        }
    }
    catch (e) {
        alert(e.message);
    }
    if (!flag)
        alert("数据验证未通过,请检查后重新提交！");
    return flag;
}

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

/**
* Check text field in the form
*/
function checkElement(element) {
    var isRequired = element.getAttribute("ValidateRequired");
    var inputValue = element.value;

    var validChar = GetValidChar(element.getAttribute("ValidateMode"), element);

    //过滤特殊字符
    //    if(inputValue.length > 1 && (element.type == "text" || element == "textarea"))
    //    {
    //        var temp  =  "\\(|\\)|'|;|and|exec|insert|select|delete|update|count|%|chr|mid|master|truncate|char|declare|&|#|\\$"; 
    //        var regex1=new RegExp(temp,"g");
    //       
    //        element.value = inputValue.replace(regex1,"");   
    //    }

    if (isRequired == "true") {
        if (inputValue.length < 1) {
            return false;
        }
        else {
            if (validChar != null) {

                var regex = new RegExp(validChar);
                return regex.test(inputValue);
            }
            return true;
        }
    }
    else {
        if (inputValue.length < 1) {
            return true;
        }
        else {
            if (validChar != null) {
                var regex = new RegExp(validChar);
                return regex.test(inputValue);
            }
        }
    }

    return true;
}

function GetValidChar(validEnum, element) {
    if (validEnum == null) return null;

    var validChar = null;
    switch (validEnum) {
        case "Int":
            validChar = "^[0-9][0-9]{0,5}$";
            //validChar = "^[1-9][0-9]{0,5}$";
            break;
        case "Float":
            validChar = "^\\d+(\\.\\d+)?$";
            break;
        case "Email":
            validChar = "^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";
            break;
        case "Tel":
            validChar = "(^((\\(\\d{2,3}\\))|(\\d{3}\\-))?(\\(0\\d{2,3}\\)|0\d{2,3}-)?[1-9]\\d{6,7}(\\-\\d{1,4})?$)|(^0{0,1}(13[0-9]|15[0-9])[0-9]{8}$)";
            break;
        case "PostalCode":
            validChar = "^\\d{6}$";
            break;
        case "IDCard":
            validChar = "(^\\d{15}$)|(^\\d{17}([0-9]|(X|x))$)";
            break;
        case "Url":
            validChar = "^http://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?$";
            break;
        case "MobilePhone":
            validChar = "^0{0,1}(13[0-9]|15[0-9])[0-9]{8}$";
            break;
        case "Custom":
            validChar = element.getAttribute("CustomRegexExpression");
            break;
        default:
            break;
    }

    return validChar;
}


function quanjiao(obj) {
    var str = obj.value;

    if (str.length > 0) {
        for (var i = str.length - 1; i >= 0; i--) {
            unicode = str.charCodeAt(i);
            if (unicode > 65280 && unicode < 65375) {
                alert("不能输入全角字符，请输入半角字符");
                return false;
            }
        }
    }

    return true;
}



