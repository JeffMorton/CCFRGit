Imports System.Data.SqlClient
Partial Public Class member
    Inherits System.Web.UI.MasterPage
    'This is the master page for members who have logged in.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ds As DataSet = New DataSet
        Dim strConnection As String
        If Not (Session("conn") Is Nothing) Then
            strConnection = Session("conn").ToString
        Else
            strConnection = ConfigurationManager.ConnectionStrings("DB_25784_ccfrsqlConnectionString").ToString
        End If

        Dim conn As New SqlConnection With {
            .ConnectionString = strConnection
        }
        conn.Open()

        Dim strSQL As String = "Select MenuID, Text, Description, ParentID from WebMenu where onMemberMaster = 'True' order by ordr"
        Dim da As SqlDataAdapter = New SqlDataAdapter(strSQL, conn)

        da.Fill(ds)
        ds.Tables(0).TableName = "Menu"
        Dim parentcolumn As DataColumn = ds.Tables(0).Columns("MenuID")
        da.Dispose()
        ds.DataSetName = "Menus"
        Dim relation As New DataRelation("ParentChild", ds.Tables("Menu").Columns("MenuID"), ds.Tables("Menu").Columns("ParentID"), True) With {
            .Nested = True
        }
        ds.Relations.Add(relation)
        XmlDataSource1.Data = ds.GetXml()
        If Not Request.Params("sel") Is Nothing Then
            Using cmd As New SqlCommand("select url from webmenu where text = @sel", conn)
                cmd.Parameters.AddWithValue("@sel", Request.Params("Sel"))

                Dim url As String
                url = cmd.ExecuteScalar.ToString
                If Not url Is Nothing Then Response.Redirect(url)
            End Using

        End If
    End Sub

End Class
