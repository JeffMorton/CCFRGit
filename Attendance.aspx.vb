Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Public Class Attendence
    Inherits PageBase
    'This is the member section attendance report.  This only show the next event either lunch or dinner as selected from the menu by the user. 
    'There is also a Amin section attendance report Attend.aspx.  It provides a report for the date selected in Adminstart. 
    'Both reports use the same attendence.rdlc.

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strConnectionString As String = GetConnectionString(False, False)
        Dim Conn As SqlConnection = New SqlConnection(strConnectionString)
        Dim strSQL As String
        Dim etype As String
        Dim Eventdate As Date
        Dim EventID As Integer
        Try
            Conn.Open()
        Catch
            'Response.Redirect("Default.aspx")
        End Try
        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "Attendance.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.ShowExportControls = True
        etype = Request.QueryString("Type")

        If etype = "Lunch" Then
            strSQL = "select ID,eventdate from Event where EventDate >= @Now and Type =  'Lunch/Discussion' order by EventDate "
        Else
            strSQL = "Select ID,eventdate from Event where EventDate >= @Now and Type =  'Dinner' order by eventdate"
        End If
        Using cmd As New SqlCommand(strSQL, Conn)
            cmd.Parameters.AddWithValue("@Now", Now().ToShortDateString)
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                EventID = CInt(dr("ID"))
                Eventdate = CDate(dr("EventDate"))
            Else
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('No Attendance Report Available')", True)

            End If

            dr.Close()
        End Using

        strSQL = "select Firstname + ' ' + lastname as Fullname, Guest from AttendenceReport(@EventID)  where lastname >' ' order by Lastname,Firstname"
        Using cmd As New SqlCommand(strSQL, Conn)
            cmd.Parameters.AddWithValue("@EventID", EventID)
            Dim adp As New SqlDataAdapter(cmd)
            Dim rs As New DataTable
            adp.Fill(rs)
            Dim ds As New DataSet("dsattendence")
            ds.Tables.Add(rs)

            Dim rds As New ReportDataSource With {
                    .Name = "dsattendence",
                    .Value = ds.Tables(0)
                }
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ds.Dispose()
            adp.Dispose()
        End Using

        Dim param As ReportParameter() = New ReportParameter(5) {}
        param(0) = New ReportParameter("DayTimePrinted", CStr(Now()))
        param(1) = New ReportParameter("MeetingDate", CStr(Eventdate))
        Dim c1 As Integer = GetScalarInt("Select count(Spouseattend) as cnt from membersignup where eventid = " & EventID & "And spouseattend = 'true'", Conn)
        param(3) = New ReportParameter("NumSpouse", CStr(c1))
        Dim c As Integer = GetScalarInt("Select count(Memberattend) as cnt from membersignup where eventid =  " & EventID & " And memberattend = 'true'", Conn)
        param(2) = New ReportParameter("NumMembers", CStr(c + c1))
        Dim c2 As Integer = GetScalarInt("Select count(id) as cnt from Guestsignup where eventid = " & EventID, Conn)
        param(4) = New ReportParameter("Guests", CStr(c2))
        param(5) = New ReportParameter("Total", CStr(c + c1 + c2))

        ReportViewer1.LocalReport.SetParameters(param)
        ReportViewer1.ShowExportControls = False
        ReportViewer1.LocalReport.Refresh()

    End Sub
    Private Sub Rrender() Handles PDF.Click
        RenderReport(ReportViewer1, "Attendance.pdf", Server.MapPath("~/Reports/"))
        Response.Redirect("~/reports/attendance.pdf")
    End Sub
End Class
