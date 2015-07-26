Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports AuthorizeNet
Imports MailChimp

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
        Public NewsType As String
    End Class


    Public Class BaseProducts
        Public ProductID As String
        Public SKU As String
        Public Category As String
    End Class

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


        Return ""
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


    Dim NewsList As New List(Of NewsPosts)

    <WebMethod()> _
    Public Function LoadNewsandEvents()

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
                i.NewsPostID = item("PipeTobaccoID")
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


    Dim BaseProductList As New List(Of BaseProducts)

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
       
                BaseProductList.Add(i)
            Next
            Return BaseProductList
        Else
            Return 0
        End If

        Return ""
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



End Class