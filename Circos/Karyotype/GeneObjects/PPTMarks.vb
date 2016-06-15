Imports System.Text
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.CsvExports
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.NCBI.Extensions.LocalBLAST.Application.RpsBLAST
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Language

Namespace Documents.Karyotype.GeneObjects

    ''' <summary>
    ''' 基因对象
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PTTMarks : Inherits GenomeDescription

        Dim _GenomeBrief As PTTDbLoader
        Dim _MyvaCog As MyvaCOG()
        Dim _DefaultColor As String

        Public Overrides ReadOnly Property Size As Integer
            Get
                Return _GenomeBrief.ORF_PTT.Size
            End Get
        End Property

        Sub New(GenomeBrief As PTTDbLoader, Optional MyvaCog As MyvaCOG() = Nothing, Optional defaultColor As String = "blue")
            _GenomeBrief = GenomeBrief
            _MyvaCog = MyvaCog
            _DefaultColor = defaultColor
        End Sub

        Sub New(GenomeBrief As GeneDumpInfo(), SourceFasta As FastaToken, Optional defaultColor As String = "blue")
            _DefaultColor = defaultColor
            _MyvaCog = LinqAPI.Exec(Of MyvaCOG) <= From gene As GeneDumpInfo
                                                   In GenomeBrief
                                                   Select New MyvaCOG With {
                                                       .COG = gene.COG,
                                                       .QueryName = gene.LocusID,
                                                       .QueryLength = gene.Length
                                                   }
            _GenomeBrief = PTTDbLoader.CreateObject(GenomeBrief, SourceFasta)
        End Sub

        Private Shared Function Generate(GenomeBrief As PTTDbLoader, MyvaCog As MyvaCOG(), Optional defaultColor As String = "blue") As String
            Dim sBuilder As StringBuilder = New StringBuilder(String.Format("chr - chr1 1 1 {0} black", GenomeBrief.RptGenomeBrief.Size) & vbCrLf, 1024)
            Dim GetColorProfile As Func(Of String, String) = GetCogColorProfile(MyvaCog, defaultColor)
            Dim GeneData = GenomeBrief.ORF_PTT

            If Not GeneData Is Nothing AndAlso Not GeneData.GeneObjects.IsNullOrEmpty Then
                For Each GeneObject In GeneData
                    Call sBuilder.AppendLine(String.Format("band chr1 {0} {1} {2} {3} {4}", GeneObject.Gene, GeneObject.Product.Replace(" ", "_"), GeneObject.Location.Left, GeneObject.Location.Right, GetColorProfile(GeneObject.Gene)))
                Next
            End If

            GeneData = GenomeBrief.RNARnt
            If Not GeneData Is Nothing AndAlso Not GeneData.GeneObjects.IsNullOrEmpty Then
                For Each GeneObject In GenomeBrief.RNARnt
                    Call sBuilder.AppendLine(String.Format("band chr1 {0} {1} {2} {3} blue", GeneObject.Gene, GeneObject.Product.Replace(" ", "_"), GeneObject.Location.Left, GeneObject.Location.Right))
                Next
            End If

            Return sBuilder.ToString
        End Function

        Private Shared Function Generate(GenomeBrief As PTTDbLoader, Optional defaultColor As String = "blue") As String
            Dim sBuilder As StringBuilder = New StringBuilder(String.Format("chr - chr1 1 1 {0} black", GenomeBrief.RptGenomeBrief.Size) & vbCrLf, 1024)

            For Each GeneObject In GenomeBrief.ORF_PTT
                Call sBuilder.AppendLine(String.Format("band chr1 {0} {1} {2} {3} {4}", GeneObject.Gene, GeneObject.Product.Replace(" ", "_"), GeneObject.Location.Left, GeneObject.Location.Right, defaultColor))
            Next
            For Each GeneObject In GenomeBrief.RNARnt
                Call sBuilder.AppendLine(String.Format("band chr1 {0} {1} {2} {3} blue", GeneObject.Gene, GeneObject.Product.Replace(" ", "_"), GeneObject.Location.Left, GeneObject.Location.Right))
            Next

            Return sBuilder.ToString
        End Function

        Protected Overrides Function GenerateDocument() As String
            If _GenomeBrief Is Nothing Then
                Console.WriteLine("[ERROR] No data was found in the genome information!")
                Return ""
            End If

            If _MyvaCog.IsNullOrEmpty Then      '绘制基本图型
                Return Generate(_GenomeBrief, defaultColor:=_DefaultColor)
            Else
                Return Generate(GenomeBrief:=_GenomeBrief, MyvaCog:=_MyvaCog, defaultColor:=_DefaultColor)
            End If
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
                Return False
            End Get
        End Property
    End Class
End Namespace