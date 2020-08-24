Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
'Produces the uppaid meals report
Public Class UnpaidMealsOwed
    Inherits PageBase
    ReadOnly conn As New SqlConnection
    Protected Sub PageLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn.ConnectionString = GetConnectionString(False, False)
        conn.Open()
        If Not IsPostBack Then
            ReportViewer1.Visible = True
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = "UnPaidMeals.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()


            Dim strSQL As String = "SELECT  [RosterName],[MealsOwed] from [Member] where [MealsOwed] <>0 and dontincludeonreports = 'false' order by lastname,firstname"
            Using cmd As New SqlCommand("select count(RosterName) from member where [MealsOwed] <>0 and dontincludeonreports = 'false'", conn)
                Dim cnt As Integer = CInt(cmd.ExecuteScalar)
                Dim param As New ReportParameter("totalcount", CStr(cnt))
                ReportViewer1.LocalReport.SetParameters(param)
                ReportViewer1.LocalReport.DataSources.Add(ReportDataS(strSQL, "dsUnPaidMeals", conn))
                ReportViewer1.LocalReport.Refresh()
            End Using
        End If
    End Sub

End Class
