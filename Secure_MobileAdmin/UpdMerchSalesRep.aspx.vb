Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Configuration


Public Class UpdMerchSalesRep
    Inherits System.Web.UI.Page
    '
    ' UpdMerchSalesRep
    ' Change Merchant Sales Rep
    '
    '
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Item("Username") = Membership.GetUser.UserName
        ' If Roles.IsUserInRole(Session.Item("Username"), "MobileAdmin_RepUpdate") or
        ' Roles.IsUserInRole(Session.Item("Username"), "SystemControl") Then
        ' Dim i As Integer = 1
        ' Else
        Response.Redirect("MobileAdminDefault.aspx?parm=noAuth")
        ' End If

    End Sub


End Class