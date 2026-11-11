/// <reference path="jquery-1.4.1-vsdoc.js" />
Utility = {
    createCookie: function (name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    },

    readCookie: function (name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    },
    eraseCookie: function (name) {
        createCookie(name, "", -1);
    },
    getAllSelectorItems: function (ctlId, hdfield) {
        var retVal = '';
        var selector = document.getElementById(ctlId);
        var hd = document.getElementById(hdfield);
        if (selector != null && hd != null) {
            var cnt = selector.options.length;
            for (var i = 0; i < cnt; i++) {
                retVal += (selector.options[i].text + ":" + selector.options[i].value + "$");
            }
        }
        //alert(retVal);
        hd.value = retVal;
        return retVal;
    },
    moveUpSelectorItem: function (ctlId) {
        var target = "#" + ctlId + " option:selected";
        $(target).each(function () {
            $(this).insertBefore($(this).prev());
        });

    },
    moveDownSelectorItem: function (ctlId) {
        var target = "#" + ctlId + " option:selected";
        $(target).each(function () {
            $(this).insertAfter($(this).next());
        });
    },

    moveSelectorItem: function (ctlId, direction) {
        var listbox = document.getElementById(ctlId);
        var selIndex = listbox.selectedIndex;
        if (listbox != null && selIndex >= 0) {
            var increment = -1;
            if (direction == 'up')
                increment = -1;
            else
                increment = 1;
            if ((selIndex + increment) < 0 || (selIndex + increment) > (listbox.options.length - 1)) {
                return;
            }
            var selValue = listbox.options[selIndex].value;
            var selText = listbox.options[selIndex].text;
            listbox.options[selIndex].value = listbox.options[selIndex + increment].value;
            listbox.options[selIndex].text = listbox.options[selIndex + increment].text;
            listbox.options[selIndex + increment].value = selValue;
            listbox.options[selIndex + increment].text = selText;
            listbox.selectedIndex = selIndex + increment;
        }
    },
    moveItemsBetweenSelectors: function (ctrlSource, ctrlTarget) {
        var Source = document.getElementById(ctrlSource);
        var Target = document.getElementById(ctrlTarget);
        if ((Source != null) && (Target != null)) {
            while (Source.options.selectedIndex >= 0) {
                var newOption = new Option(); // Create a new instance of ListItem
                newOption.text = Source.options[Source.options.selectedIndex].text;
                newOption.value = Source.options[Source.options.selectedIndex].value;

                Target.options[Target.length] = newOption; //Append the item in Target
                Source.remove(Source.options.selectedIndex);  //Remove the item from Source
            }
        }
    }
};