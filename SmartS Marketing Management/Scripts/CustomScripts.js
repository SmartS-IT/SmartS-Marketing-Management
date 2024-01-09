var bscModelData = [];
var tcModelData = [];
var currentId = 0;
var currentPage = 5;


function getPagination(table) {
    var lastPage = 1;

    $('#maxRows')
        .on('change', function (evt) {
            //alert("Test");
            $('.paginationprev').html('');						// reset pagination

            lastPage = 1;
            $('.pagination')
                .find('li')
                .slice(1, -1)
                .remove();
            var trnum = 0; // reset tr counter
            var maxRows = parseInt($(this).val()); // get Max Rows from select option

            if (maxRows == 5000) {
                $('.pagination').hide();
            } else {
                $('.pagination').show();
            }

            var totalRows = $(table + ' tbody tr').length; // numbers of rows
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
            if (totalRows > maxRows) {
                // if tr total rows gt max rows option
                var pagenum = Math.ceil(totalRows / maxRows); // ceil total(rows/maxrows) to get ..
                //	numbers of pages
                for (var i = 1; i <= pagenum;) {
                    // for each page append pagination li
                    $('.pagination #prev')
                        .before(
                            '<li data-page="' +
                            i +
                            '">\
								  <span>' +
                            i++ +
                            '<span class="sr-only">(current)</span></span>\
								</li>'
                        )
                        .show();
                } // end for i
            } // end if row count > max rows
            $('.pagination [data-page="1"]').addClass('active'); // add active class to the first li
            $('.pagination li').on('click', function (evt) {
                // on click each page
                evt.stopImmediatePropagation();
                evt.preventDefault();
                var pageNum = $(this).attr('data-page'); // get it's number

                var maxRows = parseInt($('#maxRows').val()); // get Max Rows from select option

                if (pageNum == 'prev') {
                    if (lastPage == 1) {
                        return;
                    }
                    pageNum = --lastPage;
                }
                if (pageNum == 'next') {
                    if (lastPage == $('.pagination li').length - 2) {
                        return;
                    }
                    pageNum = ++lastPage;
                }

                lastPage = pageNum;
                var trIndex = 0; // reset tr counter
                $('.pagination li').removeClass('active'); // remove active class from all li
                $('.pagination [data-page="' + lastPage + '"]').addClass('active'); // add active class to the clicked
                // $(this).addClass('active');					// add active class to the clicked
                limitPagging();
                $(table + ' tr:gt(0)').each(function () {
                    // each tr in table not the header
                    trIndex++; // tr index counter
                    // if tr index gt maxRows*pageNum or lt maxRows*pageNum-maxRows fade if out
                    if (
                        trIndex > maxRows * pageNum ||
                        trIndex <= maxRows * pageNum - maxRows
                    ) {
                        $(this).hide();
                    } else {
                        $(this).show();
                    } //else fade in
                }); // end of for each tr in table
            }); // end of on click pagination list
            limitPagging();
        })
        .val(currentPage)
        .change();

    // end of on select change

    // END OF PAGINATION
}

function limitPagging() {
    //alert($('.pagination li').length)

    if ($('.pagination li').length > 7) {
        if ($('.pagination li.active').attr('data-page') <= 3) {
            $('.pagination li:gt(5)').hide();
            $('.pagination li:lt(5)').show();
            $('.pagination [data-page="next"]').show();
        } if ($('.pagination li.active').attr('data-page') > 3) {
            $('.pagination li:gt(0)').hide();
            $('.pagination [data-page="next"]').show();
            for (let i = (parseInt($('.pagination li.active').attr('data-page')) - 2); i <= (parseInt($('.pagination li.active').attr('data-page')) + 2); i++) {
                $('.pagination [data-page="' + i + '"]').show();

            }

        }
    }
}

//$(function () {
        //    // Just to append id number for each row
        //    $('table tr:eq(0)').prepend('<th> ID </th>');

        //    var id = 0;

        //    $('table tr:gt(0)').each(function () {
        //        id++;
        //        $(this).prepend('<td>' + id + '</td>');
        //    });
        //});

//#region BSC Code
function AddNewBSCCode() {

    if ($('#bsc_code').val().length === 0 || $('#domain').val().length === 0 ) {
        ValidationError(false, "Please fill the mandatory fields..!!");
        return;
    }

    var datas = {
        Email: "",
        Domain: document.getElementById("domain").value,
        BSC_Code: document.getElementById("bsc_code").value,
    };

    ValidationError(false, "");
  
    $.ajax('/bsccode/InsertNewBSCCode', {
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(datas),
        success: function (d) {
            ValidationError(d.Status, d.ErrorString);
            if (d.Status == true) { 
                FetchAllBscCodes(); 
                $('#domain').val("");
                $('#bsc_code').val("");
            } 
        },
        error: function () {
            ValidationError(false,"Failed with Errors");
        },
        failure: function () {
            ValidationError(false, "Failed with Errors");
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
            ValidationError(false, "Failed with Errors");
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
            ValidationError(false, "Failed with Errors");
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
            ValidationError(false, "Failed with Errors");
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
        else {
            alert("Failed");
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

    if ($('#udomain').val().length === 0 || $('#ubsccode').val().length === 0) {
        ValidationError(false, "Select a valid details from the grid and fill all mandatory fields..!!");
        return;
    }

    var datas = {
        ID: currentId,
        Email: "",
        Domain: document.getElementById("udomain").value,
        BSC_Code: document.getElementById("ubsccode").value,
    }; 

    ValidationError(false, "");
    $.ajax('/bsccode/UpdateBscCode', {
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(datas),
        success: function (d) { 
            ValidationError(d.Status, d.ErrorString);

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
            ValidationError(false, "Failed with Errors");
        },
        failure: function (ex) {
            ValidationError(false, "Failed with Errors");
        }

    });
};

//#End region

//#region TC Code
function ValidationError(status, errorString) {
    if (status == true) {
         
        let lab = document.getElementById("TxtErrorField"); // access the button by id
        lab.style.color = 'Green';

        $('#TxtErrorField').text(errorString);
    }
    else {

        let lab = document.getElementById("TxtErrorField"); // access the button by id
        lab.style.color = 'Red';
        $('#TxtErrorField').text(errorString);
    }
}

function TCValidationError(status, errorString) {
    if (status == true) {

        let lab = document.getElementById("TcTxtErrorField"); // access the button by id
        lab.style.color = 'Green';

        $('#TcTxtErrorField').text(errorString);
    }
    else {

        let lab = document.getElementById("TcTxtErrorField"); // access the button by id
        lab.style.color = 'Red';
        $('#TcTxtErrorField').text(errorString);
    }
}

function AddNewTCCode() {

    if ($('#tcname').val().length === 0 || $('#tccode').val().length === 0) {
        TCValidationError(false, "Please fill the mandatory fields..!!");
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
            TCValidationError(d.Status, d.ErrorString);

            $('#tcname').val("");
            $('#tccode').val("");
        },
        error: function (ex) {
            TCValidationError(false, "Failed with error..!!");
        },
        failure: function (ex) {
            TCValidationError(false, "Failed with error..!!");
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
            TCValidationError(false, "Failed with error..!!");
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
            TCValidationError(false, "Failed with error..!!");
        }
    }
    else {
        currentId = 0;
        $('#utcname').val("");
        $('#utccode').val("");
    }
}

function UpdateTCCode() {

    if ($('#utcname').val().length === 0 || $('#utccode').val().length === 0) {
        TCValidationError(false, "Select a valid details from the grid or fill all mandatory fields..!!");
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

            TCValidationError(d.Status, d.ErrorString);
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
            TCValidationError(false, "Failed with error..!!");
        },
        failure: function (ex) {
            TCValidationError(false, "Failed with error..!!");
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
            TCValidationError(false, "Failed with error..!!");
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
            TCValidationError(false, "Failed with error..!!");
        }
    });
}
//#End Region

//#region Export File
function ExportToExcel() {
    ValidationError(false, "");
    if ($('#txtFromDate').val().length === 0 || $('#txtToDate').val().length === 0) {
        ValidationError(false, "Please select dates..!!");
        return;
    }

    if ($('#fileUpload').val().length === 0) {
        ValidationError(false, "Please choose valid file.!!");
        return;
    }

    var datas = {
        FromDate: $('#txtFromDate').val(),
        ToDate: $('#txtToDate').val(),
        FilePath: $('#fileUpload').val(),
    };

    $('#EverBuffer').show();
    $.ajax({
        type: 'POST',
        url: '/home/ExportToExcel',
        datatype: "json",
        data: datas,
        traditional: true,
        success: function (data) { 
            ValidationError(data.Status, data.ErrorString);
            $('#EverBuffer').hide();
        },
        error: function (ex) {
            $('#EverBuffer').hide();
        }
    });
}
//#End Region

//#region Job Function Add
function InsertJobfunctions()
{ 
    if ($('#txtJobFunc').val().length === 0 && $('#txtIndustry').val().length === 0) {
        JobValidationError(false, "Please enter Job Function or Industry..!!");
        return;
    } 

    var datas = {
        Function: $('#txtJobFunc').val(),
        Industry: $('#txtIndustry').val(),
    };

    $('#EverBuffer').show();
    $.ajax({
        type: 'POST',
        url: '/JobFunction/InsertFunctionAndIndustry',
        datatype: "json",
        data: datas,
        success: function (data) {
            JobValidationError(data.Status, data.ErrorString);
            $('#EverBuffer').hide();
            $('#txtJobFunc').val("");
            $('#txtIndustry').val("");
        },
        error: function (ex) {
            $('#EverBuffer').hide();
            JobValidationError(false, "Failed with error..!!");
        }
    });
}

function JobValidationError(status, errorString) {
    if (status == true) {

        let lab = document.getElementById("JbTxtErrorField"); // access the button by id
        lab.style.color = 'Green';

        $('#JbTxtErrorField').text(errorString);
    }
    else {

        let lab = document.getElementById("JbTxtErrorField"); // access the button by id
        lab.style.color = 'Red';
        $('#JbTxtErrorField').text(errorString);
    }
}
