Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports MailChimp
Imports AuthorizeNet
Imports System.Security
Imports System.Security.Cryptography



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Engine
    Inherits System.Web.Services.WebService

    Public Class WinstonsUser
        Public UserID As Integer
        Public AuthorizeNbr As String
        Public Email As String
        Public FirstName As String
        Public LastName As String
        Public Street As String
        Public City As String
        Public State As String
        Public Zip As String
        Public IsPasswordReset As Boolean
        Public Password As String
    End Class


    Public Class ShoppingCart
        Public ProductID As Integer
        Public Qty As Integer
        Public Notes As String 'Use this property to store values like Apparel Size selected,
        Public Price As Decimal
        Public Category As String
        Public ItemName As String
        Public scCount As Integer
        Public BasePrice As Decimal
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
        Public Body As String
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

        'Step 1 - Create the request
        Dim Request = New AuthorizationRequest("CARDNBR", "1216", 10.0, "Description")

        'Step 2 - Create the gateway, sending in your credentials
        Dim gate = New Gateway("APILOGIN", "TRANSKEY")
        ' Step 3 - Send the request to the gateway
        Dim response = gate.Send(Request)

        'Use for codes to showing to customer, and storing transaction id's in db
        Dim ResponseCode As String = response.ResponseCode
        Dim ResponseMsg As String = response.Message

        Dim Target As CustomerGateway = New CustomerGateway("APILOGIN", "TRANSKEY")
        Dim Custy = Target.CreateCustomer("email", "description")

        Dim CustyID As String = Custy.ProfileID

        Target.AddCreditCard(CustyID, "CARDNBR", 12, 2015, "123")

        Dim Customer = Target.GetCustomer(CustyID)


        Dim Card = Customer.PaymentProfiles(0)


        Dim Request2 = New AuthorizationRequest(Card.CardNumber, Card.CardExpiration, 10.0, "Description")

        Return ""
    End Function


    <WebMethod()> _
    Public Function UnHashIt(ByVal hashOfInput As String, ByVal ControlHash As String)

        ' Hash the input. 
        ' Dim hashOfInput As String = GetHash(md5Hash, input)

        ' Create a StringComparer an compare the hashes. 
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase

        If 0 = comparer.Compare(hashOfInput, ControlHash) Then
            Return True
        Else
            Return False
        End If
    End Function

    <WebMethod()> _
    Public Function GetHash(ByVal Hash As MD5, ByVal Input As String)

        ' Convert the input string to a byte array and compute the hash. 
        Dim data As Byte() = Hash.ComputeHash(Encoding.UTF8.GetBytes(Input))

        ' Create a new Stringbuilder to collect the bytes 
        ' and create a string. 
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data  
        ' and format each one as a hexadecimal string. 
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string. 
        Return sBuilder.ToString()

    End Function

    <WebMethod()> _
    Public Function CreateNewCustomer(ByVal cnbr As String, ByVal mm As String, ByVal yyyy As String, ByVal csv As String, ByVal email As String, ByVal Password As String)

        Dim hash As String
        Using md5Hash As MD5 = MD5.Create()
            hash = GetHash(md5Hash, Password)
        End Using

        'Remember to use hash when inserting into SQL


        Dim Target As CustomerGateway = New CustomerGateway("APILOGIN", "TRANSKEY")
        Dim Custy = Target.CreateCustomer(email, "Winston's Humidor Customer Profile")
        Dim CustyID As String = Custy.ProfileID

        ' TODO --> Save customer profileid in the database
        ' 

        Return ""
    End Function

    

    <WebMethod()> _
    Public Function LoginUser(ByVal Email As String, ByVal Password As String)

        Dim HashedPassed As String = Nothing

            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
            Dim dt As New DataTable
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM Users WHERE Email = " & Email
                cmd.Connection.Close()
            End Using

        If dt.Rows.Count > 0 Then
            'now we unhash and compare
            If UnHashIt(Password, dt.Rows(0).Item("Password")) Then


                ' [UserID]()
                ',[AuthorizeProfileID]
                ',[Email]
                ',[FirstName]
                ',[LastName]
                ',[Street]
                ',[City]
                ',[State]
                ',[Zip]
                ',[IsPasswordReset]
                ',[Password]


                Dim UserList As New List(Of WinstonsUser)
                For Each item As DataRow In dt.Rows()
                    Dim i As New WinstonsUser
                    i.UserID = item("UserID")
                    i.AuthorizeNbr = item("AuthorizeProfileID")
                    i.Email = item("Email")
                    i.FirstName = item("FirstName")
                    i.LastName = item("LastName")
                    i.Street = item("Street")
                    i.City = item("City")
                    i.State = item("State")
                    i.Zip = item("Zip")
                    UserList.Add(i)
                Next

                Return UserList
            End If
        Else
            Return 0
            ' on return zero, then the email entered is not found.
        End If

        Return ""
    End Function

    <WebMethod()> _
    Public Function ApplyDiscount(ByVal DiscountCode As String)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT DiscountAmount FROM Configuration WHERE DiscountCode = '" & DiscountCode & "' AND DiscountCodeIsValid = 1"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows().Count = 0 Then
            Return 0
        Else
            Return Math.Round(dt.Rows(0).Item("DiscountAmount"), 2)
        End If

    End Function




    <WebMethod()> _
    Public Function RetrieveCustomer(ByVal Email As String, ByVal Password As String)
        'TODO --> Pull customer profileid from the database
        Dim Target As CustomerGateway = New CustomerGateway("APILOGIN", "TRANSKEY")
        Target.GetCustomer("ProfileID")
        Return Target
    End Function

    <WebMethod(True)> _
    Public Function RemoveItemFromCart(ByVal ProdID As Integer, ByVal Notes As String, ByVal Qty As String, ByVal Price As String)
        Dim InMyCart As List(Of ShoppingCart) = Session("Cart")

        If InMyCart Is Nothing Then
            Return 0
        End If


        Dim Total As Decimal = 0.0
        For i As Integer = 0 To InMyCart.Count - 1

            If InMyCart(i).ProductID = ProdID AndAlso InMyCart(i).Notes = Notes Then
                InMyCart.RemoveAt(i)
                Session("Cart") = InMyCart
                Return ""
            End If

        Next
        Return ""
    End Function
    <WebMethod(True)> _
    Public Function UpdateItemTotal(ByVal ProdID As String, ByVal Notes As String, ByVal DesiredQty As String)
        Dim InMyCart As List(Of ShoppingCart) = Session("Cart")

        If InMyCart Is Nothing Then
            Return 0
        End If

        For i As Integer = 0 To InMyCart.Count - 1

            If InMyCart(i).ProductID = ProdID AndAlso InMyCart(i).Notes = Notes Then
                InMyCart(i).Qty = DesiredQty
                InMyCart(i).Price = CInt(DesiredQty) * InMyCart(i).BasePrice
                Session("Cart") = InMyCart
                Return ""
            End If

        Next
        Return ""
    End Function





    <WebMethod(True)> _
    Public Function AddToCart(ByVal ProductID As String, ByVal Qty As String, ByVal Notes As String, ByVal Price As String, ByVal Category As String, ByVal ItemName As String, ByVal UnitPrice As String) As String

        If Session("Cart") Is Nothing Then
            Dim i As New ShoppingCart
            Dim itemList As New List(Of ShoppingCart)
            i.ProductID = CInt(ProductID)
            i.Qty = CInt(Qty)
            i.Notes = Notes
            Dim dPrice As Decimal = CDec(Price)
            i.Price = Math.Round(dPrice, 2)

            i.Category = Category
            i.ItemName = ItemName
            i.scCount = 1
            Dim dBasePrice As Decimal = CDec(UnitPrice)
            i.BasePrice = Math.Round(dBasePrice, 2)
            itemList.Add(i)
            Session("Cart") = itemList
        Else
            Dim itemlist As List(Of ShoppingCart) = Session("Cart")
            Dim IsDuplicate As Boolean = False

            For c As Integer = 0 To itemlist.Count - 1

                If itemlist(c).ProductID = ProductID AndAlso itemlist(c).Notes = Notes Then
                    IsDuplicate = True
                    Return 2
                Else
                    IsDuplicate = False
                End If

            Next

            If IsDuplicate = False Then
                Dim i As New ShoppingCart
                i.ProductID = ProductID
                i.Qty = Qty
                i.ItemName = ItemName
                i.Notes = Notes
                Dim dPrice As Decimal = CDec(Price)
                i.Price = Math.Round(dPrice, 2)
                i.Category = Category
                Dim dBasePrice As Decimal = CDec(UnitPrice)
                i.BasePrice = Math.Round(dBasePrice, 2)
                i.scCount = itemlist.Count + 1
                itemlist.Add(i)
            Else
                Return 2
            End If




        End If

        Return ""
    End Function


    <WebMethod(True)> _
    Public Function GetCartTotal()
        Dim InMyCart As List(Of ShoppingCart) = Session("Cart")

        If InMyCart Is Nothing Then
            Return 0
        End If
        Dim ReturnList As New List(Of ShoppingCart)
        Dim Total As Decimal = 0.0
        For i As Integer = 0 To InMyCart.Count - 1
            Dim sc As New ShoppingCart

            Dim Price As Decimal = InMyCart(i).Price

            Total = Total + Price
        Next
        Return Math.Round(Total, 2)

    End Function

    <WebMethod(True)> _
    Public Function GetCartCount()
        Dim InMyCart As List(Of ShoppingCart) = Session("Cart")

        If InMyCart Is Nothing Then
            Return 0
        End If
        Return InMyCart.Count
    End Function



    <WebMethod(True)> _
    Public Function GoToCart()

        Dim InMyCart As List(Of ShoppingCart) = Session("Cart")

        If InMyCart Is Nothing Then
            Return 0
        End If
        If InMyCart.Count = 0 Then
            Return 0
        End If

        Dim ReturnList As New List(Of ShoppingCart)

        For i As Integer = 0 To InMyCart.Count - 1
            Dim sc As New ShoppingCart
            Dim ProductID As Integer = InMyCart(i).ProductID
            Dim Qty As Integer = InMyCart(i).Qty
            Dim Notes As String = InMyCart(i).Notes
            Dim Price As Decimal = InMyCart(i).Price
            Dim DisplayName As String = InMyCart(i).ItemName
            Dim Category As String = InMyCart(i).Category
            '  Dim Item As String = GetItemDetails(ProductID)
            sc.ProductID = ProductID
            sc.Qty = Qty
            sc.ItemName = DisplayName
            sc.Category = Category
            sc.Notes = Notes
            sc.Price = Math.Round(Price, 2)
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
    Public Function SortNewsandEvents(ByVal Category As String)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            If Category = "All" Then
                cmd.CommandText = " SELECT PostID,PostDate,PostedBy,HTML,PostTitle,NewsType,LEFT(PlainText,250)PlainText,PostType,Hashtag,EventLocation,EventDate FROM NewsPosts ORDER BY PostDate DESC"
            Else
                cmd.CommandText = " SELECT PostID,PostDate,PostedBy,HTML,PostTitle,NewsType,LEFT(PlainText,250)PlainText,PostType,Hashtag,EventLocation,EventDate FROM NewsPosts WHERE PostType='" & Category & "' ORDER BY PostDate DESC"
            End If

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
    Public Function LoadNewsandEvents()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = " SELECT PostID,PostDate,PostedBy,HTML,PostTitle,NewsType,LEFT(PlainText,250)PlainText,PostType,Hashtag,EventLocation,EventDate FROM NewsPosts ORDER BY PostDate DESC"
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
                        Return Math.Round(dt.Rows(0).Item("Price"), 2)
                    Case Else
                        Return " "
                End Select
            Case "Apparel"
                dt = FillFeaturedProductDetailsDT("SELECT Name,Price FROM Accessories WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Name")
                    Case "Price"
                        Return Math.Round(dt.Rows(0).Item("Price"), 2)
                    Case Else
                        Return " "
                End Select
            Case "Cigars"
                dt = FillFeaturedProductDetailsDT("SELECT Brand, Name, BoxPrice FROM Cigars WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Brand") & " " & dt.Rows(0).Item("Name")
                    Case "Price"
                        Return Math.Round(dt.Rows(0).Item("BoxPrice"), 2)
                    Case Else
                        Return " "
                End Select
            Case "Coffee"
                dt = FillFeaturedProductDetailsDT("SELECT Brand, Name, Price, FROM Coffee WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Brand") & " " & dt.Rows(0).Item("Name")
                    Case "Price"
                        Return Math.Round(dt.Rows(0).Item("Price"), 2)
                    Case Else
                        Return " "
                End Select
            Case "Pipes"
                dt = FillFeaturedProductDetailsDT("SELECT Brand, Name, Price FROM Pipes WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Brand") & " " & dt.Rows(0).Item("Name")
                    Case "Price"
                        Return Math.Round(dt.Rows(0).Item("Price"), 2)
                    Case Else
                        Return " "
                End Select
            Case "PipeTobacco"
                dt = FillFeaturedProductDetailsDT("SELECT Brand, Tobacco, Price FROM PipeTobacco WHERE ProductID =" & ProductID)
                Select Case oParam
                    Case "Name"
                        Return dt.Rows(0).Item("Brand") & " " & dt.Rows(0).Item("Tobacco")
                    Case "Price"
                        Return Math.Round(dt.Rows(0).Item("Price"), 2)
                    Case Else
                        Return " "
                End Select
            Case Else
                Return " "

        End Select





	

    End Function

    Public Function FillFeaturedProductDetailsDT(ByVal command As String)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
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
        If Trim(Email) = String.Empty Then
            Return ""
        End If
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
    Public Function GetFeaturedProductInfo(ByVal ProductCategory As String)
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
                dt = FillDataTable("SELECT TOP 4 * FROM Cigars WHERE IsFeatured = 1")

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
                        C.Body = item("Body")
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
                dt = FillDataTable("SELECT TOP 4 * FROM Pipes WHERE IsFeatured = 1 ")
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
            Case "PipeTobacco"
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

    ' Use GetProducts to pull in all products for any category page.
    <WebMethod()> _
    Public Function SearchProducts(ByVal ProductCategory As String, ByVal SearchText As String)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable

        Select Case ProductCategory

            Case "Accessory"
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Accessories")
                Else
                    dt = FillDataTable("SELECT * FROM Accessories WHERE Brand LIKE '%" & SearchText & "%' OR Name LIKE '%" & SearchText & "%'")
                End If

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
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Apparel")
                Else
                    dt = FillDataTable("SELECT * FROM Apparel WHERE Name LIKE '%" & SearchText & "%'")
                End If

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
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Cigars")
                Else
                    dt = FillDataTable("SELECT * FROM Cigars WHERE Brand LIKE '%" & SearchText & "%' OR Name LIKE '%" & SearchText & "%' OR Body LIKE '%" & SearchText & "%'")
                End If


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
                        C.Body = item("Body")
                        CigarList.Add(C)
                    Next
                    Return CigarList
                Else
                    Return 0
                End If
            Case "Coffee"
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Coffee")
                Else
                    dt = FillDataTable("SELECT * FROM Coffee WHERE Brand LIKE '%" & SearchText & "%' OR Name LIKE '%" & SearchText & "%' OR Roast LIKE '%" & SearchText & "%' OR Body LIKE '%" & SearchText & "%'")
                End If

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
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Pipes")
                Else
                    dt = FillDataTable("SELECT * FROM Pipes WHERE Brand LIKE '%" & SearchText & "%' OR Name LIKE '%" & SearchText & "%' OR StemShape LIKE '%" & SearchText & "%' OR BowlFinish LIKE '%" & SearchText & "%'  OR BodyShape LIKE '%" & SearchText & "%'  OR Material LIKE '%" & SearchText & "%'")
                End If

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
            Case "PipeTobacco"
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM PipeTobacco")
                Else
                    dt = FillDataTable("SELECT * FROM PipeTobacco WHERE Brand LIKE '%" & SearchText & "%' OR Tobacco LIKE '%" & SearchText & "%' OR Style LIKE '%" & SearchText & "%' OR Cut LIKE '%" & SearchText & "%' OR Strength LIKE '%" & SearchText & "%'")
                End If

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
                        C.Body = item("Body")
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
            Case "PipeTobacco"
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
                        C.Body = item("Body")
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
            Case "PipeTobacco"
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