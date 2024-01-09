var Codes = [];

function ChangeSelection() {
    var datas = {
        Codes: $('#SltCodes').val(),
    };
    $('#myHouse').val("");
    $.ajax({
        type: 'POST',
        url: '/MiscellaneousSearch/CodeSelectionChanged',
        datatype: "json",
        data: datas,
        success: function (d) {
            if (d.Status) { 
                AddToComboBox(d.Data);
            } 
        },
        error: function (ex) { 
        }
    });
}

function AddToComboBox(Data) {
    $('#MagicHouses').html("");
    var rows = "";
    $.each(Data, function (i, item) {
        rows += "<option>" + item + "</option>";
    });

    $('#MagicHouses').html(rows);
}

function FetchAllApplicationData() {
    AppValidationError(false, "");

    if ($('#txtFromDate').val().length === 0 || $('#txtToDate').val().length === 0) {
        AppValidationError(false, "Dates are mandatory..!!");
        return;
    }

    var datas = {
        FromDate: $('#txtFromDate').val(),
        ToDate: $('#txtToDate').val(),
        Domain: $('#SltCodes').val() == 1 ? $('#myHouse').val() : null,
        TcName: $('#SltCodes').val() == 2 ? $('#myHouse').val() : null,
        Modile: $('#OptOther').val() == "Mobile" ? $('#txtFreeTxt').val() : null,
        FreeZoneCode: $('#OptOther').val() == "FreeZone Code" ? $('#txtFreeTxt').val() : null,
    };

    $('#tb_ApplicantDtls').hide();
    $('#AppBuffer').show(); 
    $.ajax({
        type: 'POST',
        url: '/MiscellaneousSearch/FetchAllCustomerCode',
        datatype: "json",
        data: datas,
        success: function (d) {
            if (d.Status) {
                AddToDetailsTable(d.Data);
            }
            else {
                $('#tb_ApplicantDtls').show();
                $('#AppBuffer').hide();
                AppValidationError(d.Status, d.ErrorString)
            }
        },
        error: function (ex) {
        }
    });
}

function AddToDetailsTable(Data) { 
    $('#tb_ApplicantDtls tbody').html(""); 
    var rows = "";
    $.each(Data, function (i, item) {
        rows += "<tr><td>" + item.ClinicName + "</td><td>" + item.EntryDate + "</td><td>" + item.ApplicantName + "</td><td>" + item.Mobile + "</td><td>" + item.Email + "</td><td>" + item.Country + "</td><td>" + item.TcName + "</td><td>" + item.TcMobile + "</td><td>" + item.TcEmail + "</td><td>" + item.ProfessionName + "</td><td>" + item.HeardAbout + "</td><td>" + item.FreezoneCode + "</td><td>" + item.BscCode + "</td><td>" + item.TcCode + "</td></tr>";
    });
    $('#tb_ApplicantDtls tbody').html(rows);
    $('#tb_ApplicantDtls').show();
    $('#AppBuffer').hide();
}

function AppValidationError(status, errorString) {
    if (status == true) {

        let lab = document.getElementById("AppErrorField"); // access the button by id
        lab.style.color = 'Green';

        $('#AppErrorField').text(errorString);
    }
    else {

        let lab = document.getElementById("AppErrorField"); // access the button by id
        lab.style.color = 'Red';
        $('#AppErrorField').text(errorString);
    }
}