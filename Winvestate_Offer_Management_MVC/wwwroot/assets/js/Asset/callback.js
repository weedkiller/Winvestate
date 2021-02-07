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
                    field: 'asset_no',
                    title: 'Gayrimenkul No',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.company_prefix + row.asset_no + '</div>\
                                                    <div class="text-muted">' + row.asset_name + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'customer_name_surname',
                    title: 'Müşteri Adı Soyadı',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.applicant_full_name + '</div>\
                                                    <div class="text-muted">' + row.applicant_phone + '</div>\
                                                </div>\
                                            </div>';
                        return output;
                    }
                },
                {
                    field: 'record_date',
                    title: 'Oluşturulma Tarihi',
                    template: function (row) {
                        var loDate1 = new Date(row.row_create_date);
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loDate1.toLocaleDateString() + " " + loDate1.toLocaleTimeString() + '</div>\
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
                        output += '<div class="font-weight-boldest mb-0">' + row.callback_record_state + '</div>';
                        return output;
                    }
                },
                {
                    field: 'note',
                    title: 'Not',
                    template: function (row) {
                        var output = '';
                        var loNote = "";
                        if (row.note) {
                            loNote = row.note;
                        }
                        output += '<div class="font-weight-boldest mb-0">' + loNote + '</div>';
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
