Option Explicit On
Option Strict On

Imports NAnt.Core
Imports NAnt.Core.Attributes

<ElementName("string")> _
Public Class StringItem
    Inherits Element

    Private _StringValue As String

    <TaskAttribute("value", Required:=True)> _
    Public Property StringValue() As String
        Get
            Return _StringValue
        End Get
        Set(ByVal value As String)
            _StringValue = value
        End Set
    End Property

End Class
