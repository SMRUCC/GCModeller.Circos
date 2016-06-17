Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Documents.Configurations.Nodes.Plots.Lines

    Public Class Axis : Inherits CircosDocument
        Implements ICircosDocNode

        <Circos> Public Property color As String = "lgrey_a2"
        <Circos> Public Property thickness As String = "1"
        <Circos> Public Property spacing As String = "0.025r"

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("axis", IndentLevel, Nothing)
        End Function
    End Class

    Public Class Background : Inherits CircosDocument
        Implements ICircosDocNode

        <Circos> Public Property color As String = "vvlred"
        <Circos> Public Property y1 As String = "0.002"
        <Circos> Public Property y0 As String = "0.006"

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("background", IndentLevel, Nothing)
        End Function
    End Class
End Namespace