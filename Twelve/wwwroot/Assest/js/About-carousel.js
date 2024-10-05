function cahngePhoto(id){
    document.getElementById('About-img-1').classList.remove('w-40');
    document.getElementById('About-img-2').classList.remove('w-40');
    document.getElementById('About-img-3').classList.remove('w-40');
    document.getElementById('About-img-4').classList.remove('w-40');

    document.getElementById('About-img-1').classList.add('w-18');
    document.getElementById('About-img-2').classList.add('w-18');
    document.getElementById('About-img-3').classList.add('w-18');
    document.getElementById('About-img-4').classList.add('w-18');

    document.getElementById(id).classList.remove('w-18');
    document.getElementById(id).classList.add('w-40');
}