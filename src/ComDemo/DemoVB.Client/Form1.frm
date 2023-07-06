VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3015
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   4560
   LinkTopic       =   "Form1"
   ScaleHeight     =   3015
   ScaleWidth      =   4560
   StartUpPosition =   3  '窗口缺省
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
Dim obj As Object
'方式一：引用
'Set obj = New DemoWin_Plugin.Server
'obj.startwin

'方式二：反射
Set obj = CreateObject("DemoWin.Plugin.Server")
obj.startwin

Set obj = Nothing

End Sub

Private Sub Command2_Click()
Dim obj As Object

Set obj = CreateObject("DemoWin.Plugin.Server")
obj.StartWpfCore

Set obj = Nothing
End Sub
