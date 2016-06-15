Imports LANS.SystemsBiology.SequenceModel.ISequenceModel
Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.SequenceModel.FASTA

Namespace Documents.Karyotype.NtProps

    Public Class GeneGCContent : Inherits KaryotypeDocument

        Dim SourceFasta As FastaToken

        Sub New(Source As FastaToken)
            SourceFasta = Source
        End Sub

        Public Overrides Function ToString() As String
            Return SourceFasta.SequenceData
        End Function

        Protected Overrides Function GenerateDocument() As String
            Dim GC_Contents = GCContent.GetGCContentForGenes(SourceFasta)
            Dim LQuery = (From item In GC_Contents Select String.Format("chr1 {0} {1} {2}", item.Left, item.Right, item.GC))
            Return String.Join(vbCrLf, LQuery)
        End Function

        Public Overrides ReadOnly Property Max As Double
            Get
                Return 1
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                Return 0
            End Get
        End Property

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
End Namespace