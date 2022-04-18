Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
'Produces the New Member report
Public Class NewMemberReport
    Inherits PageBase
    Dim conn As SqlConnection
    Protected Sub PageLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection With {.ConnectionString = GetConnectionString(False, False)}
        conn.Open()
        If Not IsPostBack Then
            Me.NewDate.Visible = True


        End If
    End Sub
    Protected Sub goclick() Handles Go.Click
        If IsDate(NewDate.Text) Then
            Dim dt As Date = CDate(NewDate.Text)
            ProduceReport(dt)
        Else
            Dim myScript As String = "window.alert('Please enter valid date');"
            ClientScript.RegisterStartupScript(Me.GetType(), "myScript", myScript, True)
            Exit Sub
        End If
    End Sub

    Protected Sub ProduceReport(NDate As Date)
        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "NewMember.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()

        Dim strSQL As String = "SELECT  [RosterName],[DateJoined] from [Member] where  dontincludeonreports = 'false' and [RosterName] >'A'  and datejoined > " & "'" & NDate & "' order by lastname,firstname"
        Using cmd As New SqlCommand("Select count(RosterName) from member where  dontincludeonreports = 'false' and datejoined > @NDate ", conn)
            cmd.Parameters.AddWithValue("@NDate", NDate)
            Dim cnt As Integer = CInt(cmd.ExecuteScalar)
            Dim param As New ReportParameter("totalcount", CStr(cnt))
            ReportViewer1.LocalReport.SetParameters(param)
            ReportViewer1.LocalReport.DataSources.Add(ReportDataS(strSQL, "dsNewMember", conn))
            ReportViewer1.LocalReport.Refresh()
        End Using
    End Sub
End Class
