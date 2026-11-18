document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll("form").forEach(form => {

        form.addEventListener("submit", function (event) {

            const submitButton = event.submitter ||
                this.querySelector(".js-submit-once");

            if (!submitButton) return;

            // Don't disable when ModelState is invalid (server-side guard)
            if (this.dataset.disableSubmit === "false") return;

            // Don't disable when HTML5 constraint validation fails
            if (!this.checkValidity()) return;

            // Don't disable when jQuery Unobtrusive Validation has errors
            // jQuery Unobtrusive Validation stores validator in form's data attribute
            const validator = $(this).data("validator");
            if (validator && !validator.form()) return;

            // All validation passed - disable button to prevent multiple submissions
            submitButton.disabled = true;

        });

    });

});
