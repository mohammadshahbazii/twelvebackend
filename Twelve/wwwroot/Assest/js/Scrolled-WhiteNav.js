document.addEventListener('scroll', function(event)
{
    if (window.scrollY > 40){
        document.getElementById('desktop-nav').classList.add('sticky-top');
        document.getElementById('desktop-nav').classList.add('bg-light');

    }
    else{
        document.getElementById('desktop-nav').classList.remove('sticky-top');
        document.getElementById('desktop-nav').classList.remove('bg-light');
    }


});

