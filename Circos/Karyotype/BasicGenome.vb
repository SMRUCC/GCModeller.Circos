Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel

Namespace Karyotype

    ''' <summary>
    ''' 基因组的基本框架的描述信息
    ''' </summary>
    Public Class BasicGenomeSkeleton : Inherits SkeletonInfo

        Dim Length As Integer, Color As String
        Dim BandData As TripleKeyValuesPair()

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="GenomeLength"></param>
        ''' <param name="Color"></param>
        ''' <param name="BandData"><see cref="TripleKeyValuesPair.Key"/>为颜色，其余的两个属性分别为左端起始和右端结束</param>
        Sub New(GenomeLength As Integer, Color As String, BandData As TripleKeyValuesPair())
            Me.Length = GenomeLength
            Me.Color = Color
            Me.BandData = BandData
        End Sub

        Protected Overrides Function GenerateDocument() As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)

            Call sBuilder.AppendLine($"chr - chr1 1 1 {Length} {Color}") '染色体的基因信息：长度
            If Not BandData.IsNullOrEmpty Then
                For i As Integer = 0 To BandData.Count - 1
                    Dim objBand = BandData(i)
                    Dim str = $"band chr1 band{i} band{i} {BandData(i).Value1} {BandData(i).Value2} {BandData(i).Key}"
                    Call sBuilder.AppendLine(str)
                Next
            End If

            Return sBuilder.ToString
        End Function

        Public Overrides ReadOnly Property Size As Integer
            Get
                Return Length
            End Get
        End Property
    End Class
End Namespace