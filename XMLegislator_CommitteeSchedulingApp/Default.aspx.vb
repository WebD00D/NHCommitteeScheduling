﻿Imports System.Data.SqlClient
Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadResources()
            DayPilotScheduler1.StartDate = New Date(Date.Today.Year, Date.Today.Month, Date.Today.Day)
            DayPilotScheduler1.Days = Date.DaysInMonth(Date.Today.Year, Date.Today.Month) - Date.Today.Day

            DayPilotScheduler1.DataSource = DbGetEvents(DayPilotScheduler1.StartDate, DayPilotScheduler1.Days)
            DayPilotScheduler1.DataBind()
            DayPilotScheduler1.SetScrollX(Date.Today)
        End If
    End Sub

    Private Sub LoadResources()
        DayPilotScheduler1.Resources.Clear()
        Dim da As New SqlDataAdapter("SELECT [RoomID], [RoomName] FROM [room]", ConfigurationManager.ConnectionStrings("connex").ConnectionString)
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

        Dim da As New SqlDataAdapter("SELECT [CommitteeMeetingID], [StartTime], [EndTime], cm.RoomID, c.CommitteeName FROM [CommitteeMeeting] cm INNER JOIN Committee c on cm.CommitteeID = c.CommitteeID WHERE NOT (([EndTime] <= @start) OR ([StartTime] >= @end))", ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        da.SelectCommand.Parameters.AddWithValue("start", start)
        da.SelectCommand.Parameters.AddWithValue("end", start.AddDays(days))
        Dim dt As New DataTable()
        da.Fill(dt)
        Return dt

    End Function

    Private Sub DbUpdateEvent(ByVal id As String, ByVal start As Date, ByVal [end] As Date, ByVal resource As String)
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("DayPilot").ConnectionString)
            con.Open()
            Dim cmd As New SqlCommand("UPDATE [event] SET eventstart = @start, eventend = @end, resource_id = @resource WHERE id = @id", con)
            cmd.Parameters.AddWithValue("id", id)
            cmd.Parameters.AddWithValue("start", start)
            cmd.Parameters.AddWithValue("end", [end])
            cmd.Parameters.AddWithValue("resource", resource)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class