function TwilioPhoneListRefresh() {
    var customerid = $('#CustomerPhone_CustomerId').val();
    var phone = $('#PrimaryPhone').val();
    var miles = $('#Miles').val();
    $("#CustomerPhone_TwilioPhoneNumber").empty();
    $.getJSON('?handler=TwilioPhoneListRefresh&customerid=' + customerid + '&phone=' + phone + '&miles=' + miles, (data) => {
        $.each(data, function (i, item) {
            $("#CustomerPhone_TwilioPhoneNumber").append('<option value="' + item.value + '">' + item.text + '</option>');
        });
    });
}
