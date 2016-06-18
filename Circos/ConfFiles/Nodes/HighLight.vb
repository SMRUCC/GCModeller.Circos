Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Configurations.Nodes.Plots
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas.Highlights
Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic.Scripting

Namespace Configurations.Nodes.Plots

    Public Class HighLight : Inherits TracksPlot(Of ValueTrackData)

        Public ReadOnly Property Highlights As Highlights
            Get
                Return Me.TracksData.As(Of Highlights)
            End Get
        End Property

        Sub New(HighlightsDataModel As Highlights)
            Call MyBase.New(HighlightsDataModel)
        End Sub

        <Circos> Public Overrides ReadOnly Property type As String
            Get
                Return "highlight"
            End Get
        End Property

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of HighLight)(Me)
        End Function
    End Class
End Namespace