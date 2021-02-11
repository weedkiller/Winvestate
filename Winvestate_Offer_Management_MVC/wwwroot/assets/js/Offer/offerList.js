'use strict';
// Class definition

var OfferListDT = function () {
    // Private functions

    // demo initializer
    var demo = function () {

        var datatable = $('#kt_datatable').KTDatatable({
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        method: 'GET',
                        contentType: 'application/json',
                        url: HOST_URL + '/Asset/Offered',
                        // sample custom headers
                        headers: { 'Authorization': 'Bearer ' + HOST_TOKEN },
                        map: function (raw) {
                            var loMyAssetList;
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
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },

            // layout definition
            layout: {
                scroll: false,
                footer: false,
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

            detail: {
                title: 'Teklifleri Görüntüle',
                content: subTableInit,
            },

            search: {
                input: $('#kt_datatable_search_query'),
                key: 'generalSearch'
            },

            // columns definition
            columns: [
                {
                    field: 'row_guid',
                    title: '',
                    sortable: false,
                    width: 30,
                    textAlign: 'center',
                },
                {
                    field: 'company_name',
                    title: '',
                    width: '100px',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.bank_name + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'asset_no',
                    title: '',
                    width: '50px',
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
                    title: '',
                    width: '100px',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.district + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'district',
                    title: '',
                    width: '100px',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.city + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'max_offer',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        if (row.max_offer_amount) {
                            var output = '<div class="d-flex align-items-left detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.max_offer_amount.toLocaleString("tr") + " TL" + '</div>\
                                                </div>\
                                            </div>';
                            return output;
                        }
                        return "Teklif Bekleniyor";
                    }
                },
                {
                    field: 'last_offer_date',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var loDate1 = new Date(row.last_offer_date);
                        var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString()+ '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'last_offer_hour',
                    title: '',
                    width: '50px',
                    template: function (row) {
                        var loDate1 = new Date(row.last_offer_date);
                        var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleTimeString() + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'state',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var output = '';
                        var loClass = row.state == "Aktif" ? "text-success" : "text-danger"
                        output += '<div class="font-weight-boldest mb-0 ' + loClass + ' ">' + row.state + '</div>';
                        return output;
                    }
                }
            ]
        });

        $('#kt_datatable_search_status').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Status');
        });

        $('#kt_datatable_search_type').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Type');
        });

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();


        function subTableInit(e) {
            $('<div/>').attr('id', 'child_data_ajax_' + e.data.row_guid).appendTo(e.detailCell).KTDatatable({
                data: {
                    type: 'remote',
                    source: {
                        read: {
                            method: 'GET',
                            contentType: 'application/json',
                            url: HOST_URL + '/Offer/List/' + e.data.row_guid,
                            // sample custom headers
                            headers: { 'Authorization': 'Bearer ' + HOST_TOKEN },
                            map: function (raw) {
                                var loMyOfferList;
                                //console.log(raw);
                                // sample data mapping
                                if (raw.code === 200) {
                                    loMyOfferList = raw.data;
                                } else {
                                    loMyOfferList = [];
                                }
                                return loMyOfferList;
                            },
                        },
                    },
                    pageSize: 5,
                    serverPaging: true,
                    serverFiltering: false,
                    serverSorting: true,
                },

                // layout definition
                layout: {
                    scroll: false,
                    footer: false,

                    // enable/disable datatable spinner.
                    spinner: {
                        type: 1,
                        theme: 'default',
                    },
                },

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

                sortable: true,

                // columns definition
                columns: [
                    {
                        field: 'customer_full_name',
                        title: '',
                        width: 200,
                        template: function (row) {
                            var loDate1 = new Date(row.row_create_date);
                            var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.customer_full_name + '</div>\
                                                </div>\
                                            </div>';
                            return output;
                        }
                    }, {
                        field: 'customer_phone',
                        title: '',
                        width: 150,
                        template: function (row) {
                            var loDate1 = new Date(row.row_create_date);
                            var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.customer_phone + '</div>\
                                                </div>\
                                            </div>';
                            return output;
                        }
                    }, {
                        field: 'amount',
                        title: '',
                        template: function (row) {

                            var loClass = row.amount == e.data.max_offer ? "text-success" : "text-danger";
                            var output = '<div class="d-flex align-items-left detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="font-weight-bolder mb-0 '+ loClass + '">' + row.amount.toLocaleString("tr") + " TL" + '</div>\
                                                </div>\
                                            </div>';
                            return output;
                        }
                    },
                    {
                        field: 'last_offer_date',
                        title: ' ',
                        width: 150,
                        template: function (row) {
                            var loDate1 = new Date(row.row_create_date);
                            var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString()+ '</div>\
                                                </div>\
                                            </div>';
                            return output;
                        }
                    },
                    {
                        field: 'last_offer_time',
                        title: ' ',
                        width: 150,
                        template: function (row) {
                            var loDate1 = new Date(row.row_create_date);
                            var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleTimeString() + '</div>\
                                                </div>\
                                            </div>';
                            return output;
                        }
                    },
                ],
            });
        }
    };

    return {
        // Public functions
        init: function () {
            // init dmeo
            demo();
        },
    };
}();

jQuery(document).ready(function () {
    OfferListDT.init();
});
