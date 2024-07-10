(function () {
    const sidebar = document.querySelector(".sidebar");
    const menu = document.querySelector("#menu");

    const main = document.querySelector(".user-main");

    const menu_container = document.querySelector(".menu-container");
    const logout_container = document.querySelector(".logout-container");

    const icon_logout = document.querySelector(".icon-logout");

    const home = document.querySelector("#home");
    const authorPost = document.querySelector("#authorPost");
    const rejection = document.querySelector("#rejection");


    let previousToggled = null;
    let currentToggled = null;


    home.addEventListener("click", (e) => {
        toggleMenu(home);
    });


    authorPost.addEventListener("click", (e) => {
        toggleMenu(authorPost);
    });

    rejection.addEventListener("click", (e) => {
        toggleMenu(rejection);
    });




    const toggleMenu = (button) => {
        if (previousToggled && button !== menu) {
            untoggleMenu(previousToggled);
        }

        button.classList.add("toggled");
        button.style.backgroundColor = "#FCFCAE";

        if (button !== menu) {
            previousToggled = button;
        }
    };

    const untoggleMenu = (button) => {
        button.classList.remove("toggled");
        button.style.backgroundColor = "#1FFFE86";
    };

    menu.addEventListener("click", (e) => {
        sidebar.classList.contains("active") ? closeMenu() : openMenu();
    });

    const openMenu = () => {
        sidebar.classList.add("active");
        sidebar.style.width = "250px";

        toggleMenu(menu);

        let menu_logo = document.createElement("img");
        menu_logo.id = "menu-logo";
        menu_logo.src = "../images/logo.svg";
        menu_logo.style.width = "45px";
        menu_container.style.paddingLeft = "10px";
        menu_container.insertBefore(menu_logo, menu_container.childNodes[0]);


        let p_home = document.createElement("p");
        p_home.id = "p-home";
        p_home.innerHTML = "Home";
        home.style.width = "220px";
        home.style.justifyContent = "left";
        home.appendChild(p_home);


        let p_authorPost = document.createElement("p");
        p_authorPost.id = "p-authorPost";
        p_authorPost.innerHTML = "My Blogs";
        authorPost.style.width = "220px";
        authorPost.style.justifyContent = "left";
        authorPost.appendChild(p_authorPost);


        let p_rejection = document.createElement("p");
        p_rejection.id = "p-rejection";
        p_rejection.innerHTML = "Blog Posts Rejections";
        rejection.style.width = "220px";
        rejection.style.justifyContent = "left";
        rejection.appendChild(p_rejection);




        icon_logout.style.width = "25%";

        let user_container = document.createElement("div");
        user_container.id = "user-container";

        let user_name = document.createElement("p");
        user_name.id = "user-name";
        user_name.innerHTML = "Logout";


        user_container.appendChild(user_name);
        logout_container.insertBefore(user_container, logout_container.childNodes[0]);


        logout_container.style.paddingLeft = "10px";

        main.style.width = "calc(100% - 250px)";
    };

    const closeMenu = () => {
        menu_container.removeChild(document.getElementById("menu-logo"));
        menu_container.style.paddingLeft = "0px";

        untoggleMenu(menu);


        home.removeChild(document.getElementById("p-home"));
        home.style.width = "50px";
        home.style.justifyContent = "center";

        authorPost.removeChild(document.getElementById("p-authorPost"));
        authorPost.style.width = "50px";
        authorPost.style.justifyContent = "center";

        rejection.removeChild(document.getElementById("p-rejection"));
        rejection.style.width = "50px";
        rejection.style.justifyContent = "center";

        logout_container.removeChild(document.getElementById("user-container"));
        logout_container.style.paddingLeft = "0px";

        icon_logout.style.width = "100%";

        sidebar.classList.remove("active");
        sidebar.style.width = "78px";

        main.style.width = "calc(100% - 78px)";
    };


})();

