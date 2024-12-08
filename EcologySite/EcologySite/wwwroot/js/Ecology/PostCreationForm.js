$(document).ready(function () {
    // Получение элементов
    let formContainer = $("#postForm");
    let toggleButton = $("#toggleFormBtn");
    let editForm = $("#editForm");

    // Когда пользователь нажимает кнопку, открывается/закрывается форма
    toggleButton.on("click", function() {
        formContainer.toggle(1000);
    });

    // Когда пользователь нажимает на кнопку отправки формы, форма скрывается
    formContainer.on("submit", function() {
        setTimeout(function() {
            formContainer.hide(1000);
        }, 500);
    });

    // Когда пользователь нажимает в любом месте за пределами формы, она скрывается
    $(document).mouseup(function(e) {
        if (!formContainer.is(e.target) && formContainer.has(e.target).length === 0) {
            formContainer.hide(1000);
        }
    });

    // Показ формы редактирования 
    $(".edit-btn").on("click", function() {
        let postId = $(this).data("post-id");
        let imageUrl = $(this).closest("li").find(".post").attr("src");
        let text = $(this).closest("li").find("p").text();

        $("#editId").val(postId);
        $("#editImageUrl").val(imageUrl);
        $("#editImageText").val(text);

        editForm.show(1000);
    });

    // Когда пользователь нажимает в любом месте за пределами формы редактирования, она скрывается
    $(document).mouseup(function(e) {
        if (!editForm.is(e.target) && editForm.has(e.target).length === 0) {
            editForm.hide(1000);
        }
    });

    // Обработка создания нового поста
    $(".submit-post-btn").click(function () {
        const url = "/api/EcologyChat";
        const imageUrl = $("#imageUrl").val();
        const imageFile = $("#postForm input[type='file']")[0].files[0];
        const text = $("#imageText").val();

        const data = new FormData();
        data.append("Url", imageUrl);
        data.append("Text", text);
        if (imageFile) {
            data.append("ImageFile", imageFile);
        }

        $.ajax({
            url: url,
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (response) {
                const newPost = `
                    <li>
                        <div class="post-header">
                            <img src="${response.userAvatar}" alt="User Icon" class="user-icon" />
                            <span>${response.userName}</span>
                        </div>
                        <img src="${response.imageSrc}" class="post" alt="Picture" />
                        <p>${response.text}</p>

                        <div class="post-actions">
                            <button class="edit-btn" data-post-id="${response.postId}">•••</button>
                            <div class="actions-menu">
                                <a href="/Ecology/UpdatePost?id=${response.postId}&url=${response.imageSrc}&text=${response.text}">Edit</a>
                                <a class="tag" href="/Ecology/Remove?id=${response.postId}">Delete</a>
                            </div>
                        </div>

                        <div class="comments">
                            <h4>Comments</h4>
                        </div>
                    </li>
                `;
                $("ul").prepend(newPost);

                // Скрываем форму после успешного создания поста
                formContainer.hide(1000);
            },
            error: function (xhr) {
                console.error(xhr.responseText);
            }
        });
    });
});
