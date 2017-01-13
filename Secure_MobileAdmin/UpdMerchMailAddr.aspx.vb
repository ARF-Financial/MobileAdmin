Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Configuration


Public Class UpdMerchMailAddr
    Inherits System.Web.UI.Page
    '
    ' UpdMerchMailAddr - ASR 01/10/2017 ---------------- 25279
    ' Change Merchant Mailing Address
    '
    '
    Dim Err As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FillStateList
        End If
    End Sub

    Protected Sub GetMerchantButton_Click(sender As Object, e As EventArgs) Handles GetMerchantButton.Click
        clearScreen()
        LocateMerchant()
    End Sub

    Sub LocateMerchant()
        Dim sqlConn As New SqlConnection(WebConfigurationManager.ConnectionStrings("ARF_Production").ConnectionString)
        Dim sqlCmd As New SqlCommand
        Dim Rdr As SqlDataReader
        Dim SQLtxt As String = "select companyname, rest_name, mailingaddress, mailingcity, mailingstate, mailingzip, mailingaddrupdateby from tblContracts where BT_Nbr = " & ContractNbr.Text

        sqlCmd.CommandType = CommandType.Text
        sqlCmd.CommandText = SQLtxt
        sqlCmd.Connection = sqlConn
        sqlConn.Open()
        Rdr = sqlCmd.ExecuteReader
        Rdr.Read()
        If Rdr.HasRows Then
            CompanyName.Text = Rdr("companyname").ToString
            DBAName.Text = Rdr("rest_name").ToString
            CurrMailingAddress.Text = Rdr("mailingaddress").ToString
            CurrMailingCity.Text = Rdr("mailingcity").ToString
            CurrMailingState.Text = Rdr("mailingstate").ToString
            CurrMailingZip.Text = Rdr("mailingzip").ToString
            LastUpdated.Text = Rdr("MailingAddrUpdateBy").ToString
            NewMailingAddress.Enabled = True
            NewMailingCity.Enabled = True
            NewMailingState.Enabled = True
            NewMailingZip.Enabled = True
            UpdateButton.Enabled = True
            MsgLine.Text = ""
            MsgLine.ForeColor = System.Drawing.Color.Black
        Else
            MsgLine.Text = "Merchant not found"
            MsgLine.ForeColor = System.Drawing.Color.Red
            clearScreen()
        End If
        Rdr.Close()
        sqlConn.Close()

    End Sub

    Protected Sub clearScreen()
        CompanyName.Text = ""
        DBAName.Text = ""
        CurrMailingAddress.Text = ""
        CurrMailingCity.Text = ""
        CurrMailingState.Text = ""
        CurrMailingZip.Text = ""
        LastUpdated.Text = ""
        NewMailingAddress.Text = ""
        NewMailingCity.Text = ""
        NewMailingState.SelectedIndex = 0
        NewMailingZip.Text = ""
        NewMailingAddress.Enabled = False
        NewMailingCity.Enabled = False
        NewMailingState.Enabled = False
        NewMailingZip.Enabled = False
        UpdateButton.Enabled = False
    End Sub

    Protected Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        clearScreen()
        ContractNbr.Text = ""
    End Sub
    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Response.Redirect("mobileadmindefault.aspx")
    End Sub

    Protected Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
        ValidateData()
        If Err Then
            MsgLine.ForeColor = System.Drawing.Color.Red
        Else
            UpdateData()
            CurrMailingAddress.Text = NewMailingAddress.Text
            CurrMailingCity.Text = NewMailingCity.Text
            CurrMailingState.Text = NewMailingState.SelectedValue
            CurrMailingZip.Text = NewMailingZip.Text
            MsgLine.Text = "*** UPDATE COMPLETE ***"
            MsgLine.ForeColor = System.Drawing.Color.DarkGreen
        End If
    End Sub

    Protected Sub ValidateData()
        Err = False
        Dim FakeMailingAddress2 As String = ""
        Dim FakeMailingZip4 As String = ""
        Dim MultiUnitFlag As Boolean = False
        Dim ValAddress As USPSAddress = New USPSAddress(NewMailingAddress.Text, FakeMailingAddress2, NewMailingCity.Text, NewMailingState.SelectedValue, NewMailingZip.Text, FakeMailingZip4, MultiUnitFlag)
        If ValAddress.ErrorMsg = "" Then
            NewMailingAddress.Text = ValAddress.Address1
            If Not ValAddress.Address2 = "" Then
                NewMailingAddress.Text = NewMailingAddress.Text & " " & ValAddress.Address2
            End If
            NewMailingCity.Text = ValAddress.City
            NewMailingState.SelectedIndex = NewMailingState.FindItemByValue(ValAddress.State).Index
            NewMailingZip.Text = ValAddress.Zip5
        Else
            MsgLine.Text = ValAddress.ErrorMsg
            Err = True
        End If

    End Sub

    Protected Sub UpdateData()
        Dim sqlConn As New SqlConnection(WebConfigurationManager.ConnectionStrings("ARF_Production").ConnectionString)
        Dim sqlCmd As New SqlCommand
        Dim RC As Integer
        Dim SQLtxt As String

        SQLtxt = "UPDATE tblContracts set MailingAddress = @Addr, MailingCity = @City, MailingState = @State, MailingZip = @Zip, MailingAddrUpdateBy = @UpdBy Where BT_Nbr = @ContractID"
        sqlCmd.CommandText = SQLtxt
        sqlCmd.Connection = sqlConn
        sqlCmd.Parameters.AddWithValue("@Addr", NewMailingAddress.Text)
        sqlCmd.Parameters.AddWithValue("@City", NewMailingCity.Text)
        sqlCmd.Parameters.AddWithValue("@State", NewMailingState.SelectedValue)
        sqlCmd.Parameters.AddWithValue("@Zip", NewMailingZip.Text)
        LastUpdated.Text = Membership.GetUser.UserName & " " & Now().ToString
        sqlCmd.Parameters.AddWithValue("@UpdBy", LastUpdated.Text)
        sqlCmd.Parameters.AddWithValue("@ContractID", ContractNbr.Text)
        sqlConn.Open()
        RC = sqlCmd.ExecuteNonQuery
        sqlConn.Close()

    End Sub

    Protected Sub FillStateList()
        ' load State dropdown list
        Dim Conn As New SqlConnection(WebConfigurationManager.ConnectionStrings("ARF_Production").ConnectionString)
        Dim cmd As New SqlCommand("SELECT [State] from tblStates order by [State]", Conn)
        cmd.Connection.Open()
        Dim States As SqlDataReader
        States = cmd.ExecuteReader()
        NewMailingState.DataSource = States
        NewMailingState.DataValueField = "State"
        NewMailingState.DataTextField = "State"
        NewMailingState.DataBind()
        NewMailingState.Items.Insert(0, "  ")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

End Class