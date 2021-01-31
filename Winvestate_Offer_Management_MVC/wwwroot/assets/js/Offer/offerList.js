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
                        url: HOST_URL + '/Asset/List',
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
                    field: '',
                    title: 'Gayrimenkul No',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.asset_no + '</div>\
                                                    <div class="text-muted">' + row.asset_name + '</div>\
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
                                                    <div class="text-muted">' + loSize + '</div>\
                                                </div>\
                                            </div>';

                        return output;
                    }
                },
                {
                    field: 'max_offer',
                    title: 'Son Teklif',
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
                    title: 'Son Teklif Tarihi',
                    width: 240,
                    template: function (row) {
                        var loDate1 = new Date(row.last_offer_date);
                        var loDate2 = new Date(row.last_announcement_date);
                        var loDate3 = new Date(row.first_announcement_date);
                        var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString() + " " + loDate1.toLocaleTimeString() + '</div>\
                                                    <div class="text-muted">İlan Başlangıç :' + loDate2.toLocaleDateString() + " " + loDate2.toLocaleTimeString() + '</div>\
                                                    <div class="text-muted">İlan Bitiş:' + loDate3.toLocaleDateString() + " " + loDate3.toLocaleTimeString() + '</div>\
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

                sortable: true,

                // columns definition
                columns: [
                    {
                        field: 'last_offer_date',
                        title: ' Teklif Tarihi',
                        width: 150,
                        template: function (row) {
                            var loDate1 = new Date(row.row_create_date);
                            var output = '<div class="d-flex align-items-center">\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString() + " " + loDate1.toLocaleTimeString() + '</div>\
                                                </div>\
                                            </div>';
                            return output;
                        }
                    },
                    {
                        field: 'customer_full_name',
                        title: 'Müşteri Adı',
                        width: 200
                    }, {
                        field: 'customer_phone',
                        title: 'Müşteri Telefonu',
                        width: 150
                    }, {
                        field: 'amount',
                        title: 'Teklif Tutarı',
                        template: function (row) {

                            var loClass = row.amount == e.data.max_offer ? "text-success" : "text-danger";
                            var output = '<div class="d-flex align-items-left detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="font-weight-bolder mb-0 '+ loClass+'">'+ row.amount.toLocaleString("tr") + " TL" + '</div>\
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
