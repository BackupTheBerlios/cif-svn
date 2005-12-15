Option Explicit On 
Option Strict On

Imports NAnt.Core
Imports NAnt.Core.Attributes

<ElementName("text")> _
Public Class TextElement
    Inherits Element

    Private _Value As String

    Public ReadOnly Property [Value]() As String
        Get
            If Me._Value Is Nothing Then
                Me._Value = Me.XmlNode.InnerText
            End If
            Return Me._Value
        End Get
    End Property

End Class
