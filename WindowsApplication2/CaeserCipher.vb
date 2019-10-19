Imports System.String
Imports System.Windows.Forms
Public Class CaeserCipher


    Private _shiftCount As Integer
    Private DecryptIndexes As List(Of Int32)
    Private EncryptIndexes As List(Of Int32)

    Public Property ShiftCount() As Integer
        Get
            Return _shiftCount
        End Get
        Set(ByVal value As Integer)
            _shiftCount = value
        End Set
    End Property

    Public Function Decompress(ByVal cp As List(Of String), ByVal original As List(Of String)) As String

        For Each i In cp
            Dim o = original.Where(Function(x) x = i).ToList.ForEach(Sub(s As String) s = i)

        Next



    End Function

    Public Function Compress(ByVal rc As List(Of String), ByVal encrypt As Boolean) As List(Of String)
        Dim d = New List(Of String)
        d.AddRange(rc.GroupBy(Function(x) x).Where(Function(x) x.Count >= 0).Select(Function(x) x.Key).ToArray)
        Dim indexes As List(Of Integer)
        Dim r = rc
        If encrypt Then
            EncryptIndexes = New List(Of Integer)
            indexes = EncryptIndexes
        Else
            DecryptIndexes = New List(Of Integer)
            indexes = DecryptIndexes
        End If
        Console.WriteLine("Original Count = " & rc.Count.ToString & " || Compressed Count = " & d.Count & " || No. of indexes = " & indexes.Count)
        Return d
    End Function


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

    Public Function Decipher(input As List(Of String)) As String
        Dim compressed = Join("", Compress(input, False).ToArray)
        Dim output As String = String.Empty

        For Each ch As Char In compressed
            output += Cipher(ch, 26 - Me.ShiftCount)
        Next
        Return output

    End Function


End Class
