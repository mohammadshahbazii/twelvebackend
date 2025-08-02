document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.language-selector').forEach(selector => {
        selector.addEventListener('change', e => {
            const lang = e.target.value;
            document.cookie = `.AspNetCore.Culture=c=${lang}|uic=${lang}; path=/`;
        });
    });
});
