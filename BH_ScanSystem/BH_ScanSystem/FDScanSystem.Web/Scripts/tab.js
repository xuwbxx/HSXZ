// JavaScript Document

jQuery(function(){


  jQuery(".TAB li").click(function(){
    var jQueryvv=jQuery(this).parent(".TAB").attr("id");
	  jQuery(jQueryvv).hide();
	  jQuery(this).parent(".TAB").find("li").removeClass("hover");
	  var xx=jQuery(this).parent(".TAB").find("li").index(this);
	  jQuery(jQueryvv).eq(xx).show();
	  jQuery(this).addClass("hover");
	  //jQuery(".tel").text(nn.left+"/"+nv.left);
  
  });

});