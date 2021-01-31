var userValidation;
var loMyUserList = [];
var loSelectedUser;
var loMyProcess;

$(document).ready(function () {
    setUserValidation();

    $(".addUser").on('click',
        function (e) {
            e.preventDefault();
            $("#kt_modal_add_user").modal("show");
        });

    $("#saveUser").on('click',
        function (e) {
            e.preventDefault();
            userValidation.validate().then(function (status) {
                if (status == 'Valid') {
                    SaveUser();
                }
            });
        });

    $(document).on("click",
        ".deleteUser",
        function () {
            var loId = $(this).data("id");

            Swal.fire({
                title: "Emin misiniz?",
                text: "Seçtiğiniz kullanıcı silinecek!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Evet, sil!",
                cancelButtonText: "İptal Et"
            }).then(function (result) {
                if (result.value) {
                    loSelectedUser = loMyUserList.find(x => x.row_guid === loId);
                    loSelectedUser.is_deleted = true;
                    SendUserToServer(loSelectedUser, $(this));
                }
            });

          
        });

    $(document).on("click",
        ".editUser",
        function () {
            var loId = $(this).data("id");
            loSelectedUser = loMyUserList.find(x => x.row_guid === loId);
            loMyProcess = "update";
            $("#kt_modal_add_user").modal("show");


        });

    $(document).on("click",
        ".forgotPassword",
        function () {
            var loId = $(this).data("id");
            loSelectedUser = loMyUserList.find(x => x.row_guid === loId);
            $("#ModalForUpdatePassword").modal("show");


        });

    $('#kt_modal_add_user').on('hidden.bs.modal',
        function (e) {
            loMyProcess = "insert";
        });

    $('#kt_modal_add_user').on('show.bs.modal',
        function (e) {
            if (loMyProcess === "update") {
                $("#name").val(loSelectedUser.name);
                $("#surname").val(loSelectedUser.surname);
                $("#mail").val(loSelectedUser.mail);
                $("#phone").val(loSelectedUser.phone);
                $("#password").val(loSelectedUser.password);
                if (loSelectedUser.user_type == 1) {
                    $("#authorize_full").attr('checked', 'checked');
                    $("#authorize_standart").removeAttr('checked', 'checked');
                } else {
                    $("#authorize_standart").attr('checked', 'checked');
                    $("#authorize_full").removeAttr('checked', 'checked');
                }

            } else {
                $("#name").val("");
                $("#surname").val("");
                $("#mail").val("");
                $("#phone").val("");
                $("#password").val("");
            }
        });

    $('#updatePassword').on('click', function (e) {
        e.preventDefault();

        var loPasswordToUpdate = $("#passwordToUpdate").val();

        if (loPasswordToUpdate.length < 8) {
            swal.fire({
                "title": "Hata",
                "text": "Şifreniz en az 8 karakter olmalıdır.",
                "icon": "error",
                //"confirmButtonClass": "btn btn-secondary"
            }).then(result => {
                if (result.value) {

                }
            });
        }

        else if (loPasswordToUpdate != $("#passwordToUpdate2").val()) {
            swal.fire({
                "title": "Hata",
                "text": "Girmiş olduğunuz Şifreler Uyuşmamaktadır.",
                "icon": "error",
                //"confirmButtonClass": "btn btn-secondary"
            }).then(result => {
                if (result.value) {

                }
            });
        } else {
            var model = {};
            model.password = MD5(loPasswordToUpdate);
            model.phone = loSelectedUser.phone;
            UpdateUserPassword(model);
        }

    });
});

function UpdateUserPassword(model) {
    $('#updatePassword').addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true).text("Kaydet");
    $.ajax({
        method: "put",
        url: HOST_URL+'/User/Password',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(model),
        traditional: true,
        crossDomain: true,
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer '+HOST_TOKEN);
        },
        error: function () {
            $('#updatePassword').removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text("Kaydet");
            swal.fire({
                "title": "Hata",
                "text": "İşlem Tamamlanamadı",
                "icon": "error",
                //"confirmButtonClass": "btn btn-secondary"
            }).then(result => {
                if (result.value) {

                }
            });
        },
        success: function (data) {
            $('#updatePassword').removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false).text("Kaydet");
            if (data.code !== 200) {
                swal.fire({
                    "title": "Hata",
                    "text": "Şifre Kaydedilemedi. " + data.message,
                    "icon": "error",
                    //"confirmButtonClass": "btn btn-secondary"
                }).then(result => {
                    if (result.value) {
                    }

                });
            } else {
                swal.fire({
                    "title": "Başarılı",
                    "text": "Şifre başarıyla güncellendi.",
                    "icon": "success",
                    //"confirmButtonClass": "btn btn-secondary"
                }).then(result => {
                    if (result.value) {
                        $(".modal").modal("hide");
                    }

                });
            }
        }
    });
}

function SendUserToServer(model, btn) {
    var loTempText = btn.text();
    btn.addClass('spinner spinner-right spinner-white pr-15').attr('disabled', true)
        .text("Lütfen Bekleyiniz");
    $.ajax({
        method: "post",
        url: '/Account/Save',
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(model),
        error: function (xhr) {
            btn.removeClass('spinner spinner-right spinner-white pr-15').attr('disabled', false)
                .text(loTempText);
            console.log(xhr);
            swal.fire({
                text: "Kullanıcı işlemi esnasında bir problem oluştu. Lütfen tekrar deneyiniz.",
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
                    $("#kt_modal_add_user").modal("hide");
                    window.location = '/Account/List';
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

function SaveUser() {

    var loMyUser = objectifyForm($("#newUserForm").serializeArray());

    if (loMyProcess == "update") {
        if (loSelectedUser.password != $("#password").val()) {
            loMyUser.password = MD5($("#password").val());
        }
        loMyUser.id = loSelectedUser.id;
        loMyUser.row_guid = loSelectedUser.row_guid;
    } else {
        loMyUser.password = MD5($("#password").val());
    }

    loMyUser.user_type = Number($('input[name="user_type"]:checked').val());

    SendUserToServer(loMyUser, $("#saveUser"));
}

function setUserValidation() {
    var formEl = KTUtil.getById('newUserForm');
    userValidation = FormValidation.formValidation(
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
                surname: {
                    validators: {
                        notEmpty: {
                            message: 'Soyisim girilmeden işleme devam edilemez.'
                        }
                    }
                },
                mail: {
                    validators: {
                        notEmpty: {
                            message: 'Mail adresi girilmeden işleme devam edilemez.'
                        }
                    }
                },
                phone: {
                    validators: {
                        notEmpty: {
                            message: 'Telefon numarası girilmeden işleme devam edilemez.'
                        }
                    }
                },
                user_type: {
                    validators: {
                        notEmpty: {
                            message: 'Kullanıcı tipi seçilmelidir'
                        }
                    }
                },
                password: {
                    validators: {
                        notEmpty: {
                            message: 'Şifre girilmeden işleme devam edilemez.'
                        },
                        stringLength: {
                            min: 8,
                            message: 'Şifreniz en az 8 karakter olmalıdır'
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

var KTDatatableAutoColumnHideDemo = function () {
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
                        url: HOST_URL + '/User',
                        // sample custom headers
                        headers: { 'Authorization': 'Bearer ' + HOST_TOKEN },
                        map: function (raw) {
                            //console.log(raw);
                            // sample data mapping
                            if (raw.code === 200) {
                                loMyUserList = raw.data;
                            } else {
                                loMyUserList = [];
                            }
                            return loMyUserList;
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
                    field: 'surname',
                    title: 'Soyadı',
                    width: 'auto'
                },
                {
                    field: 'phone',
                    title: 'Telefon',
                    width: 'auto'
                },
                {
                    field: 'mail',
                    title: 'Mail',
                    width: '300'
                }, {
                    field: 'Actions',
                    title: 'İşlemler',
                    sortable: false,
                    width: "auto",
                    overflow: 'visible',
                    autoHide: false,
                    template: function (row) {
                        return '\
										 <button data-id=' + row.row_guid + ' class="btn btn-xs btn-clean btn-icon mr-2 editUser" title="Güncelle">\
											<span class="navi-icon"><i class="flaticon2-edit text-primary"></i></span>\
										</button>\
                                        <button data-id=' + row.row_guid + ' class="btn btn-xs btn-clean btn-icon mr-2 deleteUser" title="Sil">\
											<span class="navi-icon"><i class="flaticon2-trash  text-danger"></i></span>\
										</button>\
                                        <button data-id=' + row.row_guid + ' class="btn btn-xs btn-clean btn-icon mr-2 forgotPassword" title="Şifreyi Sıfırla">\
											<span class="navi-icon"><i class="flaticon-questions-circular-button  text-danger"></i></span>\
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