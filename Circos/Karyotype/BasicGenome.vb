Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic

Namespace Karyotype

    ''' <summary>
    ''' The very basically genome skeleton information description.(基因组的基本框架的描述信息)
    ''' </summary>
    Public Class BasicGenomeSkeleton : Inherits SkeletonInfo

        Dim Color As String
        Dim BandData As TripleKeyValuesPair()

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="GenomeLength"></param>
        ''' <param name="Color"></param>
        ''' <param name="BandData"><see cref="TripleKeyValuesPair.Key"/>为颜色，其余的两个属性分别为左端起始和右端结束</param>
        Sub New(GenomeLength As Integer, Color As String, Optional BandData As TripleKeyValuesPair() = Nothing)
            Me.Size = GenomeLength
            Me.__bands = New List(Of Band)(GenerateDocument(BandData))
            Call __karyotype(Color)
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

    End Class
End Namespace