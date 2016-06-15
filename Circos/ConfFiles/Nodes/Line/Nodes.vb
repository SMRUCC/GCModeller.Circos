Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Documents.Configurations.Nodes.Plots.Lines

    Public Class Axis : Inherits CircosDocument
        Implements ICircosDocNode

        <SimpleConfig> Public Property color As String = "lgrey_a2"
        <SimpleConfig> Public Property thickness As String = "1"
        <SimpleConfig> Public Property spacing As String = "0.025r"

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("axis", IndentLevel, Nothing)
        End Function
    End Class

    Public Class Background : Inherits CircosDocument
        Implements ICircosDocNode

        <SimpleConfig> Public Property color As String = "vvlred"
        <SimpleConfig> Public Property y1 As String = "0.002"
        <SimpleConfig> Public Property y0 As String = "0.006"

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("background", IndentLevel, Nothing)
        End Function
    End Class
End Namespace