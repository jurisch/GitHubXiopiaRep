debugger;

styleGrid();

$("#startNowBtn").on("click", function (event) {
    $("#myModal").modal('show');
})

$("#endNowBtn").on("click", function () {
    event.preventDefault();
    var newProjData = { "UserId": @Model.User.Id, "Action": "EndWork" };
    $.ajax({
        type: "POST",
        url: "/User/ActionFromView",
        data: newProjData,
        datatype: "html",
        success: function (data) {
            switch(data)
            {
                case 0:
                    window.location.replace("/User/SelectMonthWorkTimes/?month=@Model.WorkDay.Month.ToString()");
                    break;
                case 1:
                    alert("You have multiple Projects started today !");
                    break;
                case 2:
                    alert("You don't have started any Projects today !");
                    break;
                default:
                    break;
            }
        },
        error: function () {
            alert("error");
        }
    });

})

$("#tblWebGrid").on("click", function(event){
    var node = event.target;
    var nodeClasses = node.classList;
    var rowId;
    var editHtml = '<input type="text" id="editField"></input>';
    for(i=0; i<nodeClasses.length; i++)
    {
        if(nodeClasses[i] == "webGrid-PauseLength")
        {
            if(node.parentElement.firstElementChild.type == "hidden")
            {
                rowId = node.parentElement.firstElementChild.name;
            }
            node.outerHTML = '<div class="col-sm-2 webGrid-PauseLength">\
                                        <input style="text-align: center;width:3em" type="text" id="editField" value="' + node.innerText + '"/>\
                                      </div>';
            $("#editField").on("blur", function(){
                PostElementChange("Pause", rowId, $("#editField").val());
            })
            break;
        }
    }
})

function PostElementChange(element, elementId, value)
{
    var newProjData = { "UserId": @Model.User.Id, "Action": "ElementChange", "ElementId": elementId, "Element":element, "Value": value };
    $.ajax({
        type: "POST",
        url: "/User/ActionFromView",
        data: newProjData,
        datatype: "html",
        success: function (data) {
            switch(data)
            {
                case 0:
                    window.location.replace("/User/SelectMonthWorkTimes/?month=@Model.WorkDay.Month.ToString()");
                    break;
                case 1:
                    alert("You have multiple Projects started today !");
                    break;
                case 2:
                    alert("You don't have started any Projects today !");
                    break;
                default:
                    break;
            }
        },
        error: function () {
            alert("error");
        }
    });
}


function styleGrid() {
    $("#tblWebGrid tr:not(:first)").each(function () {
        var aptStatus = $(this).find("td:nth-child(1)").html();
        if (aptStatus != null) {
            if ((aptStatus.indexOf("Sa") >= 0) || (aptStatus.indexOf("So") >= 0)) {
                $(this).find("td:nth-child(1)").addClass("clsRed");
            } else {
                $(this).find("td:nth-child(1)").addClass("clsNormal");
            }
        }
    });
}