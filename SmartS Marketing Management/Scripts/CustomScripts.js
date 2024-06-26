﻿var bscModelData = [];
var tcModelData = [];
var currentId = 0;
var currentPage = 5;


function getPagination(table) {
    /*var lastPage = 1;*/

    $('#maxRows')
        .on('change', function (evt) {  
            var trnum = 0; // reset tr counter 
            var maxRows = parseInt($(this).val()); // get Max Rows from select option  

            if (maxRows == 5000) {

                if (table == "#tb_bsccode") {
                    maxRows = bscModelData.length; 
                }
                else if (table == "#tb_tccode") {
                    maxRows = tcModelData.length; 
                }
                else {
                    maxRows = JobDetails.length; 
                }
               
            }   
            $(table + ' tr:gt(0)').each(function () {
                // each TR in  table and not the header
                trnum++; // Start Counter
                if (trnum > maxRows) {
                    // if tr number gt maxRows

                    $(this).hide(); // fade it out
                }
                if (trnum <= maxRows) {
                    $(this).show();
                } // else fade in Important in case if it ..
            }); //  was fade out to fade it in  
        })
        .val(currentPage)
        .change();

    // end of on select change
    // END OF PAGINATION
}

//#region BSC Code
function AddNewBSCCode() {

    if ($('#bsc_code').val().length === 0 || $('#domain').val().length === 0 ) {
        ValidationError(false, "Please fill the mandatory fields..!!", "TxtErrorField");
        return;
    }

    var datas = {
        Email: "",
        Domain: document.getElementById("domain").value,
        BSC_Code: document.getElementById("bsc_code").value,
    }; 
  
    $.ajax('/bsccode/InsertNewBSCCode', {
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(datas),
        success: function (d) {
            ValidationError(d.Status, d.ErrorString, "TxtErrorField");
            if (d.Status == true) { 
                FetchAllBscCodes(); 
                $('#domain').val("");
                $('#bsc_code').val("");
            } 
        },
        error: function () {
            ValidationError(false, "Failed with Errors", "TxtErrorField");
        },
        failure: function () {
            ValidationError(false, "Failed with Errors", "TxtErrorField");
        }
        
    });  
};

function FetchAllBscCodes(obj) { 
    $.ajax({
        type: 'POST',
        url: '/bsccode/FetchAllBSCCode',
        dataType: 'json',
        success: function (data) {
            if (data != null) { 
                bscModelData = data;
                AddToTable(data); 
            }
            
        },
        error: function (ex) {
            ValidationError(false, "Failed with Errors", "TxtErrorField");
        }
    });
}

function SearchDomain() { 
    $.ajax({
        type: 'POST',
        url: '/bsccode/SearchBSCDomain',
        dataType: 'json', 
        data: { 'searchData': document.getElementById("txtSearchDomain").value },
        success: function (data) { 
            AddToTable(data) 
        },
        error: function (ex) {
            ValidationError(false, "Failed with Errors", "TxtErrorField");
        }
    });
}

function NewDomainSearch() {
    $.ajax({
        type: 'POST',
        url: '/bsccode/NewDomainSearch',
        dataType: 'json',
        data: { 'IsChecked': $('#lbnewcodes').is(':checked')},
        success: function (data) {
            AddToTable(data)
        },
        error: function (ex) {
            ValidationError(false, "Failed with Errors", "TxtErrorField");
        }
    });
}

function BscTableCheckBox(id) {
    $IDs = $("#tb_bsccode input:checkbox:checked").map(function () {
        return $(this).attr("id");
    }).get(); 

    if ($IDs != null) {
        var cId = $IDs.find(x => x != id);
        $('#' + cId).prop('checked', false);
    }
    if ($('#' + id).is(':checked')) {
        currentId = id;  
        if (bscModelData != null) {

            var item = bscModelData.find(x => x.ID == id);
            if (item != null) {

                $('#udomain').val(item.Domain);
                $('#ubsccode').val(item.BSC_Code);
            }
        }
        else {;
        }
    }
    else {
        currentId = 0;
        $('#udomain').val("");
        $('#ubsccode').val("");
    }
}

function AddToTable(Data) {
    currentPage = $('#maxRows').val(); 
    $('#tb_bsccode tbody').html("");
    $('#Buffer').show();
    var rows = "";
    $.each(Data, function (i, item) {
        rows += "<tr id= trid"+ item.ID +"><td>" + '<input id=' + item.ID + ' type="checkbox" onclick="BscTableCheckBox(' + item.ID + ')">' + "</td><td>" + item.Domain + "</td><td>" + item.BSC_Code + "</td></tr>";
    });
    $('#tb_bsccode tbody').html(rows);
    getPagination("#tb_bsccode");
    $('#Buffer').hide();
}

function UpdateBSCCode() {

    if (currentId ==0 || $('#udomain').val().length === 0 || $('#ubsccode').val().length === 0) {
        ValidationError(false, "Select a valid details from the grid and fill all mandatory fields..!!", "TxtErrorField");
        return;
    }

    var datas = {
        ID: currentId,
        Email: "",
        Domain: document.getElementById("udomain").value,
        BSC_Code: document.getElementById("ubsccode").value,
    }; 

    $.ajax('/bsccode/UpdateBscCode', {
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(datas),
        success: function (d) { 
            ValidationError(d.Status, d.ErrorString, "TxtErrorField");

            if (d.Status == true) {
                /* FetchAllBscCodes();*/

                $('#trid' + currentId).replaceWith("<tr id=trid" + currentId + "><td>" + '<input id=' + currentId + ' type="checkbox" onclick="BscTableCheckBox(' + currentId + ')">' + "</td><td>" + $('#udomain').val() + "</td><td>" + $('#ubsccode').val() + "</td></tr>");
                var item = bscModelData.find(x => x.ID == currentId);
                if (item != null) {

                    item.Domain = $('#udomain').val();
                    item.BSC_Code = $('#ubsccode').val();
                }

                $('#udomain').val("");
                $('#ubsccode').val("");
            }
        },
        error: function (ex) {
            ValidationError(false, "Failed with Errors", "TxtErrorField");
        },
        failure: function (ex) {
            ValidationError(false, "Failed with Errors", "TxtErrorField");
        }

    });
};

//#End region

//#region TC Code

function AddNewTCCode() {

    if ($('#tcname').val().length === 0 || $('#tccode').val().length === 0) {
        ValidationError(false, "Please fill the mandatory fields..!!", "TcTxtErrorField");
        return;
    }

    var datas = { 
        TC_Name: document.getElementById("tcname").value,
        TC_Code: document.getElementById("tccode").value,
    };

    $.ajax('/tccode/InsertNewTCCode', {
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(datas),
        success: function (d) {
            /*alert("Success");*/
            FetchAllTcCodes();
            ValidationError(d.Status, d.ErrorString, "TcTxtErrorField");

            $('#tcname').val("");
            $('#tccode').val("");
        },
        error: function (ex) {
            TCValidationError(false, "Failed with error..!!", "TcTxtErrorField");
        },
        failure: function (ex) {
            TCValidationError(false, "Failed with error..!!", "TcTxtErrorField");
        }

    });
};

function FetchAllTcCodes(obj) {
    $.ajax({
        type: 'POST',
        url: '/tccode/FetchAllTCCode',
        dataType: 'json',
        success: function (data) {
            if (data != null) {
                tcModelData = data;
                AddToTCTable(data);
            }

        },
        error: function (ex) {
            ValidationError(false, "Failed with error..!!", "TcTxtErrorField");
        }
    });
}

function AddToTCTable(Data) {
    currentPage = $('#maxRows').val();
    $('#tb_tccode tbody').html("");
    $('#Buffer').show();
    var rows = "";
    $.each(Data, function (i, item) {
        rows += "<tr id= trid" + item.ID +"><td>" + '<input id=' + item.ID + ' type="checkbox" onclick="TcTableCheckBox(' + item.ID + ')">' + "</td><td>" + item.TC_Name + "</td><td>" + item.TC_Code + "</td></tr>";
    });
    $('#tb_tccode tbody').html(rows);
    getPagination("#tb_tccode");
    $('#Buffer').hide();
}

function TcTableCheckBox(id) {
    $IDs = $("#tb_tccode input:checkbox:checked").map(function () {
        return $(this).attr("id");
    }).get();

    if ($IDs != null) {
        var cId = $IDs.find(x => x != id);
        $('#' + cId).prop('checked', false);
    }

    if ($('#' + id).is(':checked')) {
        currentId = id;
        if (tcModelData != null) {

            var item = tcModelData.find(x => x.ID == id);
            if (item != null) {

                $('#utcname').val(item.TC_Name);
                $('#utccode').val(item.TC_Code);
            }
        }
        else {
            ValidationError(false, "Failed with error..!!", "TcTxtErrorField");
        }
    }
    else {
        currentId = 0;
        $('#utcname').val("");
        $('#utccode').val("");
    }
}

function UpdateTCCode() {

    if (currentId == 0 || $('#utcname').val().length === 0 || $('#utccode').val().length === 0) {
        ValidationError(false, "Select a valid details from the grid and fill all mandatory fields..!!", "TcTxtErrorField");
        return;
    }

    var datas = {
        ID: currentId, 
        TC_Name: document.getElementById("utcname").value,
        TC_Code: document.getElementById("utccode").value,
    };

    $.ajax('/tccode/UpdateTcCode', {
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(datas),
        success: function (d) {

            ValidationError(d.Status, d.ErrorString, "TcTxtErrorField");
            if (d.Status == true) {
                /* FetchAllTcCodes();*/
                $('#trid' + currentId).replaceWith("<tr id=trid" + currentId + "><td>" + '<input id=' + currentId + ' type="checkbox" onclick="TcTableCheckBox(' + currentId + ')">' + "</td><td>" + $('#utcname').val() + "</td><td>" + $('#utccode').val() + "</td></tr>");
                var item = tcModelData.find(x => x.ID == currentId);
                if (item != null) {

                    item.TC_Name = $('#utcname').val();
                    item.TC_Code = $('#utccode').val();
                }
                $('#utcname').val("");
                $('#utccode').val("");
            }  
        },
        error: function (ex) {
            ValidationError(false, "Failed with error..!!", "TcTxtErrorField");
        },
        failure: function (ex) {
            ValidationError(false, "Failed with error..!!", "TcTxtErrorField");
        }

    });
};

function SearchTCName() {
    $.ajax({
        type: 'POST',
        url: '/tccode/SearchTCName',
        dataType: 'json',
        data: { 'searchData': document.getElementById("txttcSearchDomain").value },
        success: function (data) {
            AddToTCTable(data)
        },
        error: function (ex) { 
            ValidationError(false, "Failed with error..!!", "TcTxtErrorField");
        }
    });
}

function NewTCCodeSearch() {
    $.ajax({
        type: 'POST',
        url: '/tccode/NewTCNameSearch',
        dataType: 'json',
        data: { 'IsChecked': $('#lbnewtccodes').is(':checked') },
        success: function (data) {
            AddToTCTable(data)
        },
        error: function (ex) {
            ValidationError(false, "Failed with error..!!", "TcTxtErrorField");
        }
    });
}
//#End Region

//#region Export File
function ExportToExcel(m) {
    if ($('#txtFromDate').val().length === 0 || $('#txtToDate').val().length === 0) {
        ValidationError(false, "Please select dates..!!", "ExpErrorField");
        return;
    }

    //if ($('#fileUpload').val().length === 0) {
    //    ValidationError(false, "Please choose valid file.!!", "ExpErrorField");
    //    return;
    //}

    var datas = {
        FromDate: $('#txtFromDate').val(),
        ToDate: $('#txtToDate').val(),
        FilePath: "",
        Mode: m
    };

    $('#EverBuffer').show();
    $.ajax({
        type: 'POST',
        url: '/home/ExportToExcel',
        datatype: "json",
        data: datas,
        traditional: true,
        success: function (data) {
            if (data.Status) {
                window.location.href = "/home/CreateExcelFile?file=" + datas;
            } 
            ValidationError(data.Status, data.ErrorString, "ExpErrorField");
            $('#EverBuffer').hide();
        },
        error: function (ex) {
            alert(ex.responseText);
            $('#EverBuffer').hide();
        }
    });
}
//#End Region

//#region Job Function Add
function InsertJobfunctions()
{ 
    if ($('#txtJobFunc').val().length === 0 && $('#txtIndustry').val().length === 0) {
        ValidationError(false, "Please enter Job Function or Industry..!!","JbTxtErrorField");
        return;
    } 

    var datas = {
        Function: $('#txtJobFunc').val(),
        Industry: $('#txtIndustry').val(),
    };

    $.ajax({
        type: 'POST',
        url: '/JobFunction/InsertFunctionAndIndustry',
        datatype: "json",
        data: datas,
        success: function (data) { 
            if (data.Status) {
                FetchJobFunctionsOrIndustr(0);
                FetchJobFunctionsOrIndustr(1);
                $('#txtJobFunc').val("");
                $('#txtIndustry').val("");
            }
            ValidationError(data.Status, data.ErrorString, "JbTxtErrorField"); 
        },
        error: function (ex) { 
            ValidationError(false, "Failed with error..!!", "JbTxtErrorField");
        }
    });
}

function CheckUserDetails() {

    if ($('#Uname').val().length === 0 || $('#Pass').val().length === 0) {
        ValidationError(false, "Please fill all mandatory fields..!!", "LoginError");
        return;
    }

    var datas = {
        username: $('#Uname').val(),
        password: $('#Pass').val(),
    };

    $.ajax({
        type: 'POST',
        url: '/UserManagment/CheckAuthentication',
        dataType: 'json',
        data: datas,
        success: function (d) {
            if (!d.Status) {
                ValidationError(d.Status, d.ErrorString, "LoginError");
                return;
            }
            window.location.href = d.Data;
        },
        error: function (ex) { 
        }
    });
}

