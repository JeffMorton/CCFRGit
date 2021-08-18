<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Board.aspx.vb" MasterPageFile="~/Publ.Master" Inherits="CCFRW19.Leadership" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="pPage" style=" color:black; width: 675px; ">
       <span class="pHeader">Leadership</span><br /><br />

           <asp:Label ID="Address" Width ="715px" cssclass="pHeader2" runat="server" Text=""></asp:Label><br />

    <asp:DataList ID="DataList1" runat="server" RepeatColumns="3" DataSourceID="SqlDataSource1" Width="715px"  BackColor="White"  BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="None" style="text-align: left">
        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C"></FooterStyle>
        <ItemStyle BackColor="#FFFFFF" ForeColor="000000" Font-Size ="small" font-bold ="False" Width =" 250px"></ItemStyle>
        <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7"></SelectedItemStyle>
        <ItemTemplate><%#Container.DataItem("mfFullName")%>
        </ItemTemplate>
     <Headertemplate><span class="pHeader1"  style ="text-align:left"  >Officiers</span></Headertemplate>
    </asp:DataList><br />

    <asp:DataList ID="DataList2" runat="server" DataSourceID="BoardDataSource" width="715px" RepeatColumns="3" CellPadding="3">
        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C"></FooterStyle>
        <ItemStyle BackColor="#FFFFFF" ForeColor="000000" Font-Size ="small" font-bold ="False" Width =" 250px"></ItemStyle>
        <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7"></SelectedItemStyle>
        <ItemTemplate><%#Container.DataItem("mfFullName")%>
        </ItemTemplate>
     <Headertemplate><span class="pHeader1"  style ="text-align:left"  >Board of Directors</span></Headertemplate>
    </asp:DataList><br />

<asp:DataList ID="ProgramCommittee" runat="server" DataSourceID="ProgramDataSource" RepeatColumns="3" Width="715px" CellPadding="3">
        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C"></FooterStyle>
        <ItemStyle BackColor="#FFFFFF" ForeColor="000000" Font-Size ="small" font-bold ="False" Width =" 250px"></ItemStyle>
        <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7"></SelectedItemStyle>
        <ItemTemplate><%#Container.DataItem("mfFullName")%>
         </ItemTemplate>
     <Headertemplate><span class="pHeader1"  style ="text-align:left"  >Program Committee</span></Headertemplate>
    </asp:DataList><br />

<asp:DataList ID="DataList3" runat="server" DataSourceID="LunchDataSource" RepeatColumns="3" Width="715px" CellPadding="3">
        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C"></FooterStyle>
        <ItemStyle BackColor="#FFFFFF" ForeColor="000000" Font-Size ="small" font-bold ="False" Width =" 250px"></ItemStyle>
        <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7"></SelectedItemStyle>
        <ItemTemplate><%#Container.DataItem("mfFullName")%>
         </ItemTemplate>
     <Headertemplate><span class="pHeader1"  style ="text-align:center"  >Luncheon Committee</span></Headertemplate>
    </asp:DataList><br />
<span class="pHeader1"  style ="text-align:center"  >Membership Committee</span><br />
           <asp:Label ID="Label1" runat="server" Text="Lesley McCowen, Chair"></asp:Label><br />
<br />
<span class="pHeader1"  style ="text-align:center"  >Financial Review Committee</span><br />
           <asp:Label ID="Label2" runat="server" Width ="250px" Text="Paul Sartori, Chair"></asp:Label><asp:Label ID="Label3" runat="server" Text="Joanne Blakemore"></asp:Label><br /><br />
<span class="pHeader1"  style ="text-align:center"  >Administrator</span><br />
           <asp:Label ID="Administrator" runat="server" Text="Label"></asp:Label>
          </div>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server"  SelectCommand= "select mffullname + ', ' + position as mffullname from member inner join offices on member.position = offices.officename where member.Position <> 'Member' and position <> 'Director' order by officeorder">        
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="BoarddataSource" runat="server"  SelectCommand="select mffullname  from member where  position = 'Director' or position in  (select officename from offices where officeorder between 100 and 550) order by lastname,firstname"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ProgramDataSource" runat="server"  SelectCommand="select mffullname ,committees.position,lastname from committees inner join member on member.id=committees.memberid where committees.committee='ProgramCommittee' and committees.position='member'  union all select mffullname + ', Chair',committees.position,lastname from committees inner join member on member.id=committees.memberid where committees.committee='ProgramCommittee' and committees.position='chair' order by position ,member.lastname  "></asp:SqlDataSource>
    <asp:SqlDataSource ID="LunchDataSource" runat="server"  SelectCommand="select mffullname ,committees.position,lastname from committees inner join member on member.id=committees.memberid where committees.committee='LunchCommittee' and committees.position='member'  union all select mffullname + ', Chair',committees.position,lastname from committees inner join member on member.id=committees.memberid where committees.committee='LunchCommittee' and committees.position='chair' order by position ,member.lastname "></asp:SqlDataSource>
	 
</asp:Content>
