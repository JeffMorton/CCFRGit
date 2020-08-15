<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="TextEdit.aspx.vb" Inherits="CCFRW19.TextEdit" %>
    <%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <meta charset="utf-8">
  <meta name="robots" content="noindex, nofollow">
<script src="ckeditor/ckeditor.js"></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pPage">
       <asp:Label ID="Label1" cssclass="pHeader" width="600px" runat="server" Text="Speaker Bio:"></asp:Label>
  <br /><br />
        <asp:TextBox ID="PTextED" runat="server" TextMode="MultiLine"></asp:TextBox>
<script type="text/javascript" lang="javascript">CKEDITOR.replace('<%=PTextED.ClientID%>', { customConfig: '/ccfr_config.js' });</script>

        <asp:Button ID="Save" runat="server" width= "130px" Text="Save" />
</div>

</asp:Content>
