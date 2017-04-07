Public Class DPRedirect
    Inherits System.Web.UI.Page
    '
    ' Place in Secure_Financial folder to send Finance user to program in Sales directory
    '
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Redirect("~/Secure_MobileAdmin/DPAppList.aspx?from=Finance")
    End Sub
End Class