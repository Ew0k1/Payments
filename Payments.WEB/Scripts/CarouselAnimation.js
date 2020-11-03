var itemsCount = $('.carousel-item').length;

if (itemsCount == 4) {
    $('#prev').addClass('size4')
    $('#next').addClass('size4')
}
if (itemsCount == 3) {
    $('#prev').addClass('size3')
    $('#next').addClass('size3')
}
if (itemsCount == 2) {
    $('#prev').addClass('size2')
    $('#next').addClass('size2')
}
if (itemsCount == 1) {
    $('#prev').addClass('size1')
    $('#next').addClass('size1')
}

$('#carousel-example').on('slide.bs.carousel', function (e) {
   
    var $e = $(e.relatedTarget);
    var idx = $e.index();
    var itemsPerSlide = 4;
    var totalItems = $('.carousel-item').length;

    if (idx >= totalItems - (itemsPerSlide - 1)) {
        var it = itemsPerSlide - (totalItems - idx);
        for (var i = 0; i < it; i++) {
            // append slides to end
            if (e.direction == "left") {
                $('.carousel-item').eq(i).appendTo('.carousel-inner');
            }
            else {
                $('.carousel-item').eq(0).appendTo('.carousel-inner');
            }
        }
    }
});