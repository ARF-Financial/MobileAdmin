<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" 
    CodeFile="UpdMerchMailAddr.aspx.vb" Inherits="UpdMerchMailAddr" title="Update Merchant Mailing Address" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">   
     <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
           <table class="centered-cell" style="width: 100%">
              <tr>
                <td class="centered-cell" style="padding: 1px; font-family: Arial, Helvetica, sans-serif; font-size: small; border-top-color: #A0A0A0; border-right-color: #C0C0C0; border-bottom-color: #C0C0C0; border-left-color: #A0A0A0; font-weight: bold; ">
                    Change Merchant Mailing Address
                </td>
             </tr>
          </table>
          <p />
          <table  style="font-family:arial;" >
             <tr><td>ARF Contract Number:&nbsp;<telerik:RadTextBox ID="ContractNbr" runat="server" Height="21px" Width="66px"></telerik:RadTextBox>&nbsp;
                 <telerik:RadButton ID="GetMerchantButton" runat="server" Text="Locate"></telerik:RadButton>&nbsp;&nbsp;
                 <asp:Label ID="MsgLine" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="Red"></asp:Label>
             </td></tr><tr><td>&nbsp;</td></tr>
             <tr><td>Merchant Name:&nbsp;<asp:Label ID="CompanyName" runat="server" Font-Bold="true"></asp:Label></td></tr>
             <tr><td>DBA Name:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="DBAName" runat="server" Font-Bold="true"></asp:Label></td></tr><tr>    
                 <td>&nbsp;</td></tr>
             <tr><td class="centered-cell" style="color:white; background-color:darkblue">Current Mailing Address</td></tr> <tr><td>&nbsp;</td></tr>
             <tr><td>Street:&nbsp;<asp:Label ID="CurrMailingAddress" runat="server" Height="21px" Width="460px" Font-Bold="true"></asp:Label></td></tr>
              <tr><td></td></tr>
              <tr><td>City:&nbsp;&nbsp;<asp:Label ID="CurrMailingCity" runat="server" Height="21px" Width="260px" Font-Bold="true"></asp:Label>&nbsp;State: 
                  <asp:Label ID="CurrMailingState" runat="server" Height="21px" Width="38px" Font-Bold="true"></asp:Label>&nbsp;Zip:
                  <asp:Label ID="CurrMailingZip" runat="server" Height="21px" Width="107px" Font-Bold="true"></asp:Label></td>
             </tr>
              <tr><td></td></tr>
              <tr><td>Last Updated By:&nbsp;<asp:Label ID="LastUpdated" runat="server" Height="21px" Width="260px" Font-Bold="true"></asp:Label></td></tr>
              <tr><td>&nbsp;</td></tr>
              <tr><td class="centered-cell" style="color:white; background-color:darkblue">New Mailing Address</td></tr> <tr><td>&nbsp;</td></tr>
             <tr><td>Street:&nbsp;<telerik:radtextbox ID="NewMailingAddress" runat="server" Height="21px" Width="480px" enabled="false"></telerik:radtextbox></td></tr>
              <tr><td></td></tr>
              <tr><td>City:&nbsp;&nbsp;<telerik:radtextbox ID="NewMailingCity" runat="server" Height="21px" Width="260px" enabled="false"></telerik:radtextbox>&nbsp;State: 
                  <telerik:raddropdownlist ID="NewMailingState" runat="server" Height="25px" Width="46px" enabled="false"></telerik:raddropdownlist>&nbsp;Zip:
                  <telerik:radtextbox ID="NewMailingZip" runat="server" Height="21px" Width="95px" enabled="false"></telerik:radtextbox></td>
             </tr>
          </table>
          <p /><p />
          <table class="centered-cell"><tr><td>
            <telerik:radbutton ID="UpdateButton" runat="server" Text="Update" Enabled="false"/>&nbsp;
            <telerik:radbutton ID="ClearButton" runat="server" Text="Clear" Height="22px" Width="76px" />&nbsp;
            <telerik:radbutton ID="CancelButton" runat="server" Text="Exit" Height="22px" Width="76px"/>
          </td></tr></table>

</asp:Content>