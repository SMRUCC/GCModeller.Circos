Imports LANS.SystemsBiology.SequenceModel.ISequenceModel
Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult
Imports LANS.SystemsBiology.SequenceModel

Namespace TrackDatas.NtProps

    Public Class GenomeGCContent : Inherits TrackDataDocument

        Dim GC_Contents As GCContent.NASegment_GC()
        Dim Steps As Integer

        Sub New(SourceFasta As FASTA.FastaToken, Optional SegmentLength As Integer = -1, Optional steps As Integer = 10)
            If SegmentLength <= 0 Then SegmentLength = 10
            Me.GC_Contents = GCContent.GetGCContentForGENOME(SourceFasta, winSize:=SegmentLength, steps:=steps)
            Dim Avg = (From n In GC_Contents Select n.GC).ToArray.Average

            For Each item In Me.GC_Contents
                item.GC -= Avg
            Next

            Me.Steps = steps
        End Sub

        Public Overrides Function ToString() As String
            Return String.Join("; ", (From item In Me.GC_Contents Select str = item.ToString).ToArray)
        End Function

        Protected Overrides Function GenerateDocument() As String
            Dim LQuery = (From item In GC_Contents Select String.Format("chr1 {0} {1} {2}", item.Left, item.Left + Steps, item.GC))
            Dim sBuilder As StringBuilder = New StringBuilder(String.Join(vbCrLf, LQuery))



            Return sBuilder.ToString
        End Function

        Public Overrides ReadOnly Property Max As Double
            Get
                Return (From item In GC_Contents Select item.GC).ToArray.Max
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                Return (From item In GC_Contents Select item.GC).ToArray.Min
            End Get
        End Property

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
End Namespace