var loMyAssetList;
var datatable;

$(".text-uppercase").keyup(function () {
    this.value = this.value.toLocaleUpperCase();
    console.log(this.value);
});

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
                    responsive: {
                        visible: 'md',
                        hidden: 'sm'
                    },
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
                    field: 'city',
                    title: 'No1',
                    autoHide: false,
                    visible: false
                }, {
                    field: 'district',
                    title: 'No2',
                    autoHide: false,
                    visible: false
                }, {
                    field: 'full_asset_no',
                    title: 'asset_no',
                    visible: false,
                    autoHide: false
                },
                {
                    field: 'category',
                    title: 'No3',
                    autoHide: false,
                    visible: false
                }, {
                    field: 'asset_type',
                    title: 'No4',
                    visible: false,
                    autoHide: false
                }, {
                    field: 'my_asset_no',
                    title: 'GM No',
                    width: 80,
                    autoHide: false,
                    template: function (row) {
                        var output = '<a href="/Asset/AssetDetail?pId=' + row.row_guid + '" class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-4">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.company_prefix + row.asset_no + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'my_asset_name',
                    title: 'Başlık',
                    width: 170,
                    autoHide: false,
                    template: function (row) {
                        var output = '<a href="/Asset/AssetDetail?pId=' + row.row_guid + '" class="d-flex align-items-center detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-0">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.asset_name + '</div>\
                                                </div>\
                                            </a>';
                        return output;
                    }
                },
                {
                    field: 'myCity',
                    title: 'İl',
                    width: 100,
                    autoHide: false,
                    responsive: {
                        visible: 'lg',
                        hidden: 'sm'
                    },
                    template: function (row) {
                        var output = '<a href="/Asset/AssetDetail?pId=' + row.row_guid + '" class="d-flex detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-0">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.city + '</div>\
                                                </div>\
                                            </a>';

                        return output;
                    }
                },
                {
                    field: 'myDistrict',
                    title: 'İlçe',
                    width: 100,
                    autoHide: false,
                    responsive: {
                        visible: 'lg',
                        hidden: 'sm'
                    },
                    template: function (row) {
                        var output = '<a href="/Asset/AssetDetail?pId=' + row.row_guid + '" class="d-flex detail" data-id=' + row.row_guid + '>\
                                                <div class="ml-0">\
                                                    <div class="text-dark-75 font-weight-bolder mb-0">'+ row.district + '</div>\
                                                </div>\
                                            </a>';

                        return output;
                    }
                },
                {
                    field: 'category_type',
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
                                                    <div class="text-dark-75">' + row.asset_type + '</div>\
                                                </div>\
                                            </a>';

                        return output;
                    }
                }, {
                    field: 'max_offer',
                    title: 'Fiyat/Son Teklif (TL)',
                    width: 80,
                    autoHide: false,
                    responsive: {
                        visible: 'lg',
                        hidden: 'sm'
                    },
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
            var loTextToSearch = $("#city_id option:selected").val() == -1 ? "" : $("#city_id option:selected").text();
            datatable.search(loTextToSearch, 'city');
        });

        $('#district_id').on('change', function () {
            datatable.search($("#district_id option:selected").text(), 'district');
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

