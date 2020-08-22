
Imports System.Data.SqlClient
Public Class InformationTable
    Inherits PageBase
    Dim conn As SqlConnection
    'This page provides access to the Information Table in the dtabase.  The information table contains parameters that can change from time to time.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        Me.InformationSource.ConnectionString = conn.ConnectionString
        If Not IsPostBack Then

            fvInformationTable.DataBind()
            fvInformationTable.ChangeMode(FormViewMode.Edit)
            Dim x As Object
            Using cmd As New SqlCommand("select onlinetext from information", conn)
                x = cmd.ExecuteScalar
                If x Is Nothing Then

                    Me.onlineText.Text = ""
                Else
                    Me.onlineText.Text = x.ToString
                End If
            End Using
            Using cmd As New SqlCommand("select bylaws from information", conn)
                x = cmd.ExecuteScalar
                If x Is Nothing Then
                    Me.ByLaws.Text = ""
                Else
                    Me.ByLaws.Text = x.ToString
                End If
            End Using
        End If
    End Sub

    Protected Sub SaveInformation(sender As Object, e As EventArgs) Handles SaveI.Click
        Session("Bylaws") = ByLaws.Text
        Session("onlineText") = Me.onlineText.Text
        Me.fvInformationTable.UpdateItem(True)
        Me.fvInformationTable.ChangeMode(FormViewMode.Edit)
    End Sub

    Protected Sub DisplayErrorMessage()
        Dim message As String = "Update failed.  Male sure you are entering the correct type of information"
        Dim sb As New System.Text.StringBuilder()
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')} ")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

End Class
