Imports System.Data.SqlClient
Imports Microsoft.Ajax.Utilities
Imports Microsoft.Reporting.WebForms

Public Class RosterRP
    Inherits PageBase
    Dim conn As New SqlConnection()
    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        conn = New SqlConnection(GetConnectionStringM(False, False))

        conn.Open()

        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "RosterR.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()

        Session("conn") = conn.ConnectionString



        Dim strSQL As String = "SELECT Member.CombinedNickName, Member.Telephone, Member.Address, Member.CSZ, Member.mfFullName, Member.MEmail,Member.SpouseEmail,Member.cellphone, Member.ProgramCommittee,Member.Nickname,Member.SpouseNickname,Member.SCellphone, Offices.OfficeOrder " _
          & "FROM Member INNER JOIN Offices On Member.Position = Offices.OfficeName where dontincludeonreports='false' " _
         & "ORDER BY Member.LastName, member.FirstName, Offices.OfficeOrder"

        ReportViewer1.LocalReport.DataSources.Add(ReportDataS(strSQL, "DSRoster", conn))

        Dim param As ReportParameter() = New ReportParameter(3) {}
        param(0) = New ReportParameter("Title", "CCFR Roster Report")

        strSQL = "select mffullname + ', ' + position from member inner join offices on member.position = offices.officename where member.Position <> 'Member' and position <> 'Director' order by officeorder"
        Dim col1 As String = CreateList(strSQL)
        col1 += "<br /><br /><h5>Membership Committee</h5>" & "Gene Ecton Davis, Chair"
        col1 += "<br /><br /><h5>Financial Review Committee</h5>" & "William Adams, Chair<br />Joanne Blakemore"
        col1 += "<br /><br /><h5>Administrator</h5>" & CStr(Info("Administrator"))
        param(1) = New ReportParameter("Officers", col1)
        strSQL = "select mffullname  from member where  position = 'Director' or position in  (select officename from offices where officeorder between 100 and 550) order by lastname,firstname"
        param(2) = New ReportParameter("Directors", CreateList(strSQL))
        strSQL = "select mffullname +', ' + 'Chair' as mffullname ,lastname,firstname, 1 as pos  from member where  programcommittee = 'Chair'  union all " _
            & "select mffullname ,lastname,firstname,2 from member where  programcommittee = 'Member' order by pos,lastname,firstname"
        Dim col3 As String = CreateList(strSQL)
        strSQL = "select mffullname +', ' + 'Chair' as mffullname ,lastname,firstname, 1 as pos  from member where  Lunchcommittee = 'Chair'  union all " _
            & "select mffullname ,lastname,firstname,2 from member where  Lunchcommittee = 'Member' order by pos,lastname,firstname"
        col3 += "<br /><h5>Luncheon Committee</h5>" & CreateList(strSQL)
        param(3) = New ReportParameter("PC", col3)
        ReportViewer1.LocalReport.SetParameters(param)

        ReportViewer1.LocalReport.Refresh()
    End Sub




    Private Function CreateList(strSQL As String) As String
        Dim command As SqlCommand = New SqlCommand(strSQL, conn)

        Dim lst As String = ""
        Dim reader As SqlDataReader = command.ExecuteReader()

        If reader.HasRows Then
            Do While reader.Read()
                lst += CStr(reader.GetString(0)) & "<br/>"
            Loop
        End If

        reader.Close()
        CreateList = Left(lst, Len(lst) - 5)
    End Function
End Class
