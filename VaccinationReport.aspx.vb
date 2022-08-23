Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Public Class VaccinationReport

    Inherits PageBase
    'The meal report is uesed to indicate the meal th meber selected when making the reservation
    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strConnectionString As String = GetConnectionString(False, False)
        Session("conn") = strConnectionString
        Dim conn As New SqlConnection(strConnectionString)
        conn.Open()
        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "Vaccine.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.ShowExportControls = True

        Dim strsql As String = "select mlfullname,  Mvaccinated   ,lastname,firstname from member where lastname >'A' and dontincludeonreports='False' union all select slfullname,svaccinated,spouselastname,spousefirstname from member where spouselastname >='A' and dontincludeonreports='False' order by lastname,firstname  "
        Using cmd As New SqlCommand(strsql, conn)
            ReportViewer1.LocalReport.DataSources.Add(ReportDataSC(cmd, "dsVaccinated"))
        End Using
        ReportViewer1.LocalReport.Refresh()
        ReportViewer1.Visible = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class
