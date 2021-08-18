Imports System.Data.SqlClient
Public Class Committees
    Inherits PageBase
    Dim conn As SqlConnection

    'This page allows the administrator to view and edit committes assigned to members.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strSQL As String

        conn = New SqlConnection With {
            .ConnectionString = GetConnectionString(False, False)
        }
        conn.Open()
        Me.PositionSource.ConnectionString = conn.ConnectionString
        Me.ComNameSource.ConnectionString = conn.ConnectionString
        If Not IsPostBack Then
            If CStr(Session("Scode")) = "True" Then
                strSQL = "Select sfFullName from member where id = @ID"
            Else
                strSQL = "Select mfFullName from member where id = @ID"
            End If
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@id", Session("UserID"))
                Me.Name.Text = cmd.ExecuteScalar.ToString
            End Using
            CommitteesGridView.DataSource = CommitteesGridDataSource()
            CommitteesGridView.DataBind()
        End If


    End Sub
    Protected Sub RowCommands(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "Insert" Then
            Dim CONTROL As Control
            If (CommitteesGridView.FooterRow IsNot Nothing) Then
                CONTROL = CommitteesGridView.FooterRow
            Else
                CONTROL = CommitteesGridView.Controls(0).Controls(0)
            End If
            Dim com As String = CType(CONTROL.FindControl("Committee"), DropDownList).Text
            Dim pos As String = CType(CONTROL.FindControl("Position"), DropDownList).Text
            Dim STRSQL As String = "INSERT INTO Committees (Committee,Position,MemberID,Spouse) VALUES (@Com,@Pos,@ID,@Spouse)"
            Using CMD As New SqlCommand(STRSQL, conn)
                CMD.Parameters.AddWithValue("@Com", com)
                CMD.Parameters.AddWithValue("@Pos", pos)
                CMD.Parameters.AddWithValue("@ID", Session("UserID"))

                CMD.Parameters.AddWithValue("@Spouse", Session("Scode"))

                CMD.ExecuteNonQuery()

                CommitteesGridView.DataSource = CommitteesGridDataSource()

                CommitteesGridView.DataBind()
                'Me.PositionSource.ConnectionString = conn.ConnectionString
                'Me.ComNameSource.ConnectionString = conn.ConnectionString

            End Using
        End If


    End Sub
    Protected Sub Exform(sender As Object, e As EventArgs) Handles ExitForm.Click

        Response.Redirect("Memberupdate.aspx")
    End Sub
    Protected Sub DeleteRow(sender As Object, e As GridViewDeleteEventArgs)
        Dim row As Integer = e.RowIndex
        Dim rID As Integer = CInt(CType(CommitteesGridView.Rows(row).FindControl("ID"), Label).Text)
        Using cmd As New SqlCommand("Delete from committees where ID = @Id", conn)
            cmd.Parameters.AddWithValue("@ID", rID)
            cmd.ExecuteNonQuery()
        End Using
        CommitteesGridView.DataSource = CommitteesGridDataSource()
        CommitteesGridView.DataBind()

    End Sub
    Protected Function CommitteesGridDataSource() As DataTable
        Dim strSQL = "select * from committees where memberid=" & CStr(Session("UserID")) & " And Spouse='" & CStr(Session("Scode")) & "'"
        Using cmd As New SqlCommand(strSQL, conn)
            Using ada As SqlDataAdapter = New SqlDataAdapter(cmd)
                Using dt As DataTable = New DataTable()
                    ada.Fill(dt)
                    If dt.Rows.Count = 0 Then
                        dt.Rows.Add(0, " ", " ", 0, Session("Scode"))
                        'dt.Rows.Add(dt.NewRow)
                    End If
                    CommitteesGridDataSource = dt
                End Using
            End Using
        End Using
    End Function
End Class
