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
                    title: 'İşlem Tarihi',
                    width: 240,
                    template: function (row) {
                        var loDate1 = new Date(row.asset_update_date);
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_create_date + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString() + " " + loDate1.toLocaleTimeString() + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'buyer',
                    title: 'Müşteri',
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
                    field: 'İşlemler',
                    title: 'İşlemler',
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