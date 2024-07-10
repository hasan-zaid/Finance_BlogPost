(function () {
    const sidebar = document.querySelector(".sidebar");
    const menu = document.querySelector("#menu");

    const main = document.querySelector(".user-main");

    const menu_container = document.querySelector(".menu-container");
    const logout_container = document.querySelector(".logout-container");

    const icon_logout = document.querySelector(".icon-logout");

    const home = document.querySelector("#home");
    const dashboard = document.querySelector("#dashboard");
    const user = document.querySelector("#user");
    const tag = document.querySelector("#tag");
    const blogPost = document.querySelector("#blogPost");
    const approval = document.querySelector("#approval");


    let previousToggled = null;
    let currentToggled = null;


    home.addEventListener("click", (e) => {
        toggleMenu(home);
    });


    dashboard.addEventListener("click", (e) => {
        toggleMenu(dashboard);
    });

    user.addEventListener("click", (e) => {
        toggleMenu(user);
    });

    tag.addEventListener("click", (e) => {
        toggleMenu(tag);
    });

    blogPost.addEventListener("click", (e) => {
        toggleMenu(blogPost);
    });

    approval.addEventListener("click", (e) => {
        toggleMenu(approval);
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

        let p_dash = document.createElement("p");
        p_dash.id = "p-dashboard";
        p_dash.innerHTML = "Dashboard";
        dashboard.style.width = "220px";
        dashboard.style.justifyContent = "left";
        dashboard.appendChild(p_dash);

        let p_user = document.createElement("p");
        p_user.id = "p-user";
        p_user.innerHTML = "Users";
        user.style.width = "220px";
        user.style.justifyContent = "left";
        user.appendChild(p_user);

        let p_tag = document.createElement("p");
        p_tag.id = "p-tag";
        p_tag.innerHTML = "Blog Tags";
        tag.style.width = "220px";
        tag.style.justifyContent = "left";
        tag.appendChild(p_tag);


        let p_blogPost = document.createElement("p");
        p_blogPost.id = "p-blogPost";
        p_blogPost.innerHTML = "Blog Posts";
        blogPost.style.width = "220px";
        blogPost.style.justifyContent = "left";
        blogPost.appendChild(p_blogPost);


        let p_approval = document.createElement("p");
        p_approval.id = "p-approval";
        p_approval.innerHTML = "Blog Posts Approval";
        approval.style.width = "220px";
        approval.style.justifyContent = "left";
        approval.appendChild(p_approval);




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

        dashboard.removeChild(document.getElementById("p-dashboard"));
        dashboard.style.width = "50px";
        dashboard.style.justifyContent = "center";

        user.removeChild(document.getElementById("p-user"));
        user.style.width = "50px";
        user.style.justifyContent = "center";

        tag.removeChild(document.getElementById("p-tag"));
        tag.style.width = "50px";
        tag.style.justifyContent = "center";

        blogPost.removeChild(document.getElementById("p-blogPost"));
        blogPost.style.width = "50px";
        blogPost.style.justifyContent = "center";

        approval.removeChild(document.getElementById("p-approval"));
        approval.style.width = "50px";
        approval.style.justifyContent = "center";

        logout_container.removeChild(document.getElementById("user-container"));
        logout_container.style.paddingLeft = "0px";

        icon_logout.style.width = "100%";

        sidebar.classList.remove("active");
        sidebar.style.width = "78px";

        main.style.width = "calc(100% - 78px)";
    };


})();

