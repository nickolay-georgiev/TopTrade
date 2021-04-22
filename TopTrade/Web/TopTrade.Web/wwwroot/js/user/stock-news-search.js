document.querySelector('.search-news-button').addEventListener('click', searchStockNews);
function searchStockNews() {
    const input = document.querySelector('.search-news-form input');
    if (!input.value) { input.value = 'All' }
} 