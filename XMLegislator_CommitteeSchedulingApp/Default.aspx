﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="XMLegislator_CommitteeSchedulingApp._Default" %>
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
  eventClickHandling ="JavaScript"
  EventClickJavaScript="eventClick(e);"
   eventResizeHandling ="JavaScript"
      eventResizeJavascript="onEventResize(e, newStart, newEnd) "
  
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
                             <div class="input-field col s4">
                                <label for="dlcommitteeType"><small>Committee Type</small></label>
                                <br />
                                <br />
                                <select id="dlcommitteeType" class="comtypename browser-default"></select>

                            </div>
                            <div class="input-field col s4">
                                <label for="dlcommittee"><small>Committee</small></label>
                                <br />
                                <br />
                                <select id="dlcommittee" class="comname browser-default"></select>

                            </div>
                            <div class="input-field col s4">
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
                <a href="#!" id="btnCreateNewMeeting" class="modal-action modal-close waves-effect btn green ">SAVE</a>
            </div>
        </div>

        
        <div id="editmodal" class="modal modal-fixed-footer">
            <div class="modal-content">
                <div class="row">
                    <div class="col s12">
                        <div class="row" style="margin-bottom: 0px">
                            <div class="col s12">
                                <h4><b>Edit Meeting</b></h4>
                            </div>
                        </div>
                        <div class="row">
                               <div class="input-field col s4">
                                <label for="dlcommitteeType2"><small>Committee Type</small></label>
                                <br />
                                <br />
                                <select id="dlcommitteeType2" class="comtypename browser-default"></select>

                            </div>
                            <div class="input-field col s4">
                                <label for="dlcommittee2"><small>Committee</small></label>
                                <br />
                                <br />
                                <select id="dlcommittee2" class="comname browser-default"></select>

                            </div>
                            <div class="input-field col s4">
                                <label for="roomdropdown2"><small>Building / Room</small></label>
                                <br />
                                <br />
                                <select id="roomdropdown2" class="roomname browser-default"></select>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col s12">
                                <input id="thedate2" type="date" placeholder="No time has been selected." class="datepicker" style="margin-bottom: 0px" />
                                <label for="thedate">Meeting Day</label>
                            </div>
                        </div>
                        <div class="row" style="margin-bottom:0px">
                            <div class="input-field col l6 s6" style="margin-top: 0px">
                                <label><small>Start Time</small></label>
                                <br />
                                <input id="theStartTime2" style="margin-top: 12px" type="time" />
                            </div>
                            <div class="input-field col l6 s6" style="margin-top: 0px">
                                <label><small>End Time</small></label>
                                <br />
                                <input id="theEndTime2" style="margin-top: 12px" type="time" />
                            </div>
                        </div>
                         <div class="row">
                            <div class="input-field col l12 s12" style="margin-top: 0px">
                                <label><small>Agenda</small></label>
                                <br />
                                <div id="agendalist"></div>
                              
                            </div>
                          
                        </div>
                    </div>
                </div>
                <br />
                <br />
            </div>
            <div class="modal-footer">
                <a href="#!" id="btnEditMeeting" class="modal-action modal-close waves-effect btn green ">SAVE CHANGES</a>
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

        loadCommitteeType()
        loadrooms();
      

        function loadCommitteeType() {

            var options;
            $.ajax({
                type: "POST",
                url: "Engine.asmx/LoadCommitteeType",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    $(".comname").empty();

                    $.each(result, function (index, item) {

                        options = "<option val=" + item.CommitteeTypeID + "  data-comtypeid=" + item.CommitteeTypeID + ">" + item.CommitteeType + "</option>"
                        $(options).appendTo(".comtypename");

                    })

                    var baseoption = "<option>-- No committee Type selected --</option>"
                    $(baseoption).prependTo(".comtypename");

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


        function loaddropdown(committeeTypeID) {

            var options;
            $.ajax({
                type: "POST",
                url: "Engine.asmx/LoadCommittees",
                data: "{CommitteeTypeID:'"+ committeeTypeID +"'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    $(".comname").empty();

                    $.each(result, function (index, item) {

                        options = "<option val=" + item.CommitteeID + "  data-comid=" + item.CommitteeID + ">" + item.CommitteeName + "</option>"
                        $(options).appendTo(".comname");

                    })

                    var baseoption = "<option>-- No committee selected --</option>"
                    $(baseoption).prependTo(".comname");

                },
                failure: function (msg) {
                   
                },
                error: function (err) {
                   
                }
            }) //end ajax


            //  return "<select class='selectit'>" + options + "</select>"
        }

        function loaddropdown2(committeeTypeID, selectedcom, room) {

            var options;
            $.ajax({
                type: "POST",
                url: "Engine.asmx/LoadCommittees",
                data: "{CommitteeTypeID:'" + committeeTypeID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    $(".comname").empty();

                    $.each(result, function (index, item) {

                        options = "<option val=" + item.CommitteeID + "  data-comid=" + item.CommitteeID + ">" + item.CommitteeName + "</option>"
                        $(options).appendTo(".comname");

                    })

                    var baseoption = "<option>-- No committee selected --</option>"
                    $(baseoption).prependTo(".comname");
                    $("#dlcommittee2 option[val=" + selectedcom + "]").prop("selected", true);
                    $("#roomdropdown2 option[val=" + room + "]").prop("selected", true);

                },
                failure: function (msg) {

                },
                error: function (err) {

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

        var meetingdate;
        $("#btnCreateNewMeeting").click(function () {
            //get the set values
            var committeeId = $("#dlcommittee option:selected").attr("data-comid");
            var roomid = $("#roomdropdown option:selected").attr("data-roomid");

            if (typeof committeeId === "undefined") {
                alert("A committee has not been selected.");
                return;
            }
            if (typeof roomid === "undefined") {
                alert("A room has not been selected.");
                return;
            }

            meetingdate = new Date($("#thedate").val());
            var startTime = $("#theStartTime").val();
            var endTime = $("#theEndTime").val();
            var day = meetingdate.getDate() + 1;
            var month = meetingdate.getMonth() + 1;
            var year = meetingdate.getFullYear();
            var formattedStartDate = year + "-" + month + "-" + day + " " + startTime
            var formattedEndDate = year + "-" + month + "-" + day + " " + endTime

            $.ajax({
                type: "POST",
                url: "Engine.asmx/DateEngine",
                data: "{FormattedStartDate:'" + formattedStartDate + "',FormattedEndDate:'" + formattedEndDate + "',CommitteeID:'"+ committeeId +"',RoomID:'"+ roomid +"'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    window.location.reload(true);


                },
                failure: function (msg) {
                    alert(msg);
                },
                error: function (err) {
                    alert(err);
                }
            }) //end ajax
           
        })


        
        var meetingdate2;
        $("#btnEditMeeting").click(function () {

            var meetingid = $(this).attr('data-meeting');
        
            //get the set values
            var committeeId = $("#dlcommittee2 option:selected").attr("data-comid");
            var roomid = $("#roomdropdown2 option:selected").attr("data-roomid");

            if (typeof committeeId === "undefined") {
                alert("A committee has not been selected.");
                return;
            }
            if (typeof roomid === "undefined") {
                alert("A room has not been selected.");
                return;
            }

            meetingdate2 = new Date($("#thedate2").val());
            var startTime = $("#theStartTime2").val();
            var endTime = $("#theEndTime2").val();
            var day = meetingdate2.getDate() + 1;
            var month = meetingdate2.getMonth() + 1;
            var year = meetingdate2.getFullYear();
            var formattedStartDate = year + "-" + month + "-" + day + " " + startTime
            var formattedEndDate = year + "-" + month + "-" + day + " " + endTime

            $.ajax({
                type: "POST",
                url: "Engine.asmx/DateEngine2",
                data: "{meetingId:'"+ meetingid +"', FormattedStartDate:'" + formattedStartDate + "',FormattedEndDate:'" + formattedEndDate + "',CommitteeID:'" + committeeId + "',RoomID:'" + roomid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    window.location.reload(true);


                },
                failure: function (msg) {
                    alert(msg);
                },
                error: function (err) {
                    alert(err);
                }
            }) //end ajax

        })


       
 
        $("#dlcommitteeType2").change(function () {
            var comtype = $('#dlcommitteeType2 option:selected').attr("data-comtypeid");
            console.log(comtype);
            loaddropdown(comtype);
        })
        $("#dlcommitteeType").change(function () {
            var comtype = $('#dlcommitteeType option:selected').attr("data-comtypeid");
            loaddropdown(comtype);
        })


  
       var _StartTime;

        function eventClick(e) {
            var meetingid = e.value();
            var committeeId;
            var committeeTypeId;
            var meetingDateTime;
            var defaultRoomId;
            var startTime;
            var endTime;

            $("#btnEditMeeting").attr('data-meeting', meetingid);

            // get meeting details and set pertinent variables needed to call remaining methods

            $.ajax({
                type: "POST",
                url: "Engine.asmx/GetMeetingDetails",
                data: "{CommitteeMeetingID:'"+ meetingid +"'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                 
                    $.each(result, function (index, item) {

                        committeeId = item.CommitteeID;
                        committeeTypeId = item.CommitteeTypeID;
                        meetingDateTime = item.MeetingDateTime;
                        defaultRoomId = item.DefaultRoomID;
                        startTime = item.StartTime;
                        endTime = item.EndTime;

                    })

                    console.log('committee type id = ' + committeeTypeId + ' committee id = ' + committeeId);
                    $("#dlcommitteeType2 option[val=" + committeeTypeId + "]").prop("selected", true);
                    loadEditableCommittees(committeeTypeId, committeeId, defaultRoomId);
                    getBillList(meetingid);

                    var startDate = new Date(startTime);
                    var endDate = new Date(endTime);
                 

                    var startDay = ("0" + startDate.getDate()).slice(-2);
                    var startMonth = ("0" + (startDate.getMonth() + 1)).slice(-2);
                    var startYear = startDate.getFullYear();
                    var today = startYear + "-" + (startMonth) + "-" + (startDay);
                    var startHours = startDate.getHours();
                  
                    var endHours = endDate.getHours();

                    $('#thedate2').val(today);

                    var startFormatted;
                    if (startHours < 10) {
                        startFormatted = "0" + startHours + ":00";
                    } else {
                        startFormatted = startHours + ":00";
                    }

                    $("#theStartTime2").val(startFormatted);

                    var endFormatted;
                    if (endHours < 10) {
                        endFormatted = "0" + endHours + ":00";
                    } else {
                        endFormatted = endHours + ":00";
                    }
                    $("#theEndTime2").val(endFormatted);

                    // End Date PlayGround

                },
                failure: function (msg) {
                    console.log(msg)
                },
                error: function (err) {
                    console.log(err)
                }
            }) //end ajax

           
            // load committee types
            // load committee names
            // load rooms
           

            $("#editmodal").openModal();

        }

        function getBillList(meetingid) {
            $.ajax({
                type: "POST",
                url: "Engine.asmx/GetBillList",
                data: "{CommitteeMeetingID:'"+ meetingid +"'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    $(result).appendTo("#agendalist");
                    

                },
                failure: function (msg) {
                    console.log(msg);
                },
                error: function (err) {
                    console.log(err);
                }
            }) //end ajax
          
        }

        function loadEditableCommittees(comtype,comid,room) {
            console.log("inside editable committees");
            loaddropdown2(comtype,comid,room);
        }

        function onEventResize(e, newStart, newEnd) {
            // console.log(e.value(), newStart,newEnd);
            //Function MeetingResized(ByVal MeetingID As Integer, ByVal StartTime As String, ByVal EndTime As String)
            $.ajax({
                type: "POST",
                url: "Engine.asmx/MeetingResized",
                data: "{MeetingID:'"+ e.value() +"',StartTime:'"+ newStart +"',EndTime:'"+ newEnd +"'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var result = data.d;
                    window.location.reload(true);

                },
                failure: function (msg) {
                    console.log(msg);
                },
                error: function (err) {
                    console.log(err);
                }
            }) //end ajax
        }
  
    </script>
</body>
</html>
