const uploadDocumentBtn = document.querySelector('.upload-documents-btn');
if (uploadDocumentBtn) {
    uploadDocumentBtn.addEventListener('click', (event) => {
        event.preventDefault();
        const input = document.querySelector('.upload-documents-input');
        input.addEventListener('change', (event) => {
            if (event.target.files.length > 0) {
                document.querySelector('.documents-form').submit();
            }
        });
        input.click();
    });
};

const closeToastMark = document.querySelector('.t-close');
if (closeToastMark) {
    closeToastMark.addEventListener('click', () => {
        document.querySelector('.toaster-container').style.display = 'none';
    });
};

const toaster = document.querySelector('.toaster-container');
if (toaster) {
    setTimeout(function () {
        setTimeout(function () {
            toaster.style.display = 'none';
            console.log(1);
        }, 350);
    }, 5000);
};

$(function () {
    $('[data-toggle="popover"]').popover()
});
