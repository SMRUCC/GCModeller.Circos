Imports System.Text
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Configurations.Nodes.Plots
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace Documents.Karyotype

    Public Class Connector : Inherits TrackDataDocument

        Dim data As Connection()

        Sub New(data As IEnumerable(Of Connection))
            Me.data = data.ToArray
        End Sub

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property Max As Double
            Get
                Return 1
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                Return 0
            End Get
        End Property

        Protected Overrides Function GenerateDocument() As String
            Dim LQuery = data.ToArray(Function(x) x.ToString)
            Return String.Join(vbLf, LQuery)
        End Function
    End Class
End Namespace