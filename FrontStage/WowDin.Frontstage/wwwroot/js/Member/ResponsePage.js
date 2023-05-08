var brandData = null;

$(function () {
    $.ajax({
        url: "/Member/GetBrandData",
        method: "get",
        success: function (res) {
            
            brandData = res;
            var inner = '<option value="-1">---請選擇品牌---</option>';
            for (var i = 0; i < res.length; i++) {
                inner = inner + '<option value="'+ res[i].brandId +'">' + res[i].brandName + '</option>'
            }
            $('#brands').html(inner);
            renew()
        }
    });
})

function renew() {
    var brandID = $('#brands').val();
    var shopData = null;
    for (var i = 0; i < brandData.length; i++) {
        if (brandData[i].brandId == brandID) {
            shopData = brandData[i].shop
        }
        /*inner = inner + '<option>' + brandData[index].shop[i] + '</option>'*/
    }
    var inner = '<option>---請選擇門市---</option>';
    if (shopData != null) {
        for (var i = 0; i < shopData.length; i++) {
            inner = inner + '<option value="'+ shopData[i].shopId +'">' + shopData[i].shopName + '</option>'
        }
    }
    $('#shops').html(inner);
}

function displayMessage() {

    var response = $('#exampleFormControlTextarea1');

    if (response.val() == null || response.val().trim() == "") {
        toastr["warning"]('意見內容未填');
    }
    else {
        $.ajax({
            url: "/Member/AddResponseData",
            method: "post",
            data:
            {
                BrandId: $('#brands').val(),
                ShopId: $('#shops').val(),
                ResponseContent: $('#exampleFormControlTextarea1').val()
            }
        });
        toastr["success"]("成功送出回饋")

        $('#brands').val("");
        $('#shops').val("");
        $('#exampleFormControlTextarea1').val("");
    }
}