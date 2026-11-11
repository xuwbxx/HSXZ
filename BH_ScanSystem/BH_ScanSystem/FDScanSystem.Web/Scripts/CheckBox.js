jQuery.fn.extend(
	{
	    CascadingSelect: function (container, node) {
	        var checked = $(this)[0].checked;

	        $("input:checkbox[id^='" + container + "n']", "#" + $(this).attr("id").replace("CheckBox", "Nodes")).each(
					function () {
					    $(this).attr("checked", checked);

					});
	        $(this).parents("div[id^='" + container + "n']").each(
					function () {
					    $("input:checkbox[id='" + $(this).attr("id").replace("Nodes", "CheckBox") + "']").each(
						function () {
						    var checkeds = $("input:checkbox[id^='" + container + "n'][checked]", "#" + $(this).attr("id").replace("CheckBox", "Nodes")).length;
						    if (checkeds == 0) {
						       // $(this).attr("checked", false);
						    }
						    else if (checkeds < $("input:checkbox[id^='" + container + "n']", "#" + $(this).attr("id").replace("CheckBox", "Nodes")).length) {
						        $(this).attr("checked", true);

						    }
						    else {
						        $(this).attr("checked", true);

						    }
						});
					});
	    }

	});
