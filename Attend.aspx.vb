
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms

Public Class Attend
    Inherits PageBase
    Dim conn As SqlConnection
    'This is the admin section attendance report.  It provides a report for the date selected in Adminstart.  
    'There is also a member section attendance report Attendance.aspx.  This only show the next event either lunch or dinner as selected from the menu by the user.
    'Both reports use the same attendence.rdlc.
    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        conn = New SqlConnection(GetConnectionStringM(False, False))
        conn.Open()


        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "Attendence.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.ShowExportControls = True

        Dim strSQL As String = "select Firstname + ' ' + lastname as Fullname, Guest from AttendenceReport(@EventID)  where lastname >' ' order by Lastname,Firstname"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))

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
        End Using
        Dim param As ReportParameter() = New ReportParameter(5) {}
        param(0) = New ReportParameter("DayTimePrinted", CStr(Now()))
        param(1) = New ReportParameter("MeetingDate", Session("EventDate").ToString())
        Dim c1 As Integer = GetScalarInt("Select count(Spouseattend) as cnt from membersignup where eventid = " & Session("EventID").ToString & "And spouseattend = 'true'", conn)
        param(3) = New ReportParameter("NumSpouse", CStr(c1))
        Dim c As Integer = GetScalarInt("Select count(Memberattend) as cnt from membersignup where eventid =  " & Session("EventID").ToString & " And memberattend = 'true'", conn)
        param(2) = New ReportParameter("NumMembers", CStr(c + c1))
        Dim c2 As Integer = GetScalarInt("Select count(id) as cnt from Guestsignup where eventid = " & Session("EventID").ToString, conn)
        param(4) = New ReportParameter("Guests", CStr(c2))
        param(5) = New ReportParameter("Total", CStr(c + c1 + c2))
        ReportViewer1.LocalReport.SetParameters(param)

        ReportViewer1.LocalReport.Refresh()

    End Sub



End Class
