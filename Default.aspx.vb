Imports System.Data.SqlClient
Public Class Defaultc
    Inherits PageBase
    'When a user types ccfrcville.org into the address bar of a web browser, this is the page they see.  It shows information about the next dinner event.
    'It also displays a menu including all the public pages and options leading to the most of the members only pages.
    'The master page for this and all the public pages is Publ.master.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim strURL As String = Request.Url.ToString()
        If Request.IsSecureConnection = False Then
            Response.Redirect("https://ccfrcville.org/default.aspx")
        End If
        'Session("Debug") = True
        Session("Admin") = False
        Dim strInfo As String = Request.QueryString("dt")
        Session("test") = "Point 0"

        If IsDate(strInfo) Then
            Me.Label1.Text = CDate(strInfo).ToShortDateString
        Else
            Me.Label1.Text = Now().ToShortDateString
        End If
        Message.Text = "In planning for this fall, the CCFR Board decided at its June meeting that we did not want to go dark! For that reason, the monthly speaker presentations in September through December will occur over Zoom from 6 PM to 7:15 on the usual second Thursday of each month. The speaker’s presentation will be followed by Q&A. The Board will explore later in the fall if it is possible to have some dinner meetings in 2021 concurrently with Zoom meetings.  The luncheon meetings in the fall will also be over Zoom.
The CCFR reserves are healthy, and for that reason, the Board decided for the upcoming year only where meetings are over Zoom to reduce the annual dues from $120 per individual or couple to $60.

"

        Dim strConnection As String = GetConnectionString(True, False)
        'GetEventID("Dinner")
        Session("Conn") = strConnection
        Dim conn As New SqlConnection With {
            .ConnectionString = Session("conn").ToString
        }
        Me.SqlDataSource1.ConnectionString = Session("conn").ToString
        Dim strlo As String = Request.QueryString("Type")
        If strlo = "lo" Then Session("Loggedin") = False

        Me.SqlDataSource1.SelectCommand = "SELECT [ID],[Cost],[EventDate], [Meal1], [Meal2], [Meal3], [Speaker], [SpeakerBio], [SpeechTitle] FROM [Event] where [eventdate] >= '" & Label1.Text & "' and Type =  'Dinner' order by eventdate"
        'Throw New System.Exception("An exception has occurred.")

        FormView1.DataBind()

        Try
            If Not String.IsNullOrEmpty(CType(Me.FormView1.FindControl("EventDateLabel1"), Label).Text) Then
                Session("Eventdate") = CType(Me.FormView1.FindControl("EventDateLabel1"), Label).Text
                Session("EventID") = CType(Me.FormView1.FindControl("EventID"), Label).Text
                Session("MealCost") = CDbl(CType(Me.FormView1.FindControl("MealCost"), Label).Text)
            End If

        Catch
        End Try
    End Sub

End Class
