// JavaScript Document

$(document).ready(function(){
	// 导航hover
	$('.sum-nev').children('li').not('.sum-active').hover(function(){ 
		$(this).addClass('sum-active');
	},function(){
		$(this).removeClass('sum-active');
	});
	// 施工现场hover
	var newstrUrl = '';
	var oldstrUrl = '';
	$('.sum-content-scene').children('ul').children('li').hover(function(){ 
		var strUrl = $(this).children('a').children('div').attr("class");
		newstrUrl = 'url(img-sum/' + strUrl + '-hover'+'.png)';
		oldstrUrl = 'url(img-sum/' + strUrl +'.png)';
		$(this).css({'background':'#0155ab'});
		$(this).children('a').children('div').css({'background': newstrUrl });
	},function(){
		$(this).css({'background':'#7cbae4'});
		$(this).children('a').children('div').css({'background': oldstrUrl });
	});
	// BIM hover
	$('.sum-content-bim').children('ul').children('li').hover(function(){ 
		var strUrl = $(this).children('a').children('div').attr("class");
		newstrUrl = 'url(img-sum/' + strUrl + '-hover'+'.png)';
		oldstrUrl = 'url(img-sum/' + strUrl +'.png)';
		$(this).css({'background':'#0155ab'});
		$(this).children('a').children('div').css({'background': newstrUrl });
	},function(){
		$(this).css({'background':'#7cbae4'});
		$(this).children('a').children('div').css({'background': oldstrUrl });
	});
	
	$('.sum-content').css({'min-height':window.innerHeight-150+'px'});
	// 打开弹出层
	$('.popup-max').height( window.innerHeight );
	$('.popup-open').click(function(){	
		$('.popup-max').slideDown();
	});
	$('.popup-close').click( function(){
		$('.popup-max').slideUp();
	});
	// 打开更多选择框
	$('.choice-open').click( function(){
		$('.sum-content').hide();
		$('.choice').slideDown(300);
		 
	});
	$('.choice-return').click( function(){
		$('.sum-content').slideDown();
		$('.choice').hide();
	});
	
	
	$('.choice').css({'min-height':window.innerHeight - $('.sum-head').height() - $('.sum-footer').height()+'px'});
	$('.full-screen').click( function(){ 
		$('.popup').css({'top':'0','left':'0','margin':'0','width':'100%','height': window.innerHeight+'px'});
		$('.popup').children('div').css({'width':$('.popup').width()-40+'px','height':$('.popup').height()-60+'px'});
	});
});