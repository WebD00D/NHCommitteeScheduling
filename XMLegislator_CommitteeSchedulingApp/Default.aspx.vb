Imports System.Data.SqlClient
Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadResources()


            If Not IsNothing(Session("workingOnDate")) Then
                Dim TheDate As Date = CDate(Session("workingOnDate"))
                DayPilotScheduler1.StartDate = New Date(TheDate.Year, TheDate.Month, TheDate.Day)
            Else
                DayPilotScheduler1.StartDate = New Date(Date.Today.Year, Date.Today.Month, Date.Today.Day)
            End If

            DayPilotScheduler1.Days = 31 'Year.Days(DateTime.Today.Year)
            DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
            DayPilotScheduler1.DataBind()
            '  DayPilotScheduler1.SetScrollX(Date.Today)

        Else
            If Not IsNothing(Session("workingOnDate")) Then
                Dim TheDate As Date = CDate(Session("workingOnDate"))
                DayPilotScheduler1.StartDate = New Date(TheDate.Year, TheDate.Month, TheDate.Day)

            End If
        End If
        DayPilotScheduler1.Days = 31 'Year.Days(DateTime.Today.Year)
        DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
        DayPilotScheduler1.DataBind()


    End Sub

    Private Sub LoadResources()
        DayPilotScheduler1.Resources.Clear()
        Dim da As New SqlDataAdapter("SELECT [RoomID], [RoomName] FROM [room] WHERE Room1ID IS NULL AND Room2ID IS NULL", ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable()
        da.Fill(dt)

        For Each r As DataRow In dt.Rows
            Dim name As String = DirectCast(r("RoomName"), String)
            Dim id_Renamed As String = Convert.ToString(r("RoomID"))
            If Not id_Renamed = "1" Then
                DayPilotScheduler1.Resources.Add(name, id_Renamed)
            End If

        Next r
    End Sub

    Protected Sub DayPilotScheduler1_EventMove(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventMoveEventArgs) Handles DayPilotScheduler1.EventMove
        Dim id_Renamed As String = e.Id
        Dim start As Date = e.NewStart
        Dim [end] As Date = e.NewEnd
        Dim resource As String = e.NewResource

        DbUpdateEvent(id_Renamed, start, [end], resource)

        DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
        DayPilotScheduler1.DataBind()
        DayPilotScheduler1.Update()
    End Sub

    Private Function DbGetEvents(ByVal start As Date, ByVal days As Integer) As DataTable

        Dim da As New SqlDataAdapter("SELECT CommitteeMeetingID,StartTime,EndTime, cm.RoomID,r.RoomNbr,c.CommitteeName FROM CommitteeMeeting cm INNER JOIN Committee c on cm.CommitteeID = c.CommitteeID INNER JOIN Room r on cm.RoomID = r.RoomID WHERE NOT (([EndTime] <= @start) OR ([StartTime] >= @end)) AND Released = 1", ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        da.SelectCommand.Parameters.AddWithValue("start", start)
        da.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days))

        Dim tempDT As New DataTable()
        da.Fill(tempDT)


        ' We need to fill a temp table, and check for double room meetings by parsing the room number. If row contains
        ' a double meeting then take get the individual rooms from that and add them as two "fake" meetings to the scheduler.

        Dim dt As New DataTable()
        dt.Columns.Add("CommitteeMeetingID")
        dt.Columns.Add("StartTime")
        dt.Columns.Add("EndTime")
        dt.Columns.Add("RoomID")
        dt.Columns.Add("RoomNbr")
        dt.Columns.Add("CommitteeName")


        Dim da2 As New SqlDataAdapter("SELECT cm.ConferenceCommitteeID As CommitteeMeetingID,cm.StartTime,cm.EndTime, cm.RoomID,r.RoomNbr,l.ExpandedBillNbr + ' CofC' As CommitteeName FROM ConferenceCommitteeMeeting cm INNER JOIN ConferenceCommittee c on cm.ConferenceCommitteeID = c.ConferenceCommitteeID INNER JOIN Legislation l on c.LegislationID = l.LegislationID INNER JOIN Room r on cm.RoomID = r.RoomID WHERE NOT (([EndTime] <= @start) OR ([StartTime] >= @end)) AND Released = 1", ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        da2.SelectCommand.Parameters.AddWithValue("start", start)
        da2.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days))

        Dim CommitteeConferenceTempDT As New DataTable()
        da2.Fill(CommitteeConferenceTempDT)

        If CommitteeConferenceTempDT.Rows.Count > 1 Then
            For Each conferenceMeeting As DataRow In CommitteeConferenceTempDT.Rows
                Dim meetingRow As DataRow = dt.NewRow()
                meetingRow("CommitteeMeetingID") = conferenceMeeting("CommitteeMeetingID")
                meetingRow("StartTime") = conferenceMeeting("StartTime")
                meetingRow("EndTime") = conferenceMeeting("EndTime")
                meetingRow("RoomID") = conferenceMeeting("RoomID")
                meetingRow("RoomNbr") = conferenceMeeting("RoomNbr")
                meetingRow("CommitteeName") = conferenceMeeting("CommitteeName")
                dt.Rows.Add(meetingRow)
            Next
        End If


        For Each meeting As DataRow In tempDT.Rows

            Dim meetingRow As DataRow = dt.NewRow()

            If meeting("RoomNbr").ToString.Contains("-") Then

                'Get the two room id's from the parse room number.

                Dim RoomNumber As String = meeting("RoomNbr")
                Dim RoomArr As String() = RoomNumber.Split("-")
                Dim RoomOne As String = RoomArr(0)
                Dim RoomTwo As String = RoomArr(1)


                Dim roomOneAdapter As New SqlDataAdapter("SELECT RoomID FROM Room WHERE RoomNbr ='" & RoomOne & "'", ConfigurationManager.ConnectionStrings("connex").ConnectionString)
                Dim roomOneDT As New DataTable
                roomOneAdapter.Fill(roomOneDT)

                Dim roomTwoAdapter As New SqlDataAdapter("SELECT RoomID FROM Room WHERE RoomNbr ='" & RoomTwo & "'", ConfigurationManager.ConnectionStrings("connex").ConnectionString)
                Dim roomTwoDT As New DataTable
                roomTwoAdapter.Fill(roomTwoDT)



                'Add First Meeting
                meetingRow("CommitteeMeetingID") = meeting("CommitteeMeetingID")
                meetingRow("StartTime") = meeting("StartTime")
                meetingRow("EndTime") = meeting("EndTime")
                meetingRow("RoomID") = roomOneDT.Rows(0).Item("RoomID")
                meetingRow("RoomNbr") = RoomOne
                meetingRow("CommitteeName") = meeting("CommitteeName")

                dt.Rows.Add(meetingRow)

                Dim meetingRow2 As DataRow = dt.NewRow()
                'Add Second Meeting
                meetingRow2("CommitteeMeetingID") = meeting("CommitteeMeetingID")
                meetingRow2("StartTime") = meeting("StartTime")
                meetingRow2("EndTime") = meeting("EndTime")
                meetingRow2("RoomID") = roomTwoDT.Rows(0).Item("RoomID")
                meetingRow2("RoomNbr") = RoomTwo
                meetingRow2("CommitteeName") = meeting("CommitteeName")
                dt.Rows.Add(meetingRow2)
            Else

                meetingRow("CommitteeMeetingID") = meeting("CommitteeMeetingID")
                meetingRow("StartTime") = meeting("StartTime")
                meetingRow("EndTime") = meeting("EndTime")
                meetingRow("RoomID") = meeting("RoomID")
                meetingRow("RoomNbr") = meeting("RoomNbr")
                meetingRow("CommitteeName") = meeting("CommitteeName")
                dt.Rows.Add(meetingRow)
            End If





        Next

        



        Return dt



    End Function

    Private Sub DbUpdateEvent(ByVal id As String, ByVal start As Date, ByVal [end] As Date, ByVal resource As String)
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
            con.Open()
            Dim cmd As New SqlCommand("UPDATE [CommitteeMeeting] SET StartTime = @start, EndTime = @end, RoomID = @resource WHERE CommitteeMeetingID = @id", con)
            cmd.Parameters.AddWithValue("id", id)
            cmd.Parameters.AddWithValue("start", start)
            cmd.Parameters.AddWithValue("end", [end])
            cmd.Parameters.AddWithValue("resource", resource)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    
    Protected Sub lnkGoToDate_Click(sender As Object, e As EventArgs) Handles lnkGoToDate.Click
        If IsPostBack Then
            If Trim(dpJumpToDate.Text) = String.Empty Then
                LoadResources()
                DayPilotScheduler1.StartDate = New Date(Date.Today.Year, Date.Today.Month, Date.Today.Day)
                ' DayPilotScheduler1.Days = Date.DaysInMonth(Date.Today.Year, Date.Today.Month) - Date.Today.Day
                DayPilotScheduler1.Days = 31 'Year.Days(DateTime.Today.Year)


                DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
                DayPilotScheduler1.DataBind()
                DayPilotScheduler1.SetScrollX(Date.Today)
                Exit Sub
            End If

            LoadResources()
            Dim d = CDate(dpJumpToDate.Text())
            Dim DateToSet = New Date(d.Year, d.Month, d.Day) ' Year Month Day
            DayPilotScheduler1.StartDate = New Date(d.Year, d.Month, d.Day)
            ' DayPilotScheduler1.Days = Date.DaysInMonth(Date.Today.Year, Date.Today.Month) - Date.Today.Day
            DayPilotScheduler1.Days = 31 'Year.Days(DateTime.Today.Year)


            DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
            DayPilotScheduler1.DataBind()



            DayPilotScheduler1.SetScrollX(DateToSet)
            dpJumpToDate.Text = " "
        End If
     
    End Sub

    Protected Sub lnkResetDate_Click(sender As Object, e As EventArgs) Handles lnkResetDate.Click

        LoadResources()
        DayPilotScheduler1.StartDate = New Date(Date.Today.Year, Date.Today.Month, Date.Today.Day)
        ' DayPilotScheduler1.Days = Date.DaysInMonth(Date.Today.Year, Date.Today.Month) - Date.Today.Day
        DayPilotScheduler1.Days = 31 'Year.Days(DateTime.Today.Year)


        DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
        DayPilotScheduler1.DataBind()
        DayPilotScheduler1.SetScrollX(Date.Today)


    End Sub

    Protected Sub lnkShowSaturdaySunday_Click(sender As Object, e As EventArgs) Handles lnkShowSaturdaySunday.Click
        LoadResources()
        DayPilotScheduler1.StartDate = New Date(Date.Today.Year, Date.Today.Month, Date.Today.Day)
        ' DayPilotScheduler1.Days = Date.DaysInMonth(Date.Today.Year, Date.Today.Month) - Date.Today.Day
        DayPilotScheduler1.Days = 31 'Year.Days(DateTime.Today.Year)
        DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
        DayPilotScheduler1.DataBind()
        DayPilotScheduler1.SetScrollX(Date.Today)
        DayPilotScheduler1.ShowNonBusiness = True
       
    End Sub

    Protected Sub lnkResetBusinessDay_Click(sender As Object, e As EventArgs) Handles lnkResetBusinessDay.Click
        LoadResources()
        DayPilotScheduler1.StartDate = New Date(Date.Today.Year, Date.Today.Month, Date.Today.Day)
        ' DayPilotScheduler1.Days = Date.DaysInMonth(Date.Today.Year, Date.Today.Month) - Date.Today.Day
        DayPilotScheduler1.Days = 31 'Year.Days(DateTime.Today.Year)
        DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
        DayPilotScheduler1.DataBind()
        DayPilotScheduler1.SetScrollX(Date.Today)
        DayPilotScheduler1.ShowNonBusiness = False
        DayPilotScheduler1.BusinessEndsHour = 21
        DayPilotScheduler1.BusinessBeginsHour = 7
        
    End Sub
End Class