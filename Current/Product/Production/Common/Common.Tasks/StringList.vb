Option Explicit On
Option Strict On

Imports NAnt.Core
Imports NAnt.Core.Attributes

<ElementName("strings")> _
Public Class StringList
    Inherits LoopItems

    Private _Strings As StringItem()

    <BuildElementArray("string", ElementType:=GetType(StringItem), Required:=True)> _
    Public Property Strings() As StringItem()
        Get
            Return _Strings
        End Get
        Set(ByVal value As StringItem())
            _Strings = value
        End Set
    End Property

    Protected Overrides Function GetStrings() As System.Collections.IEnumerator
        Dim Strings As New ArrayList
        For Each StringContainer As StringItem In Me.Strings
            Strings.Add(StringContainer.StringValue)
        Next
        Return DirectCast(Strings.ToArray(GetType(String)), String()).GetEnumerator
    End Function

End Class
