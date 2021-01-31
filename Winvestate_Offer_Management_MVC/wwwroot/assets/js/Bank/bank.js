var bankValidation;
var loMyBankList = [];
var loSelectedBank;
var loMyProcess;

$(document).ready(function () {
    setBankValidation();

    $('#mespact_agreement_uuid').select2({
        placeholder: "Sözleşme Seçiniz"
    });

    $('#mespact_agreement_uuid').val(null).trigger("change");

    $(".addBank").on('click',
        function (e) {
            e.preventDefault();
            $("#kt_modal_add_bank").modal("show");
        });

    $("#saveBank").on('click',
        function (e) {
            e.preventDefault();
            bankValidation.validate().then(function (status) {
                if (status == 'Valid') {
                    SaveBank();
                }
            });
        });

    $(document).on("click",
        ".deleteBank",
        function (e) {
            e.preventDefault();
            var loId = $(this).data("id");

            Swal.fire({
                title: "Emin misiniz?",
                text: "Seçtiğiniz kurum silinecek!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Evet, sil!",
                cancelButtonText: "İptal Et"
            }).then(function (result) {
                if (result.value) {
                    loSelectedBank = loMyBankList.find(x => x.row_guid === loId);
                    loSelectedBank.is_deleted = true;
                    SendBankToServer(loSelectedBank, $(this));
                }
            });
           
        });

    $(document).on("click",
        "#togglePassword",
        function (e) {
            e.preventDefault();
            if ($("#authorized_password").hasClass("password")) {
                $("#authorized_password").removeClass("password");
                $("#authorized_password").addClass("text");
                $("#authorized_password").attr('type', 'text');
            } else {
                $("#authorized_password").addClass("password");
                $("#authorized_password").removeClass("text");
                $("#authorized_password").attr('type', 'password');
            }
        });

    $(document).on("click",
        ".editBank",
        function () {
            var loId = $(this).data("id");
            loSelectedBank = loMyBankList.find(x => x.row_guid === loId);
            loMyProcess = "update";
            $("#kt_modal_add_bank").modal("show");


        });

    $('#kt_modal_add_bank').on('hidden.bs.modal',
        function (e) {
            loMyProcess = "insert";
        });

    $('#kt_modal_add_bank').on('show.bs.modal',
        function (e) {
            if (loMyProcess === "update") {
                $("#name").val(loSelectedBank.name);
                $("#authorized_name").val(loSelectedBank.authorized_name);
                $("#authorized_surname").val(loSelectedBank.authorized_surname);
                $("#authorized_phone").val(loSelectedBank.authorized_phone);
                $("#authorized_mail").val(loSelectedBank.authorized_mail);
                $('#mespact_agreement_uuid').val(loSelectedBank.mespact_agreement_uuid).trigger("change");
                $("#authorized_password").val(loSelectedBank.authorized_password);
                $("#company_prefix").val(loSelectedBank.company_prefix);
                $('input[name="sale_in_company"]').val(loSelectedBank.sale_in_company);
            } else {
                $("#name").val("");
                $("#authorized_name").val("");
                $("#authorized_surname").val("");
                $("#authorized_phone").val("");
                $("#authorized_mail").val("");
                $("#authorized_password").val("");
                $("#company_prefix").val("");
            }
        });
});

function SendBankToServer(model, btn) {
    var loTempText = btn.text();
    btn.addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true)
        .text("Lütfen Bekleyiniz");
    $.ajax({
        method: "post",
        url: '/Bank/Save',
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
                    $("#kt_modal_add_bank").modal("hide");
                    window.location='/Bank/List';
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

function SaveBank() {

    var loMyBank = objectifyForm($("#newBankForm").serializeArray());

    if (loMyProcess == "update") {
        loMyBank.id = loSelectedBank.id;
        loMyBank.row_guid = loSelectedBank.row_guid;

        if (loSelectedBank.password != $("#authorized_password").val()) {
            loMyBank.authorized_password = MD5($("#authorized_password").val());
        }
    } else {
        loMyBank.authorized_password = MD5($("#authorized_password").val());
    }

    loMyBank.sale_in_company = $('input[name="sale_in_company"]:checked').val() == 'true';
    SendBankToServer(loMyBank, $("#saveBank"));
}

function setBankValidation() {
    var formEl = KTUtil.getById('newBankForm');
    bankValidation = FormValidation.formValidation(
        formEl,
        {
            fields: {
                name: {
                    validators: {
                        notEmpty: {
                            message: 'İsim girilmeden işleme devam edilemez.'
                        }
                    }
                },
                mespact_agreement_uuid: {
                    validators: {
                        notEmpty: {
                            message: 'Sözleşme seçilmeden işleme devam edilemez.'
                        }
                    }
                },
                authorized_name: {
                    validators: {
                        notEmpty: {
                            message: 'Yetkili adı girilmeden işleme devam edilemez.'
                        }
                    }
                },
                authorized_surname: {
                    validators: {
                        notEmpty: {
                            message: 'Yetkili soyadı girilmeden işleme devam edilemez.'
                        }
                    }
                },
                authorized_mail: {
                    validators: {
                        notEmpty: {
                            message: 'Yetkili mail adresi girilmeden işleme devam edilemez.'
                        }
                    }
                },
                authorized_password: {
                    validators: {
                        notEmpty: {
                            message: 'Yetkili şifresi girilmeden işleme devam edilemez.'
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

var KTDatatableBank= function () {
    // Private functions

    // basic demo
    var demo = function () {
        var datatable = $('#kt_datatable').KTDatatable({
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        method: 'GET',
                        contentType: 'application/json',
                        url: HOST_URL + '/Bank',
                        // sample custom headers
                        headers: { 'Authorization': 'Bearer ' + HOST_TOKEN },
                        map: function (raw) {
                            //console.log(raw);
                            // sample data mapping
                            if (raw.code === 200) {
                                loMyBankList = raw.data;
                            } else {
                                loMyBankList = [];
                            }
                            return loMyBankList;
                        },
                    },
                },
                saveState: false
            },

            layout: {
                scroll: false
            },

            // column sorting
            sortable: true,

            pagination: true,

            search: {
                input: $('#kt_datatable_search_query'),
                key: 'generalSearch'
            },

            // columns definition
            columns: [
                {
                    field: 'bank_name',
                    title: 'Adı',
                    width: '250'
                },
                {
                    field: 'name_aut',
                    title: 'Yetkili Kişi',
                    width: 'auto',
                    template: function (row) {
                        return row.authorized_name + " " + row.authorized_surname;
                    }
                },
                {
                    field: 'surname_aut',
                    title: 'Yetkili İletişim',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.authorized_phone + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.authorized_phone + '</div>\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.authorized_second_phone + '-' + row.authorized_dial_code+'</div>\
                                                    <div class="text-muted">' + row.authorized_mail + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'Actions',
                    title: 'İşlemler',
                    sortable: false,
                    width: "auto",
                    overflow: 'visible',
                    autoHide: false,
                    template: function (row) {
                        return '\
										 <button data-id=' + row.row_guid + ' class="btn btn-xs btn-clean btn-icon mr-2 editBank" title="Güncelle">\
											<span class="navi-icon"><i class="flaticon2-edit text-primary"></i></span>\
										</a>\
                                        <button data-id=' + row.row_guid + ' class="btn btn-xs btn-clean btn-icon mr-2 deleteBank" title="Sil">\
											<span class="navi-icon"><i class="flaticon2-trash  text-danger"></i></span>\
										</button>\
									';
                    },
                }
            ],

        });

        $('#kt_datatable_search_status').on('change',
            function () {
                datatable.search($(this).val().toLowerCase(), 'Status');
            });

        $('#kt_datatable_search_type').on('change',
            function () {
                datatable.search($(this).val().toLowerCase(), 'Type');
            });

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();
    };

    return {
        // public functions
        init: function () {
            demo();
        },
    };
}();