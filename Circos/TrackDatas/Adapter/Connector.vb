Imports System.Text
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace TrackDatas

    Public Class Connector : Inherits data(Of RegionTrackData)

        Sub New(data As IEnumerable(Of RegionTrackData))
            Call MyBase.New(data)
        End Sub
    End Class
End Namespace