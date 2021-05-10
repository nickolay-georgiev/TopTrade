document.addEventListener('DOMContentLoaded', function () {
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
            }, 350);
        }, 5000);
    };

    document.querySelector('.withdraw-button').addEventListener('click', (e) => {
        e.preventDefault();
        const errorSpan = document.querySelector('p.withdraw-error-message');
        const desiredAmount = Number(document.querySelector('.desired-amount').value);
        if (desiredAmount == 0) {
            errorSpan.textContent = "Please enter a valid amount";
            errorSpan.hidden = false;
            e.target.disabled = true;
        } else {
            errorSpan.hidden = true;
            e.target.disabled = false;
            document.querySelector('#withdraw-modal form').submit();
        }
    });

    document.querySelector('.desired-amount').addEventListener('keyup', (event) => {
        const desiredAmount = Number(event.target.value);
        const withdrawableAmount = Number(document.querySelector('#withdraw-modal form>input').value);
        const errorSpan = document.querySelector('p.withdraw-error-message');
        const submitWithdrawRequestBtn = document.querySelector('#withdraw-modal form button');

        if (desiredAmount > withdrawableAmount) {
            errorSpan.textContent = `You can withdraw up to ${(withdrawableAmount.toFixed(2))}`
            errorSpan.hidden = false;
            submitWithdrawRequestBtn.disabled = true;
        } else {
            errorSpan.hidden = true;
            submitWithdrawRequestBtn.disabled = false;
        }
    });

    const cardNumber = document.querySelector('.card-number');
    if (cardNumber) {
        cardNumber.addEventListener('input', function (e) {
            var cardNumber = this.value.split("-").join("");
            if (cardNumber.length > 0) {
                cardNumber = cardNumber.match(new RegExp('.{1,4}', 'g')).join("-");
            }
            this.value = cardNumber;
        });
    };

    $(function () {
        $('[data-toggle="popover"]').popover()
    });
});