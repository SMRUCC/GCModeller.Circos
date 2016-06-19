Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.AnalysisTools.ComparativeGenomics
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas

Namespace Documents.Karyotype.NtProps

    Public Class DeltaDiff : Inherits data(Of ValueTrackData)

        Dim _Steps As Integer
        Dim dbufs As Double()

        Sub New(SequenceModel As I_PolymerSequenceModel, SlideWindowSize As Integer, Steps As Integer)
            Me._Steps = Steps
            Dim NT = New NucleotideModels.NucleicAcid(SequenceModel)
            Dim SW = NT.ToArray.CreateSlideWindows(SlideWindowSize, Steps)
            Dim NT_Cache = New NucleicAcid(NT.ToArray)
            Dim ChunkBuffer = (From n In SW.AsParallel
                               Select n.Left,
                                   d = Sigma(NT_Cache, New NucleotideModels.NucleicAcid(n.Elements))
                               Order By Left Ascending).ToArray

            Dim LastSegment = SW.Last.Elements.ToList
            Dim TempChunk As List(Of NucleotideModels.DNA)
            Dim p As Long = SW.Last.Left
            Dim NT_Array = NT.ToArray
            Dim List = New List(Of Double)

            For i As Integer = 0 To LastSegment.Count - 1 Step Steps
                TempChunk = LastSegment.Skip(i).ToList
                Call TempChunk.AddRange(NT_Array.Take(i).ToArray)
                Call List.Add(Sigma(NT_Cache, New NucleotideModels.NucleicAcid(TempChunk.ToArray)))
            Next

            Dim MergeList = (From item In ChunkBuffer Select item.d).ToList
            Call MergeList.AddRange(List)
            Me.dbufs = MergeList.ToArray
        End Sub

        Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of ValueTrackData)
            Dim p As Integer

            For i As Integer = 0 To dbufs.Length - 1
                Yield New ValueTrackData With {
                    .chr = "chr1",
                    .start = p,
                    .end = p + _Steps,
                    .value = dbufs(i)
                }
                p += _Steps
            Next
        End Function
    End Class
End Namespace