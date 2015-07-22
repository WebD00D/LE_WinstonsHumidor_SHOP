Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient

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
End Class