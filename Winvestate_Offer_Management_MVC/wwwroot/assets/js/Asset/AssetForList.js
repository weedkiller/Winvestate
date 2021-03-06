﻿var loMyAssetList;

$(document).on("click",
    ".deleteAsset",
    function () {
        var loId = $(this).data("id");
        Swal.fire({
            title: "Emin misiniz?",
            text: "Seçtiğiniz gayrimenkul silinecek!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Evet, sil!",
            cancelButtonText: "İptal Et"
        }).then(function (result) {
            if (result.value) {
                var loSelectedAsset = loMyAssetList.find(x => x.row_guid === loId);
                loSelectedAsset.is_deleted = true;
                SendAssetToServer(loSelectedAsset, $(this), true);
            }
        });
    });


var AssetListDT = function () {
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
                        url: HOST_URL + '/Asset/List',
                        // sample custom headers
                        headers: { 'Authorization': 'Bearer ' + HOST_TOKEN },
                        map: function (raw) {
                            //console.log(raw);
                            // sample data mapping
                            if (raw.code === 200) {
                                loMyAssetList = raw.data;
                            } else {
                                loMyAssetList = [];
                            }
                            return loMyAssetList;
                        },
                    },
                },
                pageSize: 10, // display 20 records per page
            },

            // layout definition
            layout: {
                scroll: false, // enable/disable datatable scroll both horizontal and vertical when needed.
                footer: false, // display/hide footer
            },

            // column sorting
            sortable: true,

            pagination: true,

            translate: {
                records: {
                    processing: 'Lütfen bekleyiniz...',
                    noRecords: 'Kayıt bulunamadı..'
                },
                toolbar: {
                    pagination: {
                        items: {
                            default: {
                                first: 'İlk',
                                prev: 'Önceki',
                                next: 'Sonraki',
                                last: 'Son',
                                more: 'Daha fazla sayfa',
                                input: 'Sayfa Sayısı',
                                select: 'Kayıt Sayısı Seçiniz',
                            },
                            info: ' {{start}} - {{end}} arasındaki {{total}} kayıt gösteriliyor',
                        }
                    }
                }
            },

            search: {
                input: $('#kt_datatable_search_query'),
                delay: 400,
                key: 'generalSearch'
            },

            // columns definition
            columns: [
                {
                    field: 'image',
                    title: 'Fotoğraf',
                    width: 100,
                    template: function (data) {
                        var number = KTUtil.getRandomInt(9, 14);
                        var loImagePath = "/uploads/" + data.thumb_path.replaceAll("\\", "//");
                        var user_img = 'background-image:url(' + loImagePath + ')';

                        var output = '';
                        if (number > 8) {
                            output = '<div class="d-flex align-items-center">\
								<div class="symbol symbol-50 symbol-2by3 flex-shrink-0">\
									<div class="symbol-label" style="' + user_img + '"></div>\
								</div>\
							</div>';
                        }

                        return output;
                    },
                },
                {
                    field: 'bank_name',
                    title: 'Kurum Adı',
                },
                {
                    field: '',
                    title: 'Gayrimenkul No',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.company_prefix + row.asset_no + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'city',
                    title: 'İl-İlçe',
                    template: function (row) {
                        var loSize = row.size + "m2";
                        var loAddress = row.city + ' ' + row.district;
                        var output = '<div class="d-flex align-items-left detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loAddress + '</div>\
                                                    <div class="text-dark-75">' + loSize + '</div>\
                                                </div>\
                                            </div>';

                        return output;
                    }
                },
                {
                    field: 'last_offer_date',
                    title: 'Son Teklif Tarihi',
                    width: 240,
                    template: function (row) {
                        var loDate1 = new Date(row.last_offer_date);
                        var loDate2 = new Date(row.last_announcement_date);
                        var loDate3 = new Date(row.first_announcement_date);
                        var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString() + " " + loDate1.toLocaleTimeString() + '</div>\
                                                    <div class="text-dark-75">İlan Başlangıç :' + loDate3.toLocaleDateString() + " " + loDate3.toLocaleTimeString() + '</div>\
                                                    <div class="text-dark-75">İlan Bitiş:' + loDate2.toLocaleDateString() + " " + loDate2.toLocaleTimeString() + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'state',
                    title: 'Durum',
                    template: function (row) {
                        var output = '';
                        var loClass = row.state == "Aktif" ? "text-success" : "text-danger"
                        output += '<div class="font-weight-boldest mb-0 ' + loClass + ' ">' + row.state + '</div>';
                        return output;
                    }
                },
                {
                    field: 'İşlemler',
                    title: 'İşlemler',
                    sortable: false,
                    width: 125,
                    overflow: 'visible',
                    autoHide: false,
                    template: function (row) {
                        return '\
                        <a href="/Asset/Info?pId='+ row.row_guid + '"class="btn btn-sm btn-clean btn-icon mr-2" title="Güncelle">\
                           <i class="flaticon-edit"></i>\
                        </a>\
                        <a href="/Asset/AssetDetail?pId='+ row.row_guid + '" class="btn btn-sm btn-clean btn-icon" title="Görüntüle">\
                             <i class="flaticon-eye"></i>\
                        </a>\
                        <a href="javascript:;" class="btn btn-sm btn-clean btn-icon deleteAsset" data-id=' + row.row_guid + ' title="Sil">\
                             <i class="flaticon2-trash"></i>\
                        </a>\
                    ';
                    },
                }
            ]
        });

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();

    };

    return {
        // public functions
        init: function () {
            $('#state_id').select2({
                placeholder: "Durum Seçiniz"
            });

            demo();
        },
    };
}();

var SoldAssetListDT = function () {
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
                        url: HOST_URL + '/Asset/Sold',
                        // sample custom headers
                        headers: { 'Authorization': 'Bearer ' + HOST_TOKEN },
                        map: function (raw) {
                            //console.log(raw);
                            // sample data mapping
                            if (raw.code === 200) {
                                loMyAssetList = raw.data;
                            } else {
                                loMyAssetList = [];
                            }
                            return loMyAssetList;
                        },
                    },
                },
                pageSize: 10, // display 20 records per page
            },

            // layout definition
            layout: {
                scroll: false, // enable/disable datatable scroll both horizontal and vertical when needed.
                footer: false, // display/hide footer
            },

            // column sorting
            sortable: true,

            pagination: true,

            translate: {
                records: {
                    processing: 'Lütfen bekleyiniz...',
                    noRecords: 'Kayıt bulunamadı..'
                },
                toolbar: {
                    pagination: {
                        items: {
                            default: {
                                first: 'İlk',
                                prev: 'Önceki',
                                next: 'Sonraki',
                                last: 'Son',
                                more: 'Daha fazla sayfa',
                                input: 'Sayfa Sayısı',
                                select: 'Kayıt Sayısı Seçiniz',
                            },
                            info: ' {{start}} - {{end}} arasındaki {{total}} kayıt gösteriliyor',
                        }
                    }
                }
            },

            search: {
                input: $('#kt_datatable_search_query'),
                delay: 400,
                key: 'generalSearch'
            },

            // columns definition
            columns: [
                {
                    field: 'bank_name',
                    title: 'Kurum Adı',
                },
                {
                    field: 'asset_no',
                    title: 'Gayrimenkul No',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.company_prefix + row.asset_no + '</div>\
                                                    <div class="text-muted font-size-xs">' + row.asset_name + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'last_offer_date',
                    title: 'Satış Onay Tarihi',
                    width: 240,
                    template: function (row) {
                        var loDate1 = new Date(row.asset_update_date);
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString() + " " + loDate1.toLocaleTimeString() + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'buyer',
                    title: 'Satın Alan',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-left detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.customer_full_name + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'amount',
                    title: 'Satış Fiyatı',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-left detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.max_offer_amount.toLocaleString("tr") + " TL" + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                }
            ]
        });

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();

    };

    return {
        // public functions
        init: function () {
            $('#state_id').select2({
                placeholder: "Durum Seçiniz"
            });

            demo();
        },
    };
}();

