var _submitValidation;
var _callbackValidation;
var _captchaResponse;
$(document).ready(function () {
    setSubmitValidation();
    setCallbackValidation();

    $(".company").hide();

    $('[data-switch=true]').bootstrapSwitch();

    $('#birth_date').mask('00.00.0000', {
        placeholder: "gg.aa.yyyy"
    });

    $("#validateCustomer").on('click',
        function (e) {
            e.preventDefault();
            _submitValidation.validate().then(function (status) {
                if (status == 'Valid') {
                    checkUserIdentity();
                }
            });
        });

    $("#submitCallback").on('click',
        function (e) {
            e.preventDefault();
            _callbackValidation.validate().then(function (status) {
                if (status == 'Valid') {
                    saveCallback();
                }
            });
        });

    $('input[type=radio][name=userType]').change(function () {
        console.log(this.value);
        console.log($("input:radio[name=userType]:checked").val());
        if ($("input:radio[name=userType]:checked").val() == 1) {
            $(".company").hide();
        }
        else {
            $(".company").show();
        }
    });
});

var correctCaptcha = function (response) {
    _captchaResponse = response;
};

function checkUserIdentity() {
    var loTempButtonText = $("#validateCustomer").text();
    $("#validateCustomer").addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true).text("Lütfen Bekleyiniz");

    setTimeout(function () {

        var model = objectifyForm($('#newCustomerValidationForm').serializeArray());
        model.user_type_system_type_id = Number(model.userType);
        model.identity = Number(model.identity);
        model.birthdate = $("#birth_date").val();
        model.send_agreement = true;
        model.asset_uuid = SELECTED_ASSET;

        var loCaptcha = {};
        loCaptcha.GoogleReCaptchaResponse = _captchaResponse;

        $.ajax({
            method: "post",
            url: '/Customer/Check',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(loCaptcha),

            error: function (xhr) {
                _captchaResponse = "";
                $("#validateCustomer").removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text(loTempButtonText);
                console.log(xhr);
                swal.fire({
                    text: "Lütfen ben robot değilim kutucuğunu işaretleyiniz.",
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Tamam",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    grecaptcha.reset(0);
                    grecaptcha.reset(1);
                });
            },
            success: function (response, status, xhr, $form) {
                _captchaResponse = "";
                if (response > 0) {
                    $.ajax({
                        method: "post",
                        url: HOST_URL + '/Customer/Check',
                        dataType: "json",
                        contentType: 'application/json',
                        data: JSON.stringify(model),
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('Authorization', 'Bearer ' + HOST_TOKEN);
                        },
                        error: function (xhr) {
                            $("#validateCustomer").removeClass('spinner spinner-right spinner-white pr-15')
                                .attr('disabled', false).text(loTempButtonText);
                            console.log(xhr);
                            swal.fire({
                                text: "Kimlik bilgileri doğrulama esnasında bir problem oluştu lütfen tekrar deneyiniz..",
                                icon: "error",
                                buttonsStyling: false,
                                confirmButtonText: "Tamam",
                                customClass: {
                                    confirmButton: "btn font-weight-bold btn-light-primary"
                                }
                            }).then(function () {
                                grecaptcha.reset(0);
                                grecaptcha.reset(1);
                            });
                        },
                        success: function (response, status, xhr, $form) {
                            $("#validateCustomer").removeClass('spinner spinner-right spinner-white pr-15')
                                .attr('disabled', false).text(loTempButtonText);
                            if (response.code > 0) {
                                checkUserPhone();
                            } else if (response.code == 0) {
                                swal.fire({
                                    text:
                                        "Teklif başvurunuzu daha önce yapmışsınız. Panele yönlendiriliyorsunuz. Size daha önce yollanan kullanıcı adı ve şifre ile panele giriş yapabilirsiniz.",
                                    icon: "warning",
                                    buttonsStyling: false,
                                    confirmButtonText: "Tamam",
                                    customClass: {
                                        confirmButton: "btn font-weight-bold btn-light-primary"
                                    }
                                }).then(() => {
                                    window.location = "/Account/Login";
                                });
                            } else {
                                swal.fire({
                                    text: "Kimlik bilgileriniz doğrulanamadı. Lütfen bilgilerinizi kontrol ediniz.",
                                    icon: "error",
                                    buttonsStyling: false,
                                    confirmButtonText: "Tamam",
                                    customClass: {
                                        confirmButton: "btn font-weight-bold btn-light-primary"
                                    }
                                }).then(() => {
                                    grecaptcha.reset(0);
                                    grecaptcha.reset(1);
                                });
                            }


                            // similate 2s delay
                            //setTimeout(function() {
                            //    btn.removeClass('kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light').attr('disabled', false);
                            //    showErrorMsg(form, 'danger', 'Incorrect username or password. Please try again.');
                            //   }, 2000);
                        }
                    });
                } else {
                    swal.fire({
                        text: "Lütfen ben robot değilim kutucuğunu işaretleyiniz.",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Tamam",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        $("#validateCustomer").removeClass('spinner spinner-right spinner-white pr-15')
                            .attr('disabled', false).text(loTempButtonText);
                        KTUtil.scrollTop();
                    });
                }

            }
        });
    }, 1000); //captcha bekle
}

function saveCallback() {
    var loTempButtonText = $("#submitCallback").text();
    $("#submitCallback").addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true).text("Lütfen Bekleyiniz");

    setTimeout(function () {
        var model = objectifyForm($('#newCallbackForm').serializeArray());
        model.asset_uuid = SELECTED_ASSET;
        model.GoogleReCaptchaResponse = _captchaResponse;
        $.ajax({
            method: "post",
            url: '/Customer/Callback',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(model),
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + HOST_TOKEN);
            },
            error: function (xhr) {
                _captchaResponse = "";
                $("#submitCallback").removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text(loTempButtonText);
                console.log(xhr);
                swal.fire({
                    text: "Geri aranma talebi oluşturma esnasında bir problem oluştu lütfen tekrar deneyiniz..",
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Tamam",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    grecaptcha.reset(0);
                    grecaptcha.reset(1);
                });
            },
            success: function (response, status, xhr, $form) {
                _captchaResponse = "";
                $("#submitCallback").removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text(loTempButtonText);
                if (response.id > 0) {
                    swal.fire({
                        text: "Talebiniz kayıt edildi. Gayrimenkul danışanlarımız en kısa süre içerisinde sizlere geri dönüş sağlayacaktır. Teşekkürler.",
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Tamam",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(() => {
                        grecaptcha.reset(0);
                        grecaptcha.reset(1);
                        $("#kt_modal_callback").modal("hide");
                    });
                } else if (response.id == -2) {
                    swal.fire({
                        text: "Lütfen ben robot değilim kutusunu işaretleyiniz. Eğer işaretlediyseniz lütfen sayfayı yenileyiniz!",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Tamam",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(() => {
                        grecaptcha.reset(0);
                        grecaptcha.reset(1);
                        $("#kt_modal_callback").modal("hide");
                    });
                }
                else {
                    swal.fire({
                        text: "Talebiniz oluşturulamadı." + response.message,
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Tamam",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(() => {
                        grecaptcha.reset(0);
                        grecaptcha.reset(1);
                    });
                }


                // similate 2s delay
                //setTimeout(function() {
                //    btn.removeClass('kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light').attr('disabled', false);
                //    showErrorMsg(form, 'danger', 'Incorrect username or password. Please try again.');
                //   }, 2000);
            }
        });
    }, 1000); //captcha bekle


}

function checkUserPhone() {
    var loObj = {};
    loObj.phone = $("#phone").val();
    loObj.message_type_system_type_id = 36;
    $("#kt_modal_offer_info").modal("hide");
    sendOtp(loObj, HOST_TOKEN, $("#validateCustomer"), saveCustomerAndSendAgreement, HOST_URL + "/Otp/Send", HOST_URL + "/Otp/Validate", $("#kt_modal_offer"));
}

function saveCustomerAndSendAgreement() {
    $("kt_modal_offer").modal("show");
    var model = objectifyForm($('#newCustomerValidationForm').serializeArray());
    model.user_type_system_type_id = Number(model.userType);
    model.identity = Number(model.identity);
    model.birthdate = $("#birth_date").val();
    model.send_agreement = true;
    model.asset_uuid = SELECTED_ASSET;


    SendCustomerToServer(model, $("#validateCustomer"));
}

function SendCustomerToServer(model, btn) {
    var loTempText = btn.text();
    btn.addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true)
        .text("Lütfen Bekleyiniz");
    $.ajax({
        method: "post",
        url: '/Customer/Save',
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(model),
        error: function (xhr) {
            btn.removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false)
                .text(loTempText);
            console.log(xhr);
            swal.fire({
                text: "İşlem esnasında bir problem oluştu. Lütfen tekrar deneyiniz.",
                icon: "error",
                buttonsStyling: false,
                confirmButtonText: "Tamam",
                customClass: {
                    confirmButton: "btn font-weight-bold btn-light-primary"
                }
            }).then(function () {
                KTUtil.scrollTop();
            });
        },
        success: function (response, status, xhr, $form) {
            btn.removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false)
                .text(loTempText);
            if (response.id > 0) {
                swal.fire({
                    text: "Kayıt Başarılı! İmzalaman gereken sözleşmeyi telefon ve mail adresine ilettik. Sözleşme imzalandıktan sonra tarafınıza SMS olarak iletilecek şifreyle sisteme girip teklif verebilirsiniz.",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Tamam",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    $("#kt_modal_add_bank").modal("hide");
                    window.location = '/Asset/AssetForOffer';
                });
            } else {
                btn.removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false)
                    .text(loTempText);
                swal.fire({
                    text: "İşlem esnasında bir problem oluştu: " + response.message,
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Tamam",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                });
            }


            // similate 2s delay
            //setTimeout(function() {
            //    btn.removeClass('kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light').attr('disabled', false);
            //    showErrorMsg(form, 'danger', 'Incorrect username or password. Please try again.');
            //   }, 2000);
        }
    });
}

function setSubmitValidation() {
    var formEl = KTUtil.getById('newCustomerValidationForm');
    _submitValidation = FormValidation.formValidation(
        formEl,
        {
            fields: {
                customer_name: {
                    validators: {
                        notEmpty: {
                            message: 'Ad girilmeden işlem yapılamaz.'
                        }
                    }
                },
                customer_surname: {
                    validators: {
                        notEmpty: {
                            message: 'Soyad girilmeden işlem yapılamaz.'
                        }
                    }
                },
                identity_no: {
                    validators: {
                        notEmpty: {
                            message: 'Kimlik numarası girilmeden işlem yapılamaz.'
                        },
                        callback: {
                            message: 'Lütfen geçerli bir değer giriniz',
                            callback: function (input) {
                                var loTemp = parseFloat(input.value);
                                return loTemp == input.value;
                            }
                        }
                    }
                },
                birth_date: {
                    validators: {
                        notEmpty: {
                            message: 'Doğum tarihi girilmeden işlem yapılamaz.'
                        }
                    }
                },
                phone: {
                    validators: {
                        notEmpty: {
                            message: 'Telefon girilmeden işlem yapılamaz.'
                        }
                    }
                },
                mail: {
                    validators: {
                        notEmpty: {
                            message: 'Mail girilmeden işlem yapılamaz.'
                        }
                    }
                },
                checkKvkk: {
                    validators: {
                        notEmpty: {
                            message: 'Kvkk metni kabul edilmeden işlem yapılamaz.'
                        }
                    }
                },
                iban: {
                    validators: {
                        notEmpty: {
                            message: 'IBAN numarası girilmeden işleme devam edilemez.'
                        }
                    }
                },
                company_name: {
                    validators: {
                        notEmpty: {
                            message: 'Firma ünvanı girilmeden işleme devam edilemez.'
                        }
                    }
                },
                tax_no: {
                    validators: {
                        notEmpty: {
                            message: 'Vergi numarası girilmeden işleme devam edilemez.'
                        }
                    }
                },
                tax_office: {
                    validators: {
                        notEmpty: {
                            message: 'Vergi dairesi girilmeden işleme devam edilemez.'
                        }
                    }
                }

            },
            plugins: {
                declarative: new FormValidation.plugins.Declarative({
                    html5Input: true,
                }),
                trigger: new FormValidation.plugins.Trigger(),
                excluded: new FormValidation.plugins.Excluded(),
                // Bootstrap Framework Integration
                bootstrap: new FormValidation.plugins.Bootstrap({
                    //eleInvalidClass: '',
                    eleValidClass: '',
                })
            }
        });

}

function setCallbackValidation() {
    var formEl = KTUtil.getById('newCallbackForm');
    _callbackValidation = FormValidation.formValidation(
        formEl,
        {
            fields: {
                applicant_name: {
                    validators: {
                        notEmpty: {
                            message: 'Ad girilmeden işlem yapılamaz.'
                        }
                    }
                },
                applicant_surname: {
                    validators: {
                        notEmpty: {
                            message: 'Soyad girilmeden işlem yapılamaz.'
                        }
                    }
                },
                applicant_phone: {
                    validators: {
                        notEmpty: {
                            message: 'Telefon girilmeden işlem yapılamaz.'
                        }
                    }
                }

            },
            plugins: {
                declarative: new FormValidation.plugins.Declarative({
                    html5Input: true,
                }),
                trigger: new FormValidation.plugins.Trigger(),
                excluded: new FormValidation.plugins.Excluded(),
                // Bootstrap Framework Integration
                bootstrap: new FormValidation.plugins.Bootstrap({
                    //eleInvalidClass: '',
                    eleValidClass: '',
                })
            }
        });

}
