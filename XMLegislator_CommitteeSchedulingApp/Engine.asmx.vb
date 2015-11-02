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
        Public ChamberCode As String
        Public CommitteeName As String
        Public DefaultRoomID As Integer
    End Class

    Public Class Room
        Public RoomID As Integer
        Public RoomName As String
    End Class




    <WebMethod()> _
    Public Function LoadCommittees()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT CommitteeID, ChamberCode, CommitteeName, RoomID FROM Committee"
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
            cmd.CommandText = "SELECT RoomID,RoomName From Room WHERE RoomNbr != 000"
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
    Public Function DateEngine(ByVal FormattedStartDate As String, ByVal FormattedEndDate As String, CommitteeID As String, ByVal RoomID As String)

        Dim NewStartDate As Date = CDate(FormattedStartDate)
        Dim NewEndDate As Date = CDate(FormattedEndDate)
        Dim MeetingDay As Date = NewStartDate.Date

        ' added sproc_CreateNewCommitteeMeeting

        '@meetingdate datetime
        '@committeeid
        '@roomid int
        '@starttime int
        '@endtime int

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
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End Using

        Return "Hello World"
    End Function


    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

End Class