Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Documents.Configurations

    Public MustInherit Class CircosDocument : Implements ICircosDocNode
        MustOverride Function GenerateDocument(IndentLevel As Integer) As String Implements ICircosDocNode.GenerateDocument
    End Class

    Public Interface ICircosDocNode
        Function GenerateDocument(IndentLevel As Integer) As String
    End Interface

    Public Interface ICircosDocument : Inherits ICircosDocNode
        Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
    End Interface
End Namespace