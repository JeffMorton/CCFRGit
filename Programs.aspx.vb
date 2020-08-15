Imports System.Data.SqlClient

Partial Public Class Programs
    Inherits PageBase



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack And Not IsCrossPagePostBack Then

            Dim drange As String
            Dim Startdate As Date
            Dim EndDate As Date
            Dim strSQL As String

            Try
                drange = Request.QueryString("Date")
                Label1.Text = drange
                Startdate = CDate("08/01/" & Mid(drange, 1, 4))
                EndDate = CDate("07/30/20" & Mid(drange, 6, 2))
                strSQL = "Select EventDate,Speaker, SpeechTitle,ShortSpeakerBio,Preface from Event where EventDate between '" & Startdate & "' and '" & EndDate & "' and Type = 'Dinner' order by eventdate "
                Me.Repeater1.DataSource = FillData(strSQL)
                Me.Repeater1.DataBind()
            Catch
                'Response.Redirect("Default.aspx")
            End Try
        End If
    End Sub
    Private Function FillData(ByVal strSQL As String) As DataSet
        Dim ds As DataSet = New DataSet

        Dim conn As New SqlConnection With {.ConnectionString = GetConnectionStringM(True, False)}
        conn.Open()
        Dim myAdaptor As New SqlDataAdapter(strSQL, conn)
        myAdaptor.Fill(ds)
        Return ds
    End Function

    Protected Sub Repeater1_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If Repeater1.Items.Count < 1 Then
            If e.Item.ItemType = ListItemType.Footer Then
                Dim lblFooter As Label = CType(e.Item.FindControl("lblEmptyData"), Label)
                lblFooter.Visible = True
            End If
        End If
    End Sub
End Class
