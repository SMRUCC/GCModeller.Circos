Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.Highlights
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language

Namespace Documents.Configurations

    Public Module Extensions

        ''' <summary>
        ''' 不可以使用并行拓展，因为有顺序之分
        ''' 
        ''' {SpeciesName, Color}
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        <Extension>
        Public Function GetBlastAlignmentData(circos As Circos) As NamedValue(Of String)()
            Dim LQuery As NamedValue(Of String)() =
                LinqAPI.Exec(Of NamedValue(Of String)) <= From trackPlot
                                                          In circos.Plots
                                                          Where String.Equals(trackPlot.type, "highlight", StringComparison.OrdinalIgnoreCase) AndAlso
                                                              TypeOf trackPlot.KaryotypeDocumentData Is BlastMaps
                                                          Let Alignment = DirectCast(trackPlot.KaryotypeDocumentData, BlastMaps)
                                                          Select New NamedValue(Of String)(Alignment.SubjectSpecies, Alignment.SpeciesColor)
            Return LQuery
        End Function
    End Module
End Namespace