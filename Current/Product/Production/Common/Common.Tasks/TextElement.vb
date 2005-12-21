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
                Me._Value = Me.XmlNode.InnerXml
            End If
            Return Me._Value
        End Get
    End Property

    Protected Overrides ReadOnly Property CustomXmlProcessing() As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides Sub InitializeXml(ByVal elementNode As System.Xml.XmlNode, ByVal properties As NAnt.Core.PropertyDictionary, ByVal framework As NAnt.Core.FrameworkInfo)
        MyBase.InitializeXml(elementNode, properties, framework)
    End Sub

End Class
