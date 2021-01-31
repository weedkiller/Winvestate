var _selectedOffer = "";
var _minimumPrice = "";
var _offerValidation;

jQuery(document).ready(function () {
    $(".hide").hide();
});

$(".confirmForOffer").on('click',
    function (e) {
        e.preventDefault();
        var btn = $(this);
        var loModel = {};
        loModel.row_guid = $(this).data("id");
        var loTempText = btn.text();
        btn.addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true)
            .text("Lütfen Bekleyiniz");

        $.ajax({
            method: "post",
            url: HOST_URL + '/Offer/ConfirmSubmit',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(loModel),
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + HOST_TOKEN);
            },
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
                if (response.code == 200) {
                    swal.fire({
                        text: "İşlem Başarılı. Müşteri Teklif Verebilir!",
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Tamam",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        window.location = "/Home/Dashboard";
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
    });


$(".resend").on('click',
    function (e) {
        e.preventDefault();
        var btn = $(this);
        var loModel = {};
        loModel.mespact_session_uuid = $(this).data("id");
        var loTempText = btn.text();
        btn.addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true)
            .text("Lütfen Bekleyiniz");

        $.ajax({
            method: "post",
            url: HOST_URL + '/Offer/Resend',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(loModel),
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + HOST_TOKEN);
            },
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
                if (response.code == 200) {
                    swal.fire({
                        text: "Bağlantı gönderimi sağlandı!",
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Tamam",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
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
    });

$(".openOfferPage").on('click',
    function (e) {
        e.preventDefault();
        _selectedOffer = $(this).data("id");
        $("#kt_modal_new_offer").modal("show");
        _minimumPrice = parseFloat($(this).data("minimum"));
        $("#warningOfOffer").text("Verebileceğiniz en düşük teklif bedeli " + _minimumPrice.toLocaleString() + " TL'dir.");
        setNewOfferValidation();
    });

$('#amount').on('keydown', function (e) {
    if (e.which == 13) {
        console.log("test");
        e.preventDefault();
    }
});

$("#saveOffer").on('click',
    function (e) {
        e.preventDefault();
        var loAmountToOffer = parseFloat($("#amount").val().replaceAll(".", ""));
        if (loAmountToOffer >= _minimumPrice) {
            _offerValidation.validate().then(function (status) {
                if (status == 'Valid') {
                    Swal.fire({
                        title: "Onaylıyor Musunuz?",
                        text: "Teklifiniz "+loAmountToOffer.toLocaleString()+" olarak kaydedilecek.",
                        icon: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Evet!",
                        cancelButtonText: "İptal Et"
                    }).then(function (result) {
                        if (result.value) {
                            var btn = $(this);
                            var loModel = {};
                            loModel.offer_uuid = _selectedOffer;
                            loModel.amount = loAmountToOffer;
                            var loTempText = btn.text();
                            btn.addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true)
                                .text("Lütfen Bekleyiniz");

                            $.ajax({
                                method: "post",
                                url: HOST_URL + '/Offer/New',
                                dataType: "json",
                                contentType: 'application/json',
                                data: JSON.stringify(loModel),
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader('Authorization', 'Bearer ' + HOST_TOKEN);
                                },
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
                                    if (response.code == 200) {
                                        swal.fire({
                                            text: "İşlem Başarılı. Teklifiniz başarıyla kayıt edildi.",
                                            icon: "success",
                                            buttonsStyling: false,
                                            confirmButtonText: "Tamam",
                                            customClass: {
                                                confirmButton: "btn font-weight-bold btn-light-primary"
                                            }
                                        }).then(function () {
                                            window.location = "/Home/Dashboard";
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
                                        }).then(function () {
                                            window.location = "/Home/Dashboard";
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
                    });
                }
            });
        }
        else {
            swal.fire({
                text: "Verebileceğiniz en düşük teklif tutarı " + _minimumPrice.toLocaleString() + " TL'dir.",
                icon: "error",
                buttonsStyling: false,
                confirmButtonText: "Tamam",
                customClass: {
                    confirmButton: "btn font-weight-bold btn-light-primary"
                }
            }).then(function () {
                KTUtil.scrollTop();
            });
        }
    });

$(".showOtherOffers").on('click',
    function (e) {
        e.preventDefault();
        var loId = $(this).data("id");
        var loDomName = "#offer_detail_" + loId;
        var loBtnDomName = "#show_offer_detail_" + loId;

        if ($(loDomName).hasClass("hide")) {
            $(loDomName).removeClass("hide").addClass("show");
            $(loDomName).show();
            $(loBtnDomName).text("Diğer Tekliflerimi Kapat");
        } else {
            $(loDomName).addClass("hide").removeClass("show");
            $(loDomName).hide();
            $(loBtnDomName).text("Diğer Tekliflerimi Görüntüle");
        }
    });


function setNewOfferValidation() {
    var formEl = KTUtil.getById('newOfferForm');
    _offerValidation = FormValidation.formValidation(
        formEl,
        {
            fields: {
                amount: {
                    validators: {
                        notEmpty: {
                            message: 'Teklif Tutarı girilmeden işleme devam edilemez.'
                        },
                        callback: {
                            message: 'Lütfen geçerli bir değer giriniz.',
                            callback: function (input) {
                                var loResult = input.value.replace(/,/g, '').replaceAll('.', '').replaceAll('₺', '');
                                return loResult.length == 0 || parseFloat(loResult) == loResult;
                            }
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