DXUtilities = {};
DXUtilities.CurrentDemoGroupKey = "";
DXUtilities.CurrentDemoKey = "";
DXUtilities.CurrentThemeCookieKey = "DXCurrentTheme";
DXUtilities.CodeLoaded = false;
DXUtilities.InitDemoView = function(){
    DXUtilities.CodeLoaded = TabControl.GetActiveTab().name == "Code";
    DXUtilities.DemoViewChanged();
}
DXUtilities.DemoViewChanged = function(){
    var isCodeView = TabControl.GetActiveTab().name == "Code";
    for(var i = 0; i < TabControl.GetTabCount(); i++){
        var tab = TabControl.GetTab(i);
        document.getElementById(tab.name + "Area").style.display = (tab == TabControl.GetActiveTab()) ? "block" : "none";
    }
    if(!DXUtilities.CodeLoaded && isCodeView){
        CodePanel.PerformCallback(DXUtilities.CurrentDemoGroupKey + ";" + DXUtilities.CurrentDemoKey);
        DXUtilities.CodeLoaded = true;
    }
    document.getElementById("ThemeSelectorButton").style.display = !isCodeView ? "" : "none";
}
DXUtilities.ShowCodeInNewWindow = function(demoTitle, codeTitle){
    var codeWnd = window.open("", "_blank", "status=0,toolbar=0,scrollbars=1,resizable=1,top=74,left=74,width=" + (screen.availWidth - 150) + ",height=" + (screen.availHeight - 150));
    codeWnd.document.open();
    codeWnd.document.write("<html><head><title>");
    codeWnd.document.write(demoTitle + " - " + codeTitle);
    codeWnd.document.write("</title>");
    var links = document.getElementsByTagName("link");
    for(var i = 0; i < links.length; i++){
        if(links[i].href && links[i].href.indexOf("CodeFormatter.css") > -1){
            codeWnd.document.write("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + links[i].href + "\" />");
            break;
        }
    }
    codeWnd.document.write("</head><body><code>");
    var codeGroup = CodeNavBar.GetGroupByName(codeTitle);
    codeWnd.document.write(document.getElementById("CodeBlock" + codeGroup.index).innerHTML);
    codeWnd.document.write("</code></body></html>");
    codeWnd.document.close();
}
DXUtilities.ShowThemeSelector = function(){
    ThemeSelectorPopup.Show();
}
DXUtilities.ThemeSelectorButton = null;
DXUtilities.ThemeSelectorPopupBorder = null;
DXUtilities.ThemeSelectorPopupPopUp = function(){
    if(DXUtilities.ThemeSelectorButton == null)
        DXUtilities.ThemeSelectorButton = document.getElementById("ThemeSelectorButton");
    if(DXUtilities.ThemeSelectorPopupBorder == null){
        DXUtilities.ThemeSelectorPopupBorder = document.createElement("DIV");
        DXUtilities.ThemeSelectorPopupBorder.id = "ThemeSelectorButtonBorder";
        DXUtilities.ThemeSelectorButton.parentNode.appendChild(DXUtilities.ThemeSelectorPopupBorder);
    }
    var themeSelectorPopupBorderHeight = 7;
    DXUtilities.ThemeSelectorPopupBorder.style.height = themeSelectorPopupBorderHeight + "px";
    DXUtilities.ThemeSelectorPopupBorder.style.left = (ASPxClientUtils.GetAbsoluteX(DXUtilities.ThemeSelectorButton) + 1) + "px";
    DXUtilities.ThemeSelectorPopupBorder.style.top = (ASPxClientUtils.GetAbsoluteY(DXUtilities.ThemeSelectorButton) + DXUtilities.ThemeSelectorButton.offsetHeight - themeSelectorPopupBorderHeight) + "px";
    DXUtilities.ThemeSelectorPopupBorder.style.display = "block";
    DXUtilities.ThemeSelectorButton.className = "selected";
}
DXUtilities.ThemeSelectorPopupCloseUp = function(){
    DXUtilities.ThemeSelectorPopupBorder.style.display = "none";
    DXUtilities.ThemeSelectorButton.className = "";
}
DXUtilities.SetCurrentTheme = function(theme){
    ThemeSelectorPopup.Hide();
    ASPxClientUtils.SetCookie(DXUtilities.CurrentThemeCookieKey, theme);
    if(document.forms[0] && (!document.forms[0].onsubmit || document.forms[0].onsubmit.toString().indexOf("Sys.Mvc.AsyncForm") == -1))
        document.forms[0].submit();
    else
        window.location.reload();
}
DXUtilities.CodeNavBar_HeaderClick = function(s, e) {
    var source = _aspxGetEventSource(e.htmlEvent);
    var tag = source.tagName;
    e.cancel = tag != "A" && tag != "IMG";
}
DXUtilities.ShowScreenshotWindow = function(evt, link){
    DXUtilities.ShowScreenshot(link.href); 
    evt.cancelBubble = true; 
    return false;
}
DXUtilities.ShowScreenshot = function(src){
    var screenLeft = document.all && !document.opera ? window.screenLeft : window.screenX;
	var screenWidth = screen.availWidth;
	var screenHeight = screen.availHeight;
    var zeroX = Math.floor((screenLeft < 0 ? 0 : screenLeft) / screenWidth) * screenWidth;
    
	var windowWidth = 475;
	var windowHeight = 325;
	var windowX = parseInt((screenWidth - windowWidth) / 2);
	var windowY = parseInt((screenHeight - windowHeight) / 2);
	if(windowX + windowWidth > screenWidth)
		windowX = 0;
	if(windowY + windowHeight > screenHeight)
		windowY = 0;

    windowX += zeroX;

	var popupwnd = window.open(src,'_blank',"left=" + windowX + ",top=" + windowY + ",width=" + windowWidth + ",height=" + windowHeight + ", scrollbars=no, resizable=no", true);
	if (popupwnd != null && popupwnd.document != null && popupwnd.document.body != null) {
	    popupwnd.document.body.style.margin = "0px"; 
    }
}

DXEventMonitor = {
    TimerId: -1,
    PendingUpdates: [ ],
    
    Trace: function(sender, e, eventName) {
        var checkbox = document.getElementById("EventCheck" + eventName);
        if(!checkbox.checked) return;
        
        var self = DXEventMonitor;
        var name = sender.name;
        var lastSeparator = name.lastIndexOf("_");
        if(lastSeparator > -1)
            name = name.substr(lastSeparator + 1);

        var builder = ["<table cellspacing=0 cellpadding=0 border=0>"];
        self.BuildTraceRowHtml(builder, "Sender", name, 100);
        self.BuildTraceRowHtml(builder, "EventType", eventName.replace(/_/g, " "));
        var count = 0;
        for(var child in e) {
            if (typeof e[child] == "function") continue;
            var childValue = e[child];
            if (typeof e[child] == "object" && e[child] && e[child].name)
                childValue = "[name: '" + e[child].name + "']";
            self.BuildTraceRowHtml(builder, count ? "" : "Arguments", child + " = " + childValue);
            count++;
        }                
        builder.push("</table><br />");
        
        self.PendingUpdates.unshift(builder.join(""));
        if(self.TimerId < 0)
            self.TimerId = window.setTimeout(self.Update, 0);
    },
    
    BuildTraceRowHtml: function(builder, name, text, width) {
        builder.push("<tr><td valign=top");
        if(width)
            builder.push(" style='width: ", width, "px'");
        builder.push(">");
        if(name)
            builder.push("<b>", name, ":</b>");        
        builder.push("</td><td valign=top>", text, "</td></tr>");
    },
    
    GetLogElement: function() {
        return document.getElementById("EventLog")
    },
    
    Update: function() {                    
        var self = DXEventMonitor;
        var element = self.GetLogElement();
        
        element.innerHTML = self.PendingUpdates.join("") + element.innerHTML;
        self.TimerId = -1;
        self.PendingUpdates = [ ];                
    },
    
    Clear: function() {
       DXEventMonitor.GetLogElement().innerHTML = ""; 
    }
};
