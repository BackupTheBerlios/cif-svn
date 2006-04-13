Option Explicit On
Option Strict On

Imports System.Xml
Imports System.Reflection

Imports NAnt.Core
Imports NAnt.Core.Types
Imports NAnt.Core.Tasks
Imports NAnt.Core.Attributes

<TaskName("switch")> _
Public Class SwitchTask
    Inherits Task

    Private _Cases As CaseContainerCollection
    Private _LeftValue As String

    <TaskAttribute("value", REquired:=True)> _
    Public Property LeftValue() As String
        Get
            Return _LeftValue
        End Get
        Set(ByVal value As String)
            _LeftValue = value
        End Set
    End Property

    <BuildElementArray("case", Required:=False)> _
        Public Property Cases() As CaseContainerCollection
        Get
            If _Cases Is Nothing Then
                _Cases = New CaseContainerCollection()
            End If
            Return _Cases
        End Get
        Set(ByVal value As CaseContainerCollection)
            _Cases = value
        End Set
    End Property

    Protected Overrides Sub ExecuteTask()
        For Each [Case] As CaseElement In Me.Cases
            If Me.LeftValue = [Case].RightValue Then
                [Case].Execute()
                If [Case].Break Then
                    Exit For
                End If
            End If
        Next
    End Sub

End Class


