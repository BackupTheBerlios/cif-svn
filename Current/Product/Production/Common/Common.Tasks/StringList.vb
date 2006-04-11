Option Explicit On
Option Strict On

Imports NAnt.Core
Imports NAnt.Core.Attributes

<ElementName("strings")> _
Public Class StringList
    Inherits LoopItems

    Private _StringItems As Hashtable

    Public Property StringItems() As Hashtable
        Get
            If _StringItems Is Nothing Then
                _StringItems = New Hashtable()
            End If
            Return _StringItems
        End Get
        Set(ByVal value As Hashtable)
            _StringItems = value
        End Set
    End Property

    <BuildElementArray("string", ElementType:=GetType(StringItem), Required:=True)> _
    Public WriteOnly Property Strings() As StringItem()
        Set(ByVal value As StringItem())
            For Each Item As StringItem In value
                Me.StringItems.Add(Item.StringValue, Item)
            Next
        End Set
    End Property

    Protected Overrides Function GetStrings() As System.Collections.IEnumerator
        Dim Strings As New ArrayList
        For Each StringContainer As StringItem In Me.StringItems.Values
            Strings.Add(StringContainer.StringValue)
        Next
        Return DirectCast(Strings.ToArray(GetType(String)), String()).GetEnumerator
    End Function

End Class

