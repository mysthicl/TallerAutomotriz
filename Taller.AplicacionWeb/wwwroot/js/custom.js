var lang = 'es';
var validTrackingNumber = true;
var lastTrackingNumber = '';
var formDisabled = false;

jQuery.validator.addMethod("tracking", function (value, element) {
    return this.optional(element) || validTrackingNumber;
}, "");

function trackPackage(code, e) {
    e.preventDefault();

    if (document.documentElement.lang == 'en') {
        window.open('/en/tracking?tracking=' + code);
    } else {
        window.open('/tracking?tracking=' + code);
    }

    gtag('event', 'track', {
        event_category: 'forms',
        event_label: 'Package Tracking Form'
    });
}

var formSupportValidator;
$(function () {
    lang = $('html').prop('lang');

    let trackingBar = $('.tracking-bar');
    if (trackingBar.length) {
        $('#tracking-bar').animate({
            scrollLeft: $('.active', trackingBar).last().offset().left
        }, 2000);
    }

    formSupportValidator = $('#formSupport').validate({
        rules: {
            name: {
                required: true
            },
            email: {
                required: true
            },
            phone: {
                required: true,
                minlength:5,
                maxlength:15
            },
            type: {
                required: true
            },
            description:{
                required:true,
                minlength:10,
                maxlength:300
            },
            captcha: {
                required: true
            },     
           
        },
        messages: {
            name: lang === 'es' ? 'Por favor ingrese su nombre y apellido.' : 'Please enter your full name.',
            email: lang === 'es' ? 'Por favor ingrese su correo electrónico.' : 'Please enter your email.',
            phone:{
                required: lang === 'es' ? 'Por favor ingrese su número de teléfono.' : 'Please enter your phone.',
                minlength : lang === 'es' ? jQuery.validator.format('Su teléfono debe tener al menos {0} números.') : jQuery.validator.format('Your phone should have at least {0} numbers.'),
                maxlength : lang === 'es' ? jQuery.validator.format('Su teléfono no debe superar los {0} números.') : jQuery.validator.format('Your phone must not exceed {0} numbers.'),
            }, 
            type: lang === 'es' ? 'Por favor ingrese motivo de su consulta.' : 'Please enter the motive of your consult.',
            description: {
                required: lang === 'es' ? 'Por favor ingrese su mensaje.' : 'Please enter your message.',
                minlength : lang === 'es' ? jQuery.validator.format('Su mensaje debe tener al menos {0} caracteres.') : jQuery.validator.format('Your message should have at least {0} characters long.'),
                maxlength : lang === 'es' ? jQuery.validator.format('Su mensaje no debe superar los {0} caracteres.') : jQuery.validator.format('Your message must not exceed {0} characters.'),
            },
            captcha: lang === 'es' ? 'Por favor ingrese el codigo de verificación CAPTCHA.' : 'Please enter the human verification code.',
        },
        errorElement: "label",
        errorPlacement: function (error, element) {
            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass("error").removeClass("success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).addClass("success").removeClass("error");
        },
        submitHandler: function (form) {
            if (formDisabled) return;

            formDisabled = true;
            var fd = new FormData(form);

            $.ajax((lang == 'en' ? '/en' : '') + '/ticket', {
                method: 'post',
                processData: false,
                contentType: false,
                data: fd,
                dataType: 'json'
            }).done(function (data) {
                $('#formResponse')
                    .removeClass('alert-danger d-none')
                    .addClass('alert-success')
                    .text(data.message)
                    .show();

                $('#formSupport input[type=text], #formSupport input[type=email], #formSupport select, #formSupport textarea').val(''); 
                dataLayer.push({
                  event: 'supportForm'
                });
            }).catch(function (data) {
                $('#formResponse')
                    .removeClass('alert-success d-none')
                    .addClass('alert-danger')
                    .text(data.responseJSON.errors ? Object.values(data.responseJSON.errors)[0][0] : data.responseJSON.message)
                    .show();

                console.log(data);
            }).always(function () {
                formDisabled = false;
            })
        }
    });

    formSupportValidator = $('#formSupportDescription').validate({
        rules: {
            description: {
                required: true
            },
            captcha: {
                required: true
            }
        },
        messages: {
            description: lang === 'es' ? 'Por favor ingrese uan descripción.' : 'Please enter a description.',
            captcha: lang === 'es' ? 'Por favor ingrese el codigo de verificación CAPTCHA.' : 'Please enter the human verification code.'
        },
        errorElement: "label",
        errorPlacement: function (error, element) {
            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass("error").removeClass("success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).addClass("success").removeClass("error");
        },
        submitHandler: function (form) {
            if (formDisabled) return;

            formDisabled = true;
            var fd = new FormData(form);

            $.ajax((lang == 'en' ? '/en' : '') + '/ticket/description', {
                method: 'post',
                processData: false,
                contentType: false,
                data: fd,
                dataType: 'json'
            }).done(function (data) {
                $('#formResponse')
                    .removeClass('alert-danger d-none')
                    .addClass('alert-success')
                    .text(data.message)
                    .show();

                $('#formSupport textarea').val('');
            }).catch(function (data) {
                $('#formResponse')
                    .removeClass('alert-success d-none')
                    .addClass('alert-danger')
                    .text(data.responseJSON.errors ? Object.values(data.responseJSON.errors)[0][0] : data.responseJSON.message)
                    .show();

                console.log(data);
            }).always(function () {
                formDisabled = false;
            })
        }
    });

    $('.nav-link').each(function () {
        if ($(this).attr('href') === window.location.pathname) {
            $(this).addClass('active');
        }
    });

    const stickyElm = document.querySelector('.faq-last-event');

    if (stickyElm) {
        window.addEventListener('scroll', () => {
            let topPos = stickyElm.getBoundingClientRect().top;
            if (topPos === 73) {
                stickyElm.classList.add('sticky');
            } else {
                stickyElm.classList.remove('sticky');
            }
        })
    }
});