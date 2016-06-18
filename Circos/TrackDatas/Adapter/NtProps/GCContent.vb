Imports LANS.SystemsBiology.SequenceModel.ISequenceModel
Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.SequenceModel.FASTA

Namespace TrackDatas.NtProps

    ''' <summary>
    ''' 每一个基因的GC%的表述
    ''' </summary>
    Public Class GeneGCContent : Inherits data(Of GeneObjectGC)

        Public ReadOnly Property SourceFasta As FastaFile

        Sub New(Source As FastaFile)
            Call MyBase.New(GCContent.GetGCContentForGenes(Source))
            SourceFasta = Source
        End Sub
    End Class
End Namespace