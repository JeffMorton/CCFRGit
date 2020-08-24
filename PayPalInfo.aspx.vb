Imports System.Data.SqlClient
'This page allows the administrator to review the messages sent from PayPal to the website as part of the transaction processing.  Note, dues payments are included ith the next dinner.
Public Class PayPalInfo
    Inherits PageBase
    Dim conn As SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()

        Using cmd As New SqlCommand("FillPPTranslog", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.ExecuteNonQuery()
        End Using
        Sort_by_Date()
    End Sub
    Protected Sub Ppgridview_rowcreated(sender As Object, e As GridViewRowEventArgs)
        ppGridview.PageIndex = ppGridview.PageCount - 1
    End Sub

    Protected Sub Sort_by_Date() Handles SortbyDate.Click
        Using cmd As New SqlCommand("select count(id) from ppTranslog where eventid = @eventID", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            Me.Cnt.Text = cmd.ExecuteScalar.ToString
        End Using
        Me.EventID.Text = Session("EventID").ToString
        Using cmd As New SqlCommand("Select ppTranslog.[ID], [TransNo], [RegDate], [EventID], ppTranslog.[MemberID], [Amount], [RegDate], mlFullName FROM [dbo].[ppTransLog] inner join member on ppTranslog.memberid = member.id where status='Completed' and EventID=@EventID order by ppTranslog.Regdate desc", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            ppGridview.DataSource = GridViewDataS(cmd)
            ppGridview.DataBind()
        End Using
    End Sub
    Protected Sub Sort_by_Name() Handles SortbyName.Click
        Using cmd As New SqlCommand("select ppTranslog.[ID],[TransNo],[Date],[EventID],ppTranslog.[MemberID],[Amount],[RegDate],mlFullName FROM [dbo].[ppTransLog] inner join member on ppTranslog.memberid = member.id where status='Completed' and EventID=@EventID order by mlFullname ", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            ppGridview.DataSource = GridViewDataS(cmd)
            ppGridview.DataBind()
        End Using
    End Sub

End Class
