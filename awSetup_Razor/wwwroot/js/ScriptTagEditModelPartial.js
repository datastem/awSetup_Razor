$.changeFormatList = function () {
    //$("#ScriptTag_DataTypeCode").on("change", function () {
    var datatypecode = $('#ScriptTag_DataTypeCode').val();
    $("#ScriptTag_FormatCode").empty();
    //$("#SubCategoryId").append("<option value=''>Select SubCategory</option>");
    $.getJSON('?handler=FormatCodes&datatypecode=' + datatypecode, (data) => {
        $.each(data, function (i, item) {
            $("#ScriptTag_FormatCode").append('<option value="'+ item.value + '">' + item.text + '</option>');
        });
    });
    //});
};