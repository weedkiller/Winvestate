var _callbackList;
var CallbackList = function () {
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
                        url: HOST_URL + '/Customer/Callback',
                        // sample custom headers
                        headers: { 'Authorization': 'Bearer ' + HOST_TOKEN },
                        map: function (raw) {
                            //console.log(raw);
                            // sample data mapping
                            if (raw.code === 200) {
                                _callbackList = raw.data;
                            } else {
                                _callbackList = [];
                            }
                            return _callbackList;
                        },
                    },
                },
                pageSize: 10, // display 20 records per page
            },

            // layout definition
            layout: {
                scroll: true, // enable/disable datatable scroll both horizontal and vertical when needed.
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
                    field: 'asset_no',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var output = '<a href=/Asset/AssetDetail?pId='+row.asset_uuid+' class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.company_prefix + row.asset_no + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'customer_name_surname',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var output = '<a href=/Asset/AssetDetail?pId=' + row.asset_uuid + ' class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.applicant_full_name + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'customer_phone',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var output = '<a href=/Asset/AssetDetail?pId=' + row.asset_uuid + ' class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.applicant_phone + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'record_date',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var loDate1 = new Date(row.row_create_date);
                        var output = '<a href=/Asset/AssetDetail?pId=' + row.asset_uuid + ' class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString()+ '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'record_time',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var loDate1 = new Date(row.row_create_date);
                        var output = '<a href=/Asset/AssetDetail?pId=' + row.asset_uuid + ' class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+loDate1.toLocaleTimeString() + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'state',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var output = '<a href=/Asset/AssetDetail?pId=' + row.asset_uuid + ' class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.callback_record_state + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'note',
                    title: '',
                    width: 'auto',
                    template: function (row) {
                        var loNote = "";
                        if (row.note) {
                            loNote = row.note;
                        }
                        var output = '<a href=/Asset/AssetDetail?pId=' + row.asset_uuid + ' class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loNote + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
            ]
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

jQuery(document).ready(function () {
    CallbackList.init();
});
