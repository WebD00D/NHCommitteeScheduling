<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="XMLegislator_CommitteeSchedulingApp._Default" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NH Committee Scheduling</title>
   
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.1/css/materialize.min.css" />
    <style type="text/css">
        .comname {
            color:#0094ff;
            padding:10px;
            width:100%;
        }
        .roomname {
            color:#0094ff;
            padding:10px;
            width:100%;
        }
    </style>
</head>
    



<body>
    <form id="form1" runat="server">
        
    <div>
        
   <DayPilot:DayPilotScheduler
        
  ID="DayPilotScheduler1" 
  runat="server" 
  
  DataStartField="StartTime" 
  DataEndField="EndTime" 
  DataTextField="CommitteeName" 
  DataIdField="CommitteeMeetingID" 
  DataResourceField="RoomID" 
  
  ClientObjectName="dps1"
  TimeRangeSelectedHandling="JavaScript"
  TimeRangeSelectedJavaScript="timeRangeSelected(start, end, resource)"

  CellGroupBy="Month"
  Scale="hour"
  
  EventMoveHandling="CallBack" 
  >
       <TimeHeaders>
    <DayPilot:TimeHeader GroupBy="Month" Format="MMMM yyyy" />
    <DayPilot:TimeHeader GroupBy="Day" />
   <DayPilot:TimeHeader GroupBy="Hour" />
           
  </TimeHeaders>
</DayPilot:DayPilotScheduler>
    </div>

    <div id="modal1" class="modal modal-fixed-footer">
    <div class="modal-content">
      <h4>Schedule Committee Meeting</h4>
         
        <select class="comname browser-default"></select>
        
        <select style="margin-top:12px" class="roomname browser-default"></select>

        <input style="margin-top:12px" type="date"/>
        <input style="margin-top:12px" type="time"/>
       
      
      <br />
      <br />
      
    
    </div>
    <div class="modal-footer">
      <a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat ">Agree</a>
    </div>
  </div>
        
    </form>
  
    <script src="https://code.jquery.com/jquery-2.1.4.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.1/js/materialize.min.js"></script>
   
   
    <script type="text/javascript">
         
  
        function loaddropdown() {
            
            var options;
            $.ajax({
                type: "POST",
                url: "Engine.asmx/LoadCommittees",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    $(".comname").empty();
                    
                    $.each(result, function (index, item) {

                        options =  "<option data-comid=" + item.CommitteeID + ">" + item.CommitteeName + "</option>"
                        $(options).appendTo(".comname");

                    })

                    var baseoption = "<option>-- No committee selected --</option>"
                    $(baseoption).prependTo(".comname");

                },
                failure: function (msg) {
                    alert(msg);
                },
                error: function (err) {
                    alert(err);
                }
            }) //end ajax

            
          //  return "<select class='selectit'>" + options + "</select>"
        }


        function loadrooms() {

            var options;
            $.ajax({
                type: "POST",
                url: "Engine.asmx/LoadRooms",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    $(".roomname").empty();

                    $.each(result, function (index, item) {

                        options = "<option data-comid=" + item.RoomID + ">" + item.RoomName + "</option>"
                        $(options).appendTo(".roomname");

                    })

                    var baseoption = "<option>-- No room selected --</option>"
                    $(baseoption).prependTo(".roomname");

                },
                failure: function (msg) {
                    alert(msg);
                },
                error: function (err) {
                    alert(err);
                }
            }) //end ajax


            //  return "<select class='selectit'>" + options + "</select>"
        }

        function timeRangeSelected(start, end, resource) {
            alert(start);
            loaddropdown();
            loadrooms();
            $('#modal1').openModal();
                
                // alert(start + " " + end + " " + resource);
               // var modal = new DayPilot.Modal();
               // modal.top = 60;
               // modal.width = 300;
               // modal.opacity = 70;
               // modal.border = "10px solid #d0d0d0";
               // modal.closed = function () {
               //     if (this.result == "OK") {
               //         dps1.commandCallBack('refresh');
               //     }
               //     dps1.clearSelection();
              //  };
               // modal.showUrl("New.aspx?start=" + start.toStringSortable() + "&end=" + end.toStringSortable() + "&r=" + resource);
            }



       

  
</script>
</body>
</html>
