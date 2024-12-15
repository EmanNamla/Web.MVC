toastr.options = {
    closeButton: true,
    debug: false,
    newestOnTop: true,
    progressBar: true,
    positionClass: "toast-top-right",
    preventDuplicates: true,
    onclick: null,
    showDuration: "300",
    hideDuration: "1000",
    timeOut: "5000",
    extendedTimeOut: "1000",
    showEasing: "swing",
    hideEasing: "linear",
    showMethod: "fadeIn",
    hideMethod: "fadeOut"
};


if (typeof TempData !== 'undefined') {
    if (TempData.SuccessMessage) {
        console.log("EMan")
        toastr.success(TempData.SuccessMessage);
    }
    if (TempData.ErrorMessage) {
        toastr.error(TempData.ErrorMessage);
    }
}
document.getElementById("registerPassword").addEventListener("focus", function () {
    this.type = "text"; 
});

document.getElementById("registerPassword").addEventListener("blur", function () {
    this.type = "password"; 
});

document.getElementById("registerConfirmPassword").addEventListener("focus", function () {
    this.type = "text"; 
});

document.getElementById("registerConfirmPassword").addEventListener("blur", function () {
    this.type = "password";
});


