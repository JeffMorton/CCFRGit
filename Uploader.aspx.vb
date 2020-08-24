Public Class WebForm4
    Inherits PageBase
    'This program is used to upload attachement to emails as needed.
    Protected Sub Page_Load(ByVal sender As Object,
        ByVal e As System.EventArgs) Handles Me.Load
        Label1.Visible = True
        Label1.BackColor = Drawing.Color.White
        Label1.ForeColor = Drawing.Color.Black
        If IsPostBack Then
            Dim path As String = Server.MapPath("~/documents/")
            Dim fileOK As Boolean = False

            If FileUpload1.HasFile Then
                Dim fileExtension As String
                fileExtension = System.IO.Path.
                    GetExtension(FileUpload1.FileName).ToLower()
                Dim allowedExtensions As String() =
                    {".txt", ".bak", ".png", ".gif"}
                For i As Integer = 0 To allowedExtensions.Length - 1
                    If fileExtension = allowedExtensions(i) Then
                        fileOK = True
                    End If
                Next
                If fileOK Then
                    Try
                        FileUpload1.PostedFile.SaveAs(path &
                             FileUpload1.FileName)
                        Label1.Text = "File uploaded!"
                    Catch ex As Exception
                        Label1.Text = "File could not be uploaded."
                    End Try
                Else
                    Label1.Text = "Cannot accept files of this type."
                End If
            End If
        End If
    End Sub
End Class
