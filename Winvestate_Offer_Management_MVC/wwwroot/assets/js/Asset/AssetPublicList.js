var loMyAssetList;
var datatable

var AssetForOfferDT = function () {
    // Private functions

    // basic demo
    var demo = function () {

        datatable = $('#kt_datatable').KTDatatable({
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
                                console.log(loMyAssetList);
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
            serverFiltering: false,
            serverSorting: false,


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
                key: 'generalSearch',
                onEnter: false
            },

            // columns definition
            columns: [
                {
                    field: 'image',
                    title: 'Fotoğraf',
                    width: 80,
                    autoHide: false,
                    template: function (data) {
                        var number = KTUtil.getRandomInt(9, 14);
                        var loImagePath = "/uploads/" + data.thumb_path.replaceAll("\\", "//");
                        var user_img = 'background-image:url(' + loImagePath + ')';

                        var output = '';
                        if (number > 8) {
                            output = '<a href="/Asset/AssetDetail?pId=' + data.row_guid + '" class="d-flex align-items-center">\
								<div class="symbol symbol-50 symbol-2by3 flex-shrink-0">\
									<div class="symbol-label" style="' + user_img + '"></div>\
								</div>\
							</a>';
                        }

                        return output;
                    },
                }, {
                    field: 'city_id',
                    title: 'No1',
                    visible: false,
                    autoHide: false
                }, {
                    field: 'district_id',
                    title: 'No2',
                    visible: false,
                    autoHide: false
                }, {
                    field: 'full_asset_no',
                    title: 'asset_no',
                    visible: false,
                    autoHide: false
                }, {
                    field: '',
                    title: 'Gayrimenkul No',
                    width: 170,
                    autoHide: false,
                    responsive: {
                        visible: 'lg',
                        hidden: 'sm'
                    },
                    template: function (row) {
                        var output = '<a href="/Asset/AssetDetail?pId=' + row.row_guid + '" class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.company_prefix + row.asset_no + '</div>\
                                                    <div class="text-muted font-size-xs">' + row.asset_name + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'city',
                    title: 'İl-İlçe',
                    width:150,
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
                    field: 'category',
                    title: 'Kategori',
                    width: 100,
                    responsive: {
                        visible: 'lg',
                        hidden: 'md'
                    },
                    autoHide: false,
                    template: function (row) {
                        var output = '<a href="/Asset/AssetDetail?pId=' + row.row_guid + '" class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.category + '</div>\
                                                    <div class="text-muted font-size-xs">' + row.asset_type + '</div>\
                                                </div>\
                                            </a>';

                        return output;
                    }
                }, {
                    field: 'max_offer',
                    title: 'Son Teklif (TL)',
                    width: 150,
                    autoHide: false,
                    template: function (row) {
                        var output = '';
                        var loAmount = row.max_offer;
                        if (parseFloat(row.max_offer) == row.max_offer) {
                            loAmount = parseFloat(row.max_offer).toLocaleString();
                        }

                        output += '<a href="/Asset/AssetDetail?pId=' + row.row_guid + '" class="text-dark-75 font-weight-boldest mb-0">' + loAmount + '</a>';
                        return output;
                    }
                },
                {
                    field: 'detail',
                    title: '',
                    overflow: 'visible',
                    autoHide: false,
                    width: 50,
                    template: function (row) {
                        return '\
                        <a href="/Asset/AssetDetail?pId=' + row.row_guid + '" class="btn btn-xs btn-default btn-text-primary btn-hover-primary mr-2" data-asset-id=' + row.row_guid + '>\
                             Detay\
                        </a>\
	                    ';
                    },
                }],

        });

        $('#city_id').on('change', function () {
            if ($("#city_id option:selected").val() == -1) {
                window.location.reload();
            } else {
                datatable.search($("#city_id option:selected").val(), 'city_id');
            }

        });

        $('#district_id').on('change', function () {
            datatable.search($("#district_id option:selected").val(), 'district_id');
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

