Imports System.Text
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Colors
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.CsvExports
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat.ComponentModels
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.NCBI.Extensions.LocalBLAST.Application.RpsBLAST
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Language

Namespace Karyotype.GeneObjects

    ''' <summary>
    ''' 基因对象
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PTTMarks : Inherits SkeletonInfo

        Public Overrides ReadOnly Property Size As Integer

        Sub New(genome As PTTDbLoader, Optional MyvaCog As MyvaCOG() = Nothing, Optional defaultColor As String = "blue")
            If genome Is Nothing Then
                Throw New Exception("No data was found in the genome information!")
            End If

            If MyvaCog.IsNullOrEmpty Then      ' 绘制基本图型
                __bands = PTTMarks.Generate(genome, defaultColor:=defaultColor).ToList
            Else
                __bands = PTTMarks.Generate(genome, MyvaCog, defaultColor).ToList
            End If

            Call __karyotype()
        End Sub

        Sub New(genes As GeneDumpInfo(), nt As FastaToken, Optional defaultColor As String = "blue")
            Dim MyvaCog = LinqAPI.Exec(Of MyvaCOG) <=
                From gene As GeneDumpInfo
                In genes
                Select New MyvaCOG With {
                    .COG = gene.COG,
                    .QueryName = gene.LocusID,
                    .QueryLength = gene.Length
                }
            Dim genome = PTTDbLoader.CreateObject(genes, nt)
            __bands = PTTMarks.Generate(genome, MyvaCog, defaultColor).ToList
            Call __karyotype()
        End Sub

        Private Shared Iterator Function Generate(GenomeBrief As PTTDbLoader, MyvaCog As MyvaCOG(), Optional defaultColor As String = "blue") As IEnumerable(Of Band)
            Dim GetColorProfile As Func(Of String, String) = GetCogColorProfile(MyvaCog, defaultColor)
            Dim genome As PTT = GenomeBrief.ORF_PTT

            If Not genome Is Nothing AndAlso Not genome.GeneObjects.IsNullOrEmpty Then
                For Each gene As GeneBrief In genome
                    Yield New Band With {
                        .chrName = "chr1",
                        .bandX = gene.Gene,
                        .bandY = gene.Product.Replace(" ", "_"),
                        .start = gene.Location.Left,
                        .end = gene.Location.Right,
                        .color = GetColorProfile(gene.Gene)
                    }
                Next
            End If

            genome = GenomeBrief.RNARnt

            If Not genome Is Nothing AndAlso Not genome.GeneObjects.IsNullOrEmpty Then
                For Each gene As GeneBrief In genome
                    Yield New Band With {
                        .chrName = "chr1",
                        .bandX = gene.Gene,
                        .bandY = gene.Product.Replace(" ", "_"),
                        .start = gene.Location.Left,
                        .end = gene.Location.Right,
                        .color = "blue"
                    }
                Next
            End If
        End Function

        Private Shared Iterator Function Generate(GenomeBrief As PTTDbLoader, Optional defaultColor As String = "blue") As IEnumerable(Of Band)
            For Each gene As GeneBrief In GenomeBrief.ORF_PTT
                Yield New Band With {
                    .chrName = "chr1",
                    .bandX = gene.Gene,
                    .bandY = gene.Product.Replace(" ", "_"),
                    .start = gene.Location.Left,
                    .end = gene.Location.Right,
                    .color = defaultColor
                }
            Next
            For Each gene As GeneBrief In GenomeBrief.RNARnt
                Yield New Band With {
                    .chrName = "chr1",
                    .bandX = gene.Gene,
                    .bandY = gene.Product.Replace(" ", "_"),
                    .start = gene.Location.Left,
                    .end = gene.Location.Right,
                    .color = "blue"
                }
            Next
        End Function
    End Class
End Namespace