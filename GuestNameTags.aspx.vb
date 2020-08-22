
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
'This page prints nametags for guests and new members.  It also prints name tags for old members who have the "PrintNameTag" field in the member table set to true.
Public Class GuestNameTags
    Inherits PageBase
    Dim conn As SqlConnection
    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()

        Session("conn") = conn.ConnectionString
        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "GuestNameTags.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()

        ReportViewer1.ShowExportControls = True

        Dim strsql As String = "SELECT GuestSignUp.GuestName as mFullName, GuestSignUp.GLastName, GuestSignUp.GFirstName, 'Guest' AS gst,  'Blue' AS nColor " _
                & "From GuestSignUp WHERE (((GuestSignUp.EventID)=@EventID)) " _
                & "union all " _
                & "SELECT Member.mfNickName, Member.LastName, Member.Nickname, Member.Position AS gst, 'White' AS nColor " _
                & "FROM MemberSignup INNER JOIN Member ON MemberSignup.MemberID = Member.ID " _
                & "WHERE (((MemberSignup.EventID)=@EventID) AND ((Member.DontIncludeonReports)='False') AND ((MemberSignup.MemberAttend)='True') AND ((MemberSignUp.NewMember)='True')) " _
                & "union all " _
                & "SELECT Member.mfNickName, Member.LastName, Member.Nickname, Member.Position AS gst, 'White' AS nColor " _
                & "FROM Member  " _
                & "WHERE ( ((Member.DontIncludeonReports)='False')  AND ((Member.NewMember)='False') AND ((Member.PrintNameTag)='True')) " _
                & "union all " _
                & "SELECT Member.sfNickName, Member.SpouseLastName, Member.SpouseNickname, ' ' AS gst, 'White' AS nColor " _
                & "FROM MemberSignup INNER JOIN Member ON MemberSignup.MemberID = Member.ID " _
                & "WHERE (((MemberSignup.EventID)=@EventID) AND ((Member.DontIncludeonReports)='False') AND ((MemberSignup.SpouseAttend)='True') AND ((MemberSignUp.NewMember)='True')) " _
                & "union all " _
                & "SELECT Member.sfNickName, Member.SpouseLastName, Member.SpouseNickname, ' ' AS gst, 'White' AS nColor " _
                & "FROM Member " _
                & "WHERE (((Member.DontIncludeonReports)='False')  AND ((Member.NewMember)='False') AND ((Member.SpousePrintNameTag)='True')) order by Glastname,GfirstName "
        Using cmd As New SqlCommand(strsql, conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            ReportViewer1.LocalReport.DataSources.Add(ReportDataSC(cmd, "dsGuestNameTags"))
            ReportViewer1.LocalReport.Refresh()
        End Using
    End Sub
    Protected Sub ClearNewMemberFlag() Handles ClearPrintNametag.Click
        Dim strSQL As String = "UPDATE Member SET Member.PrintNameTag = 'False', Member.SpousePrintNameTag = 'False' " _
            & "WHERE (((Member.PrintNameTag)='True')) OR (((Member.SpousePrintNameTag)='True'))"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.ExecuteNonQuery()
        End Using
        strSQL = "update member set member.newmember='false' from member inner join membersignup on membersignup.memberid = member.id  where memberattend='true' and membersignup.newmember='true' and Eventid = @EventID"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            cmd.ExecuteNonQuery()
        End Using
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Process Complete')", True)

    End Sub
End Class
