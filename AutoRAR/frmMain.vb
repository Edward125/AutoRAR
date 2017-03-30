
'該軟件目的,監控某一目錄下文件個數,當文件個數等于500個時,將500個文件壓縮,移動到另外一個目錄,同時備份壓縮文檔并刪除原文檔
'1,使用bat調用7z壓縮文檔.默認目錄為 C:\Program Files\7-Zip

'version1.6:add .zip
'version1.5:.btr

Imports System.IO

Imports System.Diagnostics
Public Class frmMain
    Public Declare Sub Sleep Lib "kernel32" Alias "Sleep" (ByVal dwMilliseconds As Long)
    Dim zipName As String
    Dim fDate As Date
    Dim lDate As Date

    Private Sub txtFolder_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFolder.DoubleClick
        OpenFolder(txtFolder)
        If txtFolder.Text <> "" Then
            ' Label1.ForeColor = Color.Green
            My.Settings.watcherPath = Trim(txtFolder.Text)
            lblFile.Text = CheckFile(txtFolder.Text)
            FileSystemWatcher1.Path = Trim(txtFolder.Text)
        End If

    End Sub

    Private Sub OpenFolder(ByVal txt As TextBox)
        Dim strfoldername As String
        Dim FolderBrowserDialog1 As New FolderBrowserDialog
        FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer
        FolderBrowserDialog1.ShowNewFolderButton = True
        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            strfoldername = FolderBrowserDialog1.SelectedPath
            txt.Text = strfoldername
            '  txt.ForeColor = Color.Green
        End If
    End Sub

    Private Sub frmMain_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        '   CheckDllResult()
        '  Call WriteBat(txtFolder.Text)
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '   If CheckDllResult() = False Then MsgBox("该软件已过期!") : End
        txtRar.Text = My.Settings.filePath
        txtBackup.Text = My.Settings.backupPath
        txtFolder.Text = My.Settings.watcherPath

        If txtFolder.Text <> "" Then FileSystemWatcher1.Path = Trim(txtFolder.Text)
        If txtRar.Text <> "" And txtBackup.Text <> "" Then txtFolder.Enabled = True
        If txtFolder.Text <> "" Then lblFile.Text = CheckFile(txtFolder.Text)



        If UBound(Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)) > 0 Then
            MsgBox("Auto7z 已经运行,一次只能运行一个")
            End
        End If

        '  FileSystemWatcher1.Filter = "*.*"
        Dim folderExists As Boolean
        folderExists = My.Computer.FileSystem.DirectoryExists("C:\Program Files\7-Zip")
        If folderExists = True Then
            Label10.ForeColor = Color.Green
            'FileSystemWatcher1.EnableRaisingEvents = True
            Label9.ForeColor = Color.Green
            Label5.Visible = False
        Else
            Beep()
            txtFolder.Enabled = False
            txtBackup.Enabled = False
            txtRar.Enabled = False
            btnManual.Enabled = False
        End If

        lblFile.Text = "0"
        ' FileSystemWatcher1.NotifyFilter = NotifyFilters.Size
    End Sub

    Private Sub txtRar_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRar.Disposed

    End Sub

    Private Sub txtRar_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRar.DoubleClick
        OpenFolder(txtRar)
        If txtRar.Text <> "" Then
            '   Label2.ForeColor = Color.Green
            My.Settings.filePath = Trim(txtRar.Text)
            My.Settings.Save()
            If txtBackup.Text <> "" Then txtFolder.Enabled = True
        End If
    End Sub

    Private Sub txtBackup_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBackup.DoubleClick
        OpenFolder(txtBackup)
        If txtBackup.Text <> "" Then
            If txtRar.Text <> "" Then txtFolder.Enabled = True
            '  Label3.ForeColor = Color.Green
            My.Settings.backupPath = Trim(txtBackup.Text)
            My.Settings.Save()
        End If
    End Sub



    Private Function CheckFile(ByVal folder As String) As Short
        Dim i As Short = 0
        Dim di As IO.DirectoryInfo = New IO.DirectoryInfo(folder)
        For Each f As IO.FileInfo In di.GetFiles()
            'If f.Extension = ".zip" Then
            '    i = i + 1
            'End If

            '  If f.Extension = GetExtension() Then
            i = i + 1
            ' End If
        Next
        Return i
    End Function


    Private Sub AutoRar() '調用bat,完成后并刪除bat
        Call WriteBat(Application.StartupPath & "\temp")
        Try
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "/7z.bat") = True Then
                Shell(Application.StartupPath & "/7z.bat", vbNormalFocus)
            End If
            ' Call DeleteBat()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        'My.Computer.FileSystem.CopyFile(Application.StartupPath & "\" & zipName & ".7z", txtRar.Text & "\" & zipName & ".7z")
        'My.Computer.FileSystem.CopyFile(Application.StartupPath & "\" & zipName & ".7z", txtBackup.Text & "\" & zipName & ".7z")
        ' My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\7z.bat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
    End Sub

    Private Sub WriteBat(ByVal fileFolder As String)

        Dim year As String = Now.Year
        Dim month As String = Format(Now.Month, "00")
        Dim day As String = Format(Now.Day, "00")
        Dim hour As String = Format(Now.Hour, "00")
        Dim minute As String = Format(Now.Minute, "00")
        Dim sec As String = Format(Now.Second, "00")
        'Dim s() As String = DateString.Split("-")
        'Dim ss() As String = TimeString.Split(":")
        Dim rarName As String
        Dim file As String = Chr(37) & fileFolder & Chr(37)
        rarName = "WSCD_" & year & month & day & hour & minute & sec & ".zip"
        Dim line As String = "7z.exe a " & rarName & " " & Chr(34) & fileFolder & "\*.*" & Chr(34)
        Dim filecontents As String = "path C:\Program Files\7-Zip" & vbCrLf & line
        zipName = rarName
        '  My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\7z.bat", "path C:\Program Files\7-Zip" & vbCrLf, True, System.Text.Encoding.GetEncoding(950))
        '  My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\7z.bat", line, True, System.Text.Encoding.GetEncoding(950))
        My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\7z.bat", filecontents, False, System.Text.Encoding.GetEncoding(950))
    End Sub

    Private Sub txtFolder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolder.TextChanged
        'If txtFolder.Text <> "" Then
        '    FileSystemWatcher1.Path = Trim(txtFolder.Text)
        '    '    Debug.WriteLine(FileSystemWatcher1.Path)
        'End If
    End Sub

    Private Sub FileSystemWatcher1_Changed(ByVal sender As System.Object, ByVal e As System.IO.FileSystemEventArgs) Handles FileSystemWatcher1.Created
        '  FileSystemWatcher1.Filter = GetExtension()
        lblFile.Text = CheckFile(txtFolder.Text)
        If CheckFile(txtFolder.Text) >= 500 Then
            Me.Enabled = False
            Call CopyAndDelete(txtFolder.Text)
            Call AutoRar()
            Me.Enabled = True
            Sleep(1000)
            Try
                My.Computer.FileSystem.CopyFile(Application.StartupPath & "\" & zipName, txtRar.Text & "\" & zipName)
                My.Computer.FileSystem.CopyFile(Application.StartupPath & "\" & zipName, txtBackup.Text & "\" & zipName)
                My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\" & zipName, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                My.Computer.FileSystem.DeleteDirectory(Application.StartupPath & "\temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
                My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\7z.bat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
            lblFile.Text = CheckFile(txtFolder.Text)
        End If
    End Sub

    Private Sub DeleteBat()
        Dim fileExists As Boolean
        fileExists = My.Computer.FileSystem.FileExists(Application.StartupPath & "\7z.bat")
        If fileExists = True Then
            My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\7z.bat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        End If
    End Sub


    Private Sub CopyAndDelete(ByVal CopyFolder As String)
        '創建臨時文件夾,存放copy的文檔
        Call CreatTempFolder()
        'cpoy file to temp,delete file from copyfoer
        Dim i As Short = 0
        Dim di As IO.DirectoryInfo = New IO.DirectoryInfo(CopyFolder)
        For Each f As IO.FileInfo In di.GetFiles()
            ' MsgBox(f.FullName)
            My.Computer.FileSystem.CopyFile(f.FullName, Application.StartupPath & "\temp\" & f.Name)
            My.Computer.FileSystem.DeleteFile(f.FullName, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            i = i + 1
            If i = 500 Then Exit Sub
        Next
    End Sub

    Private Sub CreatTempFolder() '創建臨時文件夾
        Dim folderExists As Boolean
        folderExists = My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\temp")
        If folderExists = False Then
            My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\temp")
        End If
    End Sub



    'Private Function checkdate() As Boolean
    '    Dim bool As Boolean = False
    '    Dim s As String = "2013-08-05"
    '    Dim e As String = "2013-11-08"
    '    Dim di As IO.DirectoryInfo = New IO.DirectoryInfo("C:\")

    '    For Each f As IO.FileInfo In di.GetFiles()
    '        If f.LastAccessTime < s Then
    '            bool = False
    '            Exit For
    '        End If
    '    Next

    '    For Each f As IO.FileInfo In di.GetFiles()
    '        If f.LastAccessTime > e Then
    '            bool = False
    '            Exit For
    '        End If
    '    Next
    '    Return bool
    'End Function





    Private Function CheckDllResult() As Boolean
        Dim bool As Boolean = True
        '   Dim s() As String
        Dim span As String

        Dim fileExists As Boolean
        fileExists = My.Computer.FileSystem.FileExists("C:\Windows\Auto7z.dll")
        If fileExists = False Then
            My.Computer.FileSystem.WriteAllText("C:\Windows\Auto7z.dll", DateString, False)
            fDate = DateString
            Dim fileData As FileInfo = My.Computer.FileSystem.GetFileInfo("C:\Windows\Auto7z.dll")
            fileData.Attributes = FileAttributes.Hidden
        Else

            '设置属性为可见
            Dim fileData As FileInfo = My.Computer.FileSystem.GetFileInfo("C:\Windows\Auto7z.dll")
            fileData.Attributes = FileAttributes.Normal
            '  Dim fdate As String
            fdate = CDate(My.Computer.FileSystem.ReadAllText("C:\Windows\Auto7z.dll"))
            '  s = fileContents.Split("|")
            'If UBound(s) = 1 Then bool = True ''第一打开,只有第一次运行的时间,true
            '   If UBound(s) = 2 Then
            ' fdate = s(0) : lDate = s(1)
            lDate = DateString
            If lDate < fdate Then bool = False
            span = (lDate - fDate).ToString
            '  MsgBox(span)
            If span.Contains(".") = True Then
                Dim ss() As String
                ss = span.Split(".")
                If ss(0) > 120 Then bool = False
                fileData.Attributes = FileAttributes.Hidden
            Else
                bool = True
                'End If
                fileData.Attributes = FileAttributes.Hidden
            End If
        End If
        Return bool
    End Function


    Private Sub txtRar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRar.TextChanged

    End Sub

    Private Sub txtBackup_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBackup.TextChanged

    End Sub

    Private Sub btnManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManual.Click
        Me.Enabled = False
        Call CopyAndDeleteManual(txtFolder.Text)
        Call AutoRar()
        Me.Enabled = True
        Sleep(1000)
        Try
            My.Computer.FileSystem.CopyFile(Application.StartupPath & "\" & zipName, txtRar.Text & "\" & zipName)
            My.Computer.FileSystem.CopyFile(Application.StartupPath & "\" & zipName, txtBackup.Text & "\" & zipName)
            My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\" & zipName, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            My.Computer.FileSystem.DeleteDirectory(Application.StartupPath & "\temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\7z.bat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        lblFile.Text = CheckFile(txtFolder.Text)
        Me.Enabled = True
    End Sub


    Private Sub CopyAndDeleteManual(ByVal CopyFolder As String)
        '創建臨時文件夾,存放copy的文檔
        Call CreatTempFolder()
        'cpoy file to temp,delete file from copyfoer
        Dim i As Short = 0
        Dim di As IO.DirectoryInfo = New IO.DirectoryInfo(CopyFolder)
        For Each f As IO.FileInfo In di.GetFiles()
            ' MsgBox(f.FullName)
            My.Computer.FileSystem.CopyFile(f.FullName, Application.StartupPath & "\temp\" & f.Name)
            My.Computer.FileSystem.DeleteFile(f.FullName, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            i = i + 1
        Next
    End Sub

    'Private Function GetExtension() As String
    '    Dim extension As String = ""
    '    If ExBtr.Checked = True Then extension = ".btr"
    '    If ExZip.Checked = True Then extension = ".zip"
    '    If ExOther.Checked = True AndAlso txtExOther.Text.Trim <> "" Then extension = txtExOther.Text.Trim.ToLower
    '    Return extension
    'End Function


End Class
