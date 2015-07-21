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
        Public Property ItemList() As List(Of ShoppingCart)
            Get
                Return m_ObjectList
            End Get
            Set(value As List(Of ShoppingCart))
                m_ObjectList = value
            End Set
        End Property

        Private m_ObjectList As List(Of ShoppingCart)
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
    Public Function GoToCart() As String

        Dim itemlist As List(Of ShoppingCart) = Session("Cart")




        Return "Hello World"
    End Function


End Class