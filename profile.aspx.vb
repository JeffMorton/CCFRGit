Imports System.Data.SqlClient
Public Class profile
    Inherits PageBase
    'Ths page allows the user to update specific fields in the Member Table.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strConnection As String = GetConnectionStringM(False, False)
        Dim sqlConnection As New SqlConnection(strConnection)

        Try
            sqlConnection.Open()
        Catch
            Response.Redirect("Default.aspx")
        End Try
        If Not CBool(Session("loggedin")) Then
            Response.Redirect("login.aspx")
        End If

        Me.memSource.ConnectionString = Session("Conn").ToString
        Me.MemberForm.DataSourceID = memSource.ID
        Me.MemberForm.ChangeMode(FormViewMode.Edit)

        Me.SSource.ConnectionString = Session("Conn").ToString
        Me.SpouseForm.DataSourceID = SSource.ID
        Me.SpouseForm.ChangeMode(FormViewMode.Edit)

        Me.AddressSource.ConnectionString = Session("Conn").ToString
        Me.AddressForm.DataSourceID = AddressSource.ID
        Me.AddressForm.ChangeMode(FormViewMode.Edit)

    End Sub
    Private Sub Up(sender As Object, e As EventArgs) Handles UpdateM.Click
        Me.SpouseForm.UpdateItem(True)
        Me.MemberForm.UpdateItem(True)
        Me.AddressForm.UpdateItem(True)

        Me.MemberForm.ChangeMode(FormViewMode.Edit)
        Me.SpouseForm.ChangeMode(FormViewMode.Edit)
        Me.AddressForm.ChangeMode(FormViewMode.Edit)
    End Sub

End Class
