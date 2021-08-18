
Imports System.Data.SqlClient
Public Class MealCategoryTable
    Inherits PageBase
    Dim conn As SqlConnection
    'This page allows the administrator to view and edit the meal category table.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection With {
            .ConnectionString = GetConnectionString(False, False)
        }
        conn.Open()
        Me.MealCatSource.ConnectionString = conn.ConnectionString
    End Sub
    Protected Sub RowCommands(sender As Object, e As GridViewCommandEventArgs)

        If e.CommandName = "Insert" Then
            Dim control As Control
            If (MealCatGridView.FooterRow IsNot Nothing) Then
                control = MealCatGridView.FooterRow
            Else
                control = MealCatGridView.Controls(0).Controls(0)
            End If
            Dim MealCat As String = CType(control.FindControl("Mealcategory"), TextBox).Text
            Dim strSQL As String = "INSERT into MealCategory (MealCategory) VALUES (@MealCategory)"
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@MealCategory", MealCat)
                cmd.ExecuteNonQuery()
                MealCatGridView.DataBind()
            End Using
        End If
    End Sub
    Protected Sub DeleteRow(sender As Object, e As GridViewDeleteEventArgs)
        Dim row As Integer = e.RowIndex
        Dim rID As Integer = CInt(CType(MealCatGridView.Rows(row).FindControl("ID"), TextBox).Text)
        Using cmd As New SqlCommand("select top 1 id from MemberSignUp where MemberMeal =@Id or SpouseMeal=@ID", conn)
            cmd.Parameters.AddWithValue("@ID", rID)
            Dim id As Integer = 0
            id = CInt(cmd.ExecuteScalar)

            If id > 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('MealCategory can only be deleted if it has never been used.')", True)
                e.Cancel = True
            Else
                Using cmd1 As New SqlCommand("Delete MealCategroy where ID=@ID", conn)
                    cmd1.Parameters.AddWithValue("@ID", rID)
                    cmd1.ExecuteNonQuery()
                End Using
            End If
        End Using


    End Sub
End Class
