// JavaScript Document

$(document).ready(function(){
	// 宽度限制
	$('.content-max').width( document.body.scrollWidth-201 );
	
	// 导航hover
	$('.nev').children('li').children('a').hover(function(){ 
		$(this).parent('li').addClass('hover');
	},function(){
		$(this).parent('li').removeClass('hover');
	});
	
	$('.nev').children('li').children('a').click(function(){
		//按子菜单可见性判断展开/收起（原来的 height()===55 写法在页面缩放非100%时失效）
		var $li = $(this).parent('li');
		var isOpen = $li.children('.nev-two').is(':visible');
		$('.nev').children('li').removeClass('active');
		$('.nev').children('li').children('.nev-two').slideUp();
		if( !isOpen ){
			$li.addClass('active');
			$(this).next().stop(true,true).slideDown();
		}
	});
	
	$('.nev').children('li').children('.nev-two').hover(function(){ 
		$(this).parent().addClass('hover');
	},function(){
		$(this).parent().removeClass('hover');
	});
	$('.nev').children('li').children('.nev-two').children('li').click(function(){
		$('.nev').children('li').children('.nev-two').children('li').removeClass('clickactive');
		$(this).addClass('clickactive');

		var MenuID = $(this).children('a').attr('data-id');
		setCookie('IOTMenuActived', MenuID);

	});
	
	// 空导航判定
	for( var i=0; i<$('.nev').children('li').length; i++){
		if( $('.nev').children('li').eq(i).children('.nev-two').html() == '' ){
			$('.nev').children('li').eq(i).children('a').children('.more').css('display','none');
		}
	}
	
	// nev-max限制高度
	$('.nev-max').css({'height': window.innerHeight-71+'px'});
	
    // content限制高度
	$('.content-min').css({ 'min-height': window.innerHeight - 140 + 'px' });
	$('.tab-content').css({ 'min-height': window.innerHeight - 182 + 'px' });
	$('.choice').css({ 'min-height': window.innerHeight - 120 + 'px' });
	
	//if( $('.nev').height()>$('.content-max').height() ){
	//	$('.content-min').css({'height': $('.nev').height()+80-80+'px'});
	//	$('.content').css({'height': window.innerHeight-80+'px'});
	//	$('.tab-content').css({'height': $('.content-min').height()+'px'});
	//}else{
	//	$('.content').css({'height': $('.content-min').height()+'px'});
	//}

	// 打开弹出层
	$('.popup-max').height( window.innerHeight );
	$('.popup-open').click(function(){	
		$('.popup-max').slideDown();
	});
	//$('.popup-close').click( function(){
	//	$('.popup-max').slideUp();
	//});
	$('.popup-close').click(function () {
	    $('.popup-max').slideUp();
	    //$('.popup-max').css('display', 'none');
	    $(document).unbind('mousewheel');
	});
	
});