Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient

Public Class ImageHandler
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim PostID As String = context.Request.QueryString("id")

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("connex").ConnectionString)
        Dim cmd As New SqlCommand
        cmd.Connection = con
        cmd.Connection.Open()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT PostImage FROM NewsPosts WHERE PostID =" & PostID
        ' cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = 

        cmd.Prepare()
        Dim dr As SqlDataReader = cmd.ExecuteReader()
        dr.Read()
        context.Response.ContentType = "image/png;base64"
        context.Response.BinaryWrite(DirectCast(dr("PostImage"), Byte()))
        dr.Close()
        cmd.Connection.Close()


    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class