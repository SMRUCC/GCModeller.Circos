Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.TrackDatas

Namespace Documents.Karyotype.Base

    Public Class Histogram : Inherits Karyotype.TrackDataDocument(Of ValueTrackData)

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overrides ReadOnly Property Max As Double
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Protected Overrides Function GenerateDocument() As String
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace