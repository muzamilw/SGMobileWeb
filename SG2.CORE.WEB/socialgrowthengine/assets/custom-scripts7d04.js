
(function ($) {


    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];
    var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    var newDate = new Date();
    newDate.setDate(newDate.getDate() + 1);
    //$('#Date').html(dayNames[newDate.getDay()] + " " + newDate.getDate() + ' ' + monthNames[newDate.getMonth()] + ' ' + newDate.getFullYear());
    $('#Date').html(monthNames[newDate.getMonth()]);


    $('.slider-slick').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        dots: false,
        centerMode: true,
        centerPadding: '200px',
        focusOnSelect: true,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 480,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 1
                }
            }
        ]
    });
    
    /* carousel scripts */
    //$('.main-carousel').flickity({
    //    // options
    //    wrapAround: true,
    //    pageDots: false,
    //    adaptiveHeight: false,
    //    lazyLoad: 2
    //});

    $('.testimonial-carousel').flickity({
        // options
        pageDots: false,
        adaptiveHeight: false,
        autoPlay: 2500,
        pauseAutoPlayOnHover: false
    });
    
    /* sticky header scripts */
    // on load get height of the nav and add it to the height of the hero section
    navBar = $("nav.navbar");
    navBar.next().children(":first").css('padding-top', parseInt(navBar.next().children(":first").css('padding-top')) + navBar.height());
    // if window is scrolled make background solid
    $(window).on("scroll load", function () {
        isTop = $(window).scrollTop();
        if (isTop == 0) {
            navBar.removeClass('scrolled');
        } else {
            navBar.addClass('scrolled');
        }

    });


    /* smooth scroll */
    // Select all links with hashes
    $('a[href*="#"]')
        // Remove links that don't actually link to anything
        .not('[href="#"]')
        .not('[href="#0"]')
        .click(function (event) {
            // On-page links
            if (
                location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
                &&
                location.hostname == this.hostname
            ) {
                // Figure out element to scroll to
                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                // Does a scroll target exist?
                if (target.length) {
                    // Only prevent default if animation is actually gonna happen
                    event.preventDefault();
                    // console.log('top offset: '+ (target.offset().top - 300 ) );
                    $('html, body').animate({
                        scrollTop: (target.offset().top - 200)
                    }, 1000, function () {
                        // Callback after animation
                        // Must change focus!
                        var $target = $(target);
                        $target.focus();
                        if ($target.is(":focus")) { // Checking if the target was focused
                            return false;
                        } else {
                            $target.attr('tabindex', '-1'); // Adding tabindex for elements not focusable
                            $target.focus(); // Set focus again
                        };
                    });
                }
            }
        });

})(jQuery);


