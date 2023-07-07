VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   2160
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   4410
   LinkTopic       =   "Form1"
   ScaleHeight     =   2160
   ScaleWidth      =   4410
   StartUpPosition =   2  '屏幕中心
   Begin VB.CommandButton Command2 
      Caption         =   "启动DemoCore.Plugin"
      Height          =   495
      Left            =   360
      TabIndex        =   1
      Top             =   1200
      Width           =   3375
   End
   Begin VB.CommandButton Command1 
      Caption         =   "启动DemoWin.Plugin"
      Height          =   495
      Left            =   360
      TabIndex        =   0
      Top             =   360
      Width           =   3375
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()
On Error GoTo ErrHandler:
Dim obj As Object
Hide
'方式一：引用
'Set obj = New DemoWin_Plugin.Server
'obj.startwin

'方式二：反射
Set obj = CreateObject("DemoWin.Plugin.Server")
obj.startwin
    Set obj = Nothing
    Unload Me
finally:

    End
ErrHandler:
GoTo finally
End Sub

Private Sub Command2_Click()
On Error GoTo ErrHandler:
Dim obj As Object

Hide

Set obj = CreateObject("DemoCore.Plugin.Server")
obj.StartWpf
 Set obj = Nothing
    Unload Me
finally:
   
    End
ErrHandler:
GoTo finally
End Sub

