
new WOW().init();

var aboutSwiper = new Swiper(".aboutSwiper", {
    slidesPerView: 1,
    spaceBetween: 30,
    freeMode: true,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    breakpoints: {
        599: {
            slidesPerView: 2,
            spaceBetweenSlides: 50
        },
    }
});

var planeSwiper = new Swiper(".planeSwiper", {
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swipper-button-next",
        nextEl: ".swipper-button-prev",
    },
});

const menu = document.querySelector(".menu");
const menuBtn = document.querySelector(".menu-btn");

menuBtn.addEventListener("Click", () => {
    menu.classList.toggle('nav-toggle');
});

document.querySelector(".year").innerHTML = new Date().getFullYear();




