Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient

Public Class ImageHandler
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim ProductID As String = context.Request.QueryString("id")
        Dim Category As String = context.Request.QueryString("Category")

        Dim CommandText As String = String.Empty

        Select Case Category
            Case "Accessory"
                CommandText = "SELECT Image FROM Accessories WHERE ProductID =" & ProductID
            Case "Coffee"
                CommandText = "SELECT Image FROM Coffee WHERE ProductID =" & ProductID
            Case "Apparel"
                CommandText = "SELECT Image FROM Apparel WHERE ProductID =" & ProductID
            Case "Pipes"
                CommandText = "SELECT Image FROM Pipes WHERE ProductID=" & ProductID
            Case "Cigars"
                CommandText = "SELECT Image FROM Cigars WHERE ProductID=" & ProductID
            Case "Pipe Tobacco"
                CommandText = "SELECT Image FROM PipeTobacco WHERE ProductID=" & ProductID
            Case "NewsPost"
                CommandText = "SELECT PostImage FROM NewsPosts WHERE PostID =" & ProductID
            Case Else
                CommandText = ""
                Exit Sub
        End Select

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.Connection.Open()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = CommandText

        cmd.Prepare()
        Dim dr As SqlDataReader = cmd.ExecuteReader()
        dr.Read()
        context.Response.ContentType = "image/png;base64"
        If Category = "NewsPost" Then
            context.Response.BinaryWrite(DirectCast(dr("PostImage"), Byte()))
        Else
            context.Response.BinaryWrite(DirectCast(dr("Image"), Byte()))
        End If

        dr.Close()
        cmd.Connection.Close()


    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class