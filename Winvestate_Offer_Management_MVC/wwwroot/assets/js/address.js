var _apiAddress = "";
var _selectedCity = "";
var _selectedDistrict = "";
var _selectedVillage = "";
var _selectedDoor = "";
var _selectedApartment = "";
var _selectedHighway = "";
var _selectedStreet = "";
var _addressType = "";
var _saveAddressUrl = "";
var _saveAddress = "";
var _token = "";
var _button = "";
var _isUpdate;
var _isHeaderActive;
var loAddressObj = {};
var validation;
var onClose;
var _isLand;
var _leaflet;
var _openMap;
var _fullAddress;

jQuery(document).ready(function () {
    $('#city_id').select2({
        placeholder: "İl Seçiniz"
    });

    $('#district_id').select2({
        placeholder: "İlçe Seçiniz"
    });
});

function blockUI() {
    KTApp.block('#newAssetForm',
        {
            overlayColor: '#000000',
            state: 'warning', // a bootstrap color
            size: 'lg' //available custom sizes: sm|lg
        });
}

function unblockUI() {
    KTApp.unblock('#newAssetForm');
}

function resetAddressVariables() {
    loAddressObj = {};
    _isLand = false;
}

function setAddressVariables(customer) {
    loAddressObj = {};
    loAddressObj.select_city = customer.city_id;
    loAddressObj.select_district = customer.district_id;
    loAddressObj.row_guid = customer.row_guid;
    if (loAddressObj.select_city <= 0) {
        $('#city_id').val(null).trigger('change');
    }
    if (loAddressObj.select_district <= 0) {
        $('#district_id').val(null).trigger('change');
    }

}

function setDataToSelect(myDropdown, msg, selectedVal, filter) {
    var loData = msg.data;
    myDropdown.empty();

    if (filter) {
        myDropdown.append(
            $('<option>',
                {
                    value: -1,
                    text: "HEPSİ"
                },
                '</option>'));
    }

    if (loData) {
        for (var i = 0; i < loData.length; i++) {

            myDropdown.append(
                $('<option>',
                    {
                        value: loData[i].value,
                        text: loData[i].text
                    },
                    '</option>'));
        }

        if (loData.length == 1) {
            myDropdown.val(loData[0].value);
            myDropdown.trigger('change');
        } else {
            myDropdown.val(selectedVal);
        }
    }

}

function GetCities(filter) {
    blockUI();
    var dropdownCity = $('#city_id');
    dropdownCity.empty();
    $.ajax({
        type: "GET",
        url: HOST_URL + "/Address/City",
        dataType: "json",
        traditional: true,
        crossDomain: true,
        success: function (msg) {
            unblockUI();
            setDataToSelect(dropdownCity, msg, loAddressObj.select_city, filter);
        }
    });
}

function CallService(dropdown, route, id, parentId) {
    blockUI();
    $.ajax({
        type: "GET",
        url: HOST_URL + "/Address" + route + "?pId=" + parentId,
        dataType: "json",
        traditional: true,
        crossDomain: true,
        success: function (msg) {
            unblockUI();
            setDataToSelect(dropdown, msg, id);
        }
    });
}

function GetDistricts(dropDown) {
    if (loAddressObj.select_city == -1) {
        $('#district_id').empty();;
    } else {
        CallService(dropDown, "/Distrcit", loAddressObj.select_district, loAddressObj.select_city);
    }
        
}

$("#city_id").change(function () {
    var dropdown = $('#district_id');
    var id = $("#city_id").find('option:selected').val();
    loAddressObj.select_city = id;
    if (id) {
        GetDistricts(dropdown);
    }

});

$("#city_id").select2({
    placeholder: "Lütfen seçim yapınız",
});

$("#district_id").select2({
    placeholder: "Lütfen seçim yapınız",
});

