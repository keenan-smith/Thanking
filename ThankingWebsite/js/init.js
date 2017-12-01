(function($){
    $(function(){
        $('.button-collapse').sideNav();
        $('#download-button').click(function() {
            $('#thanking-img').css("max-width", "");
            $('#thanking-img').css("max-height", "");
            $('#thanking-img').css("min-width", $('#thanking-img').width() + "px")
            $('#thanking-img').css("min-height", $('#thanking-img').height() + "px");
		
            $("#before").animate({ 
                height: 0, 
                opacity: 0,
                margin: 0,
                padding: 0
            }, 'slow', function(){
                $(this).hide();
            }); 
        })
    }); // end of document ready
})(jQuery); // end of jQuery name space