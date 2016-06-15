Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Documents.Configurations.Nodes.Plots

    Public Class ConditionalRule : Inherits CircosDocument
        Implements ICircosDocNode

        <SimpleConfig()> Public Property condition As String = "var(value) > 0.6"
        <SimpleConfig()> Public Property color As String = "red"

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("rule", IndentLevel, Nothing)
        End Function
    End Class
End Namespace