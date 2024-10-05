document.addEventListener('scroll', function(event)
{
    if (window.scrollY > 40){
        document.getElementById('desktop-nav').classList.add('fast-slow');
        document.getElementById('desktop-nav').classList.remove('position-absolute');
        document.getElementById('desktop-nav').classList.add('bg-light');

        document.getElementById('navbar-home').classList.remove('text-light');
        document.getElementById('navbar-about').classList.remove('text-light');
        document.getElementById('navbar-contacts').classList.remove('text-light');
        document.getElementById('navbar-supports').classList.remove('text-light');
        document.getElementById('navbar-contactsclub').classList.remove('text-light');
        document.getElementById('navbar-blog').classList.remove('text-light');
        document.getElementById('navbar-toggle').classList.remove('text-light');

        document.getElementById('navbar-home').classList.add('text-darkblue');
        document.getElementById('navbar-about').classList.add('text-darkblue');
        document.getElementById('navbar-contacts').classList.add('text-darkblue');
        document.getElementById('navbar-supports').classList.add('text-darkblue');
        document.getElementById('navbar-contactsclub').classList.add('text-darkblue');
        document.getElementById('navbar-blog').classList.add('text-darkblue');
        document.getElementById('navbar-toggle').classList.add('text-darkblue');

        document.getElementById('logo').src = 'Assest/img/Logo-colorful.png';
        document.getElementById('desktop-nav').classList.add('sticky-top');

    }
    else{
        document.getElementById('desktop-nav').classList.add('position-absolute');
        document.getElementById('desktop-nav').classList.remove('sticky-top');
        document.getElementById('desktop-nav').classList.remove('bg-light');

        document.getElementById('navbar-home').classList.add('text-light');
        document.getElementById('navbar-about').classList.add('text-light');
        document.getElementById('navbar-contacts').classList.add('text-light');
        document.getElementById('navbar-supports').classList.add('text-light');
        document.getElementById('navbar-contactsclub').classList.add('text-light');
        document.getElementById('navbar-blog').classList.add('text-light');
        document.getElementById('navbar-toggle').classList.add('text-light');

        document.getElementById('navbar-home').classList.remove('text-darkblue');
        document.getElementById('navbar-about').classList.remove('text-darkblue');
        document.getElementById('navbar-contacts').classList.remove('text-darkblue');
        document.getElementById('navbar-supports').classList.remove('text-darkblue');
        document.getElementById('navbar-contactsclub').classList.remove('text-darkblue');
        document.getElementById('navbar-blog').classList.remove('text-darkblue');
        document.getElementById('navbar-toggle').classList.remove('text-darkblue');

        document.getElementById('logo').src = 'Assest/img/Logo.png';
    }


});