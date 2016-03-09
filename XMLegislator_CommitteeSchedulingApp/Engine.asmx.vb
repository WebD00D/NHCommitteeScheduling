Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Engine
    Inherits System.Web.Services.WebService

    Public Class Committee
        Public CommitteeID As Integer
        Public CommitteeTypeID As Integer
        Public CommitteeType As String
        Public ChamberCode As String
        Public CommitteeName As String
        Public CommitteeMeetingID As Integer
        Public MeetingDateTime As String
        Public DefaultRoomID As Integer
        Public StartTime As String
        Public EndTime As String
        Public BillListHTML As String
        Public MeetingNotes As String
        Public HearingTypeID As Integer
        Public ContactName As String
        Public ContactPhone As String
        Public isDoubleRoom As Boolean
        Public EquipmentList As String
    End Class

    Public Class Room
        Public RoomID As Integer
        Public RoomName As String
    End Class

    Public Class HearingType
        Public HearingTypeID As Integer
        Public HearingTypeName As String
    End Class

    Public Class MeetingDates
        Public MeetingDate As Date
        Public FormattedStartDate As String
        Public FormattedEndDate As String
    End Class


    <WebMethod()> _
    Public Function LoadCommitteeType()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT CommitteeTypeID, CommitteeType FROM CommitteeType"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using

        End Using

        Dim CommitteeList As New List(Of Committee)
        If dt.Rows.Count > 0 Then
            CommitteeList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim C As New Committee
                C.CommitteeTypeID = item("CommitteeTypeID")
                C.CommitteeType = item("CommitteeType")
              
                CommitteeList.Add(C)
            Next
            Return CommitteeList
        Else
            Return "none"
        End If


        Return ""
    End Function


    <WebMethod()> _
    Public Function LoadCommittees(ByVal CommitteeTypeID As Integer)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT CommitteeID, ChamberCode, CommitteeName, RoomID FROM Committee WHERE CommitteeTypeID = " & CommitteeTypeID & " ORDER BY CommitteeName ASC"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
        End Using

        Dim CommitteeList As New List(Of Committee)
        If dt.Rows.Count > 0 Then
            CommitteeList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim C As New Committee
                C.CommitteeID = item("CommitteeID")
                C.ChamberCode = item("ChamberCode")
                C.CommitteeName = item("CommitteeName")
                C.DefaultRoomID = item("RoomID")
                CommitteeList.Add(C)
            Next
            Return CommitteeList
        Else
            Return "none"
        End If


        Return ""
    End Function

    <WebMethod()> _
    Public Function LoadRooms()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT RoomID,RoomName From Room WHERE RoomNbr != '000'"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
        End Using

        Dim RoomList As New List(Of Room)
        If dt.Rows.Count > 0 Then
            RoomList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim R As New Room
                R.RoomID = item("RoomID")
                R.RoomName = item("RoomName")
                RoomList.Add(R)
            Next
            Return RoomList
        Else
            Return "none"
        End If


        Return ""
    End Function

    <WebMethod()> _
    Public Function LoadHearingTypes()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT HearingTypeID, HearingType FROM  HearingType"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
        End Using

        Dim HearingList As New List(Of HearingType)
        If dt.Rows.Count > 0 Then
            HearingList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim H As New HearingType
                H.HearingTypeID = item("HearingTypeID")
                H.HearingTypeName = item("HearingType")
                HearingList.Add(H)
            Next
            Return HearingList
        Else
            Return "none"
        End If
      

        Return ""
    End Function

    <WebMethod()> _
    Public Function DateEngine(ByVal EquipmentList As String, ByVal contactPhone As String, ByVal contactPerson As String, ByVal ComLongName As String, ByVal ComTypeID As String, ByVal CommitteeMeetingName As String, ByVal BookingFrequency As String, ByVal MultipleRoomBooking As String, ByVal MultipleRoomBookingDate As String, HearingTypeID As Integer, ByVal FormattedStartDate As String, ByVal FormattedEndDate As String, CommitteeID As String, ByVal RoomID As String, ByVal MeetingNotes As String)

        Dim NewStartDate As Date = CDate(FormattedStartDate)
        Dim NewEndDate As Date = CDate(FormattedEndDate)
        Dim MeetingDay As Date = NewStartDate.Date

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        MeetingNotes = MeetingNotes.Replace("@", "'")




        ' .. check if the room being booked is a double room. 
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM Room WHERE RoomID = " & RoomID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using


        ' .. get year id
        Dim dt2 As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT YearID FROM Year WHERE YearNbr = " & Date.Today.Year
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt2)
            End Using
            cmd.Connection.Close()
        End Using

        Dim TheYearID As Integer = dt2.Rows(0).Item(0)


        Dim dt3 As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT SessionID,ChamberCode,SystemCode FROM CommitteeType WHERE CommitteeTypeID = " & ComTypeID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt3)
            End Using
            cmd.Connection.Close()
        End Using

        Dim SessionID As Integer = dt3.Rows(0).Item("SessionID")
        Dim ChamberCode As String = dt3.Rows(0).Item("ChamberCode")
        Dim SystemCode As String = dt3.Rows(0).Item("SystemCode")

        Dim dt4 As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT BuildingID FROM Room WHERE RoomID =  " & RoomID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt4)
            End Using
            cmd.Connection.Close()
        End Using

        Dim BuildingID As Integer = dt4.Rows(0).Item("BuildingID")


        Dim MeetingList As New List(Of MeetingDates)

        Dim nbrOfMeetings As Integer = 1
        If MultipleRoomBooking = "yes" Then

            Dim FirstMeeting As Date = NewStartDate.Date
            Dim LastMeeting As Date = CDate(MultipleRoomBookingDate).Date
            Dim DayOfMeetings As String = FirstMeeting.Date.DayOfWeek

            While FirstMeeting <= LastMeeting

                If FirstMeeting.Date.DayOfWeek = DayOfMeetings Then

                    'Format Start and End Dates
                    ' year + "-" + month + "-" + day + " " + endTime


                    Dim msTime As DateTime = CDate(FormattedStartDate)
                    Dim msTimeString As String = msTime.TimeOfDay.ToString()
                    Dim msTimeFormatted As String = FirstMeeting.Date.Year & "-" & FirstMeeting.Date.Month & "-" & FirstMeeting.Date.Day & " " & msTimeString

                    Dim meTime As DateTime = CDate(FormattedEndDate)
                    Dim meTimeString As String = meTime.TimeOfDay.ToString()
                    Dim meTimeFormatted As String = FirstMeeting.Date.Year & "-" & FirstMeeting.Date.Month & "-" & FirstMeeting.Date.Day & " " & meTimeString

                    Dim MD As New MeetingDates
                    MD.MeetingDate = FirstMeeting
                    MD.FormattedStartDate = msTimeFormatted
                    MD.FormattedEndDate = meTimeFormatted
                    MeetingList.Add(MD)
                End If



                FirstMeeting = FirstMeeting.AddDays(1)

                'If BookingFrequency = "Weekly" Then



                'ElseIf BookingFrequency = "Bi-Weekly" Then

                '    FirstMeeting = FirstMeeting.AddDays(8)

                'Else
                '    'it's monthly
                '    FirstMeeting = FirstMeeting.AddMonths(1)

                'End If


            End While

        Else

            Dim MD As New MeetingDates
            MD.MeetingDate = MeetingDay
            MD.FormattedStartDate = FormattedStartDate
            MD.FormattedEndDate = FormattedEndDate
            MeetingList.Add(MD)

        End If

        Dim TheCommitteeID As Integer = 0

        If Trim(CommitteeMeetingName) = String.Empty Then
            TheCommitteeID = CommitteeID
        Else
            'We need to insert the new committee into the database.. Grab the new ID, then set TheCommitteeID equal to its value.

            Dim cmdtxt As String = "INSERT INTO Committee( SessionID, ChamberCode, [CommitteeName], [CommitteeTypeID], [LongName], [Description], " & _
                            "[CommitteeCode], [BuildingID], [RoomID], [MeetingDayTime], CanHavePublicMembers, NbrOfMembers, " & _
                            "[SystemCode],[Notes], [UserStamp],[DateTimeStamp]) " & _
                            " VALUES( " & SessionID & ", '" & ChamberCode & "', '" & CommitteeMeetingName & "', " & CInt(ComTypeID) & ", '" & ComLongName & "', 'Description'," & _
                                     " 'Com Code', " & BuildingID & ", " & RoomID & ", ' ', " & CByte(False) & "," & 400 & ", " & _
                                     " '" & SystemCode & "','Notes','Hadmin', '" & Date.Now & "') "

            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = cmdtxt
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            Dim dt5 As New DataTable
            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "SELECT CommitteeID FROM Committee WHERE LongName = '" & ComLongName & "'"
                Using da As New SqlDataAdapter
                    da.SelectCommand = cmd
                    da.Fill(dt5)
                End Using
                cmd.Connection.Close()
            End Using

            TheCommitteeID = dt5.Rows(0).Item("CommitteeID")

        End If



        If Trim(contactPerson) = "" Then contactPerson = " "
        If Trim(contactPhone) = "" Then contactPhone = " "


        For i As Integer = 0 To MeetingList.Count - 1

            If IsDBNull(dt.Rows(0).Item("Room1ID")) Or IsDBNull(dt.Rows(0).Item("Room2ID")) Then
                ' ... its a single room being booked ...
                Using cmd As SqlCommand = con.CreateCommand
                    cmd.Connection = con
                    cmd.Connection.Open()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "sproc_CreateNewCommitteeMeeting"
                    cmd.Parameters.AddWithValue("@meetingdate", MeetingList.Item(i).MeetingDate)
                    cmd.Parameters.AddWithValue("@committeeid", TheCommitteeID)
                    cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                    cmd.Parameters.AddWithValue("@starttime", MeetingList.Item(i).FormattedStartDate)
                    cmd.Parameters.AddWithValue("@endtime", MeetingList.Item(i).FormattedEndDate)
                    cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                    cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                    cmd.Parameters.AddWithValue("@yearid", TheYearID)
                    cmd.Parameters.AddWithValue("@contactPerson", contactPerson)
                    cmd.Parameters.AddWithValue("@contactPhone", contactPhone)
                    cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                    cmd.ExecuteNonQuery()
                    cmd.Connection.Close()
                End Using
            Else
                ' ... it is a double room being booked ... 
                Using cmd As SqlCommand = con.CreateCommand
                    cmd.Connection = con
                    cmd.Connection.Open()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "sproc_CreateNewCommitteeMeeting_DoubleRoom"
                    cmd.Parameters.AddWithValue("@meetingdate", MeetingList.Item(i).MeetingDate)
                    cmd.Parameters.AddWithValue("@committeeid", TheCommitteeID)
                    cmd.Parameters.AddWithValue("@room1ID", CInt(dt.Rows(0).Item("Room1ID")))
                    cmd.Parameters.AddWithValue("@room2ID", CInt(dt.Rows(0).Item("Room2ID")))
                    cmd.Parameters.AddWithValue("@starttime", MeetingList.Item(i).FormattedStartDate)
                    cmd.Parameters.AddWithValue("@endtime", MeetingList.Item(i).FormattedEndDate)
                    cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                    cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                    cmd.Parameters.AddWithValue("@yearid", TheYearID)
                    cmd.Parameters.AddWithValue("@contactPerson", contactPerson)
                    cmd.Parameters.AddWithValue("@contactPhone", contactPhone)
                    cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                    cmd.ExecuteNonQuery()
                    cmd.Connection.Close()
                End Using

            End If

        Next




        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function DateEngine2(ByVal EquipmentList As String, ByVal ContactName As String, ByVal ContactPhone As String, ByVal HearingTypeID As Integer, ByVal meetingId As Integer, ByVal FormattedStartDate As String, ByVal FormattedEndDate As String, CommitteeID As String, ByVal RoomID As String, ByVal MeetingNotes As String)

        Dim NewStartDate As Date = CDate(FormattedStartDate)
        Dim NewEndDate As Date = CDate(FormattedEndDate)
        Dim MeetingDay As Date = NewStartDate.Date

        MeetingNotes = MeetingNotes.Replace("@", "'")

        If Trim(ContactName) = "" Then ContactName = " "
        If Trim(ContactPhone) = "" Then ContactPhone = " "


        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)

        ' Possible Scenarios

        ' 1) Meeting is being changed from a single room meeting to a double room meeting

        ' 2) Meeting is being changed from a double room meeting to a single room meeting 

        ' 3) Meeting is in a double room and being changed to a new double room. 

        ' 4) Meeting is in a single room and being changed to a new single room. 

        ' 5) Meeting is in a double room and room is not changing

        ' 6) Meeting is in a single room and room is not changing 

        ' --------------------------------------------------------------------------------


        ' ... check if current meeting is schedules for single or double


        ' .. get year id
        Dim dt2 As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT YearID FROM Year WHERE YearNbr = " & Date.Today.Year
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt2)
            End Using
            cmd.Connection.Close()
        End Using

        Dim TheYearID As Integer = dt2.Rows(0).Item(0)




        Dim meetingDT As New DataTable
        Using cmd As New SqlCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM CommitteeMeeting WHERE CommitteeMeetingID = " & meetingId
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(meetingDT)
            End Using
            cmd.Connection.Close()
        End Using

        Dim currentMeetingIsInSingle As Boolean = False
        If IsDBNull(meetingDT.Rows(0).Item("isDoubleRoom")) Then
            currentMeetingIsInSingle = True
        Else
            currentMeetingIsInSingle = False
        End If

        Dim currentMeetingDateTime As String = meetingDT.Rows(0).Item("MeetingDateTime")
        Dim currentMeetingCommitteeID As Integer = meetingDT.Rows(0).Item("CommitteeID")
        Dim currentMeetingStartTime As String = meetingDT.Rows(0).Item("StartTime")
        Dim currentMeetingEndTime As String = meetingDT.Rows(0).Item("EndTime")


        ' ... check if passed-in room id is that of single or double room type.
        Dim roomDT As New DataTable
        Using cmd As New SqlCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM Room WHERE RoomID =" & RoomID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(roomDT)
            End Using
            cmd.Connection.Close()
        End Using



        Dim newRoomIsSingle As Boolean = Nothing


        ' new logic for checking if the room is single goes here...






        ' new logic for checking if the room is single ends here... 






        ' convoluted logic that needs to be replaced starts here...

        If IsDBNull(roomDT.Rows(0).Item("Room1ID")) AndAlso IsDBNull(roomDT.Rows(0).Item("Room2ID")) Then
            If currentMeetingIsInSingle = False Then
                newRoomIsSingle = False
            Else
                newRoomIsSingle = True
            End If

        Else
            If IsDBNull(roomDT.Rows(0).Item("Room1ID")) AndAlso IsDBNull(roomDT.Rows(0).Item("Room2ID")) Then
                If currentMeetingIsInSingle = False Then
                    If IsDBNull(roomDT.Rows(0).Item("Room1ID")) AndAlso IsDBNull(roomDT.Rows(0).Item("Room2ID")) Then
                        If CBool(meetingDT.Rows(0).Item("isDoubleRoom")) = True Then
                            newRoomIsSingle = False
                        Else
                            newRoomIsSingle = True
                        End If

                    Else
                        newRoomIsSingle = False
                    End If

                Else
                    newRoomIsSingle = True
                End If

            Else
                newRoomIsSingle = False
            End If

        End If

        ' convoluted logic that needs to be replaced ends here...




        Dim CurrentMeetingRoomID As Integer = meetingDT.Rows(0).Item("RoomID")
        Dim newRoom1ID As Integer = Nothing
        If Not IsDBNull(roomDT.Rows(0).Item("Room1ID")) Then newRoom1ID = roomDT.Rows(0).Item("Room1ID")
        Dim newRoom2ID As Integer = Nothing
        If Not IsDBNull(roomDT.Rows(0).Item("Room2ID")) Then newRoom2ID = roomDT.Rows(0).Item("Room2ID")

        ' SCENARIO 1: Meeting is being changed from a single room meeting to a double room meeting
        If currentMeetingIsInSingle = True AndAlso newRoomIsSingle = False Then

            ' Delete Single Room Meeting
            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "DELETE FROM CommitteeMeeting WHERE CommitteeMeetingID = " & meetingId
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            ' Create new double room meeting
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_CreateNewCommitteeMeeting_DoubleRoom"
                cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
                cmd.Parameters.AddWithValue("@committeeid", CInt(CommitteeID))
                cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                cmd.Parameters.AddWithValue("@room1ID", CInt(roomDT.Rows(0).Item("Room1ID")))
                cmd.Parameters.AddWithValue("@room2ID", CInt(roomDT.Rows(0).Item("Room2ID")))
                cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                cmd.Parameters.AddWithValue("@yearid", TheYearID)
                cmd.Parameters.AddWithValue("@contactPerson", ContactName)
                cmd.Parameters.AddWithValue("@contactPhone", ContactPhone)
                cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            Return "Hello World"

        End If

        ' SCENARIO 2: Meeting is being changed from a double room meeting to a single room meeting 
        If currentMeetingIsInSingle = False AndAlso newRoomIsSingle = True Then
            'Delete double.. 
            ' .. its a double, so we need to delete multiple room blocks
            Dim MeetingDate As String = meetingDT.Rows(0).Item("MeetingDateTime")
            Dim StartTime As String = meetingDT.Rows(0).Item("StartTime")
            Dim EndTime As String = meetingDT.Rows(0).Item("EndTime")
            Dim Committee As Integer = meetingDT.Rows(0).Item("CommitteeID")

            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "DELETE FROM CommitteeMeeting WHERE MeetingDateTime = '" + MeetingDate + "' AND StartTime = '" + StartTime + "' AND EndTime = '" + EndTime + "' AND CommitteeID = " & Committee
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            'Create Single
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_CreateNewCommitteeMeeting"
                cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
                cmd.Parameters.AddWithValue("@committeeid", CInt(CommitteeID))
                cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                cmd.Parameters.AddWithValue("@yearid", TheYearID)
                cmd.Parameters.AddWithValue("@contactPerson", ContactName)
                cmd.Parameters.AddWithValue("@contactPhone", ContactPhone)
                cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            Return "Hello World"


        End If

        ' SCENARIO 3: Meeting is in a double room and being changed to a new double room. 
        If currentMeetingIsInSingle = False AndAlso newRoomIsSingle = False AndAlso CurrentMeetingRoomID <> RoomID AndAlso CurrentMeetingRoomID <> newRoom1ID AndAlso CurrentMeetingRoomID <> newRoom2ID Then

            ' Delete the current double room booking
            Dim MeetingDate As String = meetingDT.Rows(0).Item("MeetingDateTime")
            Dim StartTime As String = meetingDT.Rows(0).Item("StartTime")
            Dim EndTime As String = meetingDT.Rows(0).Item("EndTime")
            Dim Committee As Integer = meetingDT.Rows(0).Item("CommitteeID")

            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "DELETE FROM CommitteeMeeting WHERE MeetingDateTime = '" + MeetingDate + "' AND StartTime = '" + StartTime + "' AND EndTime = '" + EndTime + "' AND CommitteeID = " & Committee
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            ' Create new double room meeting
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_CreateNewCommitteeMeeting_DoubleRoom"
                cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
                cmd.Parameters.AddWithValue("@committeeid", CInt(CommitteeID))
                cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                cmd.Parameters.AddWithValue("@room1ID", CInt(roomDT.Rows(0).Item("Room1ID")))
                cmd.Parameters.AddWithValue("@room2ID", CInt(roomDT.Rows(0).Item("Room2ID")))
                cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                cmd.Parameters.AddWithValue("@yearid", TheYearID)
                cmd.Parameters.AddWithValue("@contactPerson", ContactName)
                cmd.Parameters.AddWithValue("@contactPhone", ContactPhone)
                cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            Return "Hello World"
        End If


        'SCENARIO 4: Meeting is in a single room and being changed to a new single room. 
        ' or.. 
        'SCENARIO 6: Meeting is in a single room and room is not changing 
        If currentMeetingIsInSingle = True And newRoomIsSingle = True Then
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_EditCommitteeMeeting"
                cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
                cmd.Parameters.AddWithValue("@committeeid", CInt(CommitteeID))
                cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingid", meetingId)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                cmd.Parameters.AddWithValue("@contactPerson", ContactName)
                cmd.Parameters.AddWithValue("@contactPhone", ContactPhone)
                cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            Return "Hello World"
        End If

        ' SCENARIO 5: Meeting is in a double room and room is not changing
        If currentMeetingIsInSingle = False AndAlso newRoomIsSingle = False AndAlso CurrentMeetingRoomID = RoomID Or CurrentMeetingRoomID = newRoom1ID Or CurrentMeetingRoomID = newRoom2ID Then
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_EditCommitteeMeeting_DoubleRoom"
                cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
                cmd.Parameters.AddWithValue("@committeeid", CInt(CommitteeID))
                cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingid", meetingId)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)

                cmd.Parameters.AddWithValue("@oldMeetingDateTime", currentMeetingDateTime)
                cmd.Parameters.AddWithValue("@oldMeetingStart", currentMeetingStartTime)
                cmd.Parameters.AddWithValue("@oldMeetingEnd", currentMeetingEndTime)
                cmd.Parameters.AddWithValue("@oldCommitteeID", currentMeetingCommitteeID)
                cmd.Parameters.AddWithValue("@contactPerson", ContactName)
                cmd.Parameters.AddWithValue("@contactPhone", ContactPhone)

                cmd.Parameters.AddWithValue("@equipment", EquipmentList)

                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

            Return "Hello World"
        End If







        Dim dt As New DataTable




        '..check if the meeting that is about to be updated is a double room or not.. 
        ' .. this code block below is executed when the room has not changed. 

        If IsDBNull(dt.Rows(0).Item("isDoubleRoom")) Then

            '.. is single room.. 
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_EditCommitteeMeeting"
                cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
                cmd.Parameters.AddWithValue("@committeeid", CInt(CommitteeID))
                cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingid", meetingId)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using



        Else

            ' .. its a double room.. 


        End If







        If IsDBNull(dt.Rows(0).Item("Room1ID")) Or IsDBNull(dt.Rows(0).Item("Room2ID")) Then
            ' .. meeting is just booked in a single room ...
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_EditCommitteeMeeting"
                cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
                cmd.Parameters.AddWithValue("@committeeid", CInt(CommitteeID))
                cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingid", meetingId)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

        Else
            ' .. meeting is booked for a double room ...

            ' .. Need to get current data to use for where clause of the parent meeting..



            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_EditCommitteeMeeting_DoubleRoom"
                cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
                cmd.Parameters.AddWithValue("@committeeid", CInt(CommitteeID))
                cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingid", meetingId)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using
        End If






        Return "Hello World"
    End Function


    <WebMethod()> _
    Public Function GetMeetingDetails(ByVal CommitteeMeetingID As Integer)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = " SELECT CommitteeMeetingID,cm.CommitteeID,c.CommitteeTypeID,ISNULL(cm.MeetingNotes,'') MeetingNotes,cm.MeetingDateTime,cm.RoomID,cm.StartTime,cm.EndTime,cm.HearingTypeID,cm.ContactPerson,cm.ContactPhone,cm.isDoubleRoom,cm.EquipmentList FROM CommitteeMeeting cm INNER JOIN Committee c on c.CommitteeID = cm.CommitteeID WHERE CommitteeMeetingID = " & CommitteeMeetingID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
        End Using

        Dim CommitteeList As New List(Of Committee)
        If dt.Rows.Count > 0 Then
            CommitteeList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim C As New Committee
                C.CommitteeMeetingID = item("CommitteeMeetingID")
                C.CommitteeID = item("CommitteeID")
                C.CommitteeTypeID = item("CommitteeTypeID")
                C.MeetingDateTime = item("MeetingDateTime")
                C.DefaultRoomID = item("RoomID")
                C.StartTime = item("StartTime")
                C.EndTime = item("EndTime")
                C.MeetingNotes = item("MeetingNotes")
                C.HearingTypeID = item("HearingTypeID")

                If IsDBNull(item("EquipmentList")) Then
                    C.EquipmentList = " "
                Else
                    C.EquipmentList = item("EquipmentList")
                End If


                If IsDBNull(item("isDoubleRoom")) Then
                    C.ContactName = False
                Else
                    C.isDoubleRoom = CBool(item("isDoubleRoom"))
                End If



                If IsDBNull(item("ContactPerson")) Then
                    C.ContactName = " "
                Else
                    C.ContactName = item("ContactPerson")
                End If

                If IsDBNull(item("ContactPhone")) Then
                    C.ContactPhone = " "
                Else
                    C.ContactPhone = item("ContactPhone")
                End If

                CommitteeList.Add(C)
            Next
            Return CommitteeList
        Else
            Return "none"
        End If

        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function MeetingResized(ByVal MeetingID As Integer, ByVal StartTime As String, ByVal EndTime As String)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "UPDATE CommitteeMeeting SET StartTime = '" & StartTime & "' , EndTime = '" & EndTime & "' WHERE CommitteeMeetingID = " & MeetingID
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End Using

        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function GetBillList(ByVal CommitteeMeetingID As Integer)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = " SELECT LegislationID FROM AgendaSupport WHERE CommitteeMeetingID = " & CommitteeMeetingID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows.Count > 0 Then
            Dim list As New List(Of Integer)
            For Each row As DataRow In dt.Rows()
                list.Add(row("LegislationID"))
            Next

            Dim BillList As String = String.Join(",", list.ToArray())

            dt.Clear()
            dt.Columns.Clear()

            Dim commandtext As String = " SELECT cm.CommitteeMeetingID, a.AgendaSupportTitle , l.LegislationNbr , d.DocumentTypeCode " & _
                                        " FROM CommitteeMeeting cm " & _
                                        " INNER JOIN AgendaSupport a on cm.CommitteeMeetingID = a.CommitteeMeetingID " & _
                                        " INNER JOIN Legislation l on a.LegislationID = l.LegislationID " & _
                                        " INNER JOIN DocumentType d on l.DocumentTypeID = d.DocumentTypeID " & _
                                        " WHERE cm.CommitteeMeetingID = " + CStr(CommitteeMeetingID) + " AND l.LegislationID IN (" + BillList + ") "

            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = commandtext
                Using da As New SqlDataAdapter
                    da.SelectCommand = cmd
                    da.Fill(dt)
                End Using
                cmd.Connection.Close()
            End Using

            Dim HTML As New StringBuilder
            HTML.Append("<ul>")
            For Each row As DataRow In dt.Rows()
                HTML.Append("<li><b>" + row("AgendaSupportTitle") + " --> </b>" + row("DocumentTypeCode") + "" + row("LegislationNbr") + "</li>")
            Next
            HTML.Append("</ul>")

            Return HTML.ToString()
        Else
            Return " "
        End If

    End Function

    <WebMethod()> _
    Public Function DeleteMeeting(ByVal MeetingID As Integer)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        'Need to check if meeting is a double meeting. 
        Using cmd As New SqlCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM CommitteeMeeting WHERE CommitteeMeetingID = " & MeetingID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If IsDBNull(dt.Rows(0).Item("isDoubleRoom")) Then

            '.. its a single meeting
            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "DELETE FROM CommitteeMeeting WHERE CommitteeMeetingID = " & MeetingID
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

        Else

            ' .. its a double, so we need to delete multiple room blocks
            Dim MeetingDate As String = dt.Rows(0).Item("MeetingDateTime")
            Dim StartTime As String = dt.Rows(0).Item("StartTime")
            Dim EndTime As String = dt.Rows(0).Item("EndTime")
            Dim CommitteeID As Integer = dt.Rows(0).Item("CommitteeID")

            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "DELETE FROM CommitteeMeeting WHERE MeetingDateTime = '" + MeetingDate + "' AND StartTime = '" + StartTime + "' AND EndTime = '" + EndTime + "' AND CommitteeID = " & CommitteeID
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using

        End If





        

        Return ""
    End Function

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function







End Class