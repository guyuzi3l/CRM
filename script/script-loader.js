/* ----------------- Start JS Document ----------------- */

// Page Loader
$(window).load(function () {
    "use strict";    
	$('#loader').fadeOut();
});

$(document).ready(function ($) {
	"use strict";	
	////	Hidder Header	
	var headerEle = function () {
		var $headerHeight = $('header').height();
		$('.hidden-header').css({ 'height' : $headerHeight  + "px" });
	};
	
	$(window).load(function () {
	    headerEle();
	});
	
	$(window).resize(function () {
	    headerEle();
	});
	

});





