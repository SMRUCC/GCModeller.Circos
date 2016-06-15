Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic.Scripting
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.Highlights

Namespace Documents.Configurations.Nodes.Plots

    Public Class HighLight : Inherits Plots.Plot

        Public ReadOnly Property Highlights As Highlights
            Get
                Return Me.KaryotypeDocumentData.As(Of Highlights)
            End Get
        End Property

        Sub New(HighlightsDataModel As Highlights)
            Call MyBase.New(HighlightsDataModel)
        End Sub

        <SimpleConfig()> Public Overrides ReadOnly Property Type As String
            Get
                Return "highlight"
            End Get
        End Property

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of HighLight)(Me)
        End Function
    End Class
End Namespace