Imports System.Text
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas
Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Configurations.Nodes.Plots

    Public Class SeperatorCircle : Inherits Histogram

        Dim Width As Integer

        Sub New(Length As Integer, Width As Integer)
            Call MyBase.New(data:=NtProps.GCSkew.CreateLineData(Length, Width))
            Me.Width = Width
        End Sub
    End Class
End Namespace