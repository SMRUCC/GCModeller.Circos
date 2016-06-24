Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Configurations

    Public MustInherit Class CircosDocument : Implements ICircosDocNode
        MustOverride Function GenerateDocument(IndentLevel As Integer) As String Implements ICircosDocNode.GenerateDocument
    End Class

    ''' <summary>
    ''' This object can be convert to text document by using method <see cref="GenerateDocument"/>
    ''' </summary>
    Public Interface ICircosDocNode
        Function GenerateDocument(indentLevel As Integer) As String
    End Interface

    ''' <summary>
    ''' This object can be save as a text doc for the circos plot
    ''' </summary>
    Public Interface ICircosDocument : Inherits ICircosDocNode, ISaveHandle
    End Interface
End Namespace