var Codes = [];
var JobDetails = [];
var JobFunc = [];
var JobIndus = [];

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

    if ($('#txtFromDate').val().length === 0 || $('#txtToDate').val().length === 0) {
        ValidationError(false, "Dates are mandatory..!!", "AppErrorField");
        return;
    }

    if ($('#myHouse').val().length === 0) {
        ValidationError(false, "Please select Domain or TC Name..!!", "AppErrorField");
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
                ValidationError(d.Status, d.ErrorString, "AppErrorField")
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
        rows += "<tr><td>" + item.ClinicName + "</td><td>" + item.EntryDate + "</td><td>" + item.ApplicantName + "</td><td>" + item.Mobile + "</td><td>" + item.Email + "</td><td>" + item.Country + "</td><td>" + item.TcName + "</td><td>" + item.TcMobile + "</td><td>" + item.TcEmail + "</td><td>" + item.ProfessionName + "</td><td>" + item.HeardAbout + "</td><td>" + item.FreezoneCode + "</td><td>" + item.CustomerType + "</td><td>" + item.BscCode + "</td><td>" + item.TcCode + "</td></tr>";
    });
    $('#tb_ApplicantDtls tbody').html(rows);
    $('#tb_ApplicantDtls').show();
    $('#AppBuffer').hide();
}

function ValidationError(status, errorString, id) {
    fadeLabeIn(id);
    if (status == true) {

        let lab = document.getElementById(id); // access the button by id
        lab.style.color = 'Green';

        $('#'+id).text(errorString);
    }
    else {
        let lab = document.getElementById(id); // access the button by id
        lab.style.color = 'Red';
        $('#' + id).text(errorString);
    }

    setTimeout(function () {
        fadeLabelOut(id);
    }, 5000);
}

//$(document).ready(function () {
    
//});

function fadeLabelOut(Id) {
    $('#' + Id).fadeOut(0,function () {
        $(this).html(''); //reset the label after fadeout
    });
}

function fadeLabeIn(Id) {
    $('#' + Id).fadeIn(0, function () {
        $(this).html(''); //reset the label after fadeout
    });
}

function FetchJobFunctionsOrIndustr(m) {
    var datas = {
        mode: m,
    }; 
    $.ajax({
        type: 'POST',
        url: '/JobFunction/FetchJobFunctionOrIndustry',
        datatype: "json",
        data: datas,
        success: function (d) {
            if (d.Status && m == 0) {
                JobFunc = d.Data;
                AddToDropDown(d.Data, "ComboJobFunc", 0);
            }
            if (d.Status && m == 1) {
                JobIndus = d.Data;
                AddToDropDown(d.Data, "ComboJobIndus", 1);
            }
            if (!d.Status) {
                ValidationError(false, d.ErrorString, "JbTxtErrorField");
            }
        },
        error: function (ex) {
        }
    });
}

function AddToDropDown(Data, id, mode) {
    $('#'+id).html("");
    var rows = "<option value='0'>Select Any</option>";
    if (mode == 0) {
        $.each(Data, function (i, item1) {
            rows += "<option value=" + item1.Id + ">" + item1.FunctionName + "</option>";
        });
    }
    else if (mode == 1) {
        $.each(Data, function (i, item2) {
            rows += "<option value=" + item2.Id + ">" + item2.IndustryName + "</option>";
        });
    } 
    $('#'+id).html(rows);
}

function InsertJobDetails() {

    if (currentId == 0 || $('#txtTitle').val().length === 0 || ($('#ComboJobFunc').val() == 0 && $('#ComboJobIndus').val() == 0)) {
        ValidationError(false, "Select a valid details from the grid and fill all mandatory fields..!!", "JbTxtErrorField");
        return;
    } 

    var datas = {
        Id: currentId,
        Title: $('#txtTitle').val(),
        JobFunction: $('#ComboJobFunc').val(),
        Industry: $('#ComboJobIndus').val(),
    };
    $.ajax({
        type: 'POST',
        url: '/JobFunction/UpdateJobDetails',
        datatype: "json",
        data: datas,
        success: function (d) {

            ValidationError(d.Status, d.ErrorString, "JbTxtErrorField");
            if (d.Status) {
                var func = $('#ComboJobFunc').val() == 0 ? '' : $('#ComboJobFunc :selected').text();
                var ind = $('#ComboJobIndus').val() == 0 ? '' : $('#ComboJobIndus :selected').text();
                $('#trid' + currentId).replaceWith("<tr id= trid" + currentId + "><td>" + '<input id=' + currentId + ' type="checkbox" onclick="JobTableCheckBoxCheck(' + currentId + ')">' + "</td><td>" + $('#txtTitle').val() + "</td><td>" + func + "</td><td>" + ind+ "</td></tr>");
                var item = JobDetails.find(x => x.Id == currentId);
                if (item != null) {
                    item.Title = $('#txtTitle').val()
                    item.FunctionName = func;
                    item.IndustryName = ind;
                    item.JobFunction = func == '' ? -1 : $('#ComboJobFunc').val();
                    item.Industry = ind == '' ? -1 : $('#ComboJobIndus').val();
                }
                $('#txtTitle').val("");
                $('#ComboJobFunc').val(0);
                $('#ComboJobIndus').val(0);
            }
        },
        error: function (ex) {
        }, 
    });
}

function FetchAllJobDetails() {
    $('#JobBuffer').show();
    $.ajax({
        type: 'POST',
        url: '/JobFunction/FetchAllJobDetails',
        datatype: "json", 
        success: function (d) {
            if (d.Status) {
                JobDetails = d.Data;
                AddToJobTable(d.Data);
            } 
            else {
                ValidationError(false, d.ErrorString, "JbTxtErrorField");
                $('#JobBuffer').hide();
            }
        },
        error: function (ex) {
        }
    });
}

function AddToJobTable(Data) {
    currentPage = $('#maxRows').val();
    $('#tb_job_func tbody').html("");
    $('#JobBuffer').show();
    var rows = "";
    $.each(Data, function (i, item) {
        rows += "<tr id= trid" + item.Id + "><td>" + '<input id=' + item.Id + ' type="checkbox" onclick="JobTableCheckBoxCheck(' + item.Id + ')">' + "</td><td>" + item.Title + "</td><td>" + item.FunctionName + "</td><td>" + item.IndustryName + "</td></tr>";
    });
    $('#tb_job_func tbody').html(rows);
    getPagination("#tb_job_func");
    $('#JobBuffer').hide();
}

function JobTableCheckBoxCheck(id) {
    $IDs = $("#tb_job_func input:checkbox:checked").map(function () {
        return $(this).attr("id");
    }).get();

    if ($IDs != null) {
        var cId = $IDs.find(x => x != id);
        $('#' + cId).prop('checked', false);
    }

    if ($('#' + id).is(':checked')) {
        currentId = id;
        if (JobDetails != null) {

            var item = JobDetails.find(x => x.Id == id);
            if (item != null) { 
                $('#txtTitle').val(item.Title);

                var fun = JobFunc.find(x => x.FunctionName == item.FunctionName);
                if (fun != null) {
                    $('#ComboJobFunc').val(fun.Id);
                }
                else {
                    $('#ComboJobFunc').val(0);
                }
                var Ins = JobIndus.find(x => x.IndustryName == item.IndustryName);
                if (Ins != null) {
                    $('#ComboJobIndus').val(Ins.Id);
                }
                else {
                    $('#ComboJobIndus').val(0);
                }
            } 
        }
        else {
            ValidationError(false, "Failed with error..!!", "JbTxtErrorField");
        }
    }
    else {
        currentId = 0;
        $('#txtTitle').val("");
        $('#ComboJobFunc').val(0);
        $('#ComboJobIndus').val(0);
    }
}

function NewJobSearch() {
    $.ajax({
        type: 'POST',
        url: '/JobFunction/NewJobSearch',
        dataType: 'json',
        data: { 'IsChecked': $('#lbnewjobs').is(':checked') },
        success: function (data) {
            AddToJobTable(data)
        },
        error: function (ex) {
            ValidationError(false, "Failed with Errors", "JbTxtErrorField");
        }
    });
}

function SearchJobs() {
    $.ajax({
        type: 'POST',
        url: '/JobFunction/SearchJobs',
        dataType: 'json',
        data: { 'searchData': document.getElementById("txtSearchJob").value },
        success: function (data) {
            AddToJobTable(data)
        },
        error: function (ex) {
            ValidationError(false, "Failed with Errors", "JbTxtErrorField");
        }
    });
}

function ClearDetails() {
    $('#txtFromDate').val('');
    $('#txtToDate').val('');
    $('#SltCodes').val(1).change();
    $('#myHouse').val('');
    $('#OptOther').val('Mobile');
    $('#txtFreeTxt').val('');
}