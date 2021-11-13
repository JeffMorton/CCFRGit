<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/member.Master" CodeBehind="EventReview1.aspx.vb" Inherits="CCFRW19.EventReview1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="=pPage" style="color:Black; height: 489px; width: 625px;z-index:1">
	<span class="pHeader"><asp:label ID="tt" runat="Server" Text ="Activity Review"></asp:label  ></span><br /><br />
	<span class="pAnnSpeechTitle" style="font-family: Verdana"><asp:label ID = "Reserve" runat= "server" text ="Reservations for:"></asp:label> 
        <br />
	</span> <br />
<div style="margin-left:40px">
        <asp:ListBox ID ="SignedUp" runat="server" style=" margin-top: 0px" Width="400px" Height="192px" ></asp:ListBox>
    <br />	<br />
    <asp:Panel ID="Panel3" runat="server">
      <asp:GridView ID="GridView1" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="BLACK" CssClass ="pGridView"     runat="server" AutoGenerateColumns="false">
          <Columns>
              <asp:BoundField DataField="Item" HeaderText="Item" ItemStyle-Width="200" />
              <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Right " />
          </Columns>
</asp:GridView> </asp:Panel></div>
	<br />
        <asp:Label ID="Msg" Text ="" runat="server" Visible="false"></asp:Label>
 		<asp:Label ID="Msg1" Text =""  runat="server" Visible="false"></asp:Label>
       <asp:Button runat="server" Text="Complete Reservation" id="Complete" margin-left=200px visible="false" Height="40px"/> 

          

           
  
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AdditionalContent" runat="server">
     
  <div >
    <asp:Panel ID="Panel1" runat="server" Visible="false" style="position:static;margin-left:550px;margin-top:460px">
         </asp:Panel></div>
  <div style="margin-left:400px;margin-top:250px;position:relative;z-index:3">
<form  action="https://www.paypal.com/cgi-bin/webscr" method="post" style="width:100px; margin-right: 74px;margin-left:150px" > 
 
<input type="hidden" name="cmd" value="_cart"/>
<input type ="hidden" name ="upload" value ="1" />
<input type="hidden" name="business" value ="ccfr1t@gmail.com" />
<input type="hidden" name="lc" value="US"/>
<input type="hidden" name="item_name_1" value='<%=Session("Item1Name").ToString %>'/>
<input type="hidden" name="amount_1" value='<%=Session("Item1cost").ToString %>'/>
<input type="hidden" name="quantity_1" value ='<%=Session("Item1Quantity").ToString %>' /> 


<input type="hidden" name="first_name" value ='<%=Session("first_name").ToString %>' /> 
<input type="hidden" name="last_name" value ='<%=Session("last_name").ToString %>' /> 
<input type="hidden" name="address1" value ='<%=Session("address1").ToString %>' /> 
<input type="hidden" name="city" value ='<%=Session("city").ToString %>' /> 
<input type="hidden" name="state" value ='<%=Session("state").ToString %>' /> 
<input type="hidden" name="zip" value ='<%=Session("zip").ToString %>' /> 
<input type="hidden" name="email" value ='<%=Session("email").ToString %>' /> 
<input type="hidden" name="night_phone_a" value ='<%=Session("night_phone_a").ToString %>' /> 
<input type="hidden" name="currency_code" value="USD"/>
<input type="hidden" name="button_subtype" value="services"/>
<input type="hidden" name="no_note" value="1"/>
<input type="hidden" name="no_shipping" value="2"/> 
<input type="hidden" name="rm" value="2" />
<input type="hidden" name="custom" value='<%=Session("IDs").ToString %>' />

<input type="hidden" name="cancel_return" value="http://ccfrcville.org/Default.aspx"/>
<input type="hidden" name="return" value="http://ccfrcville.org/ConfirmEvent.aspx"/>
<input type="hidden" name="currency_code" value="USD"/>
<input type="hidden" name="button_subtype" value="services"/>
<input type="hidden" name="bn" value="PP-BuyNowBF:btn_paynowCC_LG.gif:NonHosted"/>
<img alt="Buy Now" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1"/>
    <input type="image" src="https://www.paypalobjects.com/en_US/i/btn/btn_paynowCC_LG.gif"  
        name="submit" id="PPbutton"  alt="PayPal - The safer, easier way to pay online!" style="height: 49px"  />
 
</form>

 </div>
</asp:Content>
           




