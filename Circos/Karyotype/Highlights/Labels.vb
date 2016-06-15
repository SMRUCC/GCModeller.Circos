Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.ComponentModel
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace Documents.Karyotype.Highlights

    Public Class HighlightLabel : Inherits Highlights

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return False
            End Get
        End Property

        Sub New(annoData As IEnumerable(Of IGeneBrief))
            Me._highLights = (From GeneObject In annoData
                              Where Not (String.IsNullOrEmpty(GeneObject.Identifier) OrElse
                                 String.Equals("-", GeneObject.Identifier) OrElse  '这些基因名都表示没有的空值，去掉
                                 String.Equals("/", GeneObject.Identifier) OrElse
                                 String.Equals("\", GeneObject.Identifier))
                              Select New HighLightsMeta With {
                                 .Left = CInt(GeneObject.Location.Left),
                                 .Right = CInt(GeneObject.Location.Right),
                                 .Value = Regex.Replace(GeneObject.Identifier, "\s+", "_")}).ToArray  ' 空格会出现问题的，所以在这里替换掉
        End Sub

        Sub New(metas As IEnumerable(Of HighLightsMeta))
            Me._highLights = metas.ToArray
        End Sub

        Protected Sub New()
        End Sub

        Protected Overrides Function GenerateDocument() As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)

            For Each Line In Me._highLights
                Call sBuilder.AppendLine(String.Format("chr1 {0} {1} {2}", Line.Left, Line.Right, Line.Value))
            Next

            Return sBuilder.ToString
        End Function
    End Class
End Namespace