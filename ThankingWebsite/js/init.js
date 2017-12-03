(function($){
    $(function(){
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

                $(".expand").each (function(j) {
                    var x = this;
                    setTimeout(function () {
                        $(x).animate({
                            width: "100%"
                        }, 3000);
                    }, 300 * $(".hidden").length + 300 * j);
                });

                $('#after').show();
            });
        });
    }); // end of document ready)
})(jQuery); // end of jQuery name space