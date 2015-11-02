<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="XMLegislator_CommitteeSchedulingApp._Default" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NH Committee Scheduling</title>
   
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.1/css/materialize.min.css" />
    <style type="text/css">
        .comname {
            color:black;
            padding:10px;
            width:100%;
        }
        .roomname {
            color:black;
            padding:10px;
            width:100%;
        }
    </style>
</head>
    



<body>
    <form id="form1" runat="server">
     <nav>
    <div class="nav-wrapper blue-grey">
      <a href="#" class="brand-logo" style="margin-left:50px"><b><small>NH Committee Scheduler</small></b></a>
      <ul id="nav-mobile" class="right hide-on-med-and-down">
        <li><a href="sass.html">Log Out</a></li>
     
      </ul>
    </div>
  </nav>
        
        
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
  BusinessBeginsHour="8"
  BusinessEndsHour="18"
  ShowNonBusiness="false"
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
                <div class="row">
                    <div class="col s12">
                        <div class="row" style="margin-bottom: 0px">
                            <div class="col s12">
                                <h4><b>Create New Meeting</b></h4>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col s6">
                                <label for="dlcommittee"><small>Committee</small></label>
                                <br />
                                <br />
                                <select id="dlcommittee" class="comname browser-default"></select>

                            </div>
                            <div class="input-field col s6">
                                <label for="roomdropdown"><small>Building / Room</small></label>
                                <br />
                                <br />
                                <select id="roomdropdown" class="roomname browser-default"></select>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col s12">
                                <input id="thedate" type="date" placeholder="No time has been selected." class="datepicker" style="margin-bottom: 0px" />
                                <label for="thedate">Meeting Day</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col l6 s6" style="margin-top: 0px">
                                <label><small>Start Time</small></label>
                                <br />
                                <input id="theStartTime" style="margin-top: 12px" type="time" />
                            </div>
                            <div class="input-field col l6 s6" style="margin-top: 0px">
                                <label><small>End Time</small></label>
                                <br />
                                <input id="theEndTime" style="margin-top: 12px" type="time" />
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <br />
            </div>
            <div class="modal-footer">
                <a href="#!" class="modal-action modal-close waves-effect btn green ">SAVE</a>
            </div>
        </div>



    </form>

    <script src="https://code.jquery.com/jquery-2.1.4.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.1/js/materialize.min.js"></script>
    <script>
        $(document).ready(function () {

            $('.datepicker').pickadate({
                selectMonths: true, 
                selectYears: 2 
            });
            $('select').material_select();
        })
    </script>

    <script type="text/javascript">
        var optionobject;
        var options = [];

        loaddropdown();
        loadrooms();


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

                        options = "<option  data-comid=" + item.CommitteeID + ">" + item.CommitteeName + "</option>"
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

                        options = "<option val=" + item.RoomID + " data-roomid=" + item.RoomID + ">" + item.RoomName + "</option>"
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


            var startDate = new Date(start);
            var endDate = new Date(end);

            var startDay = ("0" + startDate.getDate()).slice(-2);
            var startMonth = ("0" + (startDate.getMonth() + 1)).slice(-2);
            var startYear = startDate.getFullYear();
            var today = startYear + "-" + (startMonth) + "-" + (startDay);
            var startHours = startDate.getUTCHours();
            var endHours = endDate.getUTCHours();

            $('#thedate').val(today);

            var startFormatted;
            if (startHours < 10) {
                startFormatted = "0" + startHours + ":00";
            } else {
                startFormatted = startHours + ":00";
            }
            $("#theStartTime").val(startFormatted);

            var endFormatted;
            if (endHours < 10) {
                endFormatted = "0" + endHours + ":00";
            } else {
                endFormatted = endHours + ":00";
            }
            $("#theEndTime").val(endFormatted);

            $("#roomdropdown option[val=" + resource + "]").prop("selected", true);

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
