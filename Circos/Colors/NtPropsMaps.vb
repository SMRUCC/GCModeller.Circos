Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas.NtProps
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization

Namespace Colors

    ''' <summary>
    ''' Maps the colors based on the nt property
    ''' </summary>
    Public Module NtPropsMapsExtensions

        <Extension>
        Public Function FromGC(source As IEnumerable(Of GeneObjectGC)) As Mappings()
            Dim GC As Double() = source.ToArray(Function(x) x.value)
            Return GradientMaps.GradientMappings(GC)
        End Function

        <Extension>
        Public Function FromAT(source As IEnumerable(Of GeneObjectGC)) As Mappings()
            Dim AT As Double() = source.ToArray(Function(x) x.value)
            Return GradientMaps.GradientMappings(AT)
        End Function

        <Extension>
        Public Function PropertyMaps(source As IEnumerable(Of FastaToken)) As NtPropsMaps
            Dim genome As New FastaFile(source)
            Dim props As GeneObjectGC() = GCProps.GetGCContentForGenes(genome)
            Dim AT As Mappings() = props.FromAT
            Dim GC As Mappings() = props.FromGC

            Return New NtPropsMaps With {
                .source = genome,
                .props = (From x As GeneObjectGC
                          In props
                          Select x
                          Group x By x.Title Into Group) _
                               .ToDictionary(Function(x) x.Title,
                                             Function(x) x.Group.First),
                .AT = (From x As Mappings
                       In AT
                       Select x
                       Group x By x.value Into Group) _
                            .ToDictionary(Function(x) x.value,
                                          Function(x) x.Group.First.CircosColor),
                .GC = (From x As Mappings
                       In GC
                       Select x
                       Group x By x.value Into Group) _
                            .ToDictionary(Function(x) x.value,
                                          Function(x) x.Group.First.CircosColor)
            }
        End Function
    End Module

    Public Structure NtPropsMaps

        Public source As FastaFile
        ''' <summary>
        ''' {value, circos color expression}
        ''' </summary>
        Public AT As Dictionary(Of Double, String)
        ''' <summary>
        ''' {value, circos color expression}
        ''' </summary>
        Public GC As Dictionary(Of Double, String)
        Public props As Dictionary(Of String, GeneObjectGC)

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Structure
End Namespace