Imports System.Data.SqlClient



Public Class MeetingDetail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim strURL As String = Request.Url.ToString()
        Dim strInfo As String = Request.QueryString("ID")



        Dim strConnection As String = GetConnectionString(True, False)
        'GetEventID("Dinner")
        Session("Conn") = strConnection
        Dim conn As New SqlConnection With {
            .ConnectionString = Session("conn").ToString
        }
        Me.SqlDataSource1.ConnectionString = Session("conn").ToString
        Me.SqlDataSource1.SelectCommand = "SELECT [ID],[EventDate],[Speaker], [SpeakerBio], [SpeechTitle] FROM [Event] where ID = " & strInfo
        Try
            FormView1.DataBind()
        Catch
        End Try



    End Sub
    Protected Sub cleanup(sender As Object, e As EventArgs)

        Dim temp As String = CType(FormView1.Row.FindControl("SpeakerBioLabel"), Label).Text
        Dim i As Integer = InStr(temp, "6:00 Cocktails")
        If i > 1 Then
            CType(FormView1.Row.FindControl("SpeakerBioLabel"), Label).Text = Left(temp, i - 1)
        End If
        i = InStr(temp, "Save the Date")
        If i > 1 Then
            CType(FormView1.Row.FindControl("SpeakerBioLabel"), Label).Text = Left(temp, i - 1)
        End If
    End Sub


End Class
