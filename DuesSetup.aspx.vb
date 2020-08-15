Imports System.Data.SqlClient
Public Class DuesSetup
    Inherits PageBase
    Dim conn As SqlConnection
    'CCFR charges dues.  This program provides the adiminstrator the tools to set up dues.

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        If Not IsPostBack Then
            Dim strSQL As String = "select halfduesdate,zeroduesdate,dues from information"
            Using cmd As New SqlCommand(strSQL, conn)
                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader
                dr.Read()
                Me.halfduesdate.Text = dr("HalfDuesDate").ToString
                Me.ZeroDuesDate.Text = dr("ZeroDuesDate").ToString
                Dim dues As Double = CDbl(dr("Dues"))
                Me.SDues.Text = Format(dues, "Fixed")
                Me.halfdues.Text = Format(dues / 2, "Fixed")
                Me.Zerodues.Text = Format(0, "Fixed")
                dr.Close()
            End Using
        End If
    End Sub
    Protected Sub Process()
        Dim rowsaffected(3) As Integer
        Dim strSQL As String = "UPDATE Member Set Member.DuesOwed = @FullDues"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@FullDues", Me.SDues.Text)
            rowsaffected(1) = cmd.ExecuteNonQuery()
        End Using

        strSQL = "UPDATE Member Set Member.DuesOwed = @ReducedDues where  ((member.DateJoined) > @HalfDuesDate)"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@ReducedDues", Me.halfdues.Text)
            cmd.Parameters.AddWithValue("@HalfDuesDate", Me.halfduesdate.Text)
            rowsaffected(2) = cmd.ExecuteNonQuery()

        End Using

        strSQL = "UPDATE Member Set Member.DuesOwed = 0 where   ((member.DateJoined) > @ZeroDuesDate)"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@ZeroDuesDate", Me.ZeroDuesDate.Text)
            rowsaffected(3) = cmd.ExecuteNonQuery()
        End Using
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Dues Update Complete')", True)
        Me.Czero.Text = rowsaffected(3).ToString
        Me.CHalf.Text = (rowsaffected(2) - rowsaffected(3)).ToString
        Me.CFull.Text = (rowsaffected(1) - rowsaffected(2)).ToString
    End Sub
End Class
