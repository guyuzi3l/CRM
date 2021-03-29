var specialKeys = new Array();
specialKeys.push(8); //Backspace
$(function () {
    $(".numeric").bind("keypress", function (e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1 || keyCode == 46 || keyCode == 32);
        return ret;
    });

    $(".letters").bind("keypress", function (e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || specialKeys.indexOf(keyCode) != -1 || keyCode == 32);
        return ret;
    });

    $(".letters-only").bind("keypress", function (e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || specialKeys.indexOf(keyCode) != -1 || keyCode == 32);
        return ret;
    });


    $(".emailChar").bind("keypress", function (e) {
        var keyCode = e.which ? e.which : e.keyCode
        var invalid = (keyCode == 38 || keyCode == 61);
        if (invalid) {
            e.preventDefault();
        }
    });

    $(".letters-only").bind('paste input', removeAlphaChars);
    $(".letters-only").bind('drop input', removeAlphaChars);
    $(".letters").bind('paste input', removeAlphaChars);
    $(".letters").bind('drop input', removeAlphaChars);
    $(".numeric").bind('paste input', removeAlphaCharsForNum);
    $(".numeric").bind('drop input', removeAlphaCharsForNum);
});

function removeAlphaChars(e) {
    var self = $(this);
    setTimeout(function () {
        var initVal = self.val(),
            outputVal = initVal.replace(/[^A-Za-z0-9 .@]/gi, "");
        if (initVal != outputVal) self.val(outputVal);
    });
}

function removeAlphaCharsForNum(e) {
    var self = $(this);
    setTimeout(function () {
        var initVal = self.val(),
            outputVal = initVal.replace(/[^0-9.]/gi, "");
        if (initVal != outputVal) self.val(outputVal);
    });
}