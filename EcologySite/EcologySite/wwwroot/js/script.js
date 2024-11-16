// Получаем элементы
const modal = document.getElementById("myModal");
const openModalBtn = document.getElementById("openModalBtn");
const closeBtn = document.getElementsByClassName("close-btn")[0];

// Открываем модальное окно при клике на кнопку "Add Post"
openModalBtn.onclick = function() {
    modal.style.display = "block";
}

// Закрываем модальное окно при клике на "x"
closeBtn.onclick = function() {
    modal.style.display = "none";
}

// Закрываем модальное окно при клике вне его области
window.onclick = function(event) {
    if (event.target === modal) {
        modal.style.display = "none";
    }
}

// Закрываем модальное окно при сабмите формы
function closeModal() {
    modal.style.display = "none";
}
