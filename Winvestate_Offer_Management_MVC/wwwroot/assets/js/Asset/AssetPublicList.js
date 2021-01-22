﻿var loMyAssetList;

var AssetForOfferDT = function () {
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
                        url: HOST_URL + '/Asset/OfferList',
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

            search: {
                input: $('#kt_datatable_search_query'),
                delay: 400,
                key: 'generalSearch'
            },

            // columns definition
            columns: [
                {
                    field: 'image',
                    title: '',
                    width: 100,
                    template: function (data) {
                        var number = KTUtil.getRandomInt(9, 14);
                        var loImagePath = "/uploads/"+data.thumb_path.replaceAll("\\","//");
                        var user_img = 'background-image:url(' + loImagePath+')';

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
                }, {
                    field: 'asset_no',
                    title: 'No'
                }, {
                    field: 'city',
                    title: 'İl-İlçe',
                    template: function(row) {
                        var loSize = row.size + "m2";
                        var loAddress = row.city + ' ' + row.district;
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ loAddress + '</div>\
                                                    <div class="text-muted">' + loSize + '</div>\
                                                </div>\
                                            </div>';

                        return output;
                    }
                },
                {
                    field: 'category',
                    title: 'Kategori',
                    template: function (row) {
                        var output = '<div class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.category + '</div>\
                                                    <div class="text-muted">' + row.asset_type + '</div>\
                                                </div>\
                                            </div>';

                        return output;
                    }
                }, {
                    field: 'max_offer',
                    title: 'Son Teklif',
                    template: function (row) {
                        var output = '';
                        output += '<div class="text-dark-75 font-weight-boldest mb-0">' + row.max_offer.toLocaleString("tr") + '</div>';
                        return output;
                    }
                },
                {
                    field: 'detail',
                    title: '',
                    template: function (row) {
                        return '\
                        <a href="/Asset/AssetDetail?pId=' + row.row_guid +'" class="btn btn-xs btn-default btn-text-primary btn-hover-primary mr-2" data-asset-id=' + row.row_guid+'>\
                             Detay\
                        </a>\
	                    ';
                    },
                }],

        });

        $('#city_id').on('change', function () {
            datatable.search($(this).val().toLowerCase(), 'Status');
        });

        $('#district_id').on('change', function () {
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
