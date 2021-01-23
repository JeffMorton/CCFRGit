Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Public Class EventCount
    Inherits PageBase
    'The report produces a list of members and how many events they attended since a given date
    Dim conn As SqlConnection
    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strConnectionString As String = GetConnectionString(True, False)
        Session("conn") = strConnectionString
        conn = New SqlConnection(strConnectionString)
        conn.Open()
        ReportViewer1.Visible = False

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub DisplayReport() Handles Display.Click
        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "EventCount.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.ShowExportControls = True
        Dim strSQL As String = "select count(*) as cnt,  mlfullname from membersignup inner join member on member.id=membersignup.memberid inner join event on event.id =membersignup.eventid where memberattend='true' " _
            & "and Eventdate >=@StartDate group by mlfullname 	union all " _
            & "select count(*) as cnt,  slfullname from membersignup inner join member on member.id=membersignup.memberid inner join event on event.id =membersignup.eventid where Spouseattend='true' " _
             & "and Eventdate >=@StartDate And slFullName >'a' group by slfullname "
        Dim param As New ReportParameter("StartDate", StartDate.Text)

        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@Startdate", StartDate.Text)
            ReportViewer1.LocalReport.DataSources.Add(ReportDataSC(cmd, "dsEventCount"))
        End Using
        ReportViewer1.LocalReport.SetParameters(param)
        ReportViewer1.LocalReport.Refresh()
        ReportViewer1.Visible = True

    End Sub
End Class
