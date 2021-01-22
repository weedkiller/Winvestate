var bankValidation;
var loMyBankList = [];
var loSelectedBank;
var loMyProcess;

$(document).ready(function () {
    setBankValidation();

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
        function () {
            var loId = $(this).data("id");
            loSelectedBank = loMyBankList.find(x => x.row_guid === loId);
            loSelectedBank.is_deleted = true;
            SendBankToServer(loSelectedBank, $(this));
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
                $("#bank_name").val(loSelectedBank.name);
            } else {
                $("#bank_name").val("");
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
    } 


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
                    field: 'name',
                    title: 'Adı',
                    width: 'auto'
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