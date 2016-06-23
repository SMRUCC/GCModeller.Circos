Imports System.Text
Imports LANS.SystemsBiology.ComponentModel.Loci
Imports LANS.SystemsBiology.NCBI.Extensions.LocalBLAST.Application
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting

Namespace Karyotype

    ''' <summary>
    ''' The very basically genome skeleton information description.(基因组的基本框架的描述信息)
    ''' </summary>
    Public Class BasicGenomeSkeleton : Inherits SkeletonInfo

        ''' <summary>
        ''' 这个构造函数是用于单个染色体的
        ''' </summary>
        ''' <param name="GenomeLength"></param>
        ''' <param name="Color"></param>
        ''' <param name="BandData"><see cref="TripleKeyValuesPair.Key"/>为颜色，其余的两个属性分别为左端起始和右端结束</param>
        Sub New(GenomeLength As Integer, Color As String, Optional BandData As TripleKeyValuesPair() = Nothing)
            Me.Size = GenomeLength
            Me.__bands = New List(Of Band)(GenerateDocument(BandData))
            Call __karyotype(Color)
        End Sub

        Protected Sub New()
        End Sub

        Private Overloads Shared Iterator Function GenerateDocument(data As IEnumerable(Of TripleKeyValuesPair)) As IEnumerable(Of Band)
            If Not data.IsNullOrEmpty Then
                Dim i As Integer

                For Each x As TripleKeyValuesPair In data
                    Yield New Band With {
                        .chrName = "chr1",
                        .bandX = "band" & i,
                        .bandY = "band" & i,
                        .start = x.Value1.ParseInteger,
                        .end = x.Value2.ParseInteger,
                        .color = x.Key
                    }

                    i += 1
                Next
            End If
        End Function

        Public Overrides ReadOnly Property Size As Integer

        ''' <summary>
        ''' Creates the model for the multiple chromosomes genome data in circos.(使用这个函数进行创建多条染色体的)
        ''' </summary>
        ''' <param name="source">Band数据</param>
        ''' <param name="chrs">karyotype数据</param>
        ''' <returns></returns>
        Public Shared Function FromBlastnMappings(source As IEnumerable(Of BlastnMapping), chrs As IEnumerable(Of FastaToken)) As BasicGenomeSkeleton
            Dim ks As Karyotype() =
                LinqAPI.Exec(Of Karyotype) <= From nt As SeqValue(Of FastaToken)
                                              In chrs.SeqIterator
                                              Let name As String = nt.obj.Title.NormalizePathString(True).Replace(" ", "_")
                                              Select New Karyotype With {
                                                  .chrName = "chr" & nt.i,
                                                  .chrLabel = name,
                                                  .color = "",
                                                  .start = 0,
                                                  .end = nt.obj.Length
                                              }.nt.SetValue(nt.obj).As(Of Karyotype)
            Dim labels As Dictionary(Of String, Karyotype) =
                ks.ToDictionary(Function(x) x.nt.Value.Title, Function(x) x)
            Dim bands As List(Of Band) =
                LinqAPI.MakeList(Of Band) <= From x As SeqValue(Of BlastnMapping)
                                             In source.SeqIterator
                                             Let chr As String = labels(x.obj.Reference).chrName
                                             Let loci As NucleotideLocation = x.obj.MappingLocation
                                             Select New Band With {
                                                 .chrName = chr,
                                                 .start = loci.Left,
                                                 .end = loci.Right,
                                                 .color = "",
                                                 .bandX = "band" & x.i,
                                                 .bandY = "band" & x.i
                                             }
            Return New BasicGenomeSkeleton With {
                .__bands = bands,
                .__karyotypes = New List(Of Karyotype)(ks)
            }
        End Function
    End Class
End Namespace