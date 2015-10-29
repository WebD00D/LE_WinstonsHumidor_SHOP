Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports MailChimp
Imports AuthorizeNet
Imports System.Security
Imports System.Security.Cryptography
Imports System.IO.MemoryStream
Imports System.Threading.Tasks
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Net.Mail.SmtpDeliveryFormat
Imports System.Net.Mail.SmtpDeliveryMethod
Imports System.Net.Mail.SmtpAccess
Imports System.Net.Mail.SmtpPermission
Imports System.Net.Mail.MailAddress


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
        Public CardType As String
        Public Last4 As String
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
        Public IsOnSale As String
        Public SalePrice As String
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
        Public IsOnSale As String
        Public SalePrice As String
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
        Public IsOnSale As String
        Public SalePrice As String
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
        Public SingleIsOnSale As String
        Public BoxIsOnSale As String
        Public SingleSalePrice As String
        Public BoxSalePrice As String
        Public Body As String
        Public MaxBoxPurchase As String
        Public MaxSinglePurchase As String
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
        Public IsOnSale As String
        Public SalePrice As String
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
        Public IsOnSale As String
        Public SalePrice As String

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


    <WebMethod()> _
    Public Function GetCigarCounts(ByVal ProductId As Integer)

        Dim cigar As New Cigar
        Dim CigarList As New List(Of Cigar)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT BoxCount,BoxQty,SingleQty FROM Cigars WHERE ProductID =" & ProductId
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()

            If dt.Rows.Count > 0 Then

                cigar.BoxCount = dt.Rows(0).Item("BoxCount")
                cigar.BoxQty = dt.Rows(0).Item("BoxQty")
                cigar.SingleQty = dt.Rows(0).Item("SingleQty")
                CigarList.Add(cigar)
                Return CigarList
            Else
                Return ""
            End If
       
        End Using

    End Function

    <WebMethod()> _
    Public Function GetAccessoryCounts(ByVal ProductId As Integer)

        Dim AccessoryList As New List(Of Accessory)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT Qty FROM Accessories WHERE ProductID =" & ProductId
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()

            If dt.Rows.Count > 0 Then

                Return dt.Rows(0).Item("Qty")
            Else
                Return ""
            End If

        End Using

    End Function

    <WebMethod()> _
    Public Function GetApparelCounts(ByVal ProductId As Integer)
        Dim Apparel As New Apparel
        Dim ApparelList As New List(Of Apparel)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT * FROM Apparel WHERE ProductID =" & ProductId
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
  
            If dt.Rows.Count > 0 Then
                Apparel.XS = dt.Rows(0).Item("XS_Qty")
                Apparel.SM = dt.Rows(0).Item("SM_Qty")
                Apparel.MD = dt.Rows(0).Item("MD_Qty")
                Apparel.LG = dt.Rows(0).Item("LG_Qty")
                Apparel.XL = dt.Rows(0).Item("XL_Qty")
                Apparel.XXL = dt.Rows(0).Item("XXL_Qty")
                Apparel.XXXL = dt.Rows(0).Item("XXXL_Qty")
                ApparelList.Add(Apparel)
                Return ApparelList
            Else
                Return ""
            End If

        End Using

    End Function

    <WebMethod()> _
    Public Function GetPipeCounts(ByVal ProductId As Integer)

        Dim PipeList As New List(Of Pipe)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT Qty FROM Pipes WHERE ProductID =" & ProductId
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()

            If dt.Rows.Count > 0 Then

                Return dt.Rows(0).Item("Qty")
            Else
                Return ""
            End If

        End Using

    End Function

    <WebMethod()> _
    Public Function GetPipeTobaccoCounts(ByVal ProductId As Integer)

        Dim PipeTobaccoList As New List(Of PipeTobacco)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT Qty FROM PipeTobacco WHERE ProductID =" & ProductId
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()

            If dt.Rows.Count > 0 Then

                Return dt.Rows(0).Item("Qty")
            Else
                Return ""
            End If

        End Using

    End Function

    <WebMethod()> _
    Public Function GetCoffeeCounts(ByVal ProductId As Integer)

        ' Dim CoffeeList As New List(Of Coffee)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT Qty FROM Coffee WHERE ProductID =" & ProductId
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()

            If dt.Rows.Count > 0 Then

                Return dt.Rows(0).Item("Qty")
            Else
                Return ""
            End If

        End Using

    End Function

    <WebMethod(True)> _
    Public Function UpdateInventory()

    
        Dim myShoppingCart As List(Of ShoppingCart) = Session("Cart")
        Dim cartItem As New ShoppingCart
        Dim cmdtext As String = " "

        For i As Integer = 0 To myShoppingCart.Count - 1
            cartItem = myShoppingCart(i)

            Select Case cartItem.Category

                Case "Cigars"
                    Dim Cigar As List(Of Cigar) = GetCigarCounts(cartItem.ProductID)
                    Dim FullBoxCount As Integer = Cigar(0).BoxCount
                    Dim OldFullBoxQty As Integer = Cigar(0).BoxQty
                    Dim OldSingleQty As Integer = Cigar(0).SingleQty
                    Dim NewSingleQty As Integer = Nothing
                    Dim NewFullBoxQty As Integer = Nothing
                    If cartItem.Notes.Contains("Single(s)") Then
                        'How many singles did they buy and how many are left?
                        Dim SinglesPurchased As Integer = cartItem.Qty
                        NewSingleQty = OldSingleQty - SinglesPurchased
                        NewFullBoxQty = Math.Floor(NewSingleQty / FullBoxCount)
                    Else
                        ' cartItem.Notes.Contains("Box(s)") it was a box sale
                        Dim FullBoxesPurchased As Integer = cartItem.Qty
                        NewFullBoxQty = OldFullBoxQty - FullBoxesPurchased
                        NewSingleQty = OldSingleQty - (FullBoxesPurchased * FullBoxCount)
                    End If

                    cmdtext = "UPDATE Cigars SET SingleQty = " & NewSingleQty & ",BoxQty = " & NewFullBoxQty & " WHERE ProductID = " & cartItem.ProductID


                Case "Accessory"

                    Dim OldAccessoryQty As Integer = GetAccessoryCounts(cartItem.ProductID)
                    Dim NewAccessoryQty As Integer = OldAccessoryQty - cartItem.Qty
                    cmdtext = "UPDATE Accessories SET Qty = " & NewAccessoryQty & " WHERE ProductID = " & cartItem.ProductID

                Case "Apparel"
                    Dim Apparel As List(Of Apparel) = GetApparelCounts(cartItem.ProductID)
                    Dim SizeDataColumn As String = Nothing
                    Dim OldApparelQty As Integer = Nothing
                    Select Case cartItem.Notes
                        Case "Size: XS"
                            SizeDataColumn = "XS_Qty"
                            OldApparelQty = CInt(Apparel(0).XS)
                        Case "Size: SM"
                            SizeDataColumn = "SM_Qty"
                            OldApparelQty = CInt(Apparel(0).SM)
                        Case "Size: MD"
                            SizeDataColumn = "MD_Qty"
                            OldApparelQty = CInt(Apparel(0).MD)
                        Case "Size: LG"
                            SizeDataColumn = "LG_Qty"
                            OldApparelQty = CInt(Apparel(0).LG)
                        Case "Size: XL"
                            SizeDataColumn = "XL_Qty"
                            OldApparelQty = CInt(Apparel(0).XL)
                        Case "Size: XXL"
                            SizeDataColumn = "XXL_Qty"
                            OldApparelQty = CInt(Apparel(0).XXL)
                        Case "Size: XXXL"
                            SizeDataColumn = "XXXL_Qty"
                            OldApparelQty = CInt(Apparel(0).XXXL)
                    End Select
                    Dim NewApparelQty As Integer = OldApparelQty - cartItem.Qty
                    cmdtext = "UPDATE Apparel SET " & SizeDataColumn & " = " & NewApparelQty & " WHERE ProductID = " & cartItem.ProductID

                Case "Pipes"
                    Dim OldPipeQty As Integer = GetPipeCounts(cartItem.ProductID)
                    Dim NewPipeQty As Integer = OldPipeQty - cartItem.Qty
                    cmdtext = "UPDATE Pipes SET Qty = " & NewPipeQty & " WHERE ProductID = " & cartItem.ProductID

                Case "PipeTobacco"

                    Dim OldPipeTobaccoQty As Integer = GetPipeTobaccoCounts(cartItem.ProductID)
                    Dim NewPipeTobaccoQty As Integer = OldPipeTobaccoQty - cartItem.Qty
                    cmdtext = "UPDATE PipeTobacco SET Qty = " & NewPipeTobaccoQty & " WHERE ProductID = " & cartItem.ProductID

                Case "Coffee"

                    Dim OldCoffeeQty As Integer = GetCoffeeCounts(cartItem.ProductID)
                    Dim NewCoffeeQty As Integer = OldCoffeeQty - cartItem.Qty
                    cmdtext = "UPDATE Coffee SET Qty = " & NewCoffeeQty & " WHERE ProductID = " & cartItem.ProductID

                Case Else
                    cmdtext = " "

            End Select

            Try
                If Not Trim(cmdtext) = String.Empty Then
                    Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
                    Using cmd As SqlCommand = con.CreateCommand
                        cmd.Connection = con
                        cmd.Connection.Open()
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = cmdtext
                        cmd.ExecuteNonQuery()
                        cmd.Connection.Close()
                    End Using
                End If
            Catch ex As Exception

            End Try
        Next


        Return ""

        'Protected Sub Bulk_Insert(sender As Object, e As EventArgs)
        '    Dim dt As New DataTable()
        '    dt.Columns.AddRange(New DataColumn(2) {New DataColumn("Id", GetType(Integer)), New DataColumn("Name", GetType(String)), New DataColumn("Country", GetType(String))})
        '    For Each row As GridViewRow In GridView1.Rows
        '        If TryCast(row.FindControl("CheckBox1"), CheckBox).Checked Then
        '            Dim id As Integer = Integer.Parse(row.Cells(1).Text)
        '            Dim name As String = row.Cells(2).Text
        '            Dim country As String = row.Cells(3).Text
        '            dt.Rows.Add(id, name, country)
        '        End If
        '    Next
        '    If dt.Rows.Count > 0 Then
        '        Dim consString As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        '        Using con As New SqlConnection(consString)
        '            Using cmd As New SqlCommand("Insert_Customers")
        '                cmd.CommandType = CommandType.StoredProcedure
        '                cmd.Connection = con
        '                cmd.Parameters.AddWithValue("@tblCustomers", dt)
        '                con.Open()
        '                cmd.ExecuteNonQuery()
        '                con.Close()
        '            End Using
        '        End Using
        '    End If
        'End Sub
    End Function


    <WebMethod()> _
    Public Function GetShippingRate()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT ShippingRate FROM Configuration WHERE ID = 1"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using
        Return Math.Round(dt.Rows(0).Item("ShippingRate"), 2)
    End Function

    <WebMethod(True)> _
    Public Function GetUserDetails()


        If Session("WinstonsUser") Is Nothing Then
            Return False
        Else
            Dim WinstonsUser As List(Of WinstonsUser) = Session("WinstonsUser")
            Return WinstonsUser
        End If

    End Function

    <WebMethod(True)> _
    Public Function CreateNewOrder(ByVal OrderTotal As Decimal, ByVal FirstName As String, ByVal LastName As String, ByVal Street As String, ByVal City As String, ByVal State As String, ByVal Zip As String, ByVal Email As String)

        '1) Create Order and get returned id back

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim OrderID As Integer = Nothing
        Dim OrderDT As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Insert_NewOrder"
            cmd.Parameters.AddWithValue("@OrderTotal", OrderTotal)
            cmd.Parameters.AddWithValue("@FirstName", FirstName)
            cmd.Parameters.AddWithValue("@LastName", LastName)
            cmd.Parameters.AddWithValue("@Street", Street)
            cmd.Parameters.AddWithValue("@City", City)
            cmd.Parameters.AddWithValue("@State", State)
            cmd.Parameters.AddWithValue("@Zip", Zip)
            cmd.Parameters.AddWithValue("@Email", Email)
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(OrderDT)
            End Using

            cmd.Connection.Close()
        End Using

        OrderID = OrderDT.Rows(0).Item(0)

        '2) Then insert each item in the order into order details table with the referenced Order ID
        Dim ShoppingCart As List(Of ShoppingCart) = Session("Cart")
        Dim OrderDetailsDT As New DataTable

        OrderDetailsDT.Columns.Add("OrderID")
        OrderDetailsDT.Columns.Add("ProductID")
        OrderDetailsDT.Columns.Add("ItemName")
        OrderDetailsDT.Columns.Add("Category")
        OrderDetailsDT.Columns.Add("Qty")
        OrderDetailsDT.Columns.Add("Notes")
        OrderDetailsDT.Columns.Add("Price")
        OrderDetailsDT.Columns.Add("BasePrice")

        For i As Integer = 0 To ShoppingCart.Count - 1
            Dim r As DataRow = OrderDetailsDT.NewRow()
            r("OrderID") = OrderID
            r("ProductID") = ShoppingCart(i).ProductID
            r("ItemName") = ShoppingCart(i).ItemName
            r("Category") = ShoppingCart(i).Category
            r("Qty") = ShoppingCart(i).Qty
            r("Notes") = ShoppingCart(i).Notes
            r("Price") = ShoppingCart(i).Price
            r("BasePrice") = ShoppingCart(i).BasePrice
            OrderDetailsDT.Rows.Add(r)
        Next

        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Insert_OrderDetails"
            cmd.Parameters.AddWithValue("@tblOrderDetails", OrderDetailsDT)
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End Using

        Return ""
    End Function

    <WebMethod(True)> _
    Public Function CreateUser(ByVal AuthorizeID As String, ByVal Email As String, ByVal Password As String, ByVal FirstName As String, ByVal LastName As String, ByVal Street As String, ByVal City As String, ByVal State As String, ByVal Zipcode As String)
        Try
            Dim hash As String
            Using md5Hash As MD5 = MD5.Create()
                hash = GetHash(md5Hash, Password)
            End Using
            'save user info in database
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sp_Insert_NewUser"
                cmd.Parameters.AddWithValue("@AuthorizeProfileID", AuthorizeID)
                cmd.Parameters.AddWithValue("@Email", Email)
                cmd.Parameters.AddWithValue("@FirstName ", FirstName)
                cmd.Parameters.AddWithValue("@LastName ", LastName)
                cmd.Parameters.AddWithValue("@Street ", Street)
                cmd.Parameters.AddWithValue("@City ", City)
                cmd.Parameters.AddWithValue("@State ", State)
                cmd.Parameters.AddWithValue("@Zip ", Zipcode)
                cmd.Parameters.AddWithValue("@Password ", hash)
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function


    <WebMethod(True)> _
    Public Function CheckoutAsGuest(ByVal Email As String, ByVal Password As String, ByVal FirstName As String, ByVal LastName As String, ByVal Street As String, ByVal City As String, ByVal State As String, ByVal Zipcode As String, ByVal CC As String, ByVal ccMonth As String, ByVal ccYear As String, ByVal ccSecurity As String, ByVal saveDetails As Boolean, ByVal SubTotal As String, ByVal Discount As String, ByVal Shipping As String, ByVal Tax As String, ByVal TotalToCharge As String)

        saveDetails = False ' change this once I figure out how to access stored customer.

        'Validate totals and such
        Dim _grandtotal As Decimal = CDec(TotalToCharge)
        Dim _subtotal As Decimal = CDec(SubTotal)
        Dim _tax As Decimal = CDec(Tax)
        Dim _discount As Decimal = CDec(Discount)
        Dim _shipping As Decimal = CDec(Shipping)

        Dim _TotalToValidate As Decimal = (_subtotal + _tax + _shipping) - Discount

        If Not _grandtotal = _TotalToValidate Then
            Return "FAILED"
        End If

        Try
            Dim Request = New AuthorizationRequest(CC, ccMonth & "" & ccYear, CDec(TotalToCharge), "Winston's Humidor Order", True)
            'Christian's Test Credentials > Dim gate = New Gateway("2hBf5VN3S", "6Ls78h5w2dSMh56M")

            'Dim gate = New Gateway("5Rc58T5rs7", "8b6GR7w64T338fdb")
            Dim gate = New Gateway("2hBf5VN3S", "6Ls78h5w2dSMh56M")
            Dim response = gate.Send(Request)
            Dim ResponseCode As String = response.ResponseCode
            Dim ResponseMsg As String = response.Message
            If Not response.ResponseCode = "1" Then
                Select Case response.ResponseCode
                    Case "2"
                        Return "We're sorry, but your card could not be processed. Please try again, or contact support."
                    Case "3"
                        Return "A valid credit card is required."
                    Case "6"
                        Return "The card number entered is invalid. Please try again, or contact support."
                    Case "7"
                        Return "The card expiration date entered is invalid. Please try again, or contact support."
                    Case "8"
                        Return "The card entered has expired. Please try again, or contact support."
                    Case "17"
                        Return "We're sorry, but we cannot accept this type of card. Please try again, or contact support."
                    Case "19"
                        Return "We're sorry, but a network error has interupted this transaction. Please try again, or contact support."
                    Case Else
                        Return "We're sorry, but your card could not be processed. Please try again, or contact support."
                End Select

            Else
                UpdateInventory()
                CreateNewOrder(CDec(TotalToCharge), FirstName, LastName, Street, City, State, Zipcode, Email)
                UpdateDiscountMaxUsage()
                SendConfirmationEmails(FirstName, LastName, Street & " " & City & " " & State & " " & Zipcode, Email, _grandtotal)
                ClearShoppingCart()
            End If

            'Let's make the charge. If user selected save item details then we'll create the user profile, and save it.
            'If saveDetails = True Then

            '    Dim Target As CustomerGateway = New CustomerGateway("2hBf5VN3S", "6Ls78h5w2dSMh56M")
            '    Dim Custy = Target.CreateCustomer(Email, "Winston's Humidor Customer Profile")
            '    Dim CustyID As String = Custy.ProfileID
            '    Target.AddCreditCard(CustyID, CC, ccMonth, ccYear, ccSecurity)
            '    Dim Request = New AuthorizationRequest(CC, ccMonth & "" & ccYear, CDec(TotalToCharge), "Winston's Humidor Order")
            '    'Step 2 - Create the gateway, sending in your credentials
            '    Dim gate = New Gateway("2hBf5VN3S", "6Ls78h5w2dSMh56M")
            '    ' Step 3 - Send the request to the gateway
            '    Dim response = gate.Send(Request)
            '    'Use for codes to showing to customer, and storing transaction id's in db
            '    Dim ResponseCode As String = response.ResponseCode
            '    Dim ResponseMsg As String = response.Message
            '    ' Target.AuthorizeAndCapture()

            '    If Not response.ResponseCode = "1" Then
            '        'transaction failed
            '        Return "FAILED"
            '    Else
            '        'save users info
            '        CreateUser(CustyID, Email, Password, FirstName, LastName, Street, State, City, Zipcode)
            '        'update purchased product quantities
            '        UpdateInventory()
            '        'insert new order in the database 
            '        CreateNewOrder(CDec(TotalToCharge), FirstName, LastName, Street, City, State, Zipcode, Email)
            '    End If

            'Else
            '    'BASE CHARGE CODE WORKING BELOW
            'End If

            Return "Success"
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try


    End Function

    <WebMethod(True)> _
    Public Function SendConfirmationEmails(ByVal FirstName As String, ByVal LastName As String, ByVal Address As String, ByVal Email As String, ByVal GrandTotal As Decimal)


        'Get OrderSummary
        Dim CartHTML As String = String.Empty


        Dim InMyCart As New List(Of ShoppingCart)
        InMyCart = GoToCart()

        For i As Integer = 0 To InMyCart.Count - 1

            Dim ItemHTML As String = String.Empty
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

            ItemHTML = " Qty: " & Qty & " Item: " & DisplayName & " " & Notes & " Item Price: " & Price & "<br />"
            CartHTML = CartHTML & " " & ItemHTML

        Next

        'kevin@winstonshumidor.com
        'nymets2009

        Dim smtpserver As New SmtpClient()
        smtpserver.Credentials = New Net.NetworkCredential("christian@brewrocket.io", "WebD00D91") 'HASH FOR LATER
        smtpserver.Port = 587 '993 for gmail
        smtpserver.Host = "mail.oak.arvixe.com" 'aspmx.l.google.com
        smtpserver.EnableSsl = False

        Dim EmailBody As String = String.Empty
        Dim reader As StreamReader = New StreamReader(HttpContext.Current.Server.MapPath("~/email_OrderConfirmation.html"))

        'TO DO: Create HTML email for ForgotPassword.html 
        EmailBody = reader.ReadToEnd
        EmailBody = EmailBody.Replace("{oName}", FirstName & " " & LastName)
        EmailBody = EmailBody.Replace("{oDate}", Date.Today.ToString())
        EmailBody = EmailBody.Replace("{OrderList}", CartHTML)
        EmailBody = EmailBody.Replace("{OrderTotal}", "$" & GrandTotal)
        EmailBody = EmailBody.Replace("{ShippingAddress}", Address)
        Dim Emailr = New MailMessage()
        Try
            Emailr.From = New MailAddress("sales@WinstonsHumidor.com", "Winston's Humidor", System.Text.Encoding.UTF8)
            Emailr.To.Add(Email)
            Emailr.Bcc.Add("sales@WinstonsHumidor.com")
            Emailr.Subject = "Order Summary"
            Emailr.Body = EmailBody
            Emailr.IsBodyHtml = True
            smtpserver.Send(Emailr)

        Catch ex As Exception

        End Try

        Return ""
    End Function

    <WebMethod(True)> _
    Public Function ClearShoppingCart()

        Dim myShoppingCart As List(Of ShoppingCart) = Session("Cart")
        myShoppingCart.Clear()
        Session("Cart") = myShoppingCart

        Dim newshoppingCart As New List(Of ShoppingCart)
        newshoppingCart = Session("Cart")

        Return ""
    End Function


    Public Function CheckoutAsSavedUser(ByVal Email As String, ByVal Street As String, ByVal City As String, ByVal State As String, ByVal Zip As String, ByVal SubTtl As Decimal, ByVal Discount As Decimal, ByVal Total As Decimal)

        Dim WinstonsUser As List(Of WinstonsUser) = Session("WinstonsUser")

        Dim Target As CustomerGateway = New CustomerGateway("2hBf5VN3S", "6Ls78h5w2dSMh56M")
        'for testing use custy id of 36567042
        Dim Customer = Target.GetCustomer(WinstonsUser(0).AuthorizeNbr)
        Dim Payment = Customer.PaymentProfiles(0)

        Dim Request = New AuthorizationRequest(Payment.CardNumber, Payment.CardExpiration, CDec(Total), "Winston's Humidor Order")
        'Step 2 - Create the gateway, sending in your credentials
        Dim gate = New Gateway("2hBf5VN3S", "6Ls78h5w2dSMh56M")
        ' Step 3 - Send the request to the gateway
        Dim response = gate.Send(Request)
        'Use for codes to showing to customer, and storing transaction id's in db
        Dim ResponseCode As String = response.ResponseCode
        Dim ResponseMsg As String = response.Message

        If Not ResponseCode = "1" Then
            'transaction failed
            Return "FAILED"
        Else

            UpdateInventory()
            'insert new order in the database 
            CreateNewOrder(CDec(Total), WinstonsUser(0).FirstName, WinstonsUser(0).LastName, Street, City, State, Zip, Email)
        End If

        Return ""
    End Function

    <WebMethod(True)> _
    Public Function GetTAXPercentage(ByVal SubTotal As String, ByVal Discount As String)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT TaxRatePercentage FROM Configuration"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        Dim TaxRate As Decimal = (dt.Rows(0).Item("TaxRatePercentage") / 100)
        Dim GetTaxOn As Decimal = CDec(SubTotal) - CDec(Discount)
        Dim TaxableAmount As Decimal = Math.Round(GetTaxOn * TaxRate, 2)

        Return TaxableAmount
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


    <WebMethod(True)> _
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

    <WebMethod(True)> _
    Public Function UpdateDiscountMaxUsage()

        If Not IsNothing(Session("DiscountCode")) Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
            Using cmd As SqlCommand = con.CreateCommand
                cmd.Connection = con
                cmd.Connection.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "UPDATE Discounts SET MaxNbr = MaxNbr - 1 WHERE DiscountCode = '" & Session("DiscountCode") & "'"
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            End Using
        End If

        Return ""
    End Function

    <WebMethod(True)> _
    Public Function ApplyDiscount(ByVal DiscountCode As String)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim dt As New DataTable
        Using cmd As SqlCommand = con.CreateCommand
            cmd.Connection = con
            cmd.Connection.Open()
            cmd.CommandType = CommandType.Text
            ' UPDATE Discounts SET MAXNbr = MaxNbr - 1 WHERE DiscountID = 
            cmd.CommandText = "SELECT DiscountID,DiscountCode,DiscountAmount,DiscountCodeIsValid,DiscountStarts,DiscountEnds,MaxNbr FROM Discounts WHERE DiscountCode = '" & DiscountCode & "'"
            Using da As New SqlDataAdapter
                da.SelectCommand = cmd
                da.Fill(dt)
            End Using
            cmd.Connection.Close()
        End Using

        If dt.Rows().Count = 0 Then
            Return 0
        Else

            'Validate If Code is invalid or not
            If CBool(dt.Rows(0).Item("DiscountCodeIsValid")) = False Then
                Return 0
            End If

            'Check if discount has started yet
            Dim DiscountStartDate As Date = CDate(dt.Rows(0).Item("DiscountStarts"))
            Dim DiscountEndDate As Date = CDate(dt.Rows(0).Item("DiscountEnds"))

            If DiscountStartDate > Date.Today Then
                Session("DiscountCode") = Nothing
                Return 0
            ElseIf DiscountEndDate < Date.Today Then
                Session("DiscountCode") = Nothing
                Return 0
            ElseIf dt.Rows(0).Item("MaxNbr") <= 0 Then
                Session("DiscountCode") = Nothing
                Return 0
            Else
                Session("DiscountCode") = dt.Rows(0).Item("DiscountCode")
                Return Math.Round(dt.Rows(0).Item("DiscountAmount"), 2)
            End If


        End If

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
                dt = FillFeaturedProductDetailsDT("SELECT Name,Price FROM Accessories WHERE ProductID =" & ProductID & " AND IsFeatured = 1")
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
                dt = FillDataTable("SELECT * FROM Accessories WHERE IsFeatured = 1 AND ShowInStore = 1")
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
                        A.IsOnSale = item("IsOnSale")
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        A.SalePrice = CStr(DecSalePrice)
                        A.IsFeatured = item("IsFeatured")

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        A.IsOnSale = IsOnSale

                        AccessoryList.Add(A)
                    Next
                    Return AccessoryList
                Else
                    Return "0"
                End If

            Case "Apparel"
                dt = FillDataTable("SELECT * FROM Apparel WHERE IsFeatured = 1 AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        A.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        A.SalePrice = CStr(DecSalePrice)
                        ApparelList.Add(A)
                    Next
                    Return ApparelList
                Else
                    Return 0
                End If

            Case "Cigars"
                dt = FillDataTable("SELECT * FROM Cigars WHERE IsFeatured = 1 AND ShowInStore = 1")

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

                        Dim SingleIsOnSale As Boolean = True
                        If CBool(item("SingleIsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SingleSaleStartDate As Date = CDate(item("SingleSaleStartDate"))
                            Dim SingleSaleEndDate As Date = CDate(item("SingleSaleEndDate"))
                            If SingleSaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                SingleIsOnSale = False
                            End If
                            If SingleSaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                SingleIsOnSale = False
                            End If
                        Else
                            SingleIsOnSale = False
                        End If
                        C.SingleIsOnSale = SingleIsOnSale

                        Dim BoxIsOnSale As Boolean = True
                        If CBool(item("BoxIsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim BoxSaleStartDate As Date = CDate(item("BoxSaleStartDate"))
                            Dim BoxSaleEndDate As Date = CDate(item("BoxSaleEndDate"))
                            If BoxSaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                BoxIsOnSale = False
                            End If
                            If BoxSaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                BoxIsOnSale = False
                            End If
                        Else
                            BoxIsOnSale = False
                        End If
                        C.BoxIsOnSale = BoxIsOnSale

                        Dim DecSingleSalePrice As Decimal = Decimal.Round(item("SingleSalePrice"), 2)
                        Dim DecBoxSalePrice As Decimal = Decimal.Round(item("BoxSalePrice"), 2)
                        C.SingleSalePrice = CStr(DecSingleSalePrice)
                        C.BoxSalePrice = CStr(DecBoxSalePrice)
                        C.MaxBoxPurchase = item("MaxBoxPurchaseAmount")
                        C.MaxSinglePurchase = item("MaxSinglePurchaseAmount")
                        CigarList.Add(C)
                    Next
                    Return CigarList
                Else
                    Return 0
                End If
            Case "Coffee"
                dt = FillDataTable("SELECT * FROM Coffee WHERE IsFeatured = 1 AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        C.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        C.SalePrice = CStr(DecSalePrice)
                        CoffeeList.Add(C)
                    Next
                    Return CoffeeList
                Else
                    Return 0
                End If

            Case "Pipes"
                dt = FillDataTable("SELECT * FROM Pipes WHERE IsFeatured = 1 AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        P.IsOnSale = IsOnSale

                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        P.SalePrice = CStr(DecSalePrice)
                        PipeList.Add(P)
                    Next
                    Return PipeList
                Else
                    Return 0
                End If
            Case "PipeTobacco"
                dt = FillDataTable("SELECT * FROM PipeTobacco WHERE IsFeatured = 1 AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        PT.IsOnSale = IsOnSale


                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        PT.SalePrice = CStr(DecSalePrice)
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
                    dt = FillDataTable("SELECT * FROM Accessories WHERE ShowInStore = 1")
                Else
                    dt = FillDataTable("SELECT * FROM Accessories WHERE Brand LIKE '%" & SearchText & "%' OR Name LIKE '%" & SearchText & "%' AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        A.IsOnSale = IsOnSale


                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        A.SalePrice = CStr(DecSalePrice)
                        AccessoryList.Add(A)
                    Next
                    Return AccessoryList
                Else
                    Return "0"
                End If

            Case "Apparel"
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Apparel WHERE ShowInStore = 1")
                Else
                    dt = FillDataTable("SELECT * FROM Apparel WHERE Name LIKE '%" & SearchText & "%' AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        A.IsOnSale = IsOnSale

                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        A.SalePrice = CStr(DecSalePrice)
                        ApparelList.Add(A)
                    Next
                    Return ApparelList
                Else
                    Return 0
                End If

            Case "Cigars"
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Cigars WHERE ShowInStore = 1")
                Else
                    dt = FillDataTable("SELECT * FROM Cigars WHERE Brand LIKE '%" & SearchText & "%' OR Name LIKE '%" & SearchText & "%' OR Body LIKE '%" & SearchText & "%' AND ShowInStore = 1")
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

                        Dim SingleIsOnSale As Boolean = True
                        If CBool(item("SingleIsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SingleSaleStartDate As Date = CDate(item("SingleSaleStartDate"))
                            Dim SingleSaleEndDate As Date = CDate(item("SingleSaleEndDate"))
                            If SingleSaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                SingleIsOnSale = False
                            End If
                            If SingleSaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                SingleIsOnSale = False
                            End If
                        Else
                            SingleIsOnSale = False
                        End If
                        C.SingleIsOnSale = SingleIsOnSale

                        Dim BoxIsOnSale As Boolean = True
                        If CBool(item("BoxIsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim BoxSaleStartDate As Date = CDate(item("BoxSaleStartDate"))
                            Dim BoxSaleEndDate As Date = CDate(item("BoxSaleEndDate"))
                            If BoxSaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                BoxIsOnSale = False
                            End If
                            If BoxSaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                BoxIsOnSale = False
                            End If
                        Else
                            BoxIsOnSale = False
                        End If
                        C.BoxIsOnSale = BoxIsOnSale

                        Dim DecSingleSalePrice As Decimal = Decimal.Round(item("SingleSalePrice"), 2)
                        Dim DecBoxSalePrice As Decimal = Decimal.Round(item("BoxSalePrice"), 2)
                        C.SingleSalePrice = CStr(DecSingleSalePrice)
                        C.BoxSalePrice = CStr(DecBoxSalePrice)
                        C.MaxBoxPurchase = item("MaxBoxPurchaseAmount")
                        C.MaxSinglePurchase = item("MaxSinglePurchaseAmount")
                        CigarList.Add(C)
                    Next
                    Return CigarList
                Else
                    Return 0
                End If
            Case "Coffee"
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Coffee WHERE ShowInStore = 1")
                Else
                    dt = FillDataTable("SELECT * FROM Coffee WHERE Brand LIKE '%" & SearchText & "%' OR Name LIKE '%" & SearchText & "%' OR Roast LIKE '%" & SearchText & "%' OR Body LIKE '%" & SearchText & "%' AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        C.IsOnSale = IsOnSale


                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        C.SalePrice = CStr(DecSalePrice)
                        CoffeeList.Add(C)
                    Next
                    Return CoffeeList
                Else
                    Return 0
                End If

            Case "Pipes"
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM Pipes WHERE ShowInStore = 1")
                Else
                    dt = FillDataTable("SELECT * FROM Pipes WHERE Brand LIKE '%" & SearchText & "%' OR Name LIKE '%" & SearchText & "%' OR StemShape LIKE '%" & SearchText & "%' OR BowlFinish LIKE '%" & SearchText & "%'  OR BodyShape LIKE '%" & SearchText & "%'  OR Material LIKE '%" & SearchText & "%' AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        P.IsOnSale = IsOnSale


                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        P.SalePrice = CStr(DecSalePrice)
                        PipeList.Add(P)
                    Next
                    Return PipeList
                Else
                    Return 0
                End If
            Case "PipeTobacco"
                If Trim(SearchText) = String.Empty Then
                    dt = FillDataTable("SELECT * FROM PipeTobacco WHERE ShowInStore = 1")
                Else
                    dt = FillDataTable("SELECT * FROM PipeTobacco WHERE Brand LIKE '%" & SearchText & "%' OR Tobacco LIKE '%" & SearchText & "%' OR Style LIKE '%" & SearchText & "%' OR Cut LIKE '%" & SearchText & "%' OR Strength LIKE '%" & SearchText & "%' AND ShowInStore = 1")
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

                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        PT.IsOnSale = IsOnSale


                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        PT.SalePrice = CStr(DecSalePrice)
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
                dt = FillDataTable("SELECT * FROM Accessories WHERE ShowInStore = 1")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        A.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        A.SalePrice = CStr(DecSalePrice)
                        AccessoryList.Add(A)
                    Next
                    Return AccessoryList
                Else
                    Return "0"
                End If

            Case "Apparel"
                dt = FillDataTable("SELECT * FROM Apparel WHERE ShowInStore = 1")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        A.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        A.SalePrice = CStr(DecSalePrice)
                        ApparelList.Add(A)
                    Next
                    Return ApparelList
                Else
                    Return 0
                End If

            Case "Cigars"
                dt = FillDataTable("SELECT * FROM Cigars WHERE ShowInStore = 1")

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

                        Dim SingleIsOnSale As Boolean = True
                        If CBool(item("SingleIsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SingleSaleStartDate As Date = CDate(item("SingleSaleStartDate"))
                            Dim SingleSaleEndDate As Date = CDate(item("SingleSaleEndDate"))
                            If SingleSaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                SingleIsOnSale = False
                            End If
                            If SingleSaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                SingleIsOnSale = False
                            End If
                        Else
                            SingleIsOnSale = False
                        End If
                        C.SingleIsOnSale = SingleIsOnSale

                        Dim BoxIsOnSale As Boolean = True
                        If CBool(item("BoxIsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim BoxSaleStartDate As Date = CDate(item("BoxSaleStartDate"))
                            Dim BoxSaleEndDate As Date = CDate(item("BoxSaleEndDate"))
                            If BoxSaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                BoxIsOnSale = False
                            End If
                            If BoxSaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                BoxIsOnSale = False
                            End If
                        Else
                            BoxIsOnSale = False
                        End If
                        C.BoxIsOnSale = BoxIsOnSale

                        Dim DecSingleSalePrice As Decimal = Decimal.Round(item("SingleSalePrice"), 2)
                        Dim DecBoxSalePrice As Decimal = Decimal.Round(item("BoxSalePrice"), 2)
                        C.SingleSalePrice = CStr(DecSingleSalePrice)
                        C.BoxSalePrice = CStr(DecBoxSalePrice)
                        C.MaxBoxPurchase = item("MaxBoxPurchaseAmount")
                        C.MaxSinglePurchase = item("MaxSinglePurchaseAmount")
                        CigarList.Add(C)
                    Next
                    Return CigarList
                Else
                    Return 0
                End If
            Case "Coffee"
                dt = FillDataTable("SELECT * FROM Coffee WHERE ShowInStore = 1")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        C.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        C.SalePrice = CStr(DecSalePrice)
                        CoffeeList.Add(C)
                    Next
                    Return CoffeeList
                Else
                    Return 0
                End If

            Case "Pipes"
                dt = FillDataTable("SELECT * FROM Pipes WHERE ShowInStore = 1")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        P.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        P.SalePrice = CStr(DecSalePrice)
                        PipeList.Add(P)
                    Next
                    Return PipeList
                Else
                    Return 0
                End If
            Case "PipeTobacco"
                dt = FillDataTable("SELECT * FROM PipeTobacco WHERE ShowInStore = 1")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        PT.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        PT.SalePrice = CStr(DecSalePrice)

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
                dt = FillDataTable("SELECT * FROM Accessories WHERE ProductID = " & ProductID & " AND ShowInStore = 1 ")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        A.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        A.SalePrice = CStr(DecSalePrice)
                        AccessoryList.Add(A)
                    Next
                    Return AccessoryList
                Else
                    Return "0"
                End If

            Case "Apparel"
                dt = FillDataTable("SELECT * FROM Apparel  WHERE ProductID = " & ProductID & " AND ShowInStore = 1 ")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        A.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        A.SalePrice = CStr(DecSalePrice)
                        ApparelList.Add(A)
                    Next
                    Return ApparelList
                Else
                    Return 0
                End If

            Case "Cigars"
                dt = FillDataTable("SELECT * FROM Cigars  WHERE ProductID = " & ProductID & " AND ShowInStore = 1 ")

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
                        
                        Dim SingleIsOnSale As Boolean = True
                        If CBool(item("SingleIsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SingleSaleStartDate As Date = CDate(item("SingleSaleStartDate"))
                            Dim SingleSaleEndDate As Date = CDate(item("SingleSaleEndDate"))
                            If SingleSaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                SingleIsOnSale = False
                            End If
                            If SingleSaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                SingleIsOnSale = False
                            End If
                        Else
                            SingleIsOnSale = False
                        End If
                        C.SingleIsOnSale = SingleIsOnSale

                        Dim BoxIsOnSale As Boolean = True
                        If CBool(item("BoxIsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim BoxSaleStartDate As Date = CDate(item("BoxSaleStartDate"))
                            Dim BoxSaleEndDate As Date = CDate(item("BoxSaleEndDate"))
                            If BoxSaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                BoxIsOnSale = False
                            End If
                            If BoxSaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                BoxIsOnSale = False
                            End If
                        Else
                            BoxIsOnSale = False
                        End If
                        C.BoxIsOnSale = BoxIsOnSale

                        Dim DecSingleSalePrice As Decimal = Decimal.Round(item("SingleSalePrice"), 2)
                        Dim DecBoxSalePrice As Decimal = Decimal.Round(item("BoxSalePrice"), 2)
                        C.SingleSalePrice = CStr(DecSingleSalePrice)
                        C.BoxSalePrice = CStr(DecBoxSalePrice)
                        C.MaxBoxPurchase = item("MaxBoxPurchaseAmount")
                        C.MaxSinglePurchase = item("MaxSinglePurchaseAmount")
                        CigarList.Add(C)
                    Next
                    Return CigarList
                Else
                    Return 0
                End If
            Case "Coffee"
                dt = FillDataTable("SELECT * FROM Coffee  WHERE ProductID = " & ProductID & " AND ShowInStore = 1 ")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        C.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        C.SalePrice = CStr(DecSalePrice)
                        CoffeeList.Add(C)
                    Next
                    Return CoffeeList
                Else
                    Return 0
                End If

            Case "Pipes"
                dt = FillDataTable("SELECT * FROM Pipes  WHERE ProductID = " & ProductID & " AND ShowInStore = 1 ")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        P.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        P.SalePrice = CStr(DecSalePrice)
                        PipeList.Add(P)
                    Next
                    Return PipeList
                Else
                    Return 0
                End If
            Case "PipeTobacco"
                dt = FillDataTable("SELECT * FROM PipeTobacco  WHERE ProductID = " & ProductID & " AND ShowInStore = 1 ")
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
                        Dim IsOnSale As Boolean = True
                        If CBool(item("IsOnSale")) Then
                            'is it though? Maybe the date has expired. Let's check before we set the IsOnSale variable
                            Dim SaleStartDate As Date = CDate(item("SaleStartDate"))
                            Dim SaleEndDate As Date = CDate(item("SaleEndDate"))
                            If SaleStartDate.Date > Date.Today Then
                                'the sale hasn't started yet
                                IsOnSale = False
                            End If
                            If SaleEndDate.Date < Date.Today Then
                                'The sale has already ended.
                                IsOnSale = False
                            End If
                        Else
                            IsOnSale = False
                        End If

                        PT.IsOnSale = IsOnSale
                        Dim DecSalePrice As Decimal = Decimal.Round(item("SalePrice"), 2)
                        PT.SalePrice = CStr(DecSalePrice)
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