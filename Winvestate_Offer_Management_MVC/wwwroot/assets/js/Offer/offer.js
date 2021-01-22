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
            url: HOST_URL+'/Offer/Confirm',
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