Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.TrackDatas
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.ComponentModel
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace Documents.Karyotype.Highlights

    Public Class HighlightLabel : Inherits data(Of TextTrackData)

        Sub New(annoData As IEnumerable(Of IGeneBrief))
            Call MyBase.New(__textSource(annoData))
        End Sub

        Sub New(metas As IEnumerable(Of TextTrackData))
            Call MyBase.New(metas)
        End Sub

        Protected Sub New()
            Call MyBase.New(Nothing)
        End Sub

        Private Shared Iterator Function __textSource(annoData As IEnumerable(Of IGeneBrief)) As IEnumerable(Of TextTrackData)
            For Each text As TextTrackData In From gene As IGeneBrief
                                              In annoData
                                              Where Not (String.IsNullOrEmpty(gene.Identifier) OrElse
                                                  String.Equals("-", gene.Identifier) OrElse  '这些基因名都表示没有的空值，去掉
                                                  String.Equals("/", gene.Identifier) OrElse
                                                  String.Equals("\", gene.Identifier))
                                              Select New TextTrackData With {
                                                  .start = CInt(gene.Location.Left),
                                                  .end = CInt(gene.Location.Right),
                                                  .text = Regex.Replace(gene.Identifier, "\s+", "_")
                                              }  ' 空格会出现问题的，所以在这里替换掉
                Yield text
            Next
        End Function
    End Class
End Namespace