document.querySelector('.upload-documents-btn').addEventListener('click', (event) => {
    event.preventDefault();
    const input = document.querySelector('.upload-documents-input');
    input.addEventListener('change', (event) => {
        if (event.target.files.length > 0) {
            document.querySelector('.documents-form').submit();
        }
    });
    input.click();
});