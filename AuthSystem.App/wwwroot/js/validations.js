// Validação dos forms
var tempo = 150;

var form = document.getElementById("account");
form.addEventListener("input", function (event) {
    window.setTimeout(function () {
        setClass(event);
    }, tempo);
}, true);
form.addEventListener("focus", function (event) {
    window.setTimeout(function () {
        setClass(event);
    }, tempo);
}, true);
form.addEventListener("blur", function (event) {
    setClass(event);
}, true);

function setClass(event) {
    if (event.target.classList.contains("valid")) {
        event.target.classList.remove("is-invalid");
        event.target.classList.add("is-valid");
    }
    else if (event.target.classList.contains("input-validation-error")) {
        event.target.classList.remove("is-valid");
        event.target.classList.add("is-invalid");
    }
}

// Validação dos forms - Click
function validateForm() {
    window.setTimeout(function () {
        var errors = document.getElementsByClassName("field-validation-error");        
        for (var i = 0; i < errors.length; i++) {
            errors[i].previousElementSibling.classList.add('is-invalid');
        }
        if (errors.length === 0) {
            let botao = form.querySelector('button[type="submit"]');
            botao.disabled = true;
            botao.firstElementChild.classList.add("spinner-border");
            botao.firstElementChild.classList.add("spinner-border-sm");
        }
    }, 0);
}