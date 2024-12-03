// Получение элементов
let modal = document.getElementById("myModal");
let btn = document.getElementById("openFormBtn");
let span = document.getElementsByClassName("close")[0];

// Когда пользователь нажимает кнопку, открывается всплывающее окно
btn.onclick = function() {
    modal.style.display = "block";
}

// Когда пользователь нажимает на <span> (x), закрывается всплывающее окно
    span.onclick = function() {
    modal.style.display = "none";
}

// Когда пользователь нажимает в любом месте за пределами окна, оно закрывается
window.onclick = function(event) {
    if (event.target === modal) {
        modal.style.display = "none";
    }
}

// Функция для показа формы редактирования
function showEditForm(id, url, text) {
    document.getElementById("editId").value = id;
    document.getElementById("editImageUrl").value = url;
    document.getElementById("editImageText").value = text;
    document.getElementById("editForm").style.display = "block";
}
