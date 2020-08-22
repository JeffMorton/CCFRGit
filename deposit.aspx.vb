Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
'provides a list of financial transaction not processed through PayPAL but added to the account table since the last time the report was run.
'This is designed to list check that are being deposited to a bank account.
'It provides a user a way to mark items as having been deposited so they don't apprear on future repotrts.
Public Class deposit
    Inherits PageBase
    ReadOnly conn As New SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn.ConnectionString = GetConnectionString(False, False)
        conn.Open()
        If Not IsPostBack Then
            ReportViewer1.Visible = True
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = "deposit.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()

            Dim strSQL As String = "SELECT  [tID],[tCheckNumber],[tCheckDate],[tCheckAmount],[tPayee] FROM [dbo].[Account] where tDeposited = 'false'  and ttype='D' order by tID"
            ReportViewer1.LocalReport.DataSources.Add(ReportDataS(strSQL, "dsDeposit", conn))
            ReportViewer1.LocalReport.Refresh()
        End If
    End Sub
    Protected Sub MarkAsDeposited(sender As Object, e As EventArgs) Handles Update.Click
        Dim strSQL As String = "UPDATE Account SET Account.tDateDeposited = @Date, Account.tDeposited = 'True' WHERE (Account.tDeposited) ='false' AND Account.tType='D'"

        Dim d As DateTime = Convert.ToDateTime(Now.ToShortDateString)
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = d
            cmd.ExecuteNonQuery()
        End Using
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Process Complete')", True)
    End Sub

End Class
