Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports MailChimp
Imports AuthorizeNet



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Engine
    Inherits System.Web.Services.WebService

    Public Class ShoppingCart
        Public ProductID As Integer
        Public Item As String
    End Class

    Public Class Accessory
        Public AccessoryID As String
        Public ProductID As String
        Public SKU As String
        Public Name As String
        Public Qty As String
        Public Description As String
        Public Brand As String
        Public Price As String
        Public IsFeatured As String
    End Class

    Public Class Apparel
        Public ApparelID As String
        Public ProductID As String
        Public SKU As String
        Public Name As String
        Public Description As String
        Public Price As String
        Public XS As String
        Public SM As String
        Public MD As String
        Public LG As String
        Public XL As String
        Public XXL As String
        Public XXXL As String
        Public IsFeatured As String
    End Class

    Public Class Coffee
        Public CoffeeID As String
        Public ProductID As String
        Public SKU As String
        Public Name As String
        Public Brand As String
        Public Description As String
        Public Body As String
        Public Roast As String
        Public Price As String
        Public Qty As String
        Public IsFeatured As String
    End Class

    Public Class Cigar
        Public CigarID As String
        Public ProductID As String
        Public Brand As String
        Public SKU As String
        Public Name As String
        Public Length As String
        Public Ring As String
        Public BoxCount As String
        Public BoxQty As String
        Public SingleQty As String
        Public BoxPrice As String
        Public SinglePrice As String
        Public Description As String
        Public IsSingleSaleOnly As String
        Public IsBoxSaleOnly As String
        Public IsFeatured As String
    End Class

    Public Class Pipe
        Public PipeID As String
        Public ProductID As String
        Public Brand As String
        Public Name As String
        Public SKU As String
        Public Description As String
        Public Price As String
        Public Qty As String
        Public BowlFinish As String
        Public StemShape As String
        Public BodyShape As String
        Public Material As String
        Public IsFeatured As String
    End Class

    Public Class PipeTobacco
        Public PipeTobaccoID As String
        Public ProductID As String
        Public SKU As String
        Public Brand As String
        Public Tobacco As String
        Public Style As String
        Public Cut As String
        Public Strength As String
        Public Price As String
        Public Description As String
        Public Qty As String
        Public IsFeatured As String

    End Class

    Public Class NewsPosts
        Public NewsPostID As String
        Public PostTitle As String
        Public PostDate As String
        Public PostedBy As String
        Public HTML As String
        Public PlainText As String
        Public NewsType As String
        Public EventMonth As String
        Public EventDay As String
        Public EventLocation As String
    End Class

    Public Class BaseProducts
        Public ProductID As String
        Public SKU As String
        Public Category As String
        Public Price As String
        Public Name As String
    End Class


#Region "Lists of classes"
    Dim NewsList As New List(Of NewsPosts)
    Dim BaseProductList As New List(Of BaseProducts)
    Dim AccessoryList As New List(Of Accessory)
    Dim ApparelList As New List(Of Apparel)
    Dim CoffeeList As New List(Of Coffee)
    Dim CigarList As New List(Of Cigar)
    Dim PipeList As New List(Of Pipe)
    Dim PipeTobaccoList As New List(Of PipeTobacco)
#End Region

#Region "Shopping Cart and Checkout"
    <WebMethod(True)> _
    Public Function Checkout()

        'LIVE: https://secure.authorize.net/gateway/transact.dll
        Dim post_url = "https://test.authorize.net/gateway/transact.dll"
        Dim post_values As New Dictionary(Of String, String)

        'the API Login ID and Transaction Key must be replaced with valid values
        post_values.Add("x_login", "2hBf5VN3S")
        post_values.Add("x_tran_key", "88tkv2t2D29z5NQT")

        post_values.Add("x_delim_data", "TRUE")
        post_values.Add("x_delim_char", "|")
        post_values.Add("x_relay_response", "FALSE")

        post_values.Add("x_type", "AUTH_CAPTURE")
        post_values.Add("x_method", "CC")
        post_values.Add("x_card_num", "4111111111111111")
        post_values.Add("x_exp_date", "0115")

        post_values.Add("x_amount", "19.99")
        post_values.Add("x_description", "Sample Transaction")

        post_values.Add("x_first_name", "John")
        post_values.Add("x_last_name", "Doe")
        post_values.Add("x_address", "1234 Street")
        post_values.Add("x_state", "WA")
        post_values.Add("x_zip", "98004")


        'or 

        'Step 1 - Create the request
        'C#  var request = new AuthorizationRequest("4111111111111111", "1216", 10.00M, "Test Transaction ");"
        Dim Request = New AuthorizationRequest("CARDNBR", "1216", 10.0, "Description")

        'Step 2 - Create the gateway, sending in your credentials
        'C#  var gate = new Gateway("YOUR_API_LOGIN_ID", "YOUR_TRANSACTION_KEY");

        Dim gate = New Gateway("APILOGIN", "TRANSKEY")

        ' Step 3 - Send the request to the gateway
        'C#  var response = gate.Send(request);

        Dim response = gate.Send(Request)

        'Use for codes to showing to customer, and storing transaction id's in db
        Dim ResponseCode As String = response.ResponseCode
        Dim ResponseMsg As String = response.Message
          
        Return "hey dude"
    End Function
    <WebMethod(True)> _
    Public Function AddToCart(ByVal ProductID As Integer) As String

        If Session("Cart") Is Nothing Then
            Dim i As New ShoppingCart
            Dim itemList As New List(Of ShoppingCart)
            i.ProductID = ProductID
            itemList.Add(i)
            Session("Cart") = itemList
        Else
            Dim itemlist As List(Of ShoppingCart) = Session("Cart")
            Dim i As New ShoppingCart
            i.ProductID = ProductID
            itemlist.Add(i)
        End If

        Return ""
    End Function


    <WebMethod(True)> _
    Public Function GoToCart()

        Dim InMyCart As List(Of ShoppingCart) = Session("Cart")
        Dim ReturnList As New List(Of ShoppingCart)

        For i As Integer = 0 To InMyCart.Count - 1
            Dim sc As New ShoppingCart
            Dim ProductID As Integer = InMyCart(i).ProductID
            Dim Item As String = GetItemDetails(ProductID)
            sc.ProductID = ProductID
            sc.Item = Item
            ReturnList.Add(sc)
        Next
        Return ReturnList
    End Function

    <WebMethod()> _
    Public Function GetItemDetails(ByVal ProductID As Integer)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_SHOP_GetProductDetails"
            cmd.Parameters.AddWithValue("@ProductID", ProductID)
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        Return dt.Rows(0).Item(0)
    End Function


#End Region

#Region "Home Page"





    <WebMethod()> _
    Public Function LoadPost(ByVal PostID As Integer)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM NewsPosts WHERE PostID = " & PostID
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows.Count > 0 Then

            NewsList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim i As New NewsPosts
                i.NewsPostID = item("PostID")
                i.PostTitle = item("PostTitle")
                i.PostDate = CDate(item("PostDate"))
                i.PostedBy = item("PostedBy")
                i.HTML = item("HTML")
                i.PlainText = item("PlainText")
                i.NewsType = item("PostType")

            



                NewsList.Add(i)
            Next
            Return NewsList
        Else
            Return 0
        End If

    End Function


    <WebMethod()> _
    Public Function LoadNewsandEvents()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = " SELECT TOP 3 PostID,PostDate,PostedBy,HTML,PostTitle,NewsType,LEFT(PlainText,250)PlainText,PostType,Hashtag,EventLocation,EventDate FROM NewsPosts ORDER BY PostDate DESC"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows.Count > 0 Then

            NewsList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim i As New NewsPosts
                i.NewsPostID = item("PostID")
                i.PostTitle = item("PostTitle")
                i.PostDate = CDate(item("PostDate"))
                i.PostedBy = item("PostedBy")
                i.HTML = item("HTML")
                i.PlainText = item("PlainText") + "..."
                i.NewsType = item("PostType")
                If i.NewsType = "Event" Then
                    i.EventLocation = item("EventLocation")
                    i.EventMonth = GetMonth(CDate(item("EventDate")).Month)
                    i.EventDay = CDate(item("EventDate")).Day.ToString()

                Else
                    i.EventLocation = ""
                    i.EventMonth = ""
                    i.EventDay = ""
                End If
                NewsList.Add(i)
            Next
            Return NewsList
        Else
            Return 0
        End If

    End Function

    <WebMethod()> _
    Public Function GetMonth(ByVal MonthInt As Integer)
        Select Case MonthInt
            Case 1
                Return "January"
            Case 2
                Return "February"
            Case 3
                Return "March"
            Case 4
                Return "April"
            Case 5
                Return "May"
            Case 6
                Return "June"
            Case 7
                Return "July"
            Case 8
                Return "August"
            Case 9
                Return "September"
            Case 10
                Return "October"
            Case 11
                Return "November"
            Case 12
                Return "December"
            Case Else
                Return ""
        End Select

    End Function


    <WebMethod()> _
    Public Function GetNewArrivals()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT TOP 4 p.ProductID, p.SKU,p.Category FROM Products p ORDER BY ProductID DESC"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows.Count > 0 Then

            BaseProductList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim i As New BaseProducts
                i.ProductID = item("ProductId")
                i.SKU = item("SKU")
                i.Category = item("Category")
                i.Price = GetFeaturedProductDetails(i.ProductID, i.Category, "Price")
                i.Name = GetFeaturedProductDetails(i.ProductID, i.Category, "Name") 
                BaseProductList.Add(i)
            Next
            Return BaseProductList
        Else
            Return 0
        End If

        Return ""
    End Function

    <WebMethod()> _
    Public Function GetFeaturedProductDetails(ByVal ProductID As String, ByVal Category As String, ByVal oParam As String)

        Dim dt As New DataTable
        Select Case Category
            Case "Accessory"
                dt = FillFeaturedProductDetailsDT("SELECT Name,Price FROM Accessories WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Name")
                    Case "Price"
                        Return dt.Rows(0).Item("Price")
                    Case Else
                        Return " "
                End Select
            Case "Apparel"
                dt = FillFeaturedProductDetailsDT("SELECT Name,Price FROM Accessories WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Name")
                    Case "Price"
                        Return dt.Rows(0).Item("Price")
                    Case Else
                        Return " "
                End Select
            Case "Cigars"
                dt = FillFeaturedProductDetailsDT("SELECT Brand, Name, BoxPrice FROM Cigars WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Brand") & " " & dt.Rows(0).Item("Name")
                    Case "Price"
                        Return dt.Rows(0).Item("BoxPrice")
                    Case Else
                        Return " "
                End Select
            Case "Coffee"
                dt = FillFeaturedProductDetailsDT("SELECT Brand, Name, Price, FROM Coffee WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Brand") & " " & dt.Rows(0).Item("Name")
                    Case "Price"
                        Return dt.Rows(0).Item("Price")
                    Case Else
                        Return " "
                End Select
            Case "Pipes"
                dt = FillFeaturedProductDetailsDT("SELECT Brand, Name, Price, FROM Pipes WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Brand") & " " & dt.Rows(0).Item("Name")
                    Case "Price"
                        Return dt.Rows(0).Item("Price")
                    Case Else
                        Return " "
                End Select
            Case "Pipe Tobacco"
                dt = FillFeaturedProductDetailsDT("SELECT Brand, Tobacco, Price, FROM PipeTobacco WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Brand") & " " & dt.Rows(0).Item("Tobacco")
                    Case "Price"
                        Return dt.Rows(0).Item("Price")
                    Case Else
                        Return " "
                End Select
            Case Else
                Return " "

        End Select





	

    End Function

    Public Function FillFeaturedProductDetailsDT(ByVal command As String)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("conne").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = command
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using
        Return dt
    End Function

    <WebMethod()> _
    Public Function SubscribeToMailingList(ByVal Email As String)



        'This code should be working. 
        ' When I test, it seems to be going through. I get an email to confirm subscribtion via Mail Chimp.
        ' However, Within 5 min of subscribing I Don't see any subscribers in my list. 
        ' I will check this later. 
        Dim APIKey As String = "c4269d910681eaa8982069decf1e3175-us11"
        Dim MCManager = New MailChimpManager(APIKey)
        Dim MCEmail As New MailChimp.Helper.EmailParameter
        With MCEmail
            MCEmail.Email = Email
        End With
        MCManager.Subscribe("15bdfeb383", MCEmail)

        Return ""
    End Function



#End Region

#Region "Events Page"

    <WebMethod()> _
    Public Function GetAllNewsandEvents()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM NewsPosts"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows.Count > 0 Then

            NewsList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim i As New NewsPosts
                i.NewsPostID = item("PostID")
                i.PostTitle = item("PostTitle")
                i.PostDate = item("PostDate")
                i.PostedBy = item("PostedBy")
                i.HTML = item("HTML")
                NewsList.Add(i)
            Next
            Return NewsList
        Else
            Return 0
        End If
    End Function

    <WebMethod()> _
    Public Function GetAllNews()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM NewsPosts WHERE NewsType = 'News'"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows.Count > 0 Then

            NewsList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim i As New NewsPosts
                i.NewsPostID = item("PostID")
                i.PostTitle = item("PostTitle")
                i.PostDate = item("PostDate")
                i.PostedBy = item("PostedBy")
                i.HTML = item("HTML")
                NewsList.Add(i)
            Next
            Return NewsList
        Else
            Return 0
        End If
    End Function

    <WebMethod()> _
    Public Function GetAllEvents()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM NewsPosts WHERE NewsType = 'Events'"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows.Count > 0 Then

            NewsList.Clear()
            For Each item As DataRow In dt.Rows()
                Dim i As New NewsPosts
                i.NewsPostID = item("PostID")
                i.PostTitle = item("PostTitle")
                i.PostDate = item("PostDate")
                i.PostedBy = item("PostedBy")
                i.HTML = item("HTML")
                NewsList.Add(i)
            Next
            Return NewsList
        Else
            Return 0
        End If
    End Function
#End Region

#Region "Product Pages"

    ' Use GetProducts to pull in all products for any category page.
    <WebMethod()> _
    Public Function GetProducts(ByVal ProductCategory As String)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Select Case ProductCategory

            Case "Accessory"
                dt = FillDataTable("SELECT * FROM Accessories")
                If dt.Rows.Count > 0 Then
                    AccessoryList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim A As New Accessory()
                        A.AccessoryID = CStr(item("AccessoryID"))
                        A.Brand = item("Brand")
                        A.Description = item("Description")
                        A.Name = item("Name")
                        A.Qty = CStr(item("Qty"))
                        A.SKU = item("SKU")
                        A.ProductID = CStr(item("ProductID"))
                        Dim DecPrice As Decimal = Decimal.Round(item("Price"), 2)
                        A.Price = CStr(DecPrice)
                        A.IsFeatured = item("IsFeatured")
                        AccessoryList.Add(A)
                    Next
                    Return AccessoryList
                Else
                    Return "0"
                End If

            Case "Apparel"
                dt = FillDataTable("SELECT * FROM Apparel")
                If dt.Rows.Count > 0 Then

                    ApparelList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim A As New Apparel
                        A.ApparelID = item("ApparelID")
                        A.SKU = item("SKU")
                        A.Name = item("Name")
                        A.ProductID = item("ProductID")
                        A.Description = item("Description")
                        A.Price = Math.Round(item("Price"), 2)
                        A.XS = item("XS_Qty")
                        A.SM = item("SM_Qty")
                        A.MD = item("MD_Qty")
                        A.LG = item("LG_Qty")
                        A.XL = item("XL_Qty")
                        A.XXL = item("XXL_Qty")
                        A.XXXL = item("XXXL_Qty")
                        A.IsFeatured = item("IsFeatured")
                        ApparelList.Add(A)
                    Next
                    Return ApparelList
                Else
                    Return 0
                End If

            Case "Cigars"
                dt = FillDataTable("SELECT * FROM Cigars")

                If dt.Rows.Count > 0 Then

                    CigarList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim C As New Cigar
                        C.CigarID = item("CigarID")
                        C.ProductID = item("ProductID")
                        C.Brand = item("Brand")
                        C.SKU = item("SKU")
                        C.Name = item("Name")
                        C.Description = item("Description")
                        C.SinglePrice = Math.Round(item("SinglePrice"), 2)
                        C.BoxPrice = Math.Round(item("BoxPrice"), 2)
                        C.Length = item("Length")
                        C.Ring = item("Ring")
                        C.BoxCount = item("BoxCount")
                        C.BoxQty = item("BoxQty")
                        C.SingleQty = item("SingleQty")
                        C.IsBoxSaleOnly = item("IsBoxSaleOnly")
                        C.IsSingleSaleOnly = item("IsSingleSaleOnly")
                        C.IsFeatured = item("IsFeatured")
                        CigarList.Add(C)
                    Next
                    Return CigarList
                Else
                    Return 0
                End If
            Case "Coffee"
                dt = FillDataTable("SELECT * FROM Coffee")
                If dt.Rows.Count > 0 Then

                    CoffeeList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim C As New Coffee
                        C.CoffeeID = item("CoffeeID")
                        C.SKU = item("SKU")
                        C.Name = item("Name")
                        C.ProductID = item("ProductID")
                        C.Description = item("Description")
                        C.Price = Math.Round(item("Price"), 2)
                        C.Brand = item("Brand")
                        C.Qty = CStr(item("Qty"))
                        C.Roast = item("Roast")
                        C.Body = item("Body")
                        C.IsFeatured = item("IsFeatured")
                        CoffeeList.Add(C)
                    Next
                    Return CoffeeList
                Else
                    Return 0
                End If

            Case "Pipes"
                dt = FillDataTable("SELECT * FROM Pipes ")
                If dt.Rows.Count > 0 Then

                    PipeList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim P As New Pipe
                        P.PipeID = item("PipeID")
                        P.ProductID = item("ProductID")
                        P.Brand = item("Brand")
                        P.SKU = item("SKU")
                        P.Name = item("Name")
                        P.Description = item("Description")
                        P.Price = Math.Round(item("Price"), 2)
                        P.Qty = item("Qty")
                        P.StemShape = item("StemShape")
                        P.BowlFinish = item("BowlFinish")
                        P.BodyShape = item("BodyShape")
                        P.Material = item("Material")
                        P.IsFeatured = item("IsFeatured")
                        PipeList.Add(P)
                    Next
                    Return PipeList
                Else
                    Return 0
                End If
            Case "Pipe Tobacco"
                dt = FillDataTable("SELECT * FROM PipeTobacco ")
                If dt.Rows.Count > 0 Then

                    PipeTobaccoList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim PT As New PipeTobacco
                        PT.PipeTobaccoID = item("PipeTobaccoID")
                        PT.ProductID = item("ProductID")
                        PT.Brand = item("Brand")
                        PT.SKU = item("SKU")
                        PT.Tobacco = item("Tobacco")
                        PT.Description = item("Description")
                        PT.Price = Math.Round(item("Price"), 2)
                        PT.Qty = item("Qty")
                        PT.Style = item("Style")
                        PT.Cut = item("Cut")
                        PT.Strength = item("Strength")
                        PT.IsFeatured = item("IsFeatured")
                        PipeTobaccoList.Add(PT)
                    Next
                    Return PipeTobaccoList
                Else
                    Return 0
                End If
            Case Else
                Return "0"
        End Select
    End Function

    ' USe GetProductDetails to pull in specific details for 1 product
    <WebMethod()> _
    Public Function GetProductDetails(ByVal ProductID As Integer, ByVal ProductCategory As String)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Select Case ProductCategory

            Case "Accessory"
                dt = FillDataTable("SELECT * FROM Accessories WHERE ProductID = " & ProductID)
                If dt.Rows.Count > 0 Then
                    AccessoryList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim A As New Accessory()
                        A.AccessoryID = CStr(item("AccessoryID"))
                        A.Brand = item("Brand")
                        A.Description = item("Description")
                        A.Name = item("Name")
                        A.Qty = CStr(item("Qty"))
                        A.SKU = item("SKU")
                        A.ProductID = CStr(item("ProductID"))
                        Dim DecPrice As Decimal = Decimal.Round(item("Price"), 2)
                        A.Price = CStr(DecPrice)
                        A.IsFeatured = item("IsFeatured")
                        AccessoryList.Add(A)
                    Next
                    Return AccessoryList
                Else
                    Return "0"
                End If

            Case "Apparel"
                dt = FillDataTable("SELECT * FROM Apparel  WHERE ProductID = " & ProductID)
                If dt.Rows.Count > 0 Then

                    ApparelList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim A As New Apparel
                        A.ApparelID = item("ApparelID")
                        A.SKU = item("SKU")
                        A.Name = item("Name")
                        A.ProductID = item("ProductID")
                        A.Description = item("Description")
                        A.Price = Math.Round(item("Price"), 2)
                        A.XS = item("XS_Qty")
                        A.SM = item("SM_Qty")
                        A.MD = item("MD_Qty")
                        A.LG = item("LG_Qty")
                        A.XL = item("XL_Qty")
                        A.XXL = item("XXL_Qty")
                        A.XXXL = item("XXXL_Qty")
                        A.IsFeatured = item("IsFeatured")
                        ApparelList.Add(A)
                    Next
                    Return ApparelList
                Else
                    Return 0
                End If

            Case "Cigars"
                dt = FillDataTable("SELECT * FROM Cigars  WHERE ProductID = " & ProductID)

                If dt.Rows.Count > 0 Then

                    CigarList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim C As New Cigar
                        C.CigarID = item("CigarID")
                        C.ProductID = item("ProductID")
                        C.Brand = item("Brand")
                        C.SKU = item("SKU")
                        C.Name = item("Name")
                        C.Description = item("Description")
                        C.SinglePrice = Math.Round(item("SinglePrice"), 2)
                        C.BoxPrice = Math.Round(item("BoxPrice"), 2)
                        C.Length = item("Length")
                        C.Ring = item("Ring")
                        C.BoxCount = item("BoxCount")
                        C.BoxQty = item("BoxQty")
                        C.SingleQty = item("SingleQty")
                        C.IsBoxSaleOnly = item("IsBoxSaleOnly")
                        C.IsSingleSaleOnly = item("IsSingleSaleOnly")
                        C.IsFeatured = item("IsFeatured")
                        CigarList.Add(C)
                    Next
                    Return CigarList
                Else
                    Return 0
                End If
            Case "Coffee"
                dt = FillDataTable("SELECT * FROM Coffee  WHERE ProductID = " & ProductID)
                If dt.Rows.Count > 0 Then

                    CoffeeList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim C As New Coffee
                        C.CoffeeID = item("CoffeeID")
                        C.SKU = item("SKU")
                        C.Name = item("Name")
                        C.ProductID = item("ProductID")
                        C.Description = item("Description")
                        C.Price = Math.Round(item("Price"), 2)
                        C.Brand = item("Brand")
                        C.Qty = CStr(item("Qty"))
                        C.Roast = item("Roast")
                        C.Body = item("Body")
                        C.IsFeatured = item("IsFeatured")
                        CoffeeList.Add(C)
                    Next
                    Return CoffeeList
                Else
                    Return 0
                End If

            Case "Pipes"
                dt = FillDataTable("SELECT * FROM Pipes  WHERE ProductID = " & ProductID)
                If dt.Rows.Count > 0 Then

                    PipeList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim P As New Pipe
                        P.PipeID = item("PipeID")
                        P.ProductID = item("ProductID")
                        P.Brand = item("Brand")
                        P.SKU = item("SKU")
                        P.Name = item("Name")
                        P.Description = item("Description")
                        P.Price = Math.Round(item("Price"), 2)
                        P.Qty = item("Qty")
                        P.StemShape = item("StemShape")
                        P.BowlFinish = item("BowlFinish")
                        P.BodyShape = item("BodyShape")
                        P.Material = item("Material")
                        P.IsFeatured = item("IsFeatured")
                        PipeList.Add(P)
                    Next
                    Return PipeList
                Else
                    Return 0
                End If
            Case "Pipe Tobacco"
                dt = FillDataTable("SELECT * FROM PipeTobacco  WHERE ProductID = " & ProductID)
                If dt.Rows.Count > 0 Then

                    PipeTobaccoList.Clear()
                    For Each item As DataRow In dt.Rows()
                        Dim PT As New PipeTobacco
                        PT.PipeTobaccoID = item("PipeTobaccoID")
                        PT.ProductID = item("ProductID")
                        PT.Brand = item("Brand")
                        PT.SKU = item("SKU")
                        PT.Tobacco = item("Tobacco")
                        PT.Description = item("Description")
                        PT.Price = Math.Round(item("Price"), 2)
                        PT.Qty = item("Qty")
                        PT.Style = item("Style")
                        PT.Cut = item("Cut")
                        PT.Strength = item("Strength")
                        PT.IsFeatured = item("IsFeatured")
                        PipeTobaccoList.Add(PT)
                    Next
                    Return PipeTobaccoList
                Else
                    Return 0
                End If
            Case Else
                Return "0"
        End Select

    End Function

#End Region

#Region "Database"

    ' Generic dt filler
    <WebMethod()> _
    Public Function FillDataTable(ByVal commandtext As String)
        Dim con As SqlConnection = GetConnected()
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.CommandType = CommandType.Text
            cmd.CommandText = commandtext
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function


    <WebMethod()> _
    Public Function GetConnected()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Return con
    End Function

#End Region

End Class