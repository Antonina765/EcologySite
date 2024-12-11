$(document).ready(function () {
    $(".theme-toggle").click(function () {
        const theme = $(this).attr("data-theme");
        eraseCookie("theme");
        setCookie("theme", theme);
        location.reload();
    });

    function setCookie(name, value) {
        document.cookie = name + "=" + (value || "") + "; path=/";
    }

    function getCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(";");
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == " ") c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    function eraseCookie(name) {
        document.cookie =
            name + "=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;";
    }

    // Тема при загрузке страницы
    const currentTheme = getCookie("theme");
    if (currentTheme) {
        document.documentElement.setAttribute("data-theme", currentTheme);
        if (currentTheme === "dark") {
            $('link[href*="profile.css"]').attr('href', 'dark-profile.css');
        } else {
            $('link[href*="dark-profile.css"]').attr('href', 'profile.css');
        }
    }
});
