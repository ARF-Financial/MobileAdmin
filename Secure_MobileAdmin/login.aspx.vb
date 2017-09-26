
Partial Class Secure_MobileAdmin_login
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim uname As String = "hlacy"

        FormsAuthentication.RedirectFromLoginPage(uname, 1)

    End Sub

End Class
