<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="EventCount.aspx.vb" Inherits="CCFRW19.EventCount" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div  class="pPage">
		<asp:Label ID="HeadLabel" cssclass="pHeader" text="Member Event Count" runat="server"></asp:Label> <br /><br />
 
    <asp:Label ID="Label1" runat="server" Text="Enter  Start Date"></asp:Label>
    <asp:TextBox ID=StartDate runat="server"></asp:TextBox> <br /><br />
        <asp:Button ID="Display" runat="server" Text="Display Report" />
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" ShowDocumentMapButton="False" ShowParameterPrompts="False" ShowPrintButton="False" ShowPromptAreaButton="False" ShowRefreshButton="False" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Height="600px" ShowBackButton="False" ShowCredentialPrompts="False" ShowZoomControl="False" Width="715px" ShowPageNavigationControls="False">
        <LocalReport ReportPath="dsEventCount.rdlc">
          </LocalReport>
    </rsweb:ReportViewer>    </div>
</asp:Content>
