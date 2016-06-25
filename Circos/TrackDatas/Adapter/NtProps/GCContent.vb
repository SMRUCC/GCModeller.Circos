Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports LANS.SystemsBiology.SequenceModel.ISequenceModel
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Linq

Namespace TrackDatas.NtProps

    ''' <summary>
    ''' 每一个基因的GC%的表述
    ''' </summary>
    Public Class GeneGCContent : Inherits data(Of ValueTrackData)

        Public ReadOnly Property SourceFasta As FastaFile

        Sub New(Source As FastaFile)
            Call MyBase.New(
                GCProps.GetGCContentForGenes(Source) _
                       .Select(
                       Function(x) DirectCast(x, ValueTrackData)))
            SourceFasta = Source
        End Sub

        Sub New(genome As IEnumerable(Of FastaToken),
                karyotype As Karyotype.SkeletonInfo,
                winSize As Integer,
                steps As Integer,
                getValue As Func(Of NASegment_GC, Double))

            Dim list As New List(Of ValueTrackData)
            Dim chrs = karyotype.Karyotypes.ToDictionary(
                Function(x) x.chrLabel,
                Function(x) x.chrName)

            For Each nt As FastaToken In genome
                Dim raw As Double() = GCProps.GetGCContentForGENOME(
                    nt,
                    winSize,
                    steps).ToArray(getValue)
                Dim chr As String = nt.Title.Split("."c).First

                chr = chrs(chr)
                list += GCSkew.__sourceGC(chr, GCSkew.__avgData(raw), [steps])

                Call Console.Write(">")
            Next

            __source = list
        End Sub
    End Class
End Namespace