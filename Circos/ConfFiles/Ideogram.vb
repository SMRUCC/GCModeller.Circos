Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports System.Text

Namespace Configurations

    ''' <summary>
    ''' The ``&lt;ideogram>`` block defines the position, size, labels and other
    ''' properties Of the segments On which data are drawn. These segments
    ''' are usually chromosomes, but can be any Integer axis.
    ''' </summary>
    Public Class Ideogram : Inherits CircosConfig
        Implements ICircosDocument

        Public Property Ideogram As Nodes.Ideogram = New Nodes.Ideogram

        Sub New(Circos As Circos)
            Call MyBase.New(IdeogramConf, Circos)
        End Sub

        Protected Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Ideogram.GenerateDocument(IndentLevel)
        End Function
    End Class
End Namespace