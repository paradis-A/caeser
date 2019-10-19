Imports System.String
Public Class CaeserCipher


    Private _shiftCount As Integer


    Public Property ShiftCount() As Integer
        Get
            Return _shiftCount
        End Get
        Set(ByVal value As Integer)
            _shiftCount = value
        End Set
    End Property


    Public Sub New()
        Me.New(3)
    End Sub


    Public Sub New(ByVal shiftCount As Integer)
        Me.ShiftCount = shiftCount
    End Sub

    Private Shared Function Cipher(ch As Char, key As Integer) As Char
        If Not Char.IsLetter(ch) Then
            Return ch
        End If

        Dim offset As Integer = Convert.ToInt32(If(Char.IsUpper(ch), "A"c, "a"c))
        Return ChrW((((Convert.ToInt32(ch) + key) - offset) Mod 26) + offset)
    End Function

    Public Function Encipher(input As String) As String
        Dim output As String = String.Empty
        For Each ch As Char In input
            output += Cipher(ch, Me.ShiftCount)
        Next

        Return output
    End Function

    Public Function Decipher(input As String) As String
        Dim output As String = String.Empty
        For Each ch As Char In input
            output += Cipher(ch, 26 - Me.ShiftCount)
        Next
        Return output
    End Function


End Class
