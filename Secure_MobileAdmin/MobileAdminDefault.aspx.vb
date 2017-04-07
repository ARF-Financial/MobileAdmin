
Partial Public Class MobileAdminDefault
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim parm As String = Request.QueryString("parm")
        If parm = "noAuth" Then
            msg.Text = "You are not authorized for the selected function."
        Else
            msg.Text = ""
        End If
    End Sub
End Class