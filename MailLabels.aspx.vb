Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms

Public Class MailLabels
    Inherits PageBase
    ReadOnly conn As New SqlConnection
    'This page produces mailing labels for to send information to members.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn.ConnectionString = GetConnectionString(False, False)
        conn.Open()
        If Not IsPostBack Then
            ReportViewer1.Visible = True
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = "MailLabels.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()



            Session("conn") = conn.ConnectionString
            Dim strSQL As String = "SELECT  Envelopename,address,csz from member where dontincludeonreports='false' order by lastname, firstname "

            ReportViewer1.LocalReport.DataSources.Add(ReportDataS(strSQL, "dsMailLabels", conn))
            ReportViewer1.LocalReport.Refresh()
        End If
    End Sub

End Class
