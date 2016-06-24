Imports LANS.SystemsBiology.SequenceModel.ISequenceModel
Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult
Imports LANS.SystemsBiology.SequenceModel
Imports Microsoft.VisualBasic.Serialization

Namespace TrackDatas.NtProps

    Public Class GenomeGCContent : Inherits data(Of NASegment_GC)

        Sub New(SourceFasta As FASTA.FastaToken, Optional SegmentLength As Integer = -1, Optional steps As Integer = 10)
            Call MyBase.New(
                __sourceGC(SourceFasta,
                         If(SegmentLength <= 0, 10, SegmentLength),
                         steps))
        End Sub

        Private Overloads Shared Function __sourceGC(nt As FASTA.FastaToken, segLen As Integer, steps As Integer) As NASegment_GC()
            Dim source As NASegment_GC() =
                GCProps.GetGCContentForGENOME(nt, winSize:=segLen, steps:=steps)
            Dim avg As Double = (From n In source Select n.value).Average

            For Each x As NASegment_GC In source
                x.value -= avg
            Next

            Return source
        End Function

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace