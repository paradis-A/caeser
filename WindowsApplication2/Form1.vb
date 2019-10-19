Imports WindowsApplication2.CaeserCipher

Public Class Form1
    'BINDINGS

    'ENCRYPT
    Private Sub Encrypt() Handles C1Button1.Click
        Dim cc = If(IsNumeric(keyentry.Text), New CaeserCipher(keyentry.Text), Nothing)
        RichTextBox1.Clear()
        RichTextBox1.Text = If(IsNothing(cc), Nothing, cc.Encipher(RichTextBox2.Text))
    End Sub

    'DECRYPT
    Private Sub Decrypt() Handles C1Button2.Click
        'Dim cc = If(IsNumeric(keyentry.Text), New CaeserCipher(keyentry.Text), Nothing)
        'RichTextBox2.Clear()
        'RichTextBox2.Text = If(IsNothing(cc), Nothing, cc.Decipher(RichTextBox1.Text))

    End Sub

    'CRACK
    Private Sub Crack() Handles C1Button3.Click
        ListBox1.Items.Clear()
        For i = 0 To 25
            Dim cc = New CaeserCipher(i)
            ListBox1.Items.Add(i.ToString & " - " & cc.Decipher(RichTextBox1.Text))
        Next
    End Sub

    'INITITIALIZE
    Sub Init() Handles Me.Load
        Dim a = New List(Of String)

        OpenFileDialog1.Filter = "Text Files|*.txt"
        OpenFileDialog2.Filter = "Text Files|*.txt"
        OpenFileDialog3.Filter = "Text Files|*.txt"
        SaveFileDialog1.Filter = "Text Files|*.txt"
    End Sub

    'BROWSE DECRYPTED
    Private Sub C1Button5_Click(sender As Object, e As EventArgs) Handles C1Button5.Click
        OpenFileDialog1.ShowDialog()
        If (OpenFileDialog1.FileName <> "") Then
            ProcessTxtFile(OpenFileDialog1, RichTextBox2)
        End If
    End Sub

    'DISPLAY TEXTFILE
    Private Sub ProcessTxtFile(ByVal ofd As OpenFileDialog, ByVal tb As RichTextBox)
        Dim fileReader As String
        fileReader = My.Computer.FileSystem.ReadAllText(ofd.FileName)
        tb.Text = fileReader
    End Sub

    'CLEAR
    Private Sub C1Button6_Click(sender As Object, e As EventArgs) Handles C1Button6.Click
        RichTextBox2.Clear()
    End Sub

    'CLEAR
    Private Sub C1Button8_Click(sender As Object, e As EventArgs) Handles C1Button8.Click
        RichTextBox1.Clear()
    End Sub

    'SAVE ENCRYPTED
    Private Sub C1Button9_Click(sender As Object, e As EventArgs) Handles C1Button9.Click

        If RichTextBox1.Text <> "" And SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            System.IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox1.Text)
        End If
    End Sub

    'BROWSE ENCRYPTED
    Private Sub C1Button7_Click(sender As Object, e As EventArgs) Handles C1Button7.Click
        OpenFileDialog1.ShowDialog()
        If (OpenFileDialog1.FileName <> "") Then
            ProcessTxtFile(OpenFileDialog1, RichTextBox1)
        End If
    End Sub

    Private Sub C1Button11_Click(sender As Object, e As EventArgs) Handles C1Button11.Click
        OpenFileDialog2.ShowDialog()
        If (OpenFileDialog2.FileName <> "") Then
            Label4.Text = "File Name: " & OpenFileDialog2.SafeFileName
        End If
    End Sub

    Private Sub C1Button12_Click(sender As Object, e As EventArgs) Handles C1Button12.Click
        OpenFileDialog3.ShowDialog()
        If (OpenFileDialog3.FileName <> "") Then
            Label5.Text = "File Name: " & OpenFileDialog3.SafeFileName
        End If
    End Sub

    Private Sub C1Button4_Click(sender As Object, e As EventArgs) Handles C1Button4.Click

        If CheckBox1.Checked Then
            GoTo CIPHER
        Else
            GoTo NON_CIPHER
        End If

CIPHER:

        If RichTextBox1.Text = "" Or RichTextBox2.Text = "" Then Exit Sub
        Dim matches = False
        Dim r1 As New List(Of String)
        Dim r2 As New List(Of String)
        For i = 0 To 25
            Dim cc = New CaeserCipher(i)
            'matches = If(RichTextBox2.Text = , True, False)
            Dim rtc = New RichTextBox
            rtc.Text = cc.Decipher(RichTextBox1.Text)
            r1.AddRange(rtc.Lines.ToArray)
            r2.AddRange(RichTextBox2.Lines.ToArray)
            Console.WriteLine(rtc.Lines.Count)
            If (SoftCompareLists(r1, r2) = True) Then
                matches = True
                Exit For
            Else
                r1.Clear()
                r2.Clear()
            End If

        Next

        If matches = True Then
            ListBox2.Items.Clear()
            ListBox2.Items.Add(If(CompareLists(r1, r2) = True, "The two files are identical to each other",
                   "The two files are not much identical to each other"))
            ListBox2.Items.AddRange(GetDifference(r1, r2).ToArray)
        Else
            ListBox2.Items.Clear()
            ListBox2.Items.Add("Files are totally not identical")
        End If
        Exit Sub

NON_CIPHER:
        If OpenFileDialog2.FileName = "" Or OpenFileDialog3.FileName = "" Then
            Exit Sub
        End If

        ListBox2.Items.Clear()
        Dim file1 As New System.IO.StreamReader(OpenFileDialog2.FileName)
        Dim file2 As New System.IO.StreamReader(OpenFileDialog3.FileName)
        Dim a = New List(Of String)
        Dim b = New List(Of String)

        Do While file1.Peek() <> -1
            a.Add(file1.ReadLine)
        Loop

        Do While file2.Peek() <> -1
            b.Add(file2.ReadLine)
        Loop

        ListBox2.Items.Add(If(CompareLists(a, b) = True, "The two files are identical to each other",
                           "The two files are not identical to each other"))
        ListBox2.Items.AddRange(GetDifference(a, b).ToArray)

    End Sub

    Private Function CompareLists(ByVal x As List(Of String), ByVal y As List(Of String)) As Boolean
        If x.Count <> y.Count Then Return False
        For Each i In x
            If y.Contains(i) = False Then Return False
        Next
        Return True
    End Function

    Private Function SoftCompareLists(ByVal x As List(Of String), ByVal y As List(Of String)) As Boolean
        For Each i In x
            If y.Contains(i) = True Then Return True
        Next
        Return False
    End Function

    Private Function GetDifference(ByVal a As List(Of String), ByVal b As List(Of String)) As List(Of String)
        Dim r As New List(Of String)
        Dim target = If(a.Count >= b.Count, a, b)
        Dim opposing = If(a.Count < b.Count, a, b)
        Dim c = 0
        For Each i In target
            If i <> "" Then
                If opposing.Contains(i) = False Then r.Add("x: " & i)
                If opposing.Contains(i) Then r.Add("-: " & i)
            Else
                r.Add(":: " & i)
            End If
            If c <= opposing.Count - 1 Then
                If target.Contains(opposing.Item(c)) = False Then r.Add("x: " & opposing.Item(c))
            End If
            c += 1
        Next
        Return r
    End Function

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = False Then
            C1Button11.Enabled = True
            C1Button12.Enabled = True
            Exit Sub
        End If
        ListBox2.Items.Clear()
        C1Button11.Enabled = False
        C1Button12.Enabled = False
        OpenFileDialog2.FileName = ""
        OpenFileDialog3.FileName = ""
        Label4.Text = "Please select file"
        Label5.Text = "Please select file"
    End Sub

    Private Sub C1Button10_Click(sender As Object, e As EventArgs) Handles C1Button10.Click
        If RichTextBox1.Text <> "" And SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            System.IO.File.WriteAllText(SaveFileDialog1.FileName, RichTextBox2.Text)
        End If
    End Sub
End Class
