const toaster = document.querySelector('.toaster-container');
if (toaster) {
    setTimeout(function () {
        setTimeout(function () {
            toaster.style.display = 'none';
        }, 350);
    }, 5000);
};