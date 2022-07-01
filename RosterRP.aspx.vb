Imports System.Data.SqlClient
Imports Microsoft.Ajax.Utilities
Imports Microsoft.Reporting.WebForms
'This page generates the member Roster report.  For Admin there is another Roster Report produced by aRosterP.aspx.
'The only difference between the two reports is the Master Page.
Public Class RosterRP
    Inherits PageBase
    Dim conn As New SqlConnection()
    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        conn = New SqlConnection(GetConnectionString(False, False))

        conn.Open()

        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "RosterR.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()

        Session("conn") = conn.ConnectionString

        Dim strSQL As String = "SELECT Member.CombinedNickName, Member.Telephone, Member.Address, Member.CSZ, Member.mfFullName, Member.MEmail,Member.SpouseEmail,Member.cellphone, Member.ProgramCommittee,Member.Nickname,Member.SpouseNickname,Member.SCellphone, Offices.OfficeOrder " _
          & "FROM Member INNER JOIN Offices On Member.Position = Offices.OfficeName where dontincludeonreports='false' " _
         & "and lastname >'a' ORDER BY Member.LastName, member.FirstName, Offices.OfficeOrder"

        ReportViewer1.LocalReport.DataSources.Add(ReportDataS(strSQL, "DSRoster", conn))

        Dim param As ReportParameter() = New ReportParameter(3) {}
        param(0) = New ReportParameter("Title", "CCFR Roster Report")

        strSQL = "select mffullname + ', ' + position from member inner join offices on member.position = offices.officename where member.Position <> 'Member' and position <> 'Director' order by officeorder"
        Dim col1 As String = CreateList(strSQL, conn)
        col1 += "<br /><br /><h5>Membership Committee</h5>" & CreateList(CreateCommitteeSQL("Membership Committee"), conn)
        col1 += "<br /><br /><h5>Financial Review Committee</h5>" & CreateList(CreateCommitteeSQL("Financial Review Committee"), conn)
        col1 += "<br /><br /><h5>Administrator</h5>" & CStr(Info("Administrator"))
        param(1) = New ReportParameter("Officers", col1)
        strSQL = "select mffullname  from member where  position = 'Director' or position in  (select officename from offices where officeorder between 100 and 550) order by lastname,firstname"
        param(2) = New ReportParameter("Directors", CreateList(strSQL, conn))
        Dim col3 As String = CreateList(CreateCommitteeSQL("Program Committee"), conn)
        col3 += "<br /><h5>Luncheon Committee</h5>" & CreateList(CreateCommitteeSQL("Lunch Committee"), conn)
        param(3) = New ReportParameter("PC", col3)
        ReportViewer1.LocalReport.SetParameters(param)

        ReportViewer1.LocalReport.Refresh()
    End Sub

End Class


