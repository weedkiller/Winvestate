var _signedOfferList = {};
var SignedOfferListDT = function () {
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
                        url: HOST_URL + '/Offer/Summary',
                        // sample custom headers
                        headers: { 'Authorization': 'Bearer ' + HOST_TOKEN },
                        map: function (raw) {
                            //console.log(raw);
                            // sample data mapping
                            if (raw.code === 200) {
                                _signedOfferList = raw.data;
                            } else {
                                _signedOfferList = [];
                            }
                            return _signedOfferList;
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
                    title: '',
                    template: function (row) {
                        var output = '<div class="d-flex  detail" data-id=' + row.row_create_date + '>\
                                                <div class="">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.bank_name + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'asset_no',
                    title: '',
                    width:75,
                    template: function (row) {
                        var output = '<div class="d-flex detail" data-id=' + row.row_guid + '>\
                                                <div class="">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.company_prefix + row.asset_no + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'city',
                    title: '',
                    width:120,
                    template: function (row) {
                        var output = '<div class="d-flex detail" data-id=' + row.row_guid + '>\
                                                <div class="">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.city + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'district',
                    title: '',
                    template: function (row) {
                        var output = '<div class="d-flex detail" data-id=' + row.district + '>\
                                                <div class="">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.district + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'buyer',
                    title: '',
                    template: function (row) {
                        var output = '<div class="d-flex detail" data-id=' + row.row_guid + '>\
                                                <div class="">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.customer_full_name + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'buyer_phone',
                    title: '',
                    template: function (row) {
                        var output = '<div class="d-flex detail" data-id=' + row.row_guid + '>\
                                                <div class="">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.customer_phone + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'last_offer_date',
                    title: '',
                    width: 70,
                    template: function (row) {
                        var loDate1 = new Date(row.asset_update_date);
                        var output = '<div class="d-flex  detail" data-id=' + row.row_create_date + '>\
                                                <div class="">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString() + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'last_offer_time',
                    title: '',
                    width: 70,
                    template: function (row) {
                        var loDate1 = new Date(row.asset_update_date);
                        var output = '<div class="d-flex  detail" data-id=' + row.row_create_date + '>\
                                                <div class="">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleTimeString() + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'İşlemler',
                    title: '',
                    sortable: false,
                    width: 125,
                    overflow: 'visible',
                    autoHide: false,
                    template: function (row) {
                        return '\
                        <a href="/Offer/DownloadDocument?pId='+ row.mespact_session_uuid + '"class="btn btn-sm btn-clean btn-icon mr-2" title="İndir">\
                           <i class="flaticon-download"></i>\
                        </a>\
                        <a href="https://panel.esozlesme.com.tr/Document/showDocument?id='+ row.mespact_session_uuid + '" class="btn btn-sm btn-clean btn-icon" title="Görüntüle" target="_blank">\
                             <i class="flaticon-eye"></i>\
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