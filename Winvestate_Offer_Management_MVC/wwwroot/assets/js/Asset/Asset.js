var loMyAssetList = [];
var _selectedAsset;
var _myProcess;
var _assetValidation;
var _assetPictureId = 0;
var _assetId;
var _assetGuid;

function setAssetVariables(pTypes, pSessionId, pSelectedAsset) {
    if (pSelectedAsset) {
        _myProcess = "update";
        _selectedAsset = pSelectedAsset;
    } else {
        _myProcess = "insert";
        _selectedAsset = null;
    }
    
    if (_myProcess == "insert") {
        Dropzone.prototype.defaultOptions.dictDefaultMessage = "Dosyalarınızı yüklemek için buraya taşıyınız";
        Dropzone.prototype.defaultOptions.dictFallbackMessage = "Tarayıcınızı işleme izin vermemektedir";
        Dropzone.prototype.defaultOptions.dictFallbackText = "Lütfen dosyalarınızı yüklerken fallback kullanınız.";
        Dropzone.prototype.defaultOptions.dictFileTooBig = "Dosya çok büyük ({{filesize}}MiB). Maksimum dosya boyutu: {{maxFilesize}}MiB.";
        Dropzone.prototype.defaultOptions.dictInvalidFileType = "Bu dosya tipini yükleyemezsiniz";
        Dropzone.prototype.defaultOptions.dictResponseError = "Hata kodu {{statusCode}}.";
        Dropzone.prototype.defaultOptions.dictCancelUpload = "Yüklemeyi iptal et";
        Dropzone.prototype.defaultOptions.dictUploadCanceled = "Dosya yükleme iptal edildi.";
        Dropzone.prototype.defaultOptions.dictCancelUploadConfirmation = "Dosya yüklemeyi iptal etmek istediğinize emin misiniz?";
        Dropzone.prototype.defaultOptions.dictRemoveFile = "Dosyayı Sil";
        Dropzone.prototype.defaultOptions.dictRemoveFileConfirmation = "Dosyayı silmek istediğinize emin misiniz";
        Dropzone.prototype.defaultOptions.dictMaxFilesExceeded = "Daha fazla dosya yükleyemezsiniz.";
        Dropzone.prototype.defaultOptions.dictFileSizeUnits = "Daha fazla dosya yükleyemezsiniz.";
        _assetPictureId = 0;
    }


    $('#category_type_system_type_id').select2({
        placeholder: "Kategori Seçiniz"
    });

    $('#asset_type_system_type_id').select2({
        placeholder: "Tür Seçiniz"
    });

    $('#agreement_guid').select2({
        placeholder: "Sözleşme Seçiniz"
    });

    $('#bank_guid').select2({
        placeholder: "Kurum Seçiniz"
    });

    $('#first_announcement_date_str').datetimepicker({
        locale: 'tr'
    });


    $('#last_announcement_date_str').datetimepicker({
        locale: 'tr'
    });


    $('#last_offer_date_str').datetimepicker({
        locale: 'de'
    });


    $("#category_type_system_type_id").change(function () {
        var loMyListForPortfolioType;
        var id = $("#category_type_system_type_id").find('option:selected').val();

        if (id == 4) {
            loMyListForPortfolioType = pTypes.portfolio_house;
        } else if (id == 5) {
            loMyListForPortfolioType = pTypes.portfolio_office;
        } else {
            loMyListForPortfolioType = pTypes.portfolio_ground;
        }

        BindCombobox(loMyListForPortfolioType, $("#asset_type_system_type_id"));
    });

    $('#category_type_system_type_id').val(null).trigger("change");
    $('#bank_guid').val(null).trigger("change");
    $('#agreement_guid').val(null).trigger("change");

    $('#dropzone_for_asset_images').dropzone({
        url: "api/File?sessionId=" + pSessionId,
        parallelUploads: 20,
        uploadMultiple: true,
        maxFiles: 20,
        maxFilesize: 10, // MB
        addRemoveLinks: true,
        acceptedFiles: "image/*",
        accept: function (file, done) {
            done();
            _assetPictureId = 1;
        }
    });

    setAssetValidation();

    $("#saveAsset").on('click',
        function (e) {
            e.preventDefault();
            _assetValidation.validate().then(function (status) {
                if (status == 'Valid') {
                    if (_assetPictureId == 0 &&_myProcess == "insert") {
                        Swal.fire({
                            text: "İşleme devam edebilmek için en az 1 fotoğraf yüklemelesiniz.",
                            icon: "error",
                            buttonsStyling: false,
                            confirmButtonText: "Tamam",
                            customClass: {
                                confirmButton: "btn font-weight-bold btn-light"
                            }
                        }).then(function () {
                            KTUtil.scrollTop();
                        });
                    } else {
                        SaveAsset();
                    }
                }
            });
        });
}


function SendAssetToServer(model, btn) {
    var loTempText = btn.text();
    btn.addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true)
        .text("Lütfen Bekleyiniz");
    $.ajax({
        method: "post",
        url: '/Asset/Save',
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
                    text: "İşlem Başarılı!",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Tamam",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    if (_myProcess == "insert") {
                        window.location = '/Asset/List';
                    } else {
                        window.location = '/Asset/Info?pId='+model.row_guid;
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
}

function SaveAsset() {

    var loMyAsset = objectifyForm($("#newAssetForm").serializeArray());
    loMyAsset.city_id = Number(loMyAsset.city_id);
    loMyAsset.district_id = Number(loMyAsset.district_id);
    loMyAsset.city = $("#city_id").find('option:selected').text();
    loMyAsset.district = $("#district_id").find('option:selected').text();
    loMyAsset.category_type_system_type_id = Number(loMyAsset.category_type_system_type_id);
    loMyAsset.asset_type_system_type_id = Number(loMyAsset.asset_type_system_type_id);
    loMyAsset.size = Number(loMyAsset.size);
    loMyAsset.starting_amount = Number(loMyAsset.starting_amount.replaceAll(".", ""));
    loMyAsset.minimum_increate_amout = $("#minimum_increate_amout").val() == null || $("#minimum_increate_amout").val()==="" ? null : Number(loMyAsset.minimum_increate_amout.replaceAll(".", ""));
    loMyAsset.guarantee_amount = $("#guarantee_amount").val() == null || $("#guarantee_amount").val() === "" ? null : Number(loMyAsset.guarantee_amount.replaceAll(".", ""));
    loMyAsset.is_compatible_for_credit =  $("#is_compatible_for_credit").is(':checked');
    loMyAsset.explanation = $("#explanation").val();
    loMyAsset.last_announcement_date_str = $("#last_announcement_date_str").val()+":00";
    loMyAsset.first_announcement_date_str = $("#first_announcement_date_str").val() + ":00";
    loMyAsset.last_offer_date_str = $("#last_offer_date_str").val() + ":00";

    if (_myProcess == "update") {
        loMyAsset.id = _selectedAsset.id;
        loMyAsset.row_guid = _selectedAsset.row_guid;
    }

    //console.log(loMyAsset)

    SendAssetToServer(loMyAsset, $("#saveAsset"));
}

function setAssetValidation() {
    var formEl = KTUtil.getById('newAssetForm');
    _assetValidation = FormValidation.formValidation(
        formEl,
        {
            fields: {
                asset_name: {
                    validators: {
                        notEmpty: {
                            message: 'Gayrimenkul adı girilmeden işleme devam edilemez.'
                        }
                    }
                },
                first_announcement_date_str: {
                    validators: {
                        notEmpty: {
                            message: 'İlan yayınlama başlangıç tarihi seçilmelidir.'
                        }
                    }
                },
                last_announcement_date_str: {
                    validators: {
                        notEmpty: {
                            message: 'İlan yayınlama bitiş tarihi seçilmelidir.'
                        }
                    }
                },
                last_offer_date_str: {
                    validators: {
                        notEmpty: {
                            message: 'Son teklif verme tarihi seçilmelidir.'
                        }
                    }
                },
                block_number: {
                    validators: {
                        notEmpty: {
                            message: 'Ada bilgisi girilmelidir.'
                        }
                    }
                },
                plot_number: {
                    validators: {
                        notEmpty: {
                            message: 'Parsel bilgisi girilmelidir.'
                        }
                    }
                },
                agreement_guid: {
                    validators: {
                        notEmpty: {
                            message: 'Sözleşme seçimi yapılmalıdır.'
                        }
                    }
                },
                bank_guid: {
                    validators: {
                        notEmpty: {
                            message: 'Kurum seçimi yapılmalıdır.'
                        }
                    }
                },
                share: {
                    validators: {
                        notEmpty: {
                            message: 'Hisse bilgisi girilmelidir.'
                        }
                    }
                },
                city_id: {
                    validators: {
                        notEmpty: {
                            message: 'İl seçilmeden işleme devam edilemez.'
                        }
                    }
                },
                district_id: {
                    validators: {
                        notEmpty: {
                            message: 'İlçe seçilmeden işleme devam edilemez işleme devam edilemez.'
                        }
                    }
                },
                category_type_system_type_id: {
                    validators: {
                        notEmpty: {
                            message: 'Gayrimenkul kategorisi seçilmeden işleme devam edilemez.'
                        }
                    }
                },
                asset_type_system_type_id: {
                    validators: {
                        notEmpty: {
                            message: 'Gayrimenkul türü seçilmeden işleme devam edilemez.'
                        }
                    }
                },
                size: {
                    validators: {
                        notEmpty: {
                            message: 'Gayrimenkul boyutu girilmeden işleme devam edilemez..'
                        }
                    }
                },
                address: {
                    validators: {
                        notEmpty: {
                            message: 'Gayrimenkul adresi girilmeden işleme devam edilemez.'
                        }
                    }
                },
                starting_amount: {
                    validators: {
                        notEmpty: {
                            message: 'Başlangıç fiyatı girilmeden işleme devam edilemez.'
                        },
                        callback: {
                            message: 'Lütfen sayısal bir değer giriniz',
                            callback: function (input) {
                                var loResult = input.value.replace(/,/g, '').replaceAll('.', '').replaceAll('₺', '');
                                return loResult.length == 0 || parseFloat(loResult) == loResult;
                            }
                        }
                    }
                },
                guarantee_amount: {
                    validators: {
                        callback: {
                            message: 'Lütfen sayısal bir değer giriniz',
                            callback: function (input) {
                                var loResult = input.value.replace(/,/g, '').replaceAll('.', '').replaceAll('₺', '');
                                return loResult.length == 0 || parseFloat(loResult) == loResult;
                            }
                        }
                    }
                },
                minimum_increate_amout: {
                    validators: {
                        callback: {
                            message: 'Lütfen sayısal bir değer giriniz',
                            callback: function (input) {
                                var loResult = input.value.replace(/,/g, '').replaceAll('.', '').replaceAll('₺', '');
                                return loResult.length == 0 || parseFloat(loResult) == loResult;
                            }
                        }
                    }
                },
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