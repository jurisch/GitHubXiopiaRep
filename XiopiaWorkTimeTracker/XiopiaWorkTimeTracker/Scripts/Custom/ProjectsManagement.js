
//debugger;
$("#newPrjectBtn").on("click", function () {
    $("#newPrButtons").show();
    var myRow = '<tr id="newPrRow"> \
                        <td><input id="newProjName" type="text" title="Please enter the project name" autofocus  required/></td>\
                        <td><input id="newResponsible" type="text" title="Please enter project responsible" required/></td>\
                        <td><table id="membersTbl"></table></td>\
                    </tr>';
    $("#tblWebGrid > tbody:last").append(myRow);

    $.ajax({
        type: "GET",
        url: "/Projects/GetUserList",
        contentType: 'application/json; charset=utf-8',
        async: true,
        processData: false,
        cache: false,
        success: function (data) {
            $(data).each(function () {
                $("#memberselect").append($("<option>").attr('value', this.Id).text(this.FirstName + " " + this.LastName));
            });
            $("#addUser").on("click", function () {
                var newMember = '<tr><td>' + $('#memberselect :selected').text() + '<input type="hidden" value="' + $('#memberselect :selected').val() + '"</input></td></tr>';
                $("#membersTbl").append(newMember);
            });
        },
        error: function (xhr) {
            alert('error');
        }
    });

    $("#cancelNewProjBtn").on("click", function () {
        $("#newPrRow").remove();
        $("#newPrButtons").hide();
    });

    // Post Ajax data
    $("#postNewProjBtn").on("click", function () {
        var membersIdArr = new Array();
        var hi = $('input[type="hidden"]');
        for (i = 0; i < hi.length; i++)
        {
            membersIdArr.push(hi[i].value);
        }
        var newProjData = { "Name": $("#newProjName").val(), "ProjectResponsible": $("#newResponsible").val(), "MemberIds": membersIdArr };
        $.ajax({
            type: "POST",
            url: "/Projects/CreateNewProject",
            data: newProjData,
            datatype: "html",
            success: function (data) {
                alert("Erfolg");
                var newRow = '<tr class="webGrid-row-style"> \
                        <td class="webGrid-Date">' + $("#newProjName").val() + '</td>\
                        <td class="webGrid-Project">' + $("#newResponsible").val() + '</td>\
                        <td  class="webGrid-Project">aaaaa </td>\
                    </tr>';
                $("#newPrRow").remove();
                $("#newPrButtons").remove();
                $("#tblWebGrid > tbody:last").append(newRow);
            },
            error: function () {
                alert("error");
            }
        });
    });

});