Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels.NucleicAcidStaticsProperty
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization

Namespace TrackDatas.NtProps

    ''' <summary>
    ''' GC% calculation tools for drawing the circos elements.
    ''' </summary>
    ''' 
    <PackageNamespace("Circos.Nt.Attributes")>
    Public Module GCProps

        ''' <summary>
        ''' Calculate GC% for each gene its fasta sequence in the <paramref name="FASTA"/>.
        ''' </summary>
        ''' <param name="FASTA"></param>
        ''' <returns></returns>
        <ExportAPI("Get.Genes.GC")>
        <Extension>
        Public Function GetGCContentForGenes(FASTA As IEnumerable(Of FastaToken)) As GeneObjectGC()
            Dim LQuery As GeneObjectGC() =
                LinqAPI.Exec(Of GeneObjectGC) <= From fa As FastaToken
                                                 In FASTA
                                                 Let gc As Double = SegmentReader.Get_GCContent(fa.SequenceData.ToUpper)
                                                 Let at As Double = 1 - gc
                                                 Select New GeneObjectGC With {
                                                     .Title = fa.Attributes.First,
                                                     .value = gc,
                                                     .AT = at,
                                                     .GC_AT = (gc / at)
                                                 }
            Return LQuery
        End Function

        <ExportAPI("Get.Genome.GC")>
        Public Function GetGCContentForGENOME(FASTA As FastaToken, winSize As Integer, steps As Integer) As NASegment_GC()
            Dim NT As DNA() = NucleicAcid.CreateObject(FASTA.SequenceData).ToArray
            Dim slideWins As SlideWindowHandle(Of DNA)() =
            NT.CreateSlideWindows(slideWindowSize:=winSize, offset:=steps)
            Dim LQuery As List(Of NASegment_GC) =
            LinqAPI.MakeList(Of NASegment_GC) <=
                From Segment In slideWins
                Let gc As Double = Segment.Elements.GC_Content
                Let at As Double = 1 - gc
                Select New NASegment_GC With {
                    .start = Segment.Left,
                    .end = Segment.Right,
                    .length = Segment.Length,
                    .value = gc,
                    .AT = at,
                    .GC_AT = (gc / at)
                    }

            Dim LastSegment As List(Of DNA) = slideWins.Last.Elements.ToList
            Dim TempChunk As List(Of DNA)
            Dim p As Integer = LQuery.Last.start

            For i As Integer = 0 To LastSegment.Count - 1 Step steps
                TempChunk = LastSegment.Skip(i).ToList
                TempChunk += NT.Take(i)
                LQuery += New NASegment_GC With {
                            .start = p + i,
                            .length = winSize,
                            .end = p + i + winSize,
                            .value = TempChunk.GC_Content
                }
            Next

            Return LQuery.ToArray
        End Function
    End Module

    Public Class GeneObjectGC : Inherits NASegment_GC
        Implements sIdEnumerable

        Public Property Title As String Implements sIdEnumerable.Identifier
    End Class

    ''' <summary>
    ''' <see cref="ValueTrackData.value"/> is GC% value.
    ''' </summary>
    Public Class NASegment_GC : Inherits ValueTrackData

        ''' <summary>
        ''' GC%
        ''' </summary>
        ''' <returns></returns>
        <Column("GC%")> Public Overrides Property value As Double
        ''' <summary>
        ''' AT%
        ''' </summary>
        ''' <returns></returns>
        <Column("AT%")> Public Property AT As Double

        ''' <summary>
        ''' GC/AT ratio
        ''' </summary>
        ''' <returns></returns>
        <Column("GC/AT")> Public Property GC_AT As Double
        ''' <summary>
        ''' The size of this fragment
        ''' </summary>
        ''' <returns></returns>
        Public Property length As Integer

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace