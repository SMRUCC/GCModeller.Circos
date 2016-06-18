﻿Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.Highlights
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.Highlights.LocusLabels
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.TrackDatas

Namespace Documents.Configurations.Nodes.Plots

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
            Call MyBase.New(New Karyotype.Connector(data))
        End Sub

        Sub New(doc As Karyotype.Connector)
            Call MyBase.New(doc)
        End Sub

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of Connector)(Me)
        End Function
    End Class
End Namespace