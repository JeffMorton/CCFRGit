
Imports System.Data.SqlClient
Public Class Main_master
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ds As DataSet = New DataSet
        Dim strConnection As String = GetConnectionStringM(True, False)


        Dim conn As New SqlConnection With {
            .ConnectionString = strConnection
        }
        conn.Open()

        Dim strSQL As String = "Select MenuID, Text, Description, ParentID from WebMenu where onMainMaster = 'True' order by ordr"
        Dim da As SqlDataAdapter = New SqlDataAdapter(strSQL, conn)

        da.Fill(ds)
        ds.Tables(0).TableName = "Menu"
        Dim parentcolumn As DataColumn = ds.Tables(0).Columns("MenuID")
        Dim textcolumn As DataColumn = ds.Tables(0).Columns("Text")

        da.Dispose()
        ds.DataSetName = "Menus"
        Dim relation As New DataRelation("ParentChild", ds.Tables("Menu").Columns("MenuID"), ds.Tables("Menu").Columns("ParentID"), True) With {
            .Nested = True
        }
        ds.Relations.Add(relation)
        XmlDataSource1.EnableCaching = False

        XmlDataSource1.Data = ds.GetXml()
        XmlDataSource1.DataBind()
        Dim prm As String = Request.Params("sel")
        Dim url As String
        If Not (prm Is Nothing) Then
            Using cmd As New SqlCommand("select url from webmenu where text = @prm", conn)
                cmd.Parameters.AddWithValue("@prm", prm)

                url = cmd.ExecuteScalar.ToString
            End Using
            If Not (url Is Nothing) Then
                Response.Redirect(url)
            Else
                Response.Redirect("https://ccfrcville.org")
            End If
        End If

    End Sub

End Class
