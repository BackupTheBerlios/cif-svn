Option Explicit On
Option Strict On

Imports NAnt.Core
Imports NAnt.Core.Attributes
Imports System
Imports System.Collections

<ElementName("strings")> _
Public Class StringList
    Inherits LoopItems

    Private _StringItems As StringItemTable

    Public Property StringItems() As StringItemTable
        Get
            If _StringItems Is Nothing Then
                _StringItems = New StringItemTable()
            End If
            Return _StringItems
        End Get
        Set(ByVal value As StringItemTable)
            _StringItems = value
        End Set
    End Property

    <BuildElementArray("string", ElementType:=GetType(StringItem))> _
    Public WriteOnly Property Strings() As StringItem()
        Set(ByVal value As StringItem())
            For Each Item As StringItem In value
                Me.StringItems.Add(Item.StringValue, Item)
            Next
        End Set
    End Property



    Protected Overrides Function GetStrings() As System.Collections.IEnumerator
        Return Me.StringItems.Values.GetEnumerator
    End Function

End Class

Public Class StringItemTable
    Inherits DictionaryBase

    Private _changed As Boolean
    Private _values As ArrayList

    Public ReadOnly Property Values() As String()
        Get
            Return DirectCast(Me.InnerValues.ToArray(GetType(String)), String())
        End Get
    End Property

    Private ReadOnly Property InnerValues() As ArrayList
        Get
            If Me._values Is Nothing OrElse Me.Changed Then
                Me._values = New ArrayList(Me.InnerHashtable.Keys)
            End If
            Return _values
        End Get
    End Property

    Private Property Changed() As Boolean
        Get
            Return _changed
        End Get
        Set(ByVal Value As Boolean)
            _changed = Value
        End Set
    End Property

    Public Sub Sort()
        Me.InnerValues.Sort()
    End Sub

    Public Sub ReverseSort()
        Me.InnerValues.Sort()
        Me.InnerValues.Reverse()
    End Sub

    Public Sub Add(ByVal key As String, ByVal value As StringItem)
        Me.InnerHashtable.Add(key, value)
    End Sub

    Public Sub Remove(ByVal key As String)
        Me.InnerHashtable.Remove(key)
    End Sub

    Public Function Contains(ByVal key As String) As Boolean
        Return Me.InnerHashtable.Contains(key)
    End Function

    Protected Overrides Sub OnClear()
        MyBase.OnClear()
        Me.Changed = True
    End Sub

    Protected Overrides Sub OnClearComplete()
        MyBase.OnClearComplete()
        Me.Changed = True
    End Sub

    Protected Overrides Sub OnInsert(ByVal key As Object, ByVal value As Object)
        MyBase.OnInsert(key, value)
        Me.Changed = True
    End Sub

    Protected Overrides Sub OnInsertComplete(ByVal key As Object, ByVal value As Object)
        MyBase.OnInsertComplete(key, value)
        Me.Changed = True
    End Sub

    Protected Overrides Sub OnRemove(ByVal key As Object, ByVal value As Object)
        MyBase.OnRemove(key, value)
        Me.Changed = True
    End Sub

    Protected Overrides Sub OnRemoveComplete(ByVal key As Object, ByVal value As Object)
        MyBase.OnRemoveComplete(key, value)
        Me.Changed = True
    End Sub

    Protected Overrides Sub OnSet(ByVal key As Object, ByVal oldValue As Object, ByVal newValue As Object)
        MyBase.OnSet(key, oldValue, newValue)
        Me.Changed = True
    End Sub

    Protected Overrides Sub OnSetComplete(ByVal key As Object, ByVal oldValue As Object, ByVal newValue As Object)
        MyBase.OnSetComplete(key, oldValue, newValue)
        Me.Changed = True
    End Sub

End Class

