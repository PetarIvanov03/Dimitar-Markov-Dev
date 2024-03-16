// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const toggleButton = document.getElementsByClassName('toggle-button')[0];
const navbarLinks = document.getElementsByClassName('navbar-links')[0];

toggleButton.addEventListener('click', () => {
    navbarLinks.classList.toggle('active')
});

//toggleButton.addEventListener("click", function () {
//    alert("Hello World!");
//});


window.setTimeout(function () {
    $(".alert").fadeTo(500, 0).slideUp(500, function () {
        $(this) - remove();
    });
}, 4000);


var uploadField = document.getElementById("file");
var fullsize = 0;
uploadField.onchange = function () {
    for (var i = 0; i < this.files.length; i++) {
        fullsize += this.files[i].size;
        if (fullsize > 26200000) {
            alert("Файловете, които се опитвате да качите са по-големи от 25MB");
            this.value = ""; // Clear the file input
            break; // Stop checking further files
        }
    }
};

