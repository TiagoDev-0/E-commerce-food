
window.addEventListener('scroll', function () {
    const navbar = document.querySelector('.navbar-custom');
    if (window.scrollY > 50) {
        navbar.style.backgroundColor = '#c62828';
    } else {
        navbar.style.backgroundColor = '#E63946';
    }
});

const searchInput = document.querySelector('.form-control');
searchInput.addEventListener('focus', () => { searchInput.style.width = '250px'; });
searchInput.addEventListener('blur', () => { searchInput.style.width = ''; });

const toggle = document.getElementById('theme-toggle');
const icon = toggle.querySelector('.material-symbols-outlined');
toggle.addEventListener('click', () => {
    document.body.classList.toggle('dark-mode');
    icon.textContent = document.body.classList.contains('dark-mode') ? 'light_mode' : 'dark_mode';
    fetch('/Header/ToggleTheme', { method: 'POST' });
});

function atualizarCarrinho(count) {
    document.getElementById('carrinho-count').textContent = count;
}

fetch('/Header/GetCarrinhoCount')
    .then(res => res.json())
    .then(count => atualizarCarrinho(count));

