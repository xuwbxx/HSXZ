// JavaScript Document

jQuery.noConflict(); 
jQuery(document).ready(function(){ 
	jQuery(".nv li").hover(function(){
		jQuery(this).find("dl").show();
	},function(){
		jQuery(this).find("dl").hide();
	});

});