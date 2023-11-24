Imports System.ComponentModel.Design
Imports System.Net
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Http.Extensions
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Toolkit.Uwp.Notifications

Namespace My

  Partial Friend Class MyApplication

    Public Shared Sub Main(args As String())
      If args.Count = 0 OrElse Integer.TryParse(args(0), modProgram.WebPort) Then modProgram.WebPort = 55123

      ShowTrayIcon()

      Dim tsk = New Threading.Tasks.Task(AddressOf StartWebServer)
      tsk.Start()

      System.Windows.Forms.Application.Run()
    End Sub

  End Class
End Namespace

Module modProgram
  Public WebPort As Integer
  Private WApp As WebApplication

  Friend Sub ShowTrayIcon()
    Dim ni = New NotifyIcon()
    ni.Icon = My.Resources.toaster
    ni.Text = "WinWebhookToast http://*:" & WebPort
    ni.ContextMenuStrip = New ContextMenuStrip()
    Dim itm = New ToolStripMenuItem("Exit WinWebhookToast")
    AddHandler itm.Click,
      Async Sub()
        Await WApp.StopAsync
        System.Windows.Forms.Application.Exit()
      End Sub
    ni.ContextMenuStrip.Items.Add(itm)
    ni.Visible = True
  End Sub

  Friend Sub StartWebServer()
    Dim builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder()
    builder.WebHost.UseUrls("http://0.0.0.0:" & WebPort)
    WApp = builder.Build()
    WApp.MapGet("",
      Async Function(ctx As HttpContext) As Task
        Dim title = ctx.Request.Query("title")
        Dim body = ctx.Request.Query("body")
        Dim icon = ctx.Request.Query("icon")
        ShowNotify(title, body, icon)
        ctx.Response.ContentType = "text/plain; charset=utf-8"
        ctx.Response.Headers.CacheControl = "no-cache"
        ctx.Response.Headers.Server = My.Application.Info.Title
        Await ctx.Response.WriteAsync("OK")
      End Function)
    WApp.Run()
  End Sub

  Public Sub ShowNotify(title As String, body As String, Optional icon As String = Nothing)
    Dim p = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
    With New ToastContentBuilder()
      '.AddArgument("action", "viewConversation")
      '.AddArgument("conversationId", 9813)
      .AddText(title)
      .AddText(body)
      If icon IsNot Nothing Then .AddAppLogoOverride(New Uri("file:///" & p & "\icons\" & icon & ".png"))
      .Show()
    End With

  End Sub

End Module


