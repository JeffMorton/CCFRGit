Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
'Produces the unpaid dues report
Public Class UnPaidDues
    Inherits PageBase
    Dim conn As SqlConnection
    Protected Sub PageLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection With {.ConnectionString = GetConnectionString(False, False)}
        conn.Open()
        If Not IsPostBack Then
            ReportViewer1.Visible = True
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = "UnPaidDues.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()

            Dim strSQL As String = "SELECT  [RosterName],[DuesOwed] from [Member] where [DuesOwed] <>0 and dontincludeonreports = 'false' order by lastname,firstname"
            Using cmd As New SqlCommand("select count(RosterName) from member where [DuesOwed] <>0 and dontincludeonreports = 'false'", conn)
                Dim cnt As Integer = CInt(cmd.ExecuteScalar)
                Dim param As New ReportParameter("totalcount", CStr(cnt))
                ReportViewer1.LocalReport.SetParameters(param)
                ReportViewer1.LocalReport.DataSources.Add(ReportDataS(strSQL, "dsUnPaidDues", conn))
                ReportViewer1.LocalReport.Refresh()
            End Using
        End If
    End Sub

End Class
