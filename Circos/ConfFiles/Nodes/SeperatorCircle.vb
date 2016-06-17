Imports System.Text
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype
Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Documents.Configurations.Nodes.Plots

    Public Class SeperatorCircle : Inherits Plots.Histogram

        Dim Width As Integer

        Sub New(Length As Integer, Width As Integer)
            Call MyBase.New(data:=NtProps.GCSkew.CreateLineData(Length, Width))
            Me.Width = Width
        End Sub

        Protected Overrides Function GetMaxValue() As String
            Return Width.ToString
        End Function

        Protected Overrides Function GetMinValue() As String
            Return "0"
        End Function
    End Class
End Namespace