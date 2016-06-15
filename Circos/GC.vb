Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels.NucleicAcidStaticsProperty
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.Language

''' <summary>
''' GC% calculation tools for drawing the circos elements.
''' </summary>
''' 
<PackageNamespace("Circos.Nt.Attributes")>
Public Module GCContent

    <ExportAPI("Get.Genes.GC")>
    Public Function GetGCContentForGenes(FASTA As FastaFile) As GeneObjectGC()
        Dim LQuery = (From fa As FastaToken In FASTA
                      Let gc As Double = SegmentReader.Get_GCContent(fa.SequenceData.ToUpper)
                      Let at As Double = 1 - gc
                      Select New GeneObjectGC With {
                          .Title = fa.Attributes.First,
                          .GC = gc,
                          .AT = at,
                          .GC_AT = (gc / at)}).ToArray
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
                    .Left = Segment.Left,
                    .Right = Segment.Right,
                    .Length = Segment.Length,
                    .GC = gc,
                    .AT = at,
                    .GC_AT = (gc / at)
                    }

        Dim LastSegment As List(Of DNA) = slideWins.Last.Elements.ToList
        Dim TempChunk As List(Of DNA)
        Dim p As Long = LQuery.Last.Left

        For i As Integer = 0 To LastSegment.Count - 1 Step steps
            TempChunk = LastSegment.Skip(i).ToList
            Call TempChunk.AddRange(NT.Take(i).ToArray)
            Call LQuery.Add(New NASegment_GC With {
                            .Left = p + i,
                            .Length = winSize,
                            .Right = p + i + winSize,
                            .GC = TempChunk.GC_Content})
        Next

        Return LQuery.ToArray
    End Function

    Public Class GeneObjectGC : Inherits NASegment_GC
        Public Property Title As String
    End Class

    Public Class NASegment_GC

        <Column("GC%")> Public Property GC As Double
        <Column("AT%")> Public Property AT As Double
        <Column("GC/AT")> Public Property GC_AT As Double
        Public Property Left As Long
        Public Property Right As Long
        Public Property Length As Integer

        Public Overrides Function ToString() As String
            Return GC.ToString
        End Function
    End Class
End Module
