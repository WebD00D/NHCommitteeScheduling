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
        Public IsConfidential As Boolean
    End Class

    Public Class Room
        Public RoomID As Integer
        Public RoomName As String
    End Class

    Public Class HearingType
        Public HearingTypeID As Integer
        Public HearingTypeName As String
        Public ChamberCode As String
    End Class

    Public Class MeetingDates
        Public MeetingDate As Date
        Public FormattedStartDate As String
        Public FormattedEndDate As String
    End Class

    <WebMethod()> _
    Public Function checkCommitteeName(ByVal enteredName As String)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT CommitteeName, LongName FROM Committee WHERE CommitteeName LIKE '%" & enteredName & "%' OR LongName LIKE '%" & enteredName & "%'"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
        End Using

        If dt.Rows.Count = 0 Then
            Return "empty"
        Else
            Return "exists"
        End If


    End Function

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
            cmd.CommandText = "SELECT HearingTypeID, HearingType, ChamberCode FROM  HearingType ORDER BY ChamberCode"
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
                H.ChamberCode = item("ChamberCode")
                HearingList.Add(H)
            Next
            Return HearingList
        Else
            Return "none"
        End If
      
        Return ""
    End Function

    <WebMethod(True)> _
    Public Function DateEngine(ByVal IsConfidential As Boolean, ByVal EquipmentList As String, ByVal contactPhone As String, ByVal contactPerson As String, ByVal ComLongName As String, ByVal ComTypeID As String, ByVal CommitteeMeetingName As String, ByVal BookingFrequency As String, ByVal MultipleRoomBooking As String, ByVal MultipleRoomBookingDate As String, HearingTypeID As Integer, ByVal FormattedStartDate As String, ByVal FormattedEndDate As String, CommitteeID As String, ByVal RoomID As String, ByVal MeetingNotes As String)

        Dim NewStartDate As Date = CDate(FormattedStartDate)
        Dim NewEndDate As Date = CDate(FormattedEndDate)
        Dim MeetingDay As Date = NewStartDate.Date
        Session("workingOnDate") = MeetingDay

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable


        MeetingNotes = MeetingNotes.Replace("@", "''")
        ComLongName = ComLongName.Replace("@", "''")
        CommitteeMeetingName = CommitteeMeetingName.Replace("@", "''")


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

        Dim isDoubleRoom As Boolean = False
        If CStr(dt.Rows(0).Item("RoomNbr")).Contains("-") Then
            isDoubleRoom = True
        End If



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


        ' //////////////////  NEW MULTIPLE MEETING LOGIC  //////////////////////////////

        Dim NewMeetingList As New List(Of MeetingDates)
        
        If MultipleRoomBooking = "yes" Then

            Dim MeetingDayOfWeek = NewStartDate.DayOfWeek

            Select Case BookingFrequency
                Case "Weekly"
                    'Get all Nth days of every week between the two dates

                    Do While NewStartDate.Date <= CDate(MultipleRoomBookingDate).Date

                        If NewStartDate.DayOfWeek = MeetingDayOfWeek Then

                            Dim msTime As DateTime = CDate(FormattedStartDate)
                            Dim msTimeString As String = msTime.TimeOfDay.ToString()
                            Dim msTimeFormatted As String = NewStartDate.Date.Year & "-" & NewStartDate.Date.Month & "-" & NewStartDate.Date.Day & " " & msTimeString

                            Dim meTime As DateTime = CDate(FormattedEndDate)
                            Dim meTimeString As String = meTime.TimeOfDay.ToString()
                            Dim meTimeFormatted As String = NewStartDate.Date.Year & "-" & NewStartDate.Date.Month & "-" & NewStartDate.Date.Day & " " & meTimeString

                            Dim MD As New MeetingDates
                            MD.MeetingDate = NewStartDate.Date
                            MD.FormattedStartDate = msTimeFormatted
                            MD.FormattedEndDate = meTimeFormatted
                            NewMeetingList.Add(MD)
                        End If

                        NewStartDate = NewStartDate.AddDays(1)

                    Loop


                Case "Bi-Weekly"
                    'Get every other Nth day between the two dates
                    Dim SkipNextMatch = False
                    Do While NewStartDate.Date <= CDate(MultipleRoomBookingDate).Date

                        If NewStartDate.DayOfWeek = MeetingDayOfWeek Then

                            Dim msTime As DateTime = CDate(FormattedStartDate)
                            Dim msTimeString As String = msTime.TimeOfDay.ToString()
                            Dim msTimeFormatted As String = NewStartDate.Date.Year & "-" & NewStartDate.Date.Month & "-" & NewStartDate.Date.Day & " " & msTimeString

                            Dim meTime As DateTime = CDate(FormattedEndDate)
                            Dim meTimeString As String = meTime.TimeOfDay.ToString()
                            Dim meTimeFormatted As String = NewStartDate.Date.Year & "-" & NewStartDate.Date.Month & "-" & NewStartDate.Date.Day & " " & meTimeString

                            If SkipNextMatch Then
                                SkipNextMatch = False
                            Else
                                Dim MD As New MeetingDates
                                MD.MeetingDate = NewStartDate.Date
                                MD.FormattedStartDate = msTimeFormatted
                                MD.FormattedEndDate = meTimeFormatted
                                NewMeetingList.Add(MD)
                                SkipNextMatch = True
                            End If

                        End If

                        NewStartDate = NewStartDate.AddDays(1)

                    Loop

                Case "Monthly"
                    'Get every Nth day of the month between the two dates.



            End Select

        Else

            Dim msTime As DateTime = CDate(FormattedStartDate)
            Dim msTimeString As String = msTime.TimeOfDay.ToString()
            Dim msTimeFormatted As String = NewStartDate.Date.Year & "-" & NewStartDate.Date.Month & "-" & NewStartDate.Date.Day & " " & msTimeString

            Dim meTime As DateTime = CDate(FormattedEndDate)
            Dim meTimeString As String = meTime.TimeOfDay.ToString()
            Dim meTimeFormatted As String = NewStartDate.Date.Year & "-" & NewStartDate.Date.Month & "-" & NewStartDate.Date.Day & " " & meTimeString

            Dim MD As New MeetingDates
            MD.MeetingDate = NewStartDate.Date
            MD.FormattedStartDate = msTimeFormatted
            MD.FormattedEndDate = meTimeFormatted
            NewMeetingList.Add(MD)
        End If

      

        ' //////////////////  END NEW MULTIPLE MEETING LOGIC  //////////////////////////////


  

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


        For i As Integer = 0 To NewMeetingList.Count - 1


            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sproc_CreateNewCommitteeMeeting"
                cmd.Parameters.AddWithValue("@meetingdate", NewMeetingList.Item(i).MeetingDate)
                cmd.Parameters.AddWithValue("@committeeid", TheCommitteeID)
                cmd.Parameters.AddWithValue("@roomid", CInt(RoomID))
                cmd.Parameters.AddWithValue("@starttime", NewMeetingList.Item(i).FormattedStartDate)
                cmd.Parameters.AddWithValue("@endtime", NewMeetingList.Item(i).FormattedEndDate)
                cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
                cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
                cmd.Parameters.AddWithValue("@yearid", TheYearID)
                cmd.Parameters.AddWithValue("@contactPerson", contactPerson)
                cmd.Parameters.AddWithValue("@contactPhone", contactPhone)
                cmd.Parameters.AddWithValue("@equipment", EquipmentList)
                cmd.Parameters.AddWithValue("@isconfidential", CByte(IsConfidential))
                cmd.Parameters.AddWithValue("@isdoubleRoom", CByte(isDoubleRoom))
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using


        Next


        Return "Hello World"
    End Function


    <WebMethod()> _
    Public Function DateEngine2(ByVal IsConfidential As Boolean, ByVal EquipmentList As String, ByVal ContactName As String, ByVal ContactPhone As String, ByVal HearingTypeID As Integer, ByVal meetingId As Integer, ByVal FormattedStartDate As String, ByVal FormattedEndDate As String, CommitteeID As String, ByVal RoomID As String, ByVal MeetingNotes As String)

        Dim NewStartDate As Date = CDate(FormattedStartDate)
        Dim NewEndDate As Date = CDate(FormattedEndDate)
        Dim MeetingDay As Date = NewStartDate.Date

        MeetingNotes = MeetingNotes.Replace("@", "'")

        If Trim(ContactName) = "" Then ContactName = " "
        If Trim(ContactPhone) = "" Then ContactPhone = " "


        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)


        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT RoomNbr FROM Room WHERE RoomID =" & RoomID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        Dim RoomNbr As String = dt.Rows(0).Item("RoomNbr")
        Dim isDoubleRoom As Boolean = False
        If RoomNbr.Contains("-") Then
            isDoubleRoom = True
        End If

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

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sproc_EditCommitteeMeeting"
            cmd.Parameters.AddWithValue("@meetingid", meetingId)
            cmd.Parameters.AddWithValue("@meetingdate", MeetingDay)
            cmd.Parameters.AddWithValue("@committeeid", CommitteeID)
            cmd.Parameters.AddWithValue("@roomid", RoomID)
            cmd.Parameters.AddWithValue("@starttime", FormattedStartDate)
            cmd.Parameters.AddWithValue("@endtime", FormattedEndDate)
            cmd.Parameters.AddWithValue("@meetingnotes", MeetingNotes)
            cmd.Parameters.AddWithValue("@hearingid", HearingTypeID)
            cmd.Parameters.AddWithValue("@contactPerson", ContactName)
            cmd.Parameters.AddWithValue("@contactPhone", ContactPhone)
            cmd.Parameters.AddWithValue("@equipment", EquipmentList)
            cmd.Parameters.AddWithValue("@isconfidential", CByte(IsConfidential))
            cmd.Parameters.AddWithValue("@isDoubleRoom", CByte(isDoubleRoom))
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End Using

        Return "Hello World"
    End Function


    <WebMethod()> _
    Public Function GetMeetingDetails(ByVal CommitteeMeetingID As Integer)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = " SELECT CommitteeMeetingID,cm.CommitteeID,c.CommitteeTypeID,ISNULL(cm.MeetingNotes,'') MeetingNotes,cm.MeetingDateTime,cm.RoomID,cm.StartTime,cm.EndTime,cm.HearingTypeID,cm.ContactPerson,cm.ContactPhone,cm.isDoubleRoom,cm.EquipmentList,cm.IsConfidential FROM CommitteeMeeting cm INNER JOIN Committee c on c.CommitteeID = cm.CommitteeID WHERE CommitteeMeetingID = " & CommitteeMeetingID
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

                If IsDBNull(item("IsConfidential")) Then
                    C.IsConfidential = False
                Else
                    C.IsConfidential = CBool(item("IsConfidential"))
                End If

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

 
        '   Dim commandtext As String = " SELECT cm.CommitteeMeetingID, a.AgendaSupportTitle , l.LegislationNbr , d.DocumentTypeCode " & _
        '                              " FROM CommitteeMeeting cm " & _
        '                             " INNER JOIN AgendaSupport a on cm.CommitteeMeetingID = a.CommitteeMeetingID " & _
        '                             " INNER JOIN Legislation l on a.LegislationID = l.LegislationID " & _
        '                           " INNER JOIN DocumentType d on l.DocumentTypeID = d.DocumentTypeID " & _
        '                           " WHERE cm.CommitteeMeetingID = " + CStr(CommitteeMeetingID) + " AND l.LegislationID IN (" + BillList + ") "

        Dim commandtext As String = " SELECT a.CommitteeMeetingID,l.LegislationNbr,d.DocumentTypeCode, a.AgendaSupportTitle " & _
                                    " FROM AgendaSupport a " & _
                                    " INNER JOIN JoinLegislationAgendaSupport jls on a.AgendaSupportID = jls.AgendaSupportID" & _
                                    " INNER JOIN Legislation l on jls.LegislationID = l.LegislationID " & _
                                    " INNER JOIN DocumentType d on l.DocumentTypeID = d.DocumentTypeID " & _
                                    " INNER JOIN CommitteeMeeting cm on a.CommitteeMeetingID = cm.CommitteeMeetingID" & _
                                    " WHERE a.CommitteeMeetingID =  " + CStr(CommitteeMeetingID) + " AND Released = 1"

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


        If dt.Rows.Count > 0 Then
            Dim HTML As New StringBuilder
            HTML.Append("<ul>")
            For Each row As DataRow In dt.Rows()
                HTML.Append("<li><b>--> " + row("AgendaSupportTitle") + " </b> </li>")
            Next
            HTML.Append("</ul>")

            Return HTML.ToString()
        Else
            Return ""
        End If

      
      

    End Function

    <WebMethod()> _
    Public Function DeleteMeeting(ByVal MeetingID As Integer)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable


        Using cmd As New SqlCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "DELETE FROM JoinLegislationAgendaSupport WHERE AgendaSupportID IN (SELECT AgendaSupportId FROM AgendaSupport WHERE CommitteeMeetingId = " & MeetingID & ")"
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End Using
        Using cmd As New SqlCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "DELETE FROM CommitteeMeeting WHERE CommitteeMeetingID = " & MeetingID
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End Using

        Return ""
    End Function

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function







End Class