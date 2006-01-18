Option Explicit On
Option Strict On

Imports NAnt.Core
Imports NAnt.Core.Attributes

<ElementName("regexmatch")> _
Public Class RegexMatch
    Inherits LoopItems


    Protected Overrides Function GetStrings() As System.Collections.IEnumerator
        Dim Strings As New ArrayList

        Return DirectCast(Strings.ToArray(GetType(String)), String()).GetEnumerator
    End Function
End Class
