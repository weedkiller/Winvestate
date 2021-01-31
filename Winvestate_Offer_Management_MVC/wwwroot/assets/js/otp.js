var _isOtpSend = false;
var _otpSecond = 180;
var _validateOtp;
var _validateApiUrl;
var _token;

$("#kt_modal_otp").on('hide.bs.modal', function () {
    $("#secondSms").html("");
    _otpSecond = 180;
    _isOtpSend = false;
});

$(document).on("click", "#validateOtp", function () {
    var loOtp = MD5($("#otp").val());
    var loObj = {};
    loObj.phone = $("#validateOtp").data("phone");
    loObj.otp_hash = loOtp;
    $('#validateOtp').addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true).text("Lütfen Bekleyiniz");
    $.ajax({
        method: "post",
        url: _validateApiUrl,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(loObj),
        traditional: true,
        crossDomain: true,
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + _token);
        },
        error: function (request, error) {
            console.log(error);
            e.stopPropagation();
            $('#kt_modal_otp').modal("hide");
            $('#validateOtp').removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text("Doğrula");
            swal.fire({
                "title": "Hata",
                "text": "Doğrulama Yapılamadı",
                "icon": "error"
                //"confirmButtonClass": "btn btn-secondary"
            }).then(result => {
                if (result.value) {

                }

            });
        },
        success: function (data) {
            $('#validateOtp').removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text("Doğrula");

            if (data.code === 200) {
                $('#kt_modal_otp').modal("hide");
                _validateOtp();
            } else {
                swal.fire({
                    "title": "Hata",
                    "text": "Doğrulama Yapılamadı",
                    "icon": "error"
                    //"confirmButtonClass": "btn btn-secondary"
                }).then(result => {
                    if (result.value) {
                        $("#otp").val("");
                    }
                });
            }
        }
    });

});

function sendOtp(pObj, pToken, btn, pValidate, pOtpUrl, pValidateUrl,pToClose) {
    _validateApiUrl = pValidateUrl;
    _validateOtp = pValidate;
    _token = pToken;
    var loTempButtonText = btn.text();
    btn.addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true).text("Lütfen Bekleyiniz");
    $("#validateOtp").data("phone", pObj.phone);
    $("#otp").val("");
    $.ajax({
        method: "post",
        url: pOtpUrl,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(pObj),
        traditional: true,
        crossDomain: true,
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + pToken);
        },
        error: function () {
            btn.removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text(loTempButtonText);
            $('#kt_modal_otp').modal("hide");
            swal.fire({
                "title": "Hata",
                "text": "Mesaj İletilemedi",
                "icon": "error",
                //"confirmButtonClass": "btn btn-secondary"
            }).then(result => {
                if (result.value) {

                }

            });
        },
        success: function (data) {
            if (data.code !== 200) {
                btn.removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text(loTempButtonText);
                swal.fire({
                    "title": "Hata",
                    "text": "Mesaj İletilemedi! " + data.message,
                    "icon": "error",
                    //"confirmButtonClass": "btn btn-secondary"
                }).then(result => {
                    if (result.value) {
                        btn.removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text(loTempButtonText);
                    }
                });
            } else {
                _isOtpSend = true;
                if (pToClose) {
                    pToClose.modal("hide");
                }
                $("#kt_modal_otp").modal("show");
                var x = setInterval(function () {
                    _otpSecond -= 1;
                    $("#secondSms").html("Bir sonraki sms için kalan süre " + _otpSecond + " saniye..");

                    if (_otpSecond < 0) {
                        $("#secondSms").html("");
                        _otpSecond = 180;
                        _isOtpSend = false;
                        btn.removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text("Kaydet");
                        clearInterval(x);
                    }
                }, 1000);
            }
        }
    });
}