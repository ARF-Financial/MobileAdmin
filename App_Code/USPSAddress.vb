Imports System.Web.Configuration
Imports System.Xml
Public Class USPSAddress
    '
    ' Validate input address with USPS and return properly formatted address or error msg
    ' ASR 4/15/2016
    '
    ' Calling program should first check for property USPSAddress.ErrorMsg <> "" 
    '

    Private in_Address1 As String
    Private in_Address2 As String
    Private in_City As String
    Private in_State As String
    Private in_Zip5 As String
    Private in_Zip4 As String
    Private in_MultiUnitOverride As Boolean
    Private out_Address1 As String
    Private out_Address2 As String
    Private out_City As String
    Private out_State As String
    Private out_Zip5 As String
    Private out_Zip4 As String
    Private out_ErrorMsg As String = ""
    Public ReadOnly Property ErrorMsg() As String
        Get
            Return out_ErrorMsg
        End Get
    End Property
    Public ReadOnly Property Address1() As String
        Get
            Return out_Address1
        End Get
    End Property
    Public ReadOnly Property Address2() As String
        Get
            Return out_Address2
        End Get
    End Property
    Public ReadOnly Property City() As String
        Get
            Return out_City
        End Get
    End Property
    Public ReadOnly Property State() As String
        Get
            Return out_State
        End Get
    End Property
    Public ReadOnly Property Zip5() As String
        Get
            Return out_Zip5
        End Get
    End Property
    Public ReadOnly Property Zip4() As String
        Get
            Return out_Zip4
        End Get
    End Property

    Public Sub New(ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal State As String,
                   ByVal Zip5 As String, ByVal Zip4 As String, ByVal MultiUnitOverride As Boolean)
        in_Address1 = Address1
        in_Address2 = Address2
        in_City = City
        in_State = State
        in_Zip5 = Zip5
        in_Zip4 = Zip4
        in_MultiUnitOverride = MultiUnitOverride
        ValidateAddress()
    End Sub

    Private Sub ValidateAddress()
        Dim USPSBaseURL As String = WebConfigurationManager.AppSettings("USPSAPIURL")
        Dim USPSUserID As String = WebConfigurationManager.AppSettings("USPSAPIUserID")
        Dim XMLin As String
        Dim LastElementName As String = ""
        Dim wk_Address1 As String = ""
        Dim wk_Address2 As String = ""
        Dim wk_City As String = ""
        Dim wk_State As String = ""
        Dim wk_Zip5 As String = ""
        Dim wk_Zip4 As String = ""
        Dim err As Boolean = False
        Dim munit As Boolean = False

        ' format request
        XMLin = USPSBaseURL & "?API=Verify&XML=<AddressValidateRequest USERID=""" & USPSUserID & """>"
        XMLin = XMLin & "<Address ID=""0"">"
        XMLin = XMLin & "<FirmName/>"
        XMLin = XMLin & "<Address1>" & Trim(in_Address1) & "</Address1>"
        XMLin = XMLin & "<Address2>" & Trim(in_Address2) & "</Address2>"
        XMLin = XMLin & "<City>" & Trim(in_City) & "</City>"
        XMLin = XMLin & "<State>" & Trim(in_State) & "</State>"
        XMLin = XMLin & "<Zip5>" & Trim(in_Zip5) & "</Zip5>"
        XMLin = XMLin & "<Zip4>" & Trim(in_Zip4) & "</Zip4>"
        XMLin = XMLin & "</Address>"
        XMLin = XMLin & "</AddressValidateRequest>"

        ' send request
        Dim XmlReader As XmlTextReader = New XmlTextReader(XMLin)
        XmlReader.WhitespaceHandling = WhitespaceHandling.Significant

        ' process response
        While XmlReader.Read()
            Select Case XmlReader.NodeType
                Case XmlNodeType.Element
                    LastElementName = XmlReader.Name
                Case XmlNodeType.Text
                    Select Case LastElementName
                        Case "Description"
                            err = True
                            out_ErrorMsg = XmlReader.Value
                        Case "ReturnText"
                            munit = True
                            out_ErrorMsg = XmlReader.Value
                        Case "Address1"
                            wk_Address1 = XmlReader.Value
                        Case "Address2"
                            wk_Address2 = XmlReader.Value
                        Case "City"
                            wk_City = XmlReader.Value
                        Case "State"
                            wk_State = XmlReader.Value
                        Case "Zip5"
                            wk_Zip5 = XmlReader.Value
                        Case "Zip4"
                            wk_Zip4 = XmlReader.Value
                    End Select
                Case XmlNodeType.EndElement
                    LastElementName = ""
            End Select
        End While
        XmlReader.Close()

        If err Then
            'do nothing
        ElseIf munit And Not in_MultiUnitOverride Then
            'do nothing
        Else
            out_ErrorMsg = ""
            out_Address1 = wk_Address1
            out_Address2 = wk_Address2
            out_City = wk_City
            out_State = wk_State
            out_Zip5 = wk_Zip5
            out_Zip4 = wk_Zip4
            'force single line addresses to address1
            If out_Address1 = out_Address2 Then
                out_Address2 = ""
            ElseIf out_Address1 = "" And out_Address2 <> "" Then
                out_Address1 = out_Address2
                out_Address2 = ""
            ElseIf out_Address1 <> "" And out_Address2 <> "" Then
                out_Address1 = wk_Address2
                out_Address2 = wk_Address1
            End If
        End If

    End Sub

End Class
