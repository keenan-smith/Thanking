(function($){
    $(function(){
        if (document.cookie.indexOf("meme") != -1) {
            $('#thanking-img').css("max-width", "");
            $('#thanking-img').css("max-height", "");
            $('#thanking-img').css("min-width", $('#thanking-img').width() + "px")
            $('#thanking-img').css("min-height", $('#thanking-img').height() + "px");
            $("#before").delay(500).animate({ 
                   height: 0, 
                   opacity: 0,
                   margin: 0,
                   padding: 0
               }, 'slow', function(){
                   $(this).hide();
                   $(".hidden").each (function(i) {
                       var x = this;
                       setTimeout(function() {
                           $(x).fadeIn(1500).css('display','inline-block').removeClass('hidden');
                       }, 300 * i);
                });
                   
                $('#after').show();
            });
        }
        else {
            document.cookie = "meme;";
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
                   $(".hidden").each (function(i) {
                       var x = this;
                       setTimeout(function() {
                           $(x).fadeIn(1500).css('display','inline-block').removeClass('hidden');
                       }, 300 * i);
                   });
                   
                   $('#after').show();
               }); 
            });   
        }
    }); // end of document ready)
})(jQuery); // end of jQuery name space