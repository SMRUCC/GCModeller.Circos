Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas.Highlights
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Configurations.Nodes.Plots

Namespace Configurations.Nodes.Plots

    ''' <summary>
    ''' 可以用来表示调控关系
    ''' </summary>
    Public Class Connector : Inherits TracksPlot(Of RegionTrackData)

        <Circos> Public Overrides ReadOnly Property type As String
            Get
                Return "connector"
            End Get
        End Property

        Sub New(data As IEnumerable(Of RegionTrackData))
            Call MyBase.New(New TrackDatas.Connector(data))
        End Sub

        Sub New(doc As TrackDatas.Connector)
            Call MyBase.New(doc)
        End Sub

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of Connector)(Me)
        End Function
    End Class
End Namespace