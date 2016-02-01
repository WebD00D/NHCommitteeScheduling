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
    End Class


    Public Class Room
        Public RoomID As Integer
        Public RoomName As String
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
            cmd.CommandText = "SELECT CommitteeID, ChamberCode, CommitteeName, RoomID FROM Committee WHERE CommitteeTypeID = " & CommitteeTypeID
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
    Public Function DateEngine(ByVal FormattedStartDate As String, ByVal FormattedEndDate As String, CommitteeID As String, ByVal RoomID As String, ByVal MeetingNotes As String)

        Dim NewStartDate As Date = CDate(FormattedStartDate)
        Dim NewEndDate As Date = CDate(FormattedEndDate)
        Dim MeetingDay As Date = NewStartDate.Date

        ' added sproc_CreateNewCommitteeMeeting

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
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
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End Using

        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function DateEngine2(ByVal meetingId As Integer, ByVal FormattedStartDate As String, ByVal FormattedEndDate As String, CommitteeID As String, ByVal RoomID As String, ByVal MeetingNotes As String)

        Dim NewStartDate As Date = CDate(FormattedStartDate)
        Dim NewEndDate As Date = CDate(FormattedEndDate)
        Dim MeetingDay As Date = NewStartDate.Date

        ' added sproc_EditNewCommitteeMeeting

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
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
            cmd.CommandText = " SELECT CommitteeMeetingID,cm.CommitteeID,c.CommitteeTypeID,ISNULL(cm.MeetingNotes,'') MeetingNotes,cm.MeetingDateTime,cm.RoomID,cm.StartTime,cm.EndTime FROM CommitteeMeeting cm INNER JOIN Committee c on c.CommitteeID = cm.CommitteeID WHERE CommitteeMeetingID = " & CommitteeMeetingID
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
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function



 



End Class